using CommunityToolkit.Mvvm.Input;
using ImageEditor.View.ViewHelpers;
using ImageMagick;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;

namespace ImageEditor.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private BitmapImage bitmapImage;
        public BitmapImage BitmapImage { get => bitmapImage; set { bitmapImage = value; OnPropertyChanged(); } }
        private Stack<BitmapImage> bitmapImageStack;
        public Stack<BitmapImage> BitmapImageStack { get => bitmapImageStack; set => bitmapImageStack = value; }

        private double rotateAngle;
        public double RotateAngle { get => rotateAngle; set => rotateAngle = value; }

        private bool saveButtonEnabled;
        public bool SaveButtonEnabled
        {
            get => saveButtonEnabled; set { saveButtonEnabled = value; OnPropertyChanged(); }
        }

        private Dictionary<string, string> imageProperties;
        public Dictionary<string, string> ImageProperties
        {
            get => imageProperties; set { imageProperties = value; OnPropertyChanged(); }
        }

        public RelayCommand LoadImageCommand { get; }
        public RelayCommand SaveImageCommand { get; }
        public RelayCommand<object[]> ConfirmCropCommand { get; }
        public RelayCommand UndoCropCommand { get; }

        public MainWindowViewModel()
        {
            LoadImageCommand = new RelayCommand(LoadImage);
            SaveImageCommand = new RelayCommand(OnSaveImage, () => BitmapImage != null);
            ConfirmCropCommand = new RelayCommand<object[]>(OnConfirmCrop);
            UndoCropCommand = new RelayCommand(OnUndoCrop);

            this.BitmapImage = new BitmapImage();
            ImageProperties = new Dictionary<string, string>();
            BitmapImageStack = new Stack<BitmapImage>();
        }

        private void OnUndoCrop()
        {
            if (BitmapImageStack.Count > 1)
                BitmapImageStack.Pop();

            if (BitmapImageStack.Count != 0)
            {
                BitmapImage = BitmapImageStack.Count == 1
                    ? BitmapImageStack.Peek()
                    : BitmapImageStack.Pop();
            }
        }

        private void OnConfirmCrop(object[] param)
        {
            try
            {
                BitmapImageStack.Push(BitmapImage);
                var cropRectangle = param[0] as Rectangle;
                var canvas = param[1] as Canvas;
                var zoomBorder = param[2] as ZoomBorder;

                if (cropRectangle != null && canvas != null && zoomBorder != null)
                {
                    var canvasRectangle = canvas.Children.OfType<Rectangle>().FirstOrDefault();

                    if (canvasRectangle != null)
                    {
                        // HACK
                        var magicConstant = 0.75;

                        var x = Convert.ToInt32(Canvas.GetLeft(canvasRectangle) * magicConstant);
                        var y = Convert.ToInt32(Canvas.GetTop(canvasRectangle) * magicConstant);
                        var width = Convert.ToInt32(canvasRectangle.Width * magicConstant);
                        var height = Convert.ToInt32(canvasRectangle.Height * magicConstant);

                        CroppedBitmap croppedBitmap = new CroppedBitmap(BitmapImage,
                            new Int32Rect(x, y, width, height));

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
                        zoomBorder.Reset();
                        cropRectangle.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                ImageProperties.Add("UriSource", BitmapImage.UriSource.AbsolutePath);

            }
        }

        private void OnSaveImage()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = System.IO.Path.GetFileName(ImageProperties["UriSource"]);
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.Filter = "All files (*.*)|*.*|JPG - JPG/JPEG Format|*.jpg|JP2 - JPEG 2000 Format|*.jp2|J2K - J2K Format|*.j2k|PNG - Portable Network Graphics|*.png|TIF - Tagged Image File Format|*.tiff";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    //SaveImage(saveFileDialog.FileName);
                    SaveImageByMagick(saveFileDialog.FileName);
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

            //Image image = new Image();
            //image.Source = BitmapImage;

            RotateTransform rotateTransform = new RotateTransform(RotateAngle);
            TransformedBitmap transformedImage = new TransformedBitmap(BitmapImage, rotateTransform);

            //image.RenderTransform = rotateTransform;
            //RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)image.Source.Width, (int)image.Source.Height, 96, 96, PixelFormats.Pbgra32);
            //renderBitmap.Render(image);

            switch (extension)
            {
                case ".tif":
                case ".tiff":
                    var tiffEncoder = new TiffBitmapEncoder();
                    tiffEncoder.Frames.Add(BitmapFrame.Create(transformedImage));
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        tiffEncoder.Save(stream);
                    }
                    break;
                case ".jpg":
                case ":jpeg":
                    var jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.QualityLevel = 90; // nastavte kvalitu obrázku v rozmezí 0-100
                    jpegEncoder.Frames.Add(BitmapFrame.Create(transformedImage));

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
                    pngEncoder.Frames.Add(BitmapFrame.Create(transformedImage));

                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        pngEncoder.Save(stream);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SaveImageByMagick(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName);

            using (MemoryStream memoryStream = new MemoryStream())
            {           
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapImage));
                encoder.Save(memoryStream);
                memoryStream.Position = 0;

                using (MagickImage image = new MagickImage(memoryStream))
                {
                    image.Rotate(RotateAngle);                    

                    switch (extension)
                    {
                        case ".tif":
                        case ".tiff":
                            image.Write(fileName, MagickFormat.Tiff);
                            break;
                        case ".jpg":
                        case ":jpeg":
                            image.Write(fileName, MagickFormat.Jpg);
                            break;
                        case ".j2k":
                            break;
                        case ".jp2":

                            break;
                        case ".png":
                            image.Write(fileName, MagickFormat.Png);
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

