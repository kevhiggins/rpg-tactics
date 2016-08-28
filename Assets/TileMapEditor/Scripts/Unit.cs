using System;
using Rpg.Unit;
using UnityEditor;
using UnityEngine;

namespace TileMapEditor
{
    public class Unit : MonoBehaviour
    {
        public GameObject unitPrefab;

        // Updates the game objects Sprite Renderer/Animator from the prefab reference.
        public void UpdateDisplay()
        {
            var unitScript = unitPrefab.GetComponent<IUnit>();
            if(unitScript == null)
                throw new Exception("Invalid unit.");

            var animator = unitScript.GetAnimator();
            var spriteRenderer = unitScript.GetSpriteRenderer();

            if (animator != null)
            {
                var targetAnimator = gameObject.GetComponent<Animator>();
                if (targetAnimator == null)
                {
                    targetAnimator = gameObject.AddComponent<Animator>();
                }
                EditorUtility.CopySerializedIfDifferent(animator, targetAnimator);
            }

            if (spriteRenderer != null)
            {
                var targetSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (targetSpriteRenderer == null)
                {
                    targetSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                }
                EditorUtility.CopySerializedIfDifferent(spriteRenderer, targetSpriteRenderer);
            }
        }
    }
}
