using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg
{
    public class PopManager : MonoBehaviour
    {
        public GameObject experienceGainPop;
        public GameObject damagePop;
        public GameObject levelUpPop;
        public GameObject levelUpAudio;

        public void RegisterUnit(IUnit unit)
        {
            unit.OnExperienceGain += DisplayExperiencePop;
            unit.OnDamage += DisplayDamagePop;
            unit.OnLevelUp += HandleLevelUp;
        }

        private void DisplayExperiencePop(IUnit unit, int experienceWorth)
        {
            var popObject = Instantiate(experienceGainPop);
            popObject.transform.position = unit.GetGameObject().transform.position;
            var textObject = GameObjectHelper.FindChildByName(popObject, "ExperiencePop");
            var textScript = textObject.GetComponent<Text>();
            textScript.text = textScript.text.Replace("{expgain}", experienceWorth.ToString());
        }

        private void DisplayDamagePop(IUnit unit, int damage)
        {
            var popObject = Instantiate(damagePop);
            popObject.transform.position = unit.GetGameObject().transform.position;
            var textObject = GameObjectHelper.FindChildByName(popObject, "FloatingDMG");
            var textScript = textObject.GetComponent<Text>();
            textScript.text = textScript.text.Replace("{dmg}", damage.ToString());
        }

        private void HandleLevelUp(IUnit unit, int level)
        {
            var popObject = Instantiate(levelUpPop);
            popObject.transform.position = unit.GetGameObject().transform.position;
            GameManager.instance.audioManager.Play(levelUpAudio);
            
        }
    }
}
