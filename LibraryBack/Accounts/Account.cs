namespace LibraryBack.Accounts
{
    /// <summary>
    /// Registered account. Can only log in/out.
    /// </summary>
    public abstract class Account : IAccount
    {
        protected internal virtual event AccountEventDelegate LoggedIn;  // event, logged in 
        
        protected internal virtual event AccountEventDelegate LoggedOut;  // event, logged out
        
        public int Id { get; private set; }  // personal ID of account

        public Account(int id)
        {
            Id = id;
        }  // constructor 

        // methods for calling events

        public virtual void LogIn()
        {
            LoggedIn?.Invoke(this, new AccountEventArgs("You've logged in account, ID: " + Id, Id));
        }

        public virtual void LogOut()
        {
            LoggedOut?.Invoke(this, new AccountEventArgs("You've logged off account, ID: " + Id, Id));
        }
    }
}