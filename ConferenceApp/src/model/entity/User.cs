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
		public string UserName { get; set; }
		public string Password { get; set; }

		public BindingList<Role> Roles { get; set; }

		public User()
		{
			Roles = new BindingList<Role>();
		}

		public User(User user) : base()
		{
			Id = user.Id;
			FirstName = user.FirstName;
			LastName = user.LastName;
			Email = user.Email;
			UserName = user.UserName;
			Password = user.Password;
			if (user.Roles != null)
				Roles = new BindingList<Role>(user.Roles.ToList());
		}

	}
}
