using System;

namespace LibraryBack.Exceptions
{
    public class BookAlreadyTakenException : Exception
    {
        public BookAlreadyTakenException()
        {
            
        }

        public BookAlreadyTakenException(string message) : base(message)
        {
            
        }
    }
}