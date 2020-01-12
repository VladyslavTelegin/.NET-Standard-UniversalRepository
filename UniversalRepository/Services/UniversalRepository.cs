namespace UniversalRepository.Services
{
    using AutoMapper;

    using Dapper.Contrib.Extensions;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using UniversalRepository.Abstractions;
    using UniversalRepository.Exceptions;
    using UniversalRepository.Infrastructure;
    using UniversalRepository.Interfaces;
    using UniversalRepository.Models;
    using UniversalRepository.ServiceDefinitions;

    public class UniversalRepository<TDomain, TDto> : UniversalRepositoryCachedBase, IUniversalRepository<TDomain>
       where TDomain : class, IUniversalDomainObject 
       where TDto : class, IUniversalDataTransferObject<TDomain>
    {
        #region Constructor

        public UniversalRepository(bool isCachingEnabled, 
                                   IOptions<MemoryCacheOptions> memoryCacheOptions,
                                   ConnectionConfig connectionConfig)  
            : base(isCachingEnabled, 
                   memoryCacheOptions,
                   connectionConfig, 
                   typeof(TDomain).FullName) { }

        #endregion

        #region PublicMethods

        #region Default CRUD

        #region Asynchronus

        public async Task CreateAsync(TDomain item)
        {
            try
            {
                using (IDbConnection dbContext = base.ResolveConnection<TDto>())
                {
                    var mappedDto = Mapper.Map<TDto>(item);

                    var entityIdentifier = await dbContext.InsertAsync(mappedDto);
                    if (entityIdentifier != default)
                    {
                        Cache.Remove(base.CacheKey);
                    }
                    else
                    {
                        var errorMessage = $"Cannot create row.";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalRepository<TDomain, TDto>)}.{nameof(this.CreateAsync)} -> {ex.Message};";
                Logger.Instance.CreateLog(logMessage);
                throw;
            }
        }

        public async Task<TDomain> GetAsync(int id)
        {
            try
            {
                using (IDbConnection dbContext = base.ResolveConnection<TDto>())
                {
                    var allItems = await dbContext.GetAsync<TDto>(id);
                    if (allItems != null)
                    {
                        var mappedDomainObject = Mapper.Map<TDomain>(allItems);
                        return mappedDomainObject;
                    }
                    else
                    {
                        var errorMessage = $"Cannot get row.";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalRepository<TDomain, TDto>)}.{nameof(this.GetAsync)} -> {ex.Message};";
                Logger.Instance.CreateLog(logMessage);

                return default;
            }
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            IEnumerable<TDomain> result = null;

            try
            {
                if (!Cache.TryGetValue(base.CacheKey, out result))
                {
                    using (IDbConnection dbContext = base.ResolveConnection<TDto>())
                    {
                        var allItems = await dbContext.GetAllAsync<TDto>().ConfigureAwait(false);
                        if (allItems != null)
                        {
                            var mappedDomainObjects = Mapper.Map<IEnumerable<TDomain>>(allItems);

                            if (Cache.IsEnabled)
                            {
                                Cache.Set(base.CacheKey, mappedDomainObjects);
                            }
                            else
                            {
                                result = mappedDomainObjects;
                            }
                        }
                        else
                        {
                            var errorMessage = $"Cannot get all rows.";
                            throw new UniversalRepositoryException(errorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalRepository<TDomain, TDto>)}.{nameof(this.GetAllAsync)} -> {ex.Message};";
                Logger.Instance.CreateLog(logMessage);

                throw;
            }

            result = Cache.IsEnabled ? Cache.Get<IEnumerable<TDomain>>(base.CacheKey) : result;
            return result;
        }

        public async Task UpdateAsync(TDomain item)
        {
            try
            {
                using (IDbConnection dbContext = base.ResolveConnection<TDto>())
                {
                    var dataTransferObject = Mapper.Map<TDto>(item);

                    var userUpdatingResult = await dbContext.UpdateAsync(dataTransferObject);
                    if (userUpdatingResult)
                    {
                        Cache.Remove(base.CacheKey);
                    }
                    else
                    {
                        var errorMessage = $"Cannot update row.";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalRepository<TDomain, TDto>)}.{nameof(this.UpdateAsync)} -> {ex.Message};";
                Logger.Instance.CreateLog(logMessage);

                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (IDbConnection dbContext = base.ResolveConnection<TDto>())
                {
                    var itemEntry = await this.GetAsync(id);
                    if (itemEntry == default)
                    {
                        var dataTransferObject = Mapper.Map<TDto>(itemEntry);

                        var deletionResult = await dbContext.DeleteAsync(dataTransferObject);
                        if (deletionResult)
                        {
                            Cache.Remove(base.CacheKey);
                        }
                        else
                        {
                            var errorMessage = $"Cannot delete row.";
                            throw new UniversalRepositoryException(errorMessage);
                        }
                    }
                    else
                    {
                        var errorMessage = $"Cannot get row for deletion.";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalRepository<TDomain, TDto>)}.{nameof(this.DeleteAsync)} -> {ex.Message};";
                Logger.Instance.CreateLog(logMessage);

                throw;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}