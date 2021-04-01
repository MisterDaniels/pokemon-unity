using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Mechanic;
using Monster.Characters;
using Core;

namespace Map {

    public enum DestinationIdentifier { 
        A, 
        B, 
        C, 
        D, 
        E 
    }

    public class Portal : MonoBehaviour, IPlayerTriggerable {
        
        [SerializeField] int sceneToLoadId = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destinationPortal;

        public Transform SpawnPoint => spawnPoint;

        PlayerController player;

        public void OnPlayerTriggered(PlayerController player) {
            this.player = player;
            StartCoroutine(SwitchScene());
        }

        IEnumerator SwitchScene() {
            DontDestroyOnLoad(gameObject);
            
            GameController.Instance.PauseGame(true);

            yield return SceneManager.LoadSceneAsync(sceneToLoadId);

            var destPortal = FindObjectsOfType<Portal>().First(portal => portal != this && 
                portal.destinationPortal == this.destinationPortal);
            player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

            GameController.Instance.PauseGame(false);

            Destroy(gameObject);
        }

    }

}