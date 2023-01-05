using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageEditor.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private BitmapImage bitmapImage;

        public BitmapImage BitmapImage
        {
            get { return bitmapImage; }
            set
            {
                bitmapImage = value;
                OnPropertyChanged();
            }
        }

        private double rotateAngle;

        public double RotateAngle
        {
            get { return rotateAngle; }
            set
            {
                rotateAngle = value;
                OnPropertyChanged();
            }
        }

        private bool saveButtonEnabled;

        public bool SaveButtonEnabled
        {
            get { return saveButtonEnabled; }
            set
            {
                saveButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, string> imageProperties;

        public Dictionary<string, string> ImageProperties
        {
            get { return imageProperties; }
            set
            {
                imageProperties = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand LoadImageCommand { get; }
        public RelayCommand SaveImageCommand { get; }
        public RelayCommand<object[]> ConfirmCropCommand { get; }
        public RelayCommand UndoCropCommand { get; }


        public MainWindowViewModel()
        {
            LoadImageCommand = new RelayCommand(LoadImage);
            SaveImageCommand = new RelayCommand(OnSaveImage);
            ConfirmCropCommand = new RelayCommand<object[]>(OnConfirmCrop);
            UndoCropCommand = new RelayCommand(OnUndoCrop);

            BitmapImage = new BitmapImage();
            ImageProperties = new Dictionary<string, string>();
        }

        private void OnUndoCrop()
        {            
            throw new NotImplementedException();
        }

        private void OnConfirmCrop(object[] param)
        {
            var cropRectangle = param[0] as Rectangle;
            var canvas = param[1] as Canvas;

            double x = Canvas.GetLeft(cropRectangle);
            double y = Canvas.GetTop(cropRectangle);
            double width = cropRectangle.Width;
            double height = cropRectangle.Height;

            CroppedBitmap croppedBitmap = new CroppedBitmap(BitmapImage,
                new Int32Rect((int)x, (int)y, (int)width, (int)height));

            BitmapImage croppedBitmapImage = new BitmapImage();


            using (MemoryStream memory = new MemoryStream())
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
                encoder.Save(memory);
                memory.Position = 0;

                croppedBitmapImage.BeginInit();
                croppedBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                croppedBitmapImage.StreamSource = memory;
                croppedBitmapImage.EndInit();
            }

            BitmapImage = croppedBitmapImage; 

            //    var imageByteArray = mainWindowViewModel.CropImage((byte[])imageStack.Peek(), Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(width), Convert.ToInt32(height));
            //    image.Source = mainWindowViewModel.GetBitmapSource(imageByteArray);
            //    imageStack.Push(imageByteArray);

            //    DisableCropMouseEvent();
            //    border.EnableMouseEvent();
            //    ToggleCrop_Button.IsChecked = false;
            //    Undo_Button.Visibility = Visibility.Visible;
            //    ConfirmCrop_Button.Visibility = Visibility.Hidden;
            //    selectionRectangle.Visibility = Visibility.Hidden;
            //    border.Reset();
        }

        private void LoadImage()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a image";

            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.jp2;*.j2k;*.png;*.tif;*.tiff;*.pdf;";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageProperties = new Dictionary<string, string>();

                BitmapImage = new BitmapImage();
                BitmapImage.BeginInit();
                BitmapImage.UriSource = new Uri(openFileDialog.FileName);
                BitmapImage.EndInit();
                SaveButtonEnabled = true;

                ImageProperties.Add("Width", BitmapImage.Width.ToString());
                ImageProperties.Add("Height", BitmapImage.Height.ToString());
                // ImageProperties.Add("Rotation", BitmapImage.Rotation.ToString());
                ImageProperties.Add("SourceRect", BitmapImage.SourceRect.ToString());
                ImageProperties.Add("UriSource", BitmapImage.UriSource.ToString());

            }
        }

        private void OnSaveImage()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = BitmapImage.UriSource.LocalPath;// mainWindowViewModel.ImageProperties["ImageName"];
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.Filter = "All files (*.*)|*.*|JPG - JPG/JPEG Format|*.jpg|JP2 - JPEG 2000 Format|*.jp2|J2K - J2K Format|*.j2k|PNG - Portable Network Graphics|*.png|TIF - Tagged Image File Format|*.tiff";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    SaveImage(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveImage(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName);

            Image image = new Image();
            image.Source = BitmapImage;

            RotateTransform rotateTransform = new RotateTransform(RotateAngle);
            //TransformedBitmap transformedImage = new TransformedBitmap(BitmapImage, rotateTransform);
            image.RenderTransform = rotateTransform;

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)image.Source.Width, (int)image.Source.Height, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(image);

            switch (extension)
            {
                case ".tif":
                case ".tiff":
                    var tiffEncoder = new TiffBitmapEncoder();
                    tiffEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        tiffEncoder.Save(stream);
                    }
                    break;
                case ".jpg":
                case ":jpeg":
                    var jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.QualityLevel = 90; // nastavte kvalitu obrázku v rozmezí 0-100
                    jpegEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        jpegEncoder.Save(stream);
                    }
                    break;
                case ".j2k":
                    break;
                case ".jp2":

                    break;
                case ".png":
                    var pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        pngEncoder.Save(stream);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
