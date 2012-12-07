using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace COES.Helpers
{
    public static class NumericTextHelper
    {
        /// <summary>
        /// Checks whether the key pressed is a numeric value.
        /// </summary>
        /// <param name="e">The key pressed.</param>
        /// <returns>True if the key is a numeric value, else returns false.</returns>
        public static bool CheckNumericText(KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Back)
                return true;
            else
                return false;
        }
    }
}
