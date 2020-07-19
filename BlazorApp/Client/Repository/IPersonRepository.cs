using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Repository
{
    interface IPersonRepository
    {
        Task CreatePerson(Person person);
    }
}
