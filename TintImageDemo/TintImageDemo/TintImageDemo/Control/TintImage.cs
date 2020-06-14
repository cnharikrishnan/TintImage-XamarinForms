using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TintImageDemo.Control
{
    public class TintImage : Image
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintImage), Color.Transparent);

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public static readonly BindableProperty HintProperty = BindableProperty.Create(nameof(Hint), typeof(string), typeof(TintImage), string.Empty);

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }
    }
}
