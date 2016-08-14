using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unity
{
    public static class AnimatorExtension
    {
        private static Dictionary<int, Dictionary<string, bool>> parameterCache = new Dictionary<int, Dictionary<string, bool>>();

        public static bool HasParameter(this Animator animator, string parameterName)
        {
            var animatorID = animator.GetInstanceID();

            // If animator ID doesn't exist in cache, then add it.
            if (!parameterCache.ContainsKey(animatorID))
            {
                parameterCache.Add(animatorID, new Dictionary<string, bool>());
            }

            // Check to see if the cached animator data has the parameterName
            if (parameterCache[animatorID].ContainsKey(parameterName))
            {
                return parameterCache[animatorID][parameterName];
            }

            var result = false;

            // Search the animator parameters to see if the parameter exists.
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == parameterName)
                {
                    result = true;
                    break;
                }
            }
            
            // Add the result to the cache, and return the result.
            parameterCache[animatorID].Add(parameterName, result);
            return result;
        }
    }
}