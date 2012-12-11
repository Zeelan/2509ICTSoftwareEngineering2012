using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;

namespace COES.Views
{
    /// <summary>
    /// Description for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, "Error", m => ErrorPopup(m));
        }

        private void ErrorPopup(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                    
                case "ErrorPhoneNumber":
                    MessageBox.Show("Incorrect format, phone number must be 10 digit whole number.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorFirstName":
                    MessageBox.Show("Customer must have a first name.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorLastName":
                    MessageBox.Show("Customer must have a last name.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorAddressNumber":
                    MessageBox.Show("Street number must be a whole number.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorAddressStreet":
                    MessageBox.Show("Customer must have a street address.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorAddressSuburb":
                    MessageBox.Show("Suburb cannot be blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorAddressPostCode":
                    MessageBox.Show("Post code cannot be blank and must be a whole number.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorOrder":
                    MessageBox.Show("Order must have at least 1 item.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case "ErrorMenuItemAlreadyExists":
                    MessageBox.Show("Item already exists in menu", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
            }
        }
    }
}