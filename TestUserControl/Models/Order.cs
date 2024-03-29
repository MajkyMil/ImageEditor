﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUserControl.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Order> OrderChilds { get; set; }
    }
}
