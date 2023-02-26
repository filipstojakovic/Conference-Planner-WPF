using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceApp.view.usercontrol
{
    /// <summary>
    /// Interaction logic for UserControlProviders.xaml
    /// </summary>
    public partial class UserControlProviders : UserControl
    {
        public UserControlProviders()
        {
            InitializeComponent();
            List<User> data = new List<User>
            {
                new()
                {
                    name = "Raymond Banks",
                    email = "consectetuer.adipiscing.elit@hotmail.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Jonas Fletcher",
                    email = "aliquet@google.net",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Kirk Ratliff",
                    email = "justo.sit.amet@icloud.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Hilary Dickerson",
                    email = "cras.vulputate.velit@aol.com",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Jackson Cain",
                    email = "dictum.eu@protonmail.org",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Raymond Banks",
                    email = "consectetuer.adipiscing.elit@hotmail.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Jonas Fletcher",
                    email = "aliquet@google.net",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Kirk Ratliff",
                    email = "justo.sit.amet@icloud.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Hilary Dickerson",
                    email = "cras.vulputate.velit@aol.com",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Jackson Cain",
                    email = "dictum.eu@protonmail.org",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Raymond Banks",
                    email = "consectetuer.adipiscing.elit@hotmail.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Jonas Fletcher",
                    email = "aliquet@google.net",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Kirk Ratliff",
                    email = "justo.sit.amet@icloud.edu",
                    pic = @"..\..\..\resources\logo.jpg"
                },
                new()
                {
                    name = "Hilary Dickerson",
                    email = "cras.vulputate.velit@aol.com",
                    pic = @"..\..\..\resources\logo.jpg"
                }
            };
            usersList.ItemsSource = data;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersList.ItemsSource);
            view.Filter = UserFilter;
        }


        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;

            return (item as User).name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(usersList.ItemsSource).Refresh();
        }


        public class User
        {
            public string name { get; set; }

            public string email { get; set; }

            public string pic { get; set; }
        }
    }
}