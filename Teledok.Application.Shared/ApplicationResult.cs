using Teledok.Domain.Shared;

namespace Teledok.Application.Shared
{
    public class ApplicationResult <T> 
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public ErrorModel Fail { get; }

        private ApplicationResult(bool isSuccess, T value, string? errorMsg)
        {
            IsSuccess = isSuccess;
            Value = value;
            Fail = new ErrorModel(ErrorType.ApplicationError, errorMsg);
        }

        public static ApplicationResult<T> Success(T value) => new(true, value, null);
        public static ApplicationResult<T> Fail(string errorMsg) => new(false, default, errorMsg);

    }
}
