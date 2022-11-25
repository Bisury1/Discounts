using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    public class Product
    {
        public string Name { get; private set; }
        public ulong Id { get; private set; }
        public uint Cost { get; private set; }

        public Product(string name, ulong id, uint cost)
        {
            Name = name;
            Id = id;
            Cost = cost;
        }
    }
    public class Item
    {
        public Product Product { get; private set; }
        public uint CountOfProduct { get; private set; }

        public Item(Product product, uint count)
        {
            Product = product;
            CountOfProduct = count;
        }
        public ulong Id => Product.Id;
        
        public float Cost => Product.Cost * CountOfProduct;
    }
    public class ItemWithDiscount
    {
        public Item Item { get; set; }
        public ulong Id => Item.Id;
        public ItemWithDiscount(Item item)
        {
            Item = item;
        }
        public void SetDiscountValue(float value)
        {
            DiscountQuality = value;
        }
        public float DiscountQuality { get; private set; }
        public float CostWithDiscount => Item.Cost * (100 - DiscountQuality) / 100f;
        public float Cost => Item.Cost;
    }
    public class Purchase: IPurchase
    {
        private List<ItemWithDiscount> Items { get; set; }
        public float Cost => Items.Sum(item => item.Cost);
        public float CostWithDiscount => Items.Sum(item => item.CostWithDiscount);
        protected Purchase()
        {
            Items = new List<ItemWithDiscount>();
        }
        private static Purchase _purchase { get; set; }
        public static Purchase Instance
        {
            get
            {
                if (_purchase == null)
                    _purchase = new Purchase();
                return _purchase;
            }
        }
        public bool AddItem(ItemWithDiscount item) 
        {
            Items.Add(item);
            return true;
        }
        public bool AddItems(IEnumerable<ItemWithDiscount> items)
        {
            Items.AddRange(items);
            return true;
        }
        public bool RemoveAll()
        {
            Items.Clear();
            return true;
        }
        public ItemWithDiscount[] GetItems() => Items.ToArray();
        public bool RemoveItem(ItemWithDiscount item) 
        { 
            Items.Remove(item);
            return true;
        }
    }
}
