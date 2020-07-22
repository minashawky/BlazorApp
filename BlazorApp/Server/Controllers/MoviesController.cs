using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Server.DB;
using BlazorApp.Server.Helpers;
using BlazorApp.Server.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
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

        public MoviesController(ApplicationDbContext context, ILogger<GenresController> logger, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.logger = logger;
            this.fileStorageService = fileStorageService;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            return context.Movies.ToList();
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public ActionResult<Movie> Get(int id)
        {
            var movie = context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) return NotFound();

            return movie;
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Movie movie)
        {
            if (!string.IsNullOrEmpty(movie.Poster))
            {
                var posterPicture = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(posterPicture, "jpg", "movies");
            }
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }
    }
}
