namespace Bank
{
    public class Response<T>
    {
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? Payload { get; set; }

    }
}
