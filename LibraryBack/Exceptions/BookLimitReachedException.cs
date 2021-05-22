using System;

namespace LibraryBack.Exceptions
{
    public class BookLimitReachedException : Exception
    {
        public BookLimitReachedException()
        {
            
        }

        public BookLimitReachedException(string message) : base(message)
        {
            
        }
    }
}