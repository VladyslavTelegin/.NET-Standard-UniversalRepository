namespace UniversalRepository.ServiceDefinitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UniversalRepository.Models;

    public interface IUniversalDataService<TDomain>
    {
        #region Default CRUD
        
        #region Asynchronus

        Task<UniversalRepositoryResult> CreateAsync(TDomain modelToCreate);

        Task<UniversalRepositoryResult<TDomain>> GetAsync(int id);

        Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync();

        Task<UniversalRepositoryResult> UpdateAsync(TDomain modelToUpdate);

        Task<UniversalRepositoryResult> DeleteAsync(int id);

        #endregion

        #region Synchronus

        UniversalRepositoryResult<IEnumerable<TDomain>> GetAll();

        #endregion

        #endregion

        #region Customizable CRUD

        #region Asynchronus

        Task<UniversalRepositoryResult> CreateAsync(string customQuery);
       
        Task<UniversalRepositoryResult<TDomain>> GetAsync(string customQuery);

        Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync(string customQuery);

        Task<UniversalRepositoryResult> UpdateAsync(string customQuery);
    
        Task<UniversalRepositoryResult> DeleteAsync(string customQuery);

        #endregion

        #region Synchronus

        UniversalRepositoryResult<IEnumerable<TDomain>> GetAll(string customQuery);

        #endregion

        #endregion
    }
}