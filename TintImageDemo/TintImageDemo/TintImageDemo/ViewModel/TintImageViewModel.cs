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
        private double _tintHeight;
        private double _tintWidth;

        public TintImageViewModel()
        {
            this.TintColor = this.GetDefaultTintColor();
            this.TintImageSource = ImageSource.FromResource("TintImageDemo.Image.HomeIcon.png", this.GetType().Assembly);
            this.ImageName = "HomeIcon.png";
            this.ApplyDefaultCommand = new Command(ExecuteDefaultCommand);
            this.ApplyBlackCommand = new Command(ExecuteBlackCommand);
            this.ApplyBlueCommand = new Command(ExecuteBlueCommand);
            this.ChangeSizeCommand = new Command(UpdateTintImageSize);
            this.TintHeight = 200;
            this.TintWidth = 200;
        }

        public Color TintColor
        {
            get { return this._tintColor; }
            set { this._tintColor = value; OnPropertyChanged(); }
        }

        public double TintHeight
        {
            get { return this._tintHeight; }
            set { this._tintHeight = value; OnPropertyChanged(); }
        }

        public double TintWidth
        {
            get { return this._tintWidth; }
            set { this._tintWidth = value; OnPropertyChanged(); }
        }

        public ImageSource TintImageSource { get; set; }
        public string ImageName { get; set; }
        public ICommand ApplyDefaultCommand { get; set; }
        public ICommand ApplyBlackCommand { get; set; }
        public ICommand ApplyBlueCommand { get; set; }
        public ICommand ChangeSizeCommand { get; set; }

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

        private void UpdateTintImageSize(object obj)
        {
            if (TintHeight == 200)
            {
                TintHeight = 300;
                TintWidth = 300;
            }
            else
            {
                TintHeight = 200;
                TintWidth = 200;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
