using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.DTO
{
    public class PersonDetailsDTO
    {
        public Person Person { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
