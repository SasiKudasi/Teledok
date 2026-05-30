namespace Teledok.Application.Shared
{
    public class ApplicationResult <T> 
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string? ErrorMessage { get; }

        private ApplicationResult(bool isSuccess, T value, string? errorMsg)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMsg;
        }

        public static ApplicationResult<T> Success(T value) => new(true, value, null);
        public static ApplicationResult<T> Error(string errorMsg) => new(false, default, errorMsg);

    }
}
