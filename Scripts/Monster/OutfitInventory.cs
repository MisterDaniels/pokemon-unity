using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Characters;

namespace Monster.Outfits {

    public class OutfitInventory : MonoBehaviour {

        [SerializeField] List<OutfitBase> outfits;
        [SerializeField] int currentOutfitIndex = 0;

        private void Awake() {
            RefreshOutfit();
        }

        public void AddOutfit(OutfitBase outfitToAdd) {
            foreach (OutfitBase outfit in outfits) {
                if (outfit.Name == outfitToAdd.Name) {
                    return;
                }
            }

            outfits.Add(outfitToAdd);
        }

        public void ChangeOutfit(int oufitIndex) {
            currentOutfitIndex = oufitIndex;
            RefreshOutfit();
        }

        public void RefreshOutfit() {
            gameObject.GetComponent<Character>().ChangeSprites(outfits[currentOutfitIndex]);
        }

    }

}