using UnityEngine;

namespace Rpg.Unit
{
    class FriendlyUnit : Unit, IFriendlyUnit
    {
        public FriendlyUnit(GameObject gameObject) : base(gameObject)
        {
        }
    }
}
