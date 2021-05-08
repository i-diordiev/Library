namespace LibraryBack
{
    public delegate void StorageStateHandler(object sender, StorageEventArgs args);
    
    public class StorageEventArgs
    {
        public string Message { get; private set; }
        
        public int BookID { get; private set; }

        public StorageEventArgs(string message, int bookid)
        {
            Message = message;
            BookID = bookid;
        }
    }
}