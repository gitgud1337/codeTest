using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codeTest
{
    public class Inventory
    {
        public List<Item> storeItems { get; set; } = new List<Item>();
        public List<int> comboCampaigns { get; set; } = new List<int>();
        public List<int> volumeCampaigns { get; set; } = new List<int>();

        public Inventory()
        {
            storeItems.Add(new Item(111, "Hoodie", 44, 33));
            storeItems.Add(new Item(222, "Sweatshirt", 33, 22));
            storeItems.Add(new Item(555, "Jacket", 77, null));
            storeItems.Add(new Item(333, "Water", 11, 7));
            storeItems.Add(new Item(444, "Coke", 14, 11));
            storeItems.Add(new Item(666, "Cheese", 22, null));

            comboCampaigns.Add(111);
            comboCampaigns.Add(222);
            volumeCampaigns.Add(333);
            volumeCampaigns.Add(444);
        }
    }
}
