namespace UniversalRepository.Models
{
    using System;

    public class UniversalRepositoryResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static UniversalRepositoryResult Success() => new UniversalRepositoryResult() { IsSuccess = true };
        public static UniversalRepositoryResult Fail(Exception ex) => new UniversalRepositoryResult() { ErrorMessage = ex.Message };
    }

    public class UniversalRepositoryResult<T> : UniversalRepositoryResult
    {
        public T Result { get; set; }

        public static UniversalRepositoryResult<T> Success(T result) => new UniversalRepositoryResult<T>() { IsSuccess = true, Result = result };
        public new static UniversalRepositoryResult<T> Fail(Exception ex) => new UniversalRepositoryResult<T>() { ErrorMessage = ex.Message };
    }
}