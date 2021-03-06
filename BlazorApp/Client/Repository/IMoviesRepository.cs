﻿using BlazorApp.Shared;
using BlazorApp.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Repository
{
    public interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<MovieDetailsDTO> GetMovieDetailsDTO(int id);
        Task<MovieUpdateDTO> GetMovieUpdateDTO(int id);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(int id);
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
    }
}
