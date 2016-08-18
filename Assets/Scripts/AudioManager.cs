
using System.Collections.Generic;
using UnityEngine;

namespace Rpg
{
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// How often in seconds to check the played game objects to see if they are complete.
        /// </summary>
        public float audioCullThreshold;

        private List<AudioSource> audioSourceList = new List<AudioSource>();
        private float secondsSinceLastCull;

        void Update()
        {
            secondsSinceLastCull += Time.deltaTime;
            if (secondsSinceLastCull >= audioCullThreshold)
            {
                CullAudio();
                secondsSinceLastCull = 0;
            }
        }

        private void CullAudio()
        {
            var destroyAudios = new List<AudioSource>();
            foreach (var audioSource in audioSourceList)
            {
                if (audioSource.isPlaying == false)
                {
                    destroyAudios.Add(audioSource);
                }
            }

            // Destroy the audios that are ready to be culled.
            foreach (var audioSource in destroyAudios)
            {
                audioSourceList.Remove(audioSource);
                Destroy(audioSource.gameObject);
            }
        }

        public void Play(GameObject audioObject)
        {
            var audioInstance = Instantiate(audioObject);
            var audioSource = audioInstance.GetComponent<AudioSource>();
            audioSource.Play();
            audioSourceList.Add(audioSource);
        }
    }
}
