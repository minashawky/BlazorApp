using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Server.DB;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<GenresController> logger;

        public PeopleController(ApplicationDbContext context, ILogger<GenresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        // GET: api/<PeopleController>
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            return context.People.ToList();
        }

        // GET api/<PeopleController>/5
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = context.People.FirstOrDefault(x => x.Id == id);
            if (person == null) return NotFound();

            return person;
        }

        // POST api/<PeopleController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Person person)
        {
            context.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }
    }
}
