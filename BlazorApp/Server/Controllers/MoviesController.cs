using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorApp.Server.DB;
using BlazorApp.Server.Helpers;
using BlazorApp.Server.Services;
using BlazorApp.Shared;
using BlazorApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<GenresController> logger;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;
        private readonly string containerName = "movies";

        public MoviesController(ApplicationDbContext context, 
            ILogger<GenresController> logger, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;
            var moviesInTheatre = await context.Movies
                .Where(x => x.InTheatres == true)
                .Take(limit)
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();

            var today = DateTime.Today;

            var upcomingReleases = await context.Movies
                .Where(x => x.ReleaseDate > today)
                .Take(limit)
                .OrderBy(x => x.ReleaseDate)
                .ToListAsync();

            return new IndexPageDTO()
            {
                MoviesInTheatre = moviesInTheatre,
                UpcomingReleases = upcomingReleases
            };
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
            var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieActors).ThenInclude(x => x.Person)
                .FirstOrDefaultAsync();
            if (movie == null) return NotFound();

            movie.MovieActors = movie.MovieActors.OrderBy(x => x.Order).ToList();

            var model = new MovieDetailsDTO()
            {
                Movie = movie,
                Genres = movie.MovieGenres.Select(x => x.Genre).ToList(),
                Actors = movie.MovieActors.Select(x =>
                    new Person
                    {
                        Name = x.Person.Name,
                        Picture = x.Person.Picture,
                        Character = x.Character,
                        Id = x.PersonId
                    }).ToList()
            };

            return model;
        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult) return NotFound();

            var movieDetailDto = movieActionResult.Value;
            var selectedGenresIds = movieDetailDto.Genres.Select(x => x.Id).ToList();
            var notSelectedGenres = await context.Genres.
                Where(x => !selectedGenresIds.Contains(x.Id)).ToListAsync();

            var model = new MovieUpdateDTO()
            {
                Movie = movieDetailDto.Movie,
                Actors = movieDetailDto.Actors,
                SelectedGenres = movieDetailDto.Genres,
                NotSelectedGenres = notSelectedGenres
            };

            return model;
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Movie movie)
        {
            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var posterPicture = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(posterPicture, "jpg", containerName);
            }

            if(movie.MovieActors != null)
            {
                for (int i = 0; i < movie.MovieActors.Count; i++)
                {
                    movie.MovieActors[i].Order = i + 1;
                }
            }

            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        // POST api/<MoviesController>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Movie movie)
        {
            var movieDb = await context.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);
            if (movieDb == null) return NotFound();

            movieDb = mapper.Map(movie, movieDb);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var moviePoster = Convert.FromBase64String(movie.Poster);
                movieDb.Poster = await fileStorageService.EditFile(moviePoster,
                    "jpg", containerName, movieDb.Poster);
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id};delete from MoviesGenres where MovieId = {movie.Id};");

            if (movie.MovieActors != null)
            {
                for (int i = 0; i < movie.MovieActors.Count; i++)
                {
                    movie.MovieActors[i].Order = i + 1;
                }
            }

            movieDb.MovieActors = movie.MovieActors;
            movieDb.MovieGenres = movie.MovieGenres;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return NotFound();

            context.Remove(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
