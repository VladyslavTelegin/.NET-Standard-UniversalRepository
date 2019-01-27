namespace UniversalRepository.Exceptions
{
    using System;

    public class UniversalRepositoryException : Exception
    {
        public UniversalRepositoryException() { }

        public UniversalRepositoryException(string errorMessage) : base(errorMessage) { }
    }
}