using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    public abstract class ConditionСhecker
    {
        private readonly Discount _discount;
        protected IPurchase items = Purchase.Instance;
        public ConditionСhecker(Discount discount)
        {
            _discount = discount;
        }
        public Discount GetDiscount() => _discount;
        public abstract bool CoditionalIsPerformed();
    }
    public abstract class ProductDiscountChecker : ConditionСhecker
    {
        protected ProductDiscountChecker(ProductDiscount discount) : base(discount)
        {
        }
    }
    public abstract class GeneralDiscountChecker : ConditionСhecker
    {
        public GeneralDiscountChecker(GeneralDiscount discount) : base(discount)
        {
        }
    }
    public class BirthdayDiscountChecker: GeneralDiscountChecker 
    {
        private readonly DateTime _birthdayDate;
        private readonly DateTime _todayDate = DateTime.Now;
        public BirthdayDiscountChecker(GeneralDiscount discount, DateTime birthdayDate) : base(discount)
        {
            _birthdayDate = birthdayDate;
        }

        public override bool CoditionalIsPerformed() => _birthdayDate.Day.Equals(_todayDate.Day) 
            && _birthdayDate.Month.Equals(_todayDate.Month);
    }
    public class CurrentProductDiscountChecker : ProductDiscountChecker
    {
        protected ulong _requiredId;
        public CurrentProductDiscountChecker (ProductDiscount discount) : base(discount)
        {
            _requiredId = discount.Id;
        }

        public override bool CoditionalIsPerformed() => items.GetItems().Any(item => item.Id.Equals(_requiredId));
    }
    public class CountCurrentProductDiscountChecker : CurrentProductDiscountChecker 
    {
        private readonly uint _requiredCount;
        public CountCurrentProductDiscountChecker(ProductDiscount discount, uint count) : base(discount)
        {
            _requiredCount = count;
        }

        public override bool CoditionalIsPerformed() => items.GetItems().Any(item => item.Id.Equals(_requiredId) 
        && item.Item.CountOfProduct >= _requiredCount);
    }
    public class GeneralSumDiscountChecker : GeneralDiscountChecker
    {
        private readonly uint _requiredSumOfPurchase;
        public GeneralSumDiscountChecker(GeneralDiscount discount, uint sum) : base(discount)
        {
            _requiredSumOfPurchase = sum;
        }

        public override bool CoditionalIsPerformed() => items.Cost >= _requiredSumOfPurchase;
    }
}
