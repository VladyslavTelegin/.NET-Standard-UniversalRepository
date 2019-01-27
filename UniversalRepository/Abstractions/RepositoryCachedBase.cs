namespace UniversalRepository.Abstractions
{
    using Microsoft.Extensions.Caching.Memory;
    using UniversalRepository.Models;

    public abstract class RepositoryCachedBase : RepositoryBase
    {
        #region PrivateFields

        private readonly string _currentGenericTypedCacheMixIn;

        #endregion

        #region Constructor

        protected RepositoryCachedBase(IMemoryCache memoryCache, 
                                       ConnectionConfig connectionConfig,
                                       string currentGenericTypedCacheMixIn) : base(connectionConfig)
        {
            _currentGenericTypedCacheMixIn = currentGenericTypedCacheMixIn;
            this.Cache = memoryCache;
        }

        #endregion

        #region ProtectedProperties

        protected readonly IMemoryCache Cache;

        protected string CacheKey => $"{this.GetType().Name}_{_currentGenericTypedCacheMixIn}_cache";

        #endregion
    } 
}