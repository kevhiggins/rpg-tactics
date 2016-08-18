using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.Unit;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    class UnitAnimationEventManager : MonoBehaviour
    {
        public void AttackHit()
        {
            gameObject.transform.parent.GetComponent<AbstractUnit>().AttackHit();
        }

        public void AttackComplete()
        {
            gameObject.transform.parent.GetComponent<AbstractUnit>().AttackComplete();
        }

        public void DeathComplete()
        {
            gameObject.transform.parent.GetComponent<AbstractUnit>().DeathComplete();
        }
    }
}
