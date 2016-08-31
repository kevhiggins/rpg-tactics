using System;
using DG.Tweening;
using Rpg.Map;
using UnityEngine;

namespace Rpg
{
    public delegate void HandleMoveComplete();

    public class CameraManager : MonoBehaviour
    {
        public int panTileThreshold = 2;
        public float cameraPanTilesPerSecond = 0.1f;

        public bool IsMoving { get; private set; }

        public event HandleMoveComplete OnMoveComplete = () => { };

        public void CheckCamera(TilePosition tilePosition)
        {
            var map = GameManager.instance.levelManager.GetMap();

            // Get the tile at the position of the camera.
            var cameraTile = map.FindTileAtPosition(Camera.main.transform.position);
            var cameraTilePosition = cameraTile.tilePosition;

            var xDifference = tilePosition.x - cameraTilePosition.x;
            var yDifference = tilePosition.y - cameraTilePosition.y;

            var xNewTarget = cameraTilePosition.x;
            var yNewTarget = cameraTilePosition.y;

            // If the current tile is more than x tiles away from the current tile, then move one tile in that direction.
            if (Math.Abs(xDifference) > panTileThreshold)
            {
                var offset = xDifference > 0 ? -panTileThreshold : panTileThreshold;
                xNewTarget += xDifference + offset;
            }

            if (Math.Abs(yDifference) > panTileThreshold)
            {
                var offset = yDifference > 0 ? -panTileThreshold : panTileThreshold;
                yNewTarget += yDifference + offset;
            }

            var targetTilePosition = new TilePosition(xNewTarget, yNewTarget);

            if (!targetTilePosition.Equals(cameraTilePosition))
            {
                var newCameraPosition =
                    GameManager.instance.levelManager.GetMap().GetTile(targetTilePosition).GetPosition();
                newCameraPosition.z = Camera.main.transform.position.z;

                var tileDistance = Math.Max(Math.Abs(targetTilePosition.x - cameraTilePosition.x),
                    Math.Abs(targetTilePosition.y - cameraTilePosition.y));

                IsMoving = true;
                Camera.main.transform.DOMove(newCameraPosition, tileDistance*cameraPanTilesPerSecond)
                    .OnComplete(() =>
                    {
                        IsMoving = false;
                        OnMoveComplete();
                    });
            }
        }
    }
}