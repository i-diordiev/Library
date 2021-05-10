namespace LibraryBack
{
    public class UserAccount : Account
    {
        protected internal override event AccountEventDelegate Created;
        
        protected internal override event AccountEventDelegate Deleted;
        
        protected internal override event AccountEventDelegate LoggedIn;
        
        protected internal override event AccountEventDelegate LoggedOut;

        public UserAccount(int userId, int amount) : base(userId, amount) { }
        
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