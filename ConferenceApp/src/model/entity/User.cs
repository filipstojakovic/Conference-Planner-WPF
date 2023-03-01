using System.ComponentModel;

namespace ConferenceApp.model.entity
{
	public class User
	{
		public int Id { get; set; }
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
	}
}
