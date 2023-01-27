using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestUserControl.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ItemUserControl.xaml
    /// </summary>
    public partial class ItemUserControl : UserControl
    {

        public RelayCommand<object> DeleteItemCommand
        {
            get { return (RelayCommand<object>)GetValue(DeleteItemCommandProperty); }
            set { SetValue(DeleteItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteItemCommandProperty =
            DependencyProperty.Register("DeleteItemCommand", typeof(RelayCommand<object>), typeof(ItemUserControl));



        public ItemUserControl()
        {
            InitializeComponent();
        }
    }
}
