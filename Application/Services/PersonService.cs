using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;

namespace Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        public PersonService(IPersonRepository repo) { _repo = repo; }
        public Task<List<Person>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Person> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Person person) => _repo.AddAsync(person);
        public Task UpdateAsync(Person person) => _repo.UpdateAsync(person);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
} 