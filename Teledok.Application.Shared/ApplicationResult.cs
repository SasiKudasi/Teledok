using Teledok.Domain.Shared;

namespace Teledok.Application.Shared
{
    public class ApplicationResult <T> 
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public ErrorModel Error { get; }

        private ApplicationResult(bool isSuccess, T value, string? errorMsg)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = new ErrorModel(ErrorType.ApplicationError, errorMsg);
        }


        private ApplicationResult(string? errorMsg)
        {
            IsSuccess = false;
            Value = default;
            Error = new ErrorModel(ErrorType.NotFound, errorMsg);
        }


        public static ApplicationResult<T> Success(T value) => new(true, value, null);
        public static ApplicationResult<T> Fail(string errorMsg) => new(false, default, errorMsg);
        public static ApplicationResult<T> NotFound(string errorMsg) => new(errorMsg);
        

    }
}
