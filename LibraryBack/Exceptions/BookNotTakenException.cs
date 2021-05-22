using System;

namespace LibraryBack.Exceptions
{
    public class BookNotTakenException: Exception
    {
        public BookNotTakenException()
        {
            
        }

        public BookNotTakenException(string message) : base(message)
        {
            
        }
        
    }
}