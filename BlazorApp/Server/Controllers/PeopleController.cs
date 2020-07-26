using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorApp.Server.DB;
using BlazorApp.Server.Helpers;
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
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<GenresController> logger;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;
        private readonly string containerName = "people";

        public PeopleController(ApplicationDbContext context, 
            ILogger<GenresController> logger, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        // GET: api/<PeopleController>
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery]PaginationDTO paginationDTO)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);
            return await queryable.Paginate(paginationDTO).ToListAsync();
        }

        // GET api/<PeopleController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return NotFound();

            return person;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if(string.IsNullOrWhiteSpace(searchText))
            {
                return new List<Person>();
            }

            return await context.People.Where(x => x.Name.Contains(searchText)).Take(5).ToListAsync();
        }

        // POST api/<PeopleController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Person person)
        {
            if(!string.IsNullOrEmpty(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await fileStorageService.SaveFile(personPicture, "jpg", containerName);
            }
            context.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }

        // PUT api/<PeopleController>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Person person)
        {
            var personDb = await context.People.FirstOrDefaultAsync(x => x.Id == person.Id);
            if (personDb == null) return NotFound();

            personDb = mapper.Map(person, personDb);

            if(!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDb.Picture = await fileStorageService.EditFile(personPicture, 
                    "jpg", containerName, personDb.Picture);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<PeopleController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return NotFound();

            context.Remove(person);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
