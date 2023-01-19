using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace TestUserControl.Views.UserControls
{
    /// <summary>
    /// Interaction logic for OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        public RelayCommand AddItemCommand
        {
            get { return (RelayCommand)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register("AddItemCommand", typeof(RelayCommand), typeof(OrderUserControl));


        public RelayCommand DeleteOrderCommand
        {
            get { return (RelayCommand)GetValue(DeleteOrderCommandProperty); }
            set { SetValue(DeleteOrderCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteOrderCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteOrderCommandProperty =
            DependencyProperty.Register("DeleteOrderCommand", typeof(RelayCommand), typeof(OrderUserControl));


        public OrderUserControl()
        {
            InitializeComponent();
        }
    }
}
