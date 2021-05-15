namespace LibraryBack
{
    /// <summary>
    /// User account. Can take and return books, also can view catalogue.
    /// </summary>
    public class UserAccount : Account
    {
        protected internal override event AccountEventDelegate Created;  // account creation event
        
        protected internal override event AccountEventDelegate Deleted;  // account deletion event
        
        protected internal override event AccountEventDelegate LoggedIn;  // log in event
        
        protected internal override event AccountEventDelegate LoggedOut;  // log out event

        public UserAccount(int userId, int amount) : base(userId, amount) { }  // constructor, calling base constructor
        
        
        // methods for calling events
        public override void Create()
        {
            Created?.Invoke(this, new AccountEventArgs("You've successfully created user account, ID - " + Id, Id));
        }
        
        public override void Delete()
        {
            Deleted?.Invoke(this, new AccountEventArgs("You've successfully deleted user account, ID - " + Id, Id));
        }
        
        public override void LogIn()
        {
            LoggedIn?.Invoke(this, new AccountEventArgs("You logged in, user account, ID - " + Id, Id));
        }

        public override void LogOut()
        {
            LoggedOut?.Invoke(this, new AccountEventArgs("You logged off, user account, ID - " + Id, Id));
        }
    }
}