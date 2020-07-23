using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared.DTO
{
    public class IndexPageDTO
    {
        public List<Movie> MoviesInTheatre { get; set; }
        public List<Movie> UpcomingReleases { get; set; }
    }
}
