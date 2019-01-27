namespace UniversalRepository.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AppDomainExtensions
    {
        public static IEnumerable<Type> GetImplementations(this Assembly assembly, Type baseType)
        {
            if (!baseType.IsGenericType)
            {
                return assembly.GetTypes().Where(baseType.IsAssignableFrom)
                                          .Where(x => !x.IsAbstract && !x.IsInterface && x.IsPublic);
            }

            var candidates = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsPublic);

            var result = new List<Type>();

            foreach (var candidate in candidates)
            {
                var interfaces = candidate.GetInterfaces();
                if (interfaces.Length < 0)
                {
                    continue;
                }

                var target = interfaces.FirstOrDefault(_ => _.IsGenericType && _.GetGenericTypeDefinition() == baseType);
                if (target == null)
                {
                    continue;
                }

                result.Add(candidate);
            }

            return result;
        }
    }
}