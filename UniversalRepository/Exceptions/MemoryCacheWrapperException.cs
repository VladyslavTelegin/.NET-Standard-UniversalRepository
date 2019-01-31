namespace UniversalRepository.Exceptions
{
    using System;

    public class MemoryCacheWrapperException : Exception
    {
        public MemoryCacheWrapperException() : this("Cache is disabled by user.") { }

        public MemoryCacheWrapperException(string errorMessage) : base(errorMessage) { }
    }
}