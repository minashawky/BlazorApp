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
        private List<Movie> movies;
        public InMemoryRepository()
        {
            genres = new List<Genre>()
            {
                new Genre () { Id=1, Name="Action"},
                new Genre() {Id=2, Name="Comedy"}
            };

            movies = new List<Movie>()
            {
                new Movie(){
                    Id=1,
                    Title = "Spider-Man: Far From Home",
                    ReleaseDate = new DateTime(2019, 7, 2),
                    Poster = "https://m.media-amazon.com/images/M/MV5BMGZlNTY1ZWUtYTMzNC00ZjUyLWE0MjQtMTMxN2E3ODYxMWVmXkEyXkFqcGdeQXVyMDM2NDM2MQ@@._V1_SY1000_CR0,0,674,1000_AL_.jpg"
                },
                new Movie(){
                    Id=2,
                    Title = "Moana",
                    ReleaseDate = new DateTime(2016, 11, 23),
                    Poster = "https://m.media-amazon.com/images/M/MV5BMjI4MzU5NTExNF5BMl5BanBnXkFtZTgwNzY1MTEwMDI@._V1_SY1000_CR0,0,674,1000_AL_.jpg"
                },
                new Movie(){
                    Id=3,
                    Title = "Inception",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Poster = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_SY1000_CR0,0,675,1000_AL_.jpg"
                }
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

        public List<Movie> GetMovies()
        {
            return movies;
        }

        public Movie GetMovieById(int id)
        {
            return movies.FirstOrDefault(x => x.Id == id);
        }

        public void AddMovie(Movie movie)
        {
            movie.Id = movies.Max(x => x.Id) + 1;
            movies.Add(movie);
        }
    }
}
