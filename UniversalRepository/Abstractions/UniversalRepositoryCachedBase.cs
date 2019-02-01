namespace UniversalRepository.Abstractions
{
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;

    using UniversalRepository.Models;
    using UniversalRepository.Wrappers;

    public abstract class UniversalRepositoryCachedBase : UniversalRepositoryBase
    {
        #region PrivateFields

        private readonly string _currentGenericTypedCacheMixIn;

        #endregion

        #region Constructor

        protected UniversalRepositoryCachedBase(bool isCachingEnabled,
                                                IOptions<MemoryCacheOptions> memoryCacheOptions,
                                                ConnectionConfig connectionConfig,
                                                string currentGenericTypedCacheMixIn) : base(connectionConfig)
        {
            _currentGenericTypedCacheMixIn = currentGenericTypedCacheMixIn;

            Cache = new MemoryCacheWrapper(memoryCacheOptions)
            {
                IsEnabled = isCachingEnabled
            };
        }

        #endregion

        #region ProtectedProperties

        protected static MemoryCacheWrapper Cache;

        protected string CacheKey => $"{this.GetType().Name}_{_currentGenericTypedCacheMixIn}_cache";

        #endregion
    }
}