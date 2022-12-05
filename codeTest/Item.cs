using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CampaignType
{
    NONE,
    COMBO,
    VOLUME
}
namespace codeTest
{
    public class Item
    {
        public int EAN { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public int? CampaignPrice { get; set; }

        public Item(int ean, string itemName, int price, int? campaignPrice)
        {
            EAN = ean;
            ItemName = itemName;
            Price = price;
            CampaignPrice = campaignPrice;
        }
    }
}
