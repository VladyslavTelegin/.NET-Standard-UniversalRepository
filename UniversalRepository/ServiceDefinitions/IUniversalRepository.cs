namespace UniversalRepository.ServiceDefinitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUniversalRepository<TDomain>
    {
        #region Default CRUD
        
        #region Asynchronus

        Task CreateAsync(TDomain item);
        Task<TDomain> GetAsync(int id);
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task UpdateAsync(TDomain item);
        Task DeleteAsync(int id);

        #endregion

        #endregion
    }
}