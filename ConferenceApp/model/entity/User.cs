using System.ComponentModel;
using System.Linq;

namespace ConferenceApp.model.entity
{
    public class User
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public BindingList<Role> Roles { get; set; }

        public User()
        {
            Roles = new BindingList<Role>();
        }

        public User(User user)
        {
            copy(user);
        }

        public void copy(User user)
        {
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Username = user.Username;
            this.Password = user.Password;
            if (user.Roles != null)
                Roles = new BindingList<Role>(user.Roles.ToList());
        }
    }
}