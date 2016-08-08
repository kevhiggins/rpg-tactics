using Rpg.Map;
using UnityEngine;

namespace Rpg.Unit
{
    public interface IUnit : ITileChild
    {
        GameObject GetGameObject();
        int MovementSpeed { get; }
        
    }
}
