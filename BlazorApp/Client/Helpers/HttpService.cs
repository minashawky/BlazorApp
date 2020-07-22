﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Client.Helpers
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;
        private JsonSerializerOptions defaultJsonSerializerOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if(response.IsSuccessStatusCode)
            {
                var deserializedResponse = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(deserializedResponse, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }

        private async Task<T> Deserialize<T> (HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}
