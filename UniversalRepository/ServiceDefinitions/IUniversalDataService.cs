namespace UniversalRepository.ServiceDefinitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UniversalRepository.Models;

    public interface IUniversalDataService<TDomain>
    {
        #region Default CRUD

        Task<UniversalRepositoryResult> CreateAsync(TDomain modelToCreate);

        Task<UniversalRepositoryResult<TDomain>> GetAsync(int id);

        Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync();

        Task<UniversalRepositoryResult> UpdateAsync(TDomain modelToUpdate);

        Task<UniversalRepositoryResult> DeleteAsync(int id);

        #endregion

        #region Customizable CRUD

        Task<UniversalRepositoryResult> CreateAsync(string customQuery);
       
        Task<UniversalRepositoryResult<TDomain>> GetAsync(string customQuery);

        Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync(string customQuery);

        Task<UniversalRepositoryResult> UpdateAsync(string customQuery);
    
        Task<UniversalRepositoryResult> DeleteAsync(string customQuery);

        #endregion
    }
}