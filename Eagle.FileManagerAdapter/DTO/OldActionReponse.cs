namespace Eagle.FileManagerAdapter
{
    public interface IOldActionResponse<T>
    {
        bool IsSuccessfull { get; set; }
        string Message { get; set; }
        T Result { get; set; }
    }

    public class OldActionResponse<T> : IOldActionResponse<T>
    {
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
