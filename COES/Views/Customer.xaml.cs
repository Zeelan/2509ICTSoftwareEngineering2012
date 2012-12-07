using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using COES.Helpers;

namespace COES.Views
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        public Customer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles text input so that only whole numbers (0-9) and backspace can be entered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The key pressed.</param>
        private void NumericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = NumericTextHelper.CheckNumericText(e);
        }

    }
}
