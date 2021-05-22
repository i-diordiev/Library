using System;

namespace LibraryBack.Exceptions
{
    public class WrongIdException: Exception
    {
        public WrongIdException()
        {
            
        }

        public WrongIdException(string message) : base(message)
        {
            
        }
    }
}