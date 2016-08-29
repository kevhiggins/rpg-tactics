using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Importer
{
    [Tiled2Unity.CustomTiledImporter]
    public class TiledImporter : Tiled2Unity.ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            Debug.Log(customProperties.Count);

            foreach (var test in customProperties)
            {
                Debug.Log(test.Key);
                Debug.Log(test.Value);
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}
