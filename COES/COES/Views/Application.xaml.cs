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

            Messenger.Default.Register<NotificationMessage>(this, m => Navigate(m));
        }

        private void Navigate(NotificationMessage msg)
        {
            
        }
    }
}