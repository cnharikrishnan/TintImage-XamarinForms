using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TintImageDemo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //this.homeIcon.Source = ImageSource.FromResource("TintImageDemo.Image.HomeIcon.png", this.GetType().Assembly);
        }

        private void ChangeSize_Clicked(object sender, EventArgs e)
        {
            if(homeIcon.HeightRequest == 200)
            {
                homeIcon.HeightRequest = 300;
                homeIcon.WidthRequest = 300;
            }
            else
            {
                homeIcon.HeightRequest = 200;
                homeIcon.WidthRequest = 200;
            }
        }
    }
}
