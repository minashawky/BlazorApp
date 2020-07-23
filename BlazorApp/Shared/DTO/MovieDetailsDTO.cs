using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.DTO
{
    public class MovieDetailsDTO
    {
        public Movie Movie { get; set; }
        public List<Person> Actors { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
