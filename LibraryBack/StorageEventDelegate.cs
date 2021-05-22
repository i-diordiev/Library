namespace LibraryBack
{
    /// <summary>
    /// used to bind front and back
    /// </summary>
    public delegate void StorageEventDelegate(object sender, StorageEventArgs args);
    
    public class StorageEventArgs
    {
        public string Message { get; private set; }
        
        public int BookId { get; private set; }

        public StorageEventArgs(string message, int bookid)
        {
            Message = message;
            BookId = bookid;
        }
    }
}