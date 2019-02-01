namespace UniversalRepository.CoreExtensions
{
    using AutoMapper;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UniversalRepository.Interfaces;
    using UniversalRepository.Models;
    using UniversalRepository.ServiceDefinitions;
    using UniversalRepository.Services;

    public static class CoreExtensions
    {
        #region PublicMethods

        public static void AddUniversalRepository(this IServiceCollection serviceCollection,
                                                  Assembly dataTransferObjectsContainerAssembly,
                                                  ConnectionConfig connectionConfig, 
                                                  IEnumerable<Profile> mappingProfilesList = null,
                                                  bool isTransientScoped = false,
                                                  bool isCachingEnabled = false,
                                                  IOptions<MemoryCacheOptions> memoryCacheOptions = null)
        {
            AddAutoMapper(mappingProfilesList);

            var dtoInterfaceType = typeof(IUniversalDataTransferObject<>);

            var dtos = dataTransferObjectsContainerAssembly.GetImplementations(dtoInterfaceType);

            foreach (var dtoType in dtos)
            {
                var domainType = dtoType.GetInterfaces().First(_ => _.IsGenericType && _.GetGenericTypeDefinition() == dtoInterfaceType)
                                                        .GetGenericArguments()[0];

                var universalRepository = typeof(IUniversalDataService<>);
                var implementation = typeof(UniversalRepository<,>);

                var universalRepositoryGeneric = universalRepository.MakeGenericType(domainType);
                var implementationGeneric = implementation.MakeGenericType(domainType, dtoType);

                var serviceConstructorParams = new object[]
                {
                    isCachingEnabled,
                    memoryCacheOptions ?? new MemoryCacheOptions(),
                    connectionConfig,
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
                                                  bool isTransientScoped = false,
                                                  bool isCachingEnabled = false,
                                                  IOptions<MemoryCacheOptions> memoryCacheOptions = null)
        {
            serviceCollection.AddUniversalRepository(dataTransferObjectsContainerAssembly,
                                                     new ConnectionConfig(connectionString),
                                                     mappingProfilesList,
                                                     isTransientScoped,
                                                     isCachingEnabled,
                                                     memoryCacheOptions);
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