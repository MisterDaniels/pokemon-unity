using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Monster.Characters;

namespace Map {

    public class Portal : MonoBehaviour, IPlayerTriggerable {
        
        public void OnPlayerTriggered(PlayerController player) {
            Debug.Log("Player entered Portal");
        }

    }

}