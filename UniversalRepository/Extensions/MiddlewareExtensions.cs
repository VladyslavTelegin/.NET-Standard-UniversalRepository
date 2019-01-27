namespace UniversalRepository.Extensions
{
    using AutoMapper;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UniversalRepository.Interfaces;
    using UniversalRepository.Models;
    using UniversalRepository.ServiceDefinitions;
    using UniversalRepository.Services;

    public static class MiddlewareExtensions
    {
        #region PrivateFields

        private static readonly IMemoryCache _memoryCache;

        #endregion

        #region Constructor

        static MiddlewareExtensions()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        #endregion

        #region PublicMethods

        public static void AddUniversalRepository(this IServiceCollection serviceCollection,
                                                  Assembly dataTransferObjectsContainerAssembly,
                                                  ConnectionConfig connectionConfig, 
                                                  IEnumerable<Profile> mappingProfilesList = null,
                                                  bool isTransientScoped = false)
        {
            AddAutoMapper(mappingProfilesList);

            var dtoInterfaceType = typeof(IUniversalDataTransferObject<>);

            var dtos = dataTransferObjectsContainerAssembly.GetImplementations(dtoInterfaceType);

            foreach (var dtoType in dtos)
            {
                var domainType = dtoType.GetInterfaces().First(_ => _.IsGenericType && _.GetGenericTypeDefinition() == dtoInterfaceType)
                                                        .GetGenericArguments()[0];

                var universalRepository = typeof(IUniversalDataService<>);
                var implementation = typeof(UniversalDataService<,>);

                var universalRepositoryGeneric = universalRepository.MakeGenericType(domainType);
                var implementationGeneric = implementation.MakeGenericType(domainType, dtoType);

                var serviceConstructorParams = new object[]
                {
                    _memoryCache,
                    connectionConfig
                };

                object serviceImplementationFactoryFunc
                    (IServiceProvider _) => Activator.CreateInstance(implementationGeneric, serviceConstructorParams);

                if (isTransientScoped)
                {
                    serviceCollection.AddTransient(universalRepositoryGeneric, serviceImplementationFactoryFunc);
                }
                else
                {
                    serviceCollection.AddScoped(universalRepositoryGeneric, serviceImplementationFactoryFunc);
                }
            }
        }

        public static void AddUniversalRepository(this IServiceCollection serviceCollection,
                                                  Assembly dataTransferObjectsContainerAssembly,
                                                  string connectionString,
                                                  IEnumerable<Profile> mappingProfilesList = null,
                                                  bool isTransientScoped = false)
        {
            serviceCollection.AddUniversalRepository(dataTransferObjectsContainerAssembly,
                                                     new ConnectionConfig(connectionString),
                                                     mappingProfilesList,
                                                     isTransientScoped);
        }

        #endregion

        #region PrivateMethods

        private static void AddAutoMapper(IEnumerable<Profile> mappingProfilesList)
        {
            Mapper.Initialize(_ => mappingProfilesList?.ToList().ForEach(__ => _.AddProfile(__)));

            Mapper.Configuration.AssertConfigurationIsValid();
        }

        #endregion
    }
}