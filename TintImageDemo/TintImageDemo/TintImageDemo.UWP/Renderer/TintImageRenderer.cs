using CompositionProToolkit;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TintImageDemo.Control;
using TintImageDemo.UWP.Renderer;
using Native = Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.IO;

[assembly: ExportRenderer(typeof(TintImage), typeof(TintImageRenderer))]
namespace TintImageDemo.UWP.Renderer
{
    public class TintImageRenderer : ImageRenderer
    {
        private Compositor tintImageCompositor; 
        private CompositionEffectBrush tintCompositionEffectBrush;
        private CompositionSurfaceBrush tintCompositionSurfaceBrush;
        private ICompositionGenerator compositionGenerator;
        private IImageSurface imageSurface;
        private SpriteVisual spriteVisual;
        private TintImage tintImage;


        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.tintImage = e.NewElement as TintImage;
                this.GetCompositorAndCreateCompositionGenerator();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.tintImage == null)
                return;

            try
            {
                if (e.PropertyName == TintImage.TintColorProperty.PropertyName)
                {
                    if (this.tintImage.TintColor == Color.Transparent)
                    {
                        //Cancelling the applied tint.
                        this.tintCompositionEffectBrush = null;
                        this.UpdateSpriteVisualBrushAndElementChildVisual(this.tintCompositionSurfaceBrush);
                    }
                    else
                    {
                        this.ConfigureFilePathAndCreateSpriteVisualAndTintCompositeEffectBrush();
                        //Applying tint color
                        UpdateTintColor(this.tintImage.TintColor.ToWindowsColor());
                        return;
                    }
                }

                if (e.PropertyName == Image.SourceProperty.PropertyName)
                    this.ConfigureFilePathAndCreateSpriteVisualAndTintCompositeEffectBrush();
            }
            catch (Exception ex)
            {

            }
        }

        protected override Native.Size ArrangeOverride(Native.Size finalSize)
        {
            var arrangeSize = base.ArrangeOverride(finalSize);
            if(arrangeSize.Height != 0 && arrangeSize.Width != 0)
            {
                this.ConfigureFilePathAndCreateSpriteVisualAndTintCompositeEffectBrush();
                if (this.spriteVisual != null && this.imageSurface != null)
                {
                    //To refresh the native tint effect the SpriteVisual and ImageSurface are resized here
                    this.spriteVisual.Size = new Vector2((float)Element.Width, (float)Element.Height);
                    this.imageSurface.Resize(new Native.Size(Element.Width, Element.Height));
                }
            }
            return arrangeSize;
        }

        private void GetCompositorAndCreateCompositionGenerator()
        {
            if (this.tintImageCompositor != null && this.compositionGenerator != null)
                return;

            this.tintImageCompositor = ElementCompositionPreview.GetElementVisual(Control).Compositor;
            this.compositionGenerator = this.tintImageCompositor.CreateCompositionGenerator();
        }

        private void UpdateTintColor(Windows.UI.Color color)
        {
            if (this.tintCompositionEffectBrush == null)
                return;

            //Updating the tint color of the native view.
            this.tintCompositionEffectBrush.Properties.InsertColor("colorSource.Color", color);
        }

        private async void ConfigureFilePathAndCreateSpriteVisualAndTintCompositeEffectBrush()
        {
            //Skip when tint composition effect brush is already created when applying a tint color.
            if (this.spriteVisual != null && this.tintCompositionEffectBrush != null)
                return;

            var source = Element.Source;
            if (source == null)
                return;

            var fileSource = source as FileImageSource;
            string filePath = string.Empty;
            if (fileSource == null)
                filePath = (this.Element as TintImage)?.Hint;
            else
                filePath = Path.GetDirectoryName(fileSource.File);
            await CreateSpriteVisualAndTintCompositeEffectBrushAsync(new Uri($"ms-appx:///{filePath}"));
        }

        private void UpdateSpriteVisualBrushAndElementChildVisual(CompositionBrush compositionBrush)
        {
            this.spriteVisual.Brush = compositionBrush;
            ElementCompositionPreview.SetElementChildVisual(Control, this.spriteVisual);
        }

        private async Task CreateSpriteVisualAndTintCompositeEffectBrushAsync(Uri uri)
        {
            if (Control == null || Element == null || Element.Width < 0 || Element.Height < 0)
                return;

            if (spriteVisual == null)
            {
                this.spriteVisual = this.tintImageCompositor.CreateSpriteVisual();
                this.spriteVisual.Size = new Vector2((float)Element.Width, (float)Element.Height);
            }

            if (this.imageSurface == null)
                this.imageSurface = await this.compositionGenerator.CreateImageSurfaceAsync(uri, new Native.Size(Element.Width, Element.Height), ImageSurfaceOptions.DefaultOptimized);

            if (this.tintCompositionSurfaceBrush == null)
                this.tintCompositionSurfaceBrush = this.tintImageCompositor.CreateSurfaceBrush(this.imageSurface.Surface);

            if (this.tintImage.TintColor == Color.Transparent)
            {
                //Cancelling/Skipping the native tint effect.
                this.tintCompositionEffectBrush = null;
                this.UpdateSpriteVisualBrushAndElementChildVisual(this.tintCompositionSurfaceBrush);
                return;
            }
            
            #region Applying tint color

            //Creating Composite effect based on tint color to reflect on the native view. 
            IGraphicsEffect graphicsEffect = new CompositeEffect
            {
                Mode = CanvasComposite.DestinationIn,
                Sources =
                    {
                        new ColorSourceEffect
                        {
                            Name = "colorSource",
                            Color = this.tintImage.TintColor.ToWindowsColor()
                        },
                        new CompositionEffectSourceParameter("mask")
                    }
            };

            //Creating effect factory based on the created composite effect to apply tint effect.
            CompositionEffectFactory effectFactory = this.tintImageCompositor.CreateEffectFactory(graphicsEffect,
                new[] { "colorSource.Color" });
            this.tintCompositionEffectBrush = effectFactory.CreateBrush();
            this.tintCompositionEffectBrush.SetSourceParameter("mask", tintCompositionSurfaceBrush);

            this.UpdateTintColor(this.tintImage.TintColor.ToWindowsColor());
            this.UpdateSpriteVisualBrushAndElementChildVisual(this.tintCompositionEffectBrush);

            #endregion
        }

        private string GetImageName()
        {
            try
            {
                var streamSource = this.Element.Source as StreamImageSource;
                if (streamSource != null)
                {
                    var streamTarget = streamSource.Stream?.Target;
                    var streamValue = streamTarget?.GetType().GetField("stream")?.GetValue(streamTarget);
                    var targetValue = streamValue?.GetType().GetProperty("Target")?.GetValue(streamValue);
                    var imageName = targetValue?.GetType().GetField("resource")?.GetValue(targetValue);
                    var resultArray = imageName?.ToString().Split(".");
                    if (resultArray.Length >= 2)
                        return resultArray[resultArray.Length - 2] + "." + resultArray[resultArray.Length - 1];
                }
            }
            catch { }
            return null;
        }
    }
}
