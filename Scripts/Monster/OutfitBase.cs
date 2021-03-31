using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Outfits {

    public enum OutfitType {
        Character,
        Pokemon
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Outfit", menuName = "Outfit/Create", order = 0)]
    public class OutfitBase : ScriptableObject {

        [SerializeField] string name;
        [SerializeField] OutfitType outfitType;

        [SerializeField] List<Sprite> walkDownSprites;
        [SerializeField] List<Sprite> walkUpSprites;
        [SerializeField] List<Sprite> walkRightSprites;
        [SerializeField] List<Sprite> walkLeftSprites; 

        public string Name {
            get { return name; }
        }

        public OutfitType OutfitType {
            get { return outfitType; }
        }

        public List<Sprite> WalkDownSprites {
            get { return walkDownSprites; }
        }

        public List<Sprite> WalkUpSprites {
            get { return walkUpSprites; }
        }

        public List<Sprite> WalkRightSprites {
            get { return walkRightSprites; }
        }

        public List<Sprite> WalkLeftSprites {
            get { return walkLeftSprites; }
        }

    }

}