using System.Windows;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog
{
    public partial class UserDialog : Window
    {
        public User UserDialogData { get; set; }
        public bool Edit { get; set; }

        public UserDialog(User user = null, bool edit = true)
        {
            InitializeComponent();
            Edit = edit;
            this.UserDialogData = new User(user);
            DataContext = UserDialogData;
            Button.Content = edit ? "Edit" : "Create";
            // editBtn.Content = edit ? "Edit" : "Create";
            // editBtn.DataContext = this;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}