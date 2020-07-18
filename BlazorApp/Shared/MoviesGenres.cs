﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class MoviesGenres
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
