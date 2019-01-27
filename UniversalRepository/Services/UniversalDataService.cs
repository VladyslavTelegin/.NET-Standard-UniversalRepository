namespace UniversalRepository.Services
{
    using AutoMapper;

    using Dapper.Contrib.Extensions;

    using Microsoft.Extensions.Caching.Memory;

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

    public class UniversalDataService<TDomain, TDto> : RepositoryCachedBase, IUniversalDataService<TDomain>
       where TDomain : class, IUniversalDomainObject
       where TDto : class, IUniversalDataTransferObject<TDomain>
    {
        #region Constuctor

        public UniversalDataService(IMemoryCache memoryCache,
                                    ConnectionConfig connectionConfig) 
            : base(memoryCache,
                   connectionConfig, 
                   typeof(TDomain).FullName) { }

        #endregion

        #region PublicMethods

        #region Default CRUD

        public async Task<UniversalRepositoryResult> CreateAsync(TDomain modelToCreate)
        {
            try
            {
                using (IDbConnection dbContext = base.Connection)
                {
                    var mappedDto = Mapper.Map<TDto>(modelToCreate);

                    var affectedRows = await dbContext.InsertAsync(mappedDto);
                    if (affectedRows > 0)
                    {
                        base.Cache.Remove(base.CacheKey);
                        return UniversalRepositoryResult.Success();
                    }
                    else
                    {
                        var errorMessage = $"Cannot create row.)";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalDataService<TDomain, TDto>)}.{nameof(this.CreateAsync)} -> {ex.Message};";

                Logger.Instance.CreateLog(logMessage);

                return UniversalRepositoryResult.Fail(ex);
            }
        }

        public async Task<UniversalRepositoryResult<TDomain>> GetAsync(int id)
        {
            try
            {
                using (IDbConnection dbContext = base.Connection)
                {
                    var allItems = await dbContext.GetAsync<TDto>(id);
                    if (allItems != null)
                    {
                        var mappedDomainObject = Mapper.Map<TDomain>(allItems);
                        return UniversalRepositoryResult<TDomain>.Success(mappedDomainObject);
                    }
                    else
                    {
                        var errorMessage = $"Cannot get row.)";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalDataService<TDomain, TDto>)}.{nameof(this.GetAsync)} -> {ex.Message};";

                Logger.Instance.CreateLog(logMessage);

                return UniversalRepositoryResult<TDomain>.Fail(ex);
            }
        }

        public async Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync()
        {
            IEnumerable<TDomain> cachedResult = null;

            try
            {
                if (!base.Cache.TryGetValue(base.CacheKey, out cachedResult))
                {
                    using (IDbConnection dbContext = base.Connection)
                    {
                        var allItems = await dbContext.GetAllAsync<TDto>();
                        if (allItems != null)
                        {
                            var mappedDomainObjects = Mapper.Map<IEnumerable<TDomain>>(allItems);
                            base.Cache.Set(base.CacheKey, mappedDomainObjects);
                        }
                        else
                        {
                            var errorMessage = $"Cannot get all rows.)";
                            throw new UniversalRepositoryException(errorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalDataService<TDomain, TDto>)}.{nameof(this.GetAllAsync)} -> {ex.Message};";

                Logger.Instance.CreateLog(logMessage);

                return UniversalRepositoryResult<IEnumerable<TDomain>>.Fail(ex);
            }

            cachedResult = base.Cache.Get<IEnumerable<TDomain>>(base.CacheKey);

            return UniversalRepositoryResult<IEnumerable<TDomain>>.Success(cachedResult);
        }

        public async Task<UniversalRepositoryResult> UpdateAsync(TDomain modelToUpdate)
        {
            try
            {
                using (IDbConnection dbContext = base.Connection)
                {
                    var dataTransferObject = Mapper.Map<TDto>(modelToUpdate);

                    var userUpdatingResult = await dbContext.UpdateAsync(dataTransferObject);
                    if (userUpdatingResult)
                    {
                        base.Cache.Remove(base.CacheKey);
                        return UniversalRepositoryResult.Success();
                    }
                    else
                    {
                        var errorMessage = $"Cannot get update row.)";
                        throw new UniversalRepositoryException(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalDataService<TDomain, TDto>)}.{nameof(this.UpdateAsync)} -> {ex.Message};";

                Logger.Instance.CreateLog(logMessage);

                return UniversalRepositoryResult.Fail(ex);
            }
        }

        public async Task<UniversalRepositoryResult> DeleteAsync(int id)
        {
            try
            {
                using (IDbConnection dbContext = base.Connection)
                {
                    var itemEntry = await this.GetAsync(id);
                    if (itemEntry.IsSuccess)
                    {
                        var dataTransferObject = Mapper.Map<TDto>(itemEntry.Result);

                        var deletionResult = await dbContext.DeleteAsync(dataTransferObject);
                        if (deletionResult)
                        {
                            base.Cache.Remove(base.CacheKey);
                            return UniversalRepositoryResult.Success();
                        }
                        else
                        {
                            var errorMessage = $"Cannot delete row.)";
                            throw new UniversalRepositoryException(errorMessage);
                        }
                    }
                    else
                    {
                        throw new UniversalRepositoryException(itemEntry.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = $"{DateTime.UtcNow}: {nameof(UniversalDataService<TDomain, TDto>)}.{nameof(this.CreateAsync)} -> {ex.Message};";

                Logger.Instance.CreateLog(logMessage);

                return UniversalRepositoryResult.Fail(ex);
            }
        }

        #endregion

        #region Customizable CRUD

        public Task<UniversalRepositoryResult> CreateAsync(string customQuery)
        {
            throw new NotImplementedException();
        }

        public Task<UniversalRepositoryResult<TDomain>> GetAsync(string customQuery)
        {
            throw new NotImplementedException();
        }

        public Task<UniversalRepositoryResult<IEnumerable<TDomain>>> GetAllAsync(string customQuery)
        {
            throw new NotImplementedException();
        }

        public Task<UniversalRepositoryResult> UpdateAsync(string customQuery)
        {
            throw new NotImplementedException();
        }

        public Task<UniversalRepositoryResult> DeleteAsync(string customQuery)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}