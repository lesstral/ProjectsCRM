using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Enums;

namespace ApplicationCore
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }
        public T? Value { get; set; }

        public static Result<T> Success(T value) =>
            new Result<T> { IsSuccess = true, Value = value };

        public static Result<T> Failure(string? error, ErrorType errorType) =>
            new Result<T>
            {
                IsSuccess = false,
                ErrorMessage = error,
                ErrorType = errorType,
            };
    }

    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }

        public static Result Success() => new Result { IsSuccess = true };

        public static Result Failure(string? error, ErrorType errorType) =>
            new Result
            {
                IsSuccess = false,
                ErrorMessage = error,
                ErrorType = errorType,
            };
    }
}
