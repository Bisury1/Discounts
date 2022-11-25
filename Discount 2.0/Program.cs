using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    internal class Program
    {
        static void Main()
        {
            var checkersBase = ConditionCheckersBase.Instance;
            checkersBase.AddElement(new CurrentProductDiscountChecker(new ProductDiscount(12, 123)));
            checkersBase.AddElement(new CountCurrentProductDiscountChecker(new ProductDiscount(10, 12), 4));
            checkersBase.AddElement(new GeneralSumDiscountChecker(new GeneralDiscount(10), 1000));
            checkersBase.AddElement(new GeneralSumDiscountChecker(new GeneralDiscount(12), 1200));
            checkersBase.AddElement(new BirthdayDiscountChecker(new GeneralDiscount(14), DateTime.Now));
            checkersBase.AddCardElement(new GeneralSumDiscountChecker(new GeneralDiscount(20), 0));
            var products = Purchase.Instance;
            var items = new List<Item>() { new Item(new Product("gdfgd", 123, 20), 10), new Item(new Product("wferh", 12, 23), 20) };
            products.AddItems(items.Select(item => new ItemWithDiscount(item)));
            var discountsMaker = new DiscountMaker(new DiscountSetter(), new DiscountGetter(new DiscountManager()));
            discountsMaker.MakeADiscount(true);
            Console.WriteLine(products.Cost);
            Console.WriteLine(products.CostWithDiscount);
            Console.ReadLine();
        }
    }
}
