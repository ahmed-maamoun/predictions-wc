using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IPredictionRepository
    {
        Task<List<Prediction>> GetAllAsync();
        Task<Prediction> GetByIdAsync(int id);
        Task AddAsync(Prediction prediction);
        Task UpdateAsync(Prediction prediction);
        Task DeleteAsync(int id);
        Task<List<Prediction>> GetByMatchIdAsync(int matchId);
        Task<List<Prediction>> GetByPersonIdAsync(int personId);
    }
} 