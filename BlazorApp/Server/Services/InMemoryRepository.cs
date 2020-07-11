using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Server.Services
{
    public class InMemoryRepository : IRepository
    {
        private List<Genre> genres;
        public InMemoryRepository()
        {
            genres = new List<Genre>()
            {
                new Genre () { Id=1, Name="Action"},
                new Genre() {Id=2, Name="Comedy"}
            };
        }
        public List<Genre> GetAllGenres()
        {
            return genres;
        }

        public Genre GetGenreById(int id)
        {
            return genres.FirstOrDefault(x => x.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            genre.Id = genres.Max(x => x.Id) + 1;
            genres.Add(genre);
        }
    }
}
