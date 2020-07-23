﻿using BlazorApp.Client.Helpers;
using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IHttpService httpService;
        private string url = "api/genres";

        public GenreRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        public async Task CreateGenre(Genre genre)
        {
            var response = await httpService.Post(url, genre);
            if(!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<List<Genre>> GetGenres()
        {
            var response = await httpService.Get<List<Genre>>(url);
            if(!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            
            return response.Response;
        }
    }
}
