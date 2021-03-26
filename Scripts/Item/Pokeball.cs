using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Items {
    
    [CreateAssetMenu(fileName = "Pokeball", menuName = "Item/Pokeball/Create", order = 0)]
    public class Pokeball : ItemBase {

        [SerializeField] Pokemon pokemon;
        [SerializeField] int catchRate;

        public Pokemon Pokemon {
            get { return pokemon; }
        }

        public override ItemType GetType() {
            if (pokemon != null) {
                return ItemType.Pokemon;
            }

            return ItemType.Catch;
        }

        public override void Use() {
            Debug.Log("Pokeball was used");
        }

    }

}