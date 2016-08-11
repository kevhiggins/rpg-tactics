using System.Collections.Generic;

namespace Rpg.Unit
{
    public class SortUnitsByCT : IComparer<IUnit>
    {
        public int Compare(IUnit x, IUnit y)
        {
            if (x.ChargeTime < y.ChargeTime)
            {
                return 1;
            }
            else if(x.ChargeTime > y.ChargeTime)
            {
                return -1;
            }
            else
            {
                if(x.Speed == y.Speed)
                {
                    return 0;
                }
                return x.Speed < y.Speed ? 1 : -1;
            }
        }
    }
}
