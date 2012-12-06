using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace COES.Converters
{
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
