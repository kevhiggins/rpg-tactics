using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class EndSceneListener : MonoBehaviour
    {
        // Use this for initialization
        void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Scenes/MainScene");
            }
        }

    }
}
