using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature.Moves;

namespace Monster.Creature {

    [CreateAssetMenu(fileName = "Pokémon", menuName = "Pokémon/Create", order = 0)]

    public class PokemonBase : ScriptableObject {

        [SerializeField] string name;
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Sprite frontSprite;
        [SerializeField] Sprite backSprite;
        [SerializeField] Sprite iconSprite;

        [SerializeField] List<Sprite> walkDownSprites;
        [SerializeField] List<Sprite> walkUpSprites;
        [SerializeField] List<Sprite> walkRightSprites;
        [SerializeField] List<Sprite> walkLeftSprites;

        [SerializeField] PokemonType mainType;
        [SerializeField] PokemonType subType;
        [SerializeField] PokemonSize pokemonSize;

        // Base Stats
        [SerializeField] int maxHp;
        [SerializeField] int attack;
        [SerializeField] int defence;
        [SerializeField] int speed;
        [SerializeField] int spAttack;
        [SerializeField] int spDefence;

        [SerializeField] List<LearnableMove> learnableMoves;

        [SerializeField] Vector2 saddleLocation = new Vector2(0f, 0f);

        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        public Sprite FrontSprite {
            get { return frontSprite; }
        }

        public Sprite BackSprite {
            get { return backSprite; }
        }

        public Sprite IconSprite {
            get { return iconSprite; }
        }

        public PokemonType MainType {
            get { return mainType; }
        }

        public PokemonType SubType {
            get { return subType; }
        }

        public PokemonSize PokemonSize {
            get { return pokemonSize; }
        }

        public int MaxHp {
            get { return maxHp; }
        }

        public int Attack {
            get { return attack; }
        }

        public int Defence {
            get { return defence; }
        }

        public int Speed {
            get { return speed; }
        }

        public int SpAttack {
            get { return spAttack; }
        }

        public int SpDefence {
            get { return spDefence; }
        }

        public List<LearnableMove> LearnableMoves {
            get { return learnableMoves; }
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

        public Vector2 SaddleLocation {
            get { return saddleLocation; }
        }

        public float GetPokemonSizeMultiplier(bool isOverworld = false) {
            switch (pokemonSize) {
                case PokemonSize.Small: {
                    return 1f;
                } case PokemonSize.Medium: {
                    if (isOverworld) {
                        return 1.1f;
                    }

                    return 2f;
                } case PokemonSize.Large: {
                    if (isOverworld) {
                        return 1.2f;
                    }

                    return 3f;
                } case PokemonSize.ExtraLarge: {
                    if (isOverworld) {
                        return 1.3f;
                    }

                    return 4f;
                } default: {
                    if (isOverworld) {
                        return 1.4f;
                    }

                    return 1f;
                }
            }
        }

        public Color GetPokemonMainTypeColor() {
            switch (mainType) {
                case PokemonType.None: {
                    return new Color32(104, 160, 144, 255);
                } case PokemonType.Normal: {
                    return new Color32(168, 168, 120, 255);
                } case PokemonType.Fire: {
                    return new Color32(240, 128, 48, 255);
                } case PokemonType.Water: {
                    return new Color32(104, 144, 240, 255);
                } case PokemonType.Electric: {
                    return new Color32(248, 208, 48, 255);
                } case PokemonType.Grass: {
                    return new Color32(120, 200, 80, 255);
                } case PokemonType.Ice: {
                    return new Color32(152, 216, 216, 255);
                } case PokemonType.Fighting: {
                    return new Color32(192, 48, 40, 255);
                } case PokemonType.Poison: {
                    return new Color32(160, 64, 160, 255);
                } case PokemonType.Ground: {
                    return new Color32(224, 192, 104, 255);
                } case PokemonType.Flying: {
                    return new Color32(168, 144, 240, 255);
                } case PokemonType.Psychic: {
                    return new Color32(248, 88, 136, 255);
                } case PokemonType.Bug: {
                    return new Color32(168, 184, 32, 255);
                } case PokemonType.Rock: {
                    return new Color32(184, 160, 56, 255);
                } case PokemonType.Ghost: {
                    return new Color32(112, 88, 152, 255);
                } case PokemonType.Dark: {
                    return new Color32(112, 88, 72, 255);
                } case PokemonType.Steel: {
                    return new Color32(184, 184, 208, 255);
                } case PokemonType.Fairy: {
                    return new Color32(238, 153, 172, 255);
                } default: {
                    return new Color32(0, 0, 0, 255);
                }
            }
        }

    }

    [System.Serializable]
    public class LearnableMove {
        
        [SerializeField] MoveBase moveBase;
        [SerializeField] int level;

        public MoveBase Base {
            get { return moveBase; }
        }

        public int Level {
            get { return level; }
        }

    }

    public enum PokemonType {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy
    }

    public enum PokemonSize {
        Small,
        Medium,
        Large,
        ExtraLarge
    }

    public enum Stat {
        Attack,
        Defence,
        SpAttack,
        SpDefence,
        Speed
    }

    public class TypeChart {

        static float[][] chart = {
            //                    NOR   FIR   WAT   ELE   GRA   ICE   FIG   POI   GRO   FLY   PSY   BUG   ROC   GHO   DRA   DAR   STE   FAI
            /*NOR*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0f,   1f,   1f,   0.5f, 1f },
            /*FIR*/ new float[] { 1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f },
            /*WAT*/ new float[] { 1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f },
            /*ELE*/ new float[] { 1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f },
            /*GRA*/ new float[] { 1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f },
            /*ICE*/ new float[] { 1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f },
            /*FIG*/ new float[] { 2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f },
            /*POI*/ new float[] { 1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f },
            /*GRO*/ new float[] { 1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   1f,   2f,   1f },
            /*FLY*/ new float[] { 1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f },
            /*PSY*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f },
            /*BUG*/ new float[] { 1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f },
            /*ROC*/ new float[] { 1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f },
            /*GHO*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f },
            /*DRA*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   2f,   1f,   0.5f, 0f },
            /*DAR*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f },
            /*STE*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   1f,   0.5f, 2f },
            /*FAI*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   2f,   2f,   0.5f, 1f }
        };

        public static float GetEffectiveness(PokemonType attackType, PokemonType defenceType) {
            if (attackType == PokemonType.None || defenceType == PokemonType.None) {
                return 1;
            }

            int row = (int) attackType - 1;
            int col = (int) defenceType - 1;

            return chart[row][col];
        }

    }

}