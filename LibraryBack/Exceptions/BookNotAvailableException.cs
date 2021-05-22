using System;

namespace LibraryBack.Exceptions
{
    public class BookNotAvailableException : Exception
    {
        public BookNotAvailableException()
        {
            
        }

        public BookNotAvailableException(string message) : base(message)
        {
            
        }
    }
}