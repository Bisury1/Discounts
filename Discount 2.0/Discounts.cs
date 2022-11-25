using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    #region Discount
    public abstract class Discount
    {
        private float _qualities;
        protected IPurchase _purchase = Purchase.Instance;

        public Discount(float qualities)
        {
            Qualities = qualities;
        }

        public float Qualities
        {
            get => _qualities;
            set
            {
                _qualities = (value > 0 && value <= 100) ? value : throw new ArgumentException("Invalid value entered!", nameof(_qualities));
            }
        }
        public abstract float GetDiscountValue();
    }
    public class ProductDiscount : Discount
    {
        private readonly ulong _id;
        public ProductDiscount(float qualities, ulong id) : base(qualities)
        {
            _id = id;
        }

        public override float GetDiscountValue()
        {
            var discount = _purchase.GetItems().First(item => item.Id.Equals(_id)).Cost * Qualities / 100f;
            return discount;
        }
        public ulong Id
        {
            get => _id;
        }
    }
    public class GeneralDiscount : Discount
    {
        public GeneralDiscount(float qualities) : base(qualities)
        {
        }

        public override float GetDiscountValue()
        {
            var discount = _purchase.Cost * Qualities/ 100f;
            return discount;
        }
    }
    #endregion



    #region DiscountHelpTools

    public class DiscountManager: IDiscountManagerFactory
    {
        private readonly ICheckersBase _checkers = ConditionCheckersBase.Instance;
        public Discount[] GetAvailableDiscounts(bool haveACard)
        {
            var discounts = new List<Discount>();
            ConditionСhecker[] conditionСheckers;
            conditionСheckers = (haveACard)
                ? _checkers.GetCheckersWithCard() 
                : _checkers.GetCheckersWithoutCard();
            foreach(var item in conditionСheckers)
            {
                if(item.CoditionalIsPerformed())
                {
                    discounts.Add(item.GetDiscount());
                }
            }
            return discounts.ToArray();
        }
    }
    public class DiscountMaker
    {
        private IDiscountSetter _discountSetter;
        private IDiscountGetter _discountGetter;
        public DiscountMaker(IDiscountSetter discountSetter, IDiscountGetter discountGetter)
        {
            _discountSetter = discountSetter;
            _discountGetter = discountGetter;
        }
        public bool MakeADiscount(bool haveACard)
        {
            _discountSetter.ClearDiscounts();
            var discounts = _discountGetter.GetDiscounts(haveACard);
            foreach(var item in discounts)
            {
                if(item is ProductDiscount)
                {
                    _discountSetter.SetDiscount(item.Qualities, ((ProductDiscount)item).Id);
                }
                else if(item is GeneralDiscount)
                {
                    _discountSetter.SetAllDiscounts(item.Qualities);
                }
            }
            return true;
        }
    }
    public class DiscountGetter: IDiscountGetter
    {
        private readonly IDiscountManagerFactory _manager;
        public DiscountGetter(IDiscountManagerFactory manager)
        {
            _manager = manager;
        }
        public Discount[] GetDiscounts(bool haveACard)
        {
            var discounts = _manager.GetAvailableDiscounts(haveACard);
            var maxSumOfDiscount = 0f;
            List<Discount> maxDiscounts = new List<Discount>();
            for (int i = 0; i < discounts.Length; i++)
            {
                var currentDiscounts = new List<Discount>() { discounts[i] };
                var currentSum = discounts[i].GetDiscountValue();
                for (int j = 0; j < discounts.Length; j++)
                {
                    if(discounts[i].IsCompatible(discounts[j]))
                    {
                        currentSum += discounts[j].GetDiscountValue();
                        currentDiscounts.Add(discounts[j]);
                    }
                }
                if(currentSum > maxSumOfDiscount)
                {
                    maxSumOfDiscount = currentSum;
                    maxDiscounts = currentDiscounts;
                }
            }
            return maxDiscounts.ToArray();
        }
    }
    #endregion
    public class DiscountSetter: IDiscountSetter
    {
        private IPurchase _purchase = Purchase.Instance;
        public bool SetDiscount(float value, ulong id)
        {
            foreach (var item in _purchase.GetItems())
            {
                if(item.Id == id)
                {
                    item.SetDiscountValue(item.DiscountQuality + value);
                }
            }
            return true;
        }
        public bool SetAllDiscounts(float value)
        {
            foreach (var item in _purchase.GetItems())
            {
                item.SetDiscountValue(item.DiscountQuality + value);
            }
            return true;
        }
        public bool ClearDiscounts()
        {
            foreach (var item in _purchase.GetItems())
            {
                item.SetDiscountValue(0);
            }
            return true;
        }
    }
    public static class DiscountCompabilitiesExtencion
    {
        public static bool IsCompatible(this Discount discount1, Discount discount2)
        {
            if((discount1 is ProductDiscount && discount2 is ProductDiscount) 
                || (discount1 is ProductDiscount && discount2 is GeneralDiscount)
                || (discount2 is ProductDiscount && discount1 is GeneralDiscount))
            {
                return true;
            }
            return false;
        }
    }
}
