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
        public RelayCommand<object> AddItemCommand
        {
            get { return (RelayCommand<object>)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register("AddItemCommand", typeof(RelayCommand<object>), typeof(OrderUserControl));


        public RelayCommand<object> DeleteOrderCommand
        {
            get { return (RelayCommand<object>)GetValue(DeleteOrderCommandProperty); }
            set { SetValue(DeleteOrderCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteOrderCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteOrderCommandProperty =
            DependencyProperty.Register("DeleteOrderCommand", typeof(RelayCommand<object>), typeof(OrderUserControl));

        public RelayCommand<object> DeleteItemCommand
        {
            get { return (RelayCommand<object>)GetValue(DeleteItemCommandProperty); }
            set { SetValue(DeleteItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteItemCommandProperty =
            DependencyProperty.Register("DeleteItemCommand", typeof(RelayCommand<object>), typeof(OrderUserControl));

        public OrderUserControl()
        {
            InitializeComponent();
        }
    }
}
