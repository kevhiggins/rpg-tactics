using UnityEngine;

namespace Rpg.Unity
{
    public class AnimationAutoDestroy : MonoBehaviour
    {
        public float delay = 0f;
        public bool destroyParent = false;

        // Use this for initialization
        void Start()
        {
            var target = gameObject;
            if (destroyParent)
            {
                target = gameObject.transform.parent.gameObject;
            }
            Destroy(target, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
    }
}