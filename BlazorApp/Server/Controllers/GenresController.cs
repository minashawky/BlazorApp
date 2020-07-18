using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Server.DB;
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
        private readonly ApplicationDbContext context;
        private readonly ILogger<GenresController> logger;

        public GenresController(ApplicationDbContext context, ILogger<GenresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        // GET: api/<GenresController>
        [HttpGet]
        public ActionResult<List<Genre>> Get()
        {
            return context.Genres.ToList();
        }

        // GET api/<GenresController>/5
        [HttpGet("{id}", Name = "getGenre")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Id == id);
            if (genre == null) return NotFound();

            return genre;
        }

        // POST api/<GenresController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
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
