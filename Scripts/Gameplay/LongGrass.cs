using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Monster.Characters;
using Core;

namespace Map {

    public class LongGrass : MonoBehaviour, IPlayerTriggerable {
        
        public void OnPlayerTriggered(PlayerController player) {
            Debug.Log("Player entered Grass");

            if (UnityEngine.Random.Range(1, 101) <= 10) {
                GameController.Instance.StartBattle();
            }
        }

    }

}
