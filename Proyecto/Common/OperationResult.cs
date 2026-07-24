using System.Collections.Generic;

namespace Proyecto.Common
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static OperationResult Ok() => new OperationResult { Success = true };
        public static OperationResult Fail(string error) => new OperationResult { Success = false, Errors = new List<string> { error } };
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
        public static OperationResult<T> Ok(T data) => new OperationResult<T> { Success = true, Data = data };
        public new static OperationResult<T> Fail(string error) => new OperationResult<T> { Success = false, Errors = new List<string> { error } };
    }
}