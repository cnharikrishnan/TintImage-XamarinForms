using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using TintImageDemo.Control;
using TintImageDemo.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TintImage), typeof(TintImageRenderer))]
namespace TintImageDemo.iOS.Renderer
{
    public class TintImageRenderer : ImageRenderer
    {
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
            if (Control?.Image == null || Element == null)
                return;

            var tintImage = Element as TintImage;
            if (tintImage == null)
                return;

            if (tintImage.TintColor == Color.Transparent)
            {
                //Cancelling the applied tint.
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);
                Control.TintColor = null;
            }
            else
            {
                //Applying tint color
                Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = tintImage.TintColor.ToUIColor();
            }
        }
    }
}