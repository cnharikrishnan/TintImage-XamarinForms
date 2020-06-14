using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TintImageDemo.ViewModel
{
    public class TintImageViewModel : INotifyPropertyChanged
    {
        private Color _tintColor;

        public TintImageViewModel()
        {
            this.TintColor = this.GetDefaultTintColor();
            this.TintImageSource = ImageSource.FromResource("TintImageDemo.Image.HomeIcon.png", this.GetType().Assembly);
            this.ImageName = "HomeIcon.png";
            this.ApplyDefaultCommand = new Command(ExecuteDefaultCommand);
            this.ApplyBlackCommand = new Command(ExecuteBlackCommand);
            this.ApplyBlueCommand = new Command(ExecuteBlueCommand);
        }

        public Color TintColor
        {
            get { return this._tintColor; }
            set { this._tintColor = value; OnPropertyChanged(); }
        }

        public ImageSource TintImageSource { get; set; }
        public string ImageName { get; set; }
        public ICommand ApplyDefaultCommand { get; set; }
        public ICommand ApplyBlackCommand { get; set; }
        public ICommand ApplyBlueCommand { get; set; }

        private Color GetDefaultTintColor()
        {
            return (Color)Control.TintImage.TintColorProperty.DefaultValue;
        }

        private void ExecuteDefaultCommand(object obj)
        {
            this.TintColor = this.GetDefaultTintColor();
        }

        private void ExecuteBlackCommand(object obj)
        {
            this.TintColor = Color.Black;
        }

        private void ExecuteBlueCommand(object obj)
        {
            this.TintColor = Color.Blue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
