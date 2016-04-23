using System;
using System.Collections.Generic;
using System.Text;

namespace MyMedicare
{
    class UserDetails
    {
        public List<User> Users { get; set; }
        private static UserDetails instance;

        private UserDetails()
        {
            Users = new List<User>();
        }

        public static UserDetails GetInstance()
        {
            if(instance == null)
                instance = new UserDetails();
            return instance;
        }

        public User AddUser(User u)
        {
            Users.Add(u);
            return u;
        }

        public void RemoveUser(User u)
        {
            Users.Remove(u);
        }
    }
}
