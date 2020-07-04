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
    public class GenresController : ControllerBase
    {
        private readonly IRepository repo;
        private readonly ILogger<GenresController> logger;

        public GenresController(IRepository repo, ILogger<GenresController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }
        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await repo.GetAllGenres();
        }

        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = repo.GetGenreById(id);
            if (genre == null) return NotFound();

            return genre;
        }

        // POST api/<GenresController>
        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            return NoContent();
        }

        // PUT api/<GenresController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Genre genre)
        {
            return NoContent();
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
