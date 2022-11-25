using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_2._0
{
    public class ConditionCheckersBase : ICheckersBase
    {
        private readonly List<ConditionСhecker> _conditionСheckers;
        private readonly List<ConditionСhecker> _conditionСheckersOnTheCard;
        private static ConditionCheckersBase _checkersBase;
        protected ConditionCheckersBase()
        {
            _conditionСheckers = new List<ConditionСhecker>();
            _conditionСheckersOnTheCard = new List<ConditionСhecker>();
            //желательно читать из базы
        }
        public static ConditionCheckersBase Instance
        {
            get
            {
                if (_checkersBase == null)
                    _checkersBase = new ConditionCheckersBase();
                return _checkersBase;
            }
        }
        public bool AddElement(ConditionСhecker сonditionСhecker)
        {
            _conditionСheckers.Add(сonditionСhecker);
            return true;
        }
        public bool AddCardElement(ConditionСhecker сonditionСhecker)
        {
            _conditionСheckersOnTheCard.Add(сonditionСhecker);
            return true;
        }
        public bool RemoveElement(ConditionСhecker сonditionСhecker)
        {
            _conditionСheckers.Remove(сonditionСhecker);
            return true;
        }
        public bool RemoveCardElement(ConditionСhecker сonditionСhecker)
        {
            _conditionСheckers.Remove(сonditionСhecker);
            return true;
        }
        public ConditionСhecker[] GetCheckersWithoutCard() => _conditionСheckers.ToArray();
        public ConditionСhecker[] GetCheckersWithCard() => _conditionСheckers.Union(_conditionСheckersOnTheCard).ToArray();
    }
}
