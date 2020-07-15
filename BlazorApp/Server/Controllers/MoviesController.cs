using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IRepository repo;
        private readonly ILogger<GenresController> logger;

        public MoviesController(IRepository repo, ILogger<GenresController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            return repo.GetMovies();
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public ActionResult<Movie> Get(int id)
        {
            var movie = repo.GetMovieById(id);
            if (movie == null) return NotFound();

            return movie;
        }

        // POST api/<MoviesController>
        [HttpPost]
        public ActionResult Post([FromBody] Movie movie)
        {
            repo.AddMovie(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movie);
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Movie movie)
        {
            return NoContent();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
