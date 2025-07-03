using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
    }
} 