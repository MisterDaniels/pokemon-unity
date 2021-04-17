using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util {

    public abstract class GameUtils : MonoBehaviour {

        public void SetCharacterPositionAndSnapToTile(Vector2 pos) {
            // 2.3 -> Floor -> 2 -> 2.5
            pos.x = Mathf.Floor(pos.x) + 0.5f;
            pos.y = Mathf.Ceil(pos.y);

            transform.position = pos;
        }

        public void SetObjectPositionAndSnapToTile(Vector2 pos) {
            pos.x = Mathf.Floor(pos.x) + 0.5f;
            pos.y = Mathf.Ceil(pos.y) - 0.5f;

            transform.position = pos;
        }

    }

}