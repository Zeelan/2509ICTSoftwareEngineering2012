using System.Windows;
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

            Messenger.Default.Register<NotificationMessage>(this, "Error", m => NotificationMessage(m));
        }

        private void NotificationMessage(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "ErrorPhoneNumber":
                    MessageBoxResult result = MessageBox.Show("Incorrect format, phone number must be 10 digit whole number.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
            }
        }
    }
}