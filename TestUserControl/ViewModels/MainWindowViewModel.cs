using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TestUserControl.Models;

namespace TestUserControl.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<Order> Orders { get; set; }

        public RelayCommand<object> AddOrderCommand { get; set; }
        public RelayCommand DeleteAllOrderCommand { get; set; }
        public RelayCommand<object> DeleteOrderCommand { get; set; }
        public RelayCommand<object> AddItemCommand { get; set; }
        public RelayCommand<object> DeleteItemCommand { get; set; }

        public MainWindowViewModel()
        {
            Orders = new ObservableCollection<Order>();
            AddOrderCommand = new RelayCommand<object>(AddOrder);
            DeleteAllOrderCommand = new RelayCommand(DeleteAllOrder);
            DeleteOrderCommand = new RelayCommand<object>(DeleteOrder);
            AddItemCommand = new RelayCommand<object>(AddItem);
            DeleteItemCommand = new RelayCommand<object>(DeleteItem);

            InitData();
        }

        private void DeleteItem(object param)
        {
            //TODO:
            MessageBox.Show(param?.ToString());
        }

        private void AddItem(object param)
        {
            Orders.First(x => x.Id == (int)param).Items.ToList().Add(new Item() { Id = (int)param });
        }

        private void DeleteOrder(object param)
        {
            Orders.RemoveAt((int)param);
        }

        private void AddOrder(object param)
        {
            Orders.Add(new Order() { Id = Orders.Count + 1 });
        }

        private void DeleteAllOrder()
        {
            Orders.Clear();
        }

        private void InitData()
        {
            Orders.Add(new Order
            {
                Id = 1,
                Name = "První objednávka",
                Items = new List<Item> {
                    new Item { Name = "První položka", Id = 1 },
                    new Item { Id = 2, Name = "Druhá položka" }
                }
            });
            Orders.Add(new Order
            {
                Id = 2,
                Name = "Druhá objednávka",
                Items = new List<Item> {
                    new Item { Name = "Třetí položka", Id = 3 },
                    new Item { Id = 4, Name = "Čtvrtá položka" },
                    new Item { Id = 5, Name = "Pátá položka" }
                },
                OrderChilds = new List<Order> {
                    new Order
            {
                Id = 2,
                Name = "Druhá objednávka",
                Items = new List<Item> {
                    new Item { Name = "Třetí položka", Id = 3 },
                    new Item { Id = 4, Name = "Čtvrtá položka" },
                    new Item { Id = 5, Name = "Pátá položka" }            } , OrderChilds= new List<Order>
                    {
                        new Order { OrderChilds= new List<Order> { new Order { } }}
                    }
                   }
            }
            });
            Orders.Add(new Order
            {
                Id = 3,
                Name = "Třetí objednávka",
                Items = new List<Item> {
                    new Item { Name = "Šestá položka", Id = 6 }
                }
            });
        }
    }
}
