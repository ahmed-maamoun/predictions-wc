using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IPredictionService
    {
        Task<List<Prediction>> GetAllAsync();
        Task<Prediction> GetByIdAsync(int id);
        Task<bool> AddAsync(Prediction prediction);
        Task UpdateAsync(Prediction prediction);
        Task DeleteAsync(int id);
        Task<List<Prediction>> GetByMatchIdAsync(int matchId);
        Task<List<Prediction>> GetByPersonIdAsync(int personId);
    }
} 