using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Server.Services
{
    public interface IRepository
    {
        void AddGenre(Genre genre);
        List<Genre> GetAllGenres();
        Genre GetGenreById(int id);
    }
}
