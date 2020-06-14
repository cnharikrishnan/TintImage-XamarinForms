using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TintImageDemo.Control;
using TintImageDemo.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TintImage), typeof(TintImageRenderer))]
namespace TintImageDemo.Droid.Renderer
{
    public class TintImageRenderer : ImageRenderer
    {
        public TintImageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && this.Control != null)
                this.UpdateTintColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "TintColor" || e.PropertyName == "Source" || e.PropertyName == "IsLoading")
                this.UpdateTintColor();
        }

        private void UpdateTintColor()
        {
            if (this.Control == null || this.Element == null)
                return;

            var tintImage = this.Element as TintImage;
            if (tintImage == null)
                return;

            if (tintImage.TintColor == Xamarin.Forms.Color.Transparent)
            {
                //Cancelling the applied tint.
                if (Control.ColorFilter != null)
                    Control.ClearColorFilter();
            }
            else
            {
                //Applying tint color
                var colorFilter = new PorterDuffColorFilter(tintImage.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                Control.SetColorFilter(tintImage.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
        }
    }
}