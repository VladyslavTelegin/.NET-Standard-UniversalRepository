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

        #region PublicMethods

        public static void ChangeConnection(string connectionString, bool invalidateCache = true)
            => ChangeConnection(new ConnectionConfig(connectionString), invalidateCache);

        public static void ChangeConnection(ConnectionConfig connectionConfig, bool invalidateCache = true)
        {
            ConnectionConfig = connectionConfig;

            if (invalidateCache)
            {
                Cache.Dispose();
                Cache = new MemoryCacheWrapper(new MemoryCacheOptions());
            }
        }

        #endregion
    }
}