namespace TP.Domain.Models.Result
{
    public partial class WorkResult<T>
    {
        public WorkResult(T? value)
        {
            Value = value;
        }

        public bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public T? Value { get; set; }
    }
}
