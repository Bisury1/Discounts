using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    public interface IPurchase
    {
        float Cost { get; }
        ItemWithDiscount[] GetItems();
    }
    public interface IDiscountManagerFactory
    {
        Discount[] GetAvailableDiscounts(bool haveACard);
    }
    public interface ICheckersBase
    {
        ConditionСhecker[] GetCheckersWithCard();
        ConditionСhecker[] GetCheckersWithoutCard();
    }
    public interface IDiscountSetter
    {
        bool SetDiscount(float value, ulong id);
        bool SetAllDiscounts(float value);
        bool ClearDiscounts();
    }
    public interface IDiscountGetter
    {
        Discount[] GetDiscounts(bool haveACard);
    }
}
