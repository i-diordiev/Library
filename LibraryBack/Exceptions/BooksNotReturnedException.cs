using System;

namespace LibraryBack.Exceptions
{
    public class BooksNotReturnedException : Exception
    {
        public BooksNotReturnedException()
        {
            
        }

        public BooksNotReturnedException(string message) : base(message)
        {
            
        }
        
    }
}