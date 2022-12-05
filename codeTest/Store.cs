using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codeTest
{
    public class Store
    {
        private readonly Inventory inventory = new Inventory();
        private List<Item> shoppingCart = new List<Item>();
        private int comboTotal = 0;
        private int volumeTotal = 0;
        private int totalPrice = 0;
        private bool exitStore = false;

        public void StorePrompt()
        {
            GreetingsText();
            while (!exitStore)
            {
                StoreLoop();
            }
        }

        private void GreetingsText()
        {
            Console.WriteLine("####### #     # #######     #####  ####### ####### ######  ####### ");
            Console.WriteLine("   #    #     # #          #     #    #    #     # #     # #       ");
            Console.WriteLine("   #    #     # #          #          #    #     # #     # #       ");
            Console.WriteLine("   #    ####### #####       #####     #    #     # ######  #####   ");
            Console.WriteLine("   #    #     # #                #    #    #     # #   #   #       ");
            Console.WriteLine("   #    #     # #          #     #    #    #     # #    #  #       ");
            Console.WriteLine("   #    #     # #######     #####     #    ####### #     # ####### ");
            Console.WriteLine("");
            Console.WriteLine("Hello, welcome to the STORE!");
        }

        private void StoreLoop()
        {
            Console.WriteLine("Press [0] to see all items.");
            Console.WriteLine("Press [1] to see combo campaigns.");
            Console.WriteLine("Press [2] to see volume campaigns.");
            if (shoppingCart.Count > 0)
            {
                Console.WriteLine("Press [3] to see your cart.");
                Console.WriteLine("Press [4] to proceed to checkout and buy all items.");
            }
            Console.WriteLine("Press any other key to exit the store.");
            Console.WriteLine("");

            var option = Console.ReadLine();

            switch (option)
            {
                case "0":
                    AllItemsInStore();
                    break;
                case "1":
                    ComboCampaigns();
                    break;
                case "2":
                    VolumeCampaigns();
                    break;
                case "3":
                    SeeCart();
                    break;
                case "4":
                    Checkout();
                    break;
                default:
                    exitStore = true;
                    break;
            }
            Console.WriteLine("Good bye!");
        }

        private void AllItemsInStore()
        {
            Console.WriteLine("Here's all the items in the store.");
            foreach (var item in inventory.storeItems)
            {
                Console.WriteLine("Name: " + item.ItemName + " Regular price: " + item.Price + " Campaign price: " + item.CampaignPrice);
            }
            Buy();
        }

        private void ComboCampaigns()
        {
            Console.WriteLine("Here's all the items in the combo campaign.");
            Console.WriteLine("You need to combine any 2 items to get the campaign price.");
            Console.WriteLine("");
            foreach (var item in inventory.storeItems)
            {
                if (inventory.comboCampaigns.Exists(x => x == item.EAN))
                {
                    Console.WriteLine("Name: " + item.ItemName + " Regular price: " + item.Price + " Campaign price: " + item.CampaignPrice);
                }
            }
            Console.WriteLine("");
            Buy();
        }

        private void VolumeCampaigns()
        {
            Console.WriteLine("Here's all the items in the volume campaign.");
            Console.WriteLine("You need to buy them in volumes of 2 (4, 6, 8 etc) of the same to get the the campaign price.");
            Console.WriteLine("");
            foreach (var item in inventory.storeItems)
            {
                if (inventory.volumeCampaigns.Exists(x => x == item.EAN))
                {
                    Console.WriteLine("Name: " + item.ItemName + " Regular price: " + item.Price + " Campaign price: " + item.CampaignPrice);
                }
            }
            Console.WriteLine("");
            Buy();
        }

        private void Buy()
        {
            Console.WriteLine("Write the name of the item you want to buy followed by a space and the amount of items.");
            Console.WriteLine("'ITEMTOBUY AMOUNTTOBUY'");
            var input = Console.ReadLine();
            string[] buy;
            if (input != null && input.Length != 0)
            {
                buy = input.Split(' ');
                var item = inventory.storeItems.Find(x => x.ItemName.ToLower() == buy[0].ToLower());
                var amount = int.Parse(buy[1]);
                if (item != null && amount > 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        shoppingCart.Add(item);
                    }
                }
                else
                {
                    Console.WriteLine("Something wrong with your order. Either item is null or amount lower than 1.");
                }
            }
            else
            {
                Console.WriteLine("No input please try again.");
            }
            Console.WriteLine("");
        }

        private void SeeCart()
        {
            Console.WriteLine("You have " + shoppingCart.Count.ToString() + " items in your cart.");
            Console.WriteLine("The following items are in you cart: ");
            foreach (var item in shoppingCart)
            {
                Console.WriteLine(item.ItemName);
            }
            Console.WriteLine("");
        }

        private void Checkout()
        {
            CalculateComboPrice();
            CalculateVolumePrice();
            CalculateTotalPrice();
            Console.WriteLine("Total price: " + totalPrice);
            exitStore = true;
        }

        private void CalculateComboPrice()
        {
            comboTotal = 0;
            var eligibleForCombo = 0;
            var combo = shoppingCart.Where(item => inventory.comboCampaigns.Any(comboEAN => comboEAN == item.EAN)).ToList();
            (eligibleForCombo, comboTotal) = CalculateEligibleItemsAndTotalPrice(combo);
            if (combo.Count > 0)
            {
                Console.WriteLine(eligibleForCombo + "/" + combo.Count + " combo items eligible for discount combo price.");
                Console.WriteLine("Total price for combo items: " + comboTotal);
                Console.WriteLine("");
            }
        }

        private void CalculateVolumePrice()
        {
            volumeTotal = 0;
            var eligibleForVolume = 0;
            var volume = shoppingCart.Where(item => inventory.volumeCampaigns.Any(volumeEAN => volumeEAN == item.EAN)).OrderBy(x => x.EAN).ToList();
            foreach (var ean in inventory.volumeCampaigns)
            {
                List<Item> items = new();
                foreach (var item in volume)
                {
                    if (item.EAN == ean)
                    {
                        items.Add(item);
                    }
                }
                var eligiblePerEan = 0;
                var pricePerEan = 0;
                var calculatedNumbers = CalculateEligibleItemsAndTotalPrice(items);
                eligiblePerEan = calculatedNumbers.eligible;
                pricePerEan = calculatedNumbers.price;
                eligibleForVolume += eligiblePerEan;
                volumeTotal += pricePerEan;
            }
            if (volume.Count > 0)
            {
                Console.WriteLine(eligibleForVolume + "/" + volume.Count + "volume items eligible for discount volume price.");
                Console.WriteLine("Total price for volume items: " + volumeTotal);
                Console.WriteLine("");
            }
        }

        private (int eligible, int price) CalculateEligibleItemsAndTotalPrice(List<Item> items)
        {
            int eligible = 0;
            int price = 0;
            if (AllEligible(items.Count))
            {
                eligible = items.Count;
                price += items.Sum(x => x.CampaignPrice.Value);
            }
            else
            {
                eligible = items.Count - 1;
                price += items.SkipLast(1).Sum(x => x.CampaignPrice.Value);
                price += items[items.Count - 1].Price;
            }
            return (eligible, price);
        }

        private bool AllEligible(int count)
        {
            return count % 2 == 0 ? true : false;
        }

        private void CalculateTotalPrice()
        {
            totalPrice = comboTotal + volumeTotal;
        }
    }
}
