using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pokemon;

namespace UI.Battle {

    public class BattleSystem : MonoBehaviour {

        [SerializeField] BattleUnit playerUnit;
        [SerializeField] BattleUnit enemyUnit;
        [SerializeField] BattleHud playerHud;
        [SerializeField] BattleHud enemyHud;

        private void Start() {
            SetupBattle();
        }

        public void setupBattle() {
            playerUnit.Setup();
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup();
            enemyHud.SetData(enemyUnit.Pokemon);
        }

    }

}