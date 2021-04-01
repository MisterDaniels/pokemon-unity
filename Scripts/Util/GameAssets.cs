using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Monster.Creature;
using Monster.Characters;
using UI.Battle;
using Map;
using UI;

namespace Core {

    [System.Serializable]
    public class GameAssets : MonoBehaviour {
        
        public static GameAssets i { get; set; }

        private void Awake() {
            i = this;
        }

        private Dictionary<string, string> DataPath { 
            get {
                return new Dictionary<string, string> {
                    { "pokemon", Path.Combine(Application.dataPath, "/Data/monster/creature/pokemon.json") },
                    { "party", Path.Combine(Application.dataPath, "/Data/monster/character/party.json") }
                };
            }
        }

        public PokemonBase LoadPokemon(int pokemonId) {            
            if (File.Exists(Application.dataPath + DataPath["pokemon"])) {
                return JsonUtility.FromJson<PokemonBase>(File.ReadAllText(DataPath["pokemon"]));
            }

            return new PokemonBase();
        }

        public PokemonParty LoadPokemonParty(int trainerId) {
            if (File.Exists(DataPath["party"])) {
                return JsonUtility.FromJson<PokemonParty>(File.ReadAllText(DataPath["party"]));
            }

            return new PokemonParty();
        }

        public void SavePokemonParty(PokemonParty pokemonParty) {
            File.WriteAllText(DataPath["party"], JsonUtility.ToJson(pokemonParty));
        }

        
    }

}