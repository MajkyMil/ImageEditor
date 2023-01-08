using ImageEditor.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ImageEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel;
        private bool isDragging = false;
        private Point anchorPoint;


        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            mainWindowViewModel = viewModel;
        }

        private void Crop_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((ToggleButton)sender).IsChecked)
            {
                border.DisableMouseEvent();
                EnabletCropMouseEvent();
                ConfirmCrop_Button.Visibility = Visibility.Visible;
                Undo_Button.Visibility = Visibility.Visible;
            }
            else
            {
                border.EnableMouseEvent();
                DisableCropMouseEvent();
                selectionRectangle.Visibility = Visibility.Hidden;
                ConfirmCrop_Button.Visibility = Visibility.Hidden;
                Undo_Button.Visibility = Visibility.Hidden;

            }
        }   

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(sender as UIElement);
            Canvas.SetLeft(selectionRectangle, mousePosition.X);
            Canvas.SetTop(selectionRectangle, mousePosition.Y);
            selectionRectangle.Visibility = System.Windows.Visibility.Visible;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(sender as UIElement);
                selectionRectangle.Width = Math.Abs(mousePosition.X - Canvas.GetLeft(selectionRectangle));
                selectionRectangle.Height = Math.Abs(mousePosition.Y - Canvas.GetTop(selectionRectangle));
            }
        }


        private void EnabletCropMouseEvent()
        {
            GridImage.MouseLeftButtonDown += new MouseButtonEventHandler(image_MouseLeftButtonDown);
            GridImage.MouseMove += new MouseEventHandler(image_MouseMove);
            GridImage.MouseLeftButtonUp += new MouseButtonEventHandler(image_MouseLeftButtonUp);
        }

        private void DisableCropMouseEvent()
        {
            GridImage.MouseLeftButtonDown -= new MouseButtonEventHandler(image_MouseLeftButtonDown);
            GridImage.MouseMove -= new MouseEventHandler(image_MouseMove);
            GridImage.MouseLeftButtonUp -= new MouseButtonEventHandler(image_MouseLeftButtonUp);
        }

        #region "Mouse events"
        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDragging == false)
            {
                anchorPoint.X = e.GetPosition(BackPanel).X;
                anchorPoint.Y = e.GetPosition(BackPanel).Y;
                isDragging = true;
            }
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                double x = e.GetPosition(BackPanel).X;
                double y = e.GetPosition(BackPanel).Y;
                selectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(x, anchorPoint.X));
                selectionRectangle.SetValue(Canvas.TopProperty, Math.Min(y, anchorPoint.Y));
                selectionRectangle.Width = Math.Abs(x - anchorPoint.X);
                selectionRectangle.Height = Math.Abs(y - anchorPoint.Y);

                if (selectionRectangle.Visibility != Visibility.Visible)
                    selectionRectangle.Visibility = Visibility.Visible;
            }
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                if (selectionRectangle.Width > 0)
                {
                }
                if (selectionRectangle.Visibility != Visibility.Visible)
                    selectionRectangle.Visibility = Visibility.Visible;
            }
        }

        private void RestRect()
        {
            selectionRectangle.Visibility = Visibility.Collapsed;
            isDragging = false;
        }

        #endregion
    }
}
