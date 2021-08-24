using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature.Moves;
using Monster.Creature.Data;

namespace Monster.Creature {

    [System.Serializable]
    public class Pokemon {

        [SerializeField] PokemonBase _base;
        [SerializeField] int level;

        public PokemonBase Base { 
            get {
                return _base;
            } 
        }
        public int Level { 
            get {
                return level;
            } 
        }

        public int HP { get; set; }
        public List<Move> Moves { get; set; }
        public Dictionary<Stat, int> Stats { get; private set; }
        public Dictionary<Stat, int> StatBoosts { get; private set; }
        public Condition Status { get; private set; }
        public int StatusTime { get; set; }

        public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
        public bool HPChanged { get; set; }

        public void Init() {
            Moves = new List<Move>();
            foreach (var move in Base.LearnableMoves) {
                if (move.Level <= Level) {
                    Moves.Add(new Move(move.Base));
                }

                if (Moves.Count == 4) {
                    break;
                }
            }

            CalculateStats();
            HP = MaxHp;

            ResetStatBoost();
        }

        private void ResetStatBoost() {
            StatBoosts = new Dictionary<Stat, int>() {
                { Stat.Attack, 0 },
                { Stat.Defence, 0 },
                { Stat.Speed, 0 },
                { Stat.SpAttack, 0 },
                { Stat.SpDefence, 0 }
            };
        }

        private void CalculateStats() {
            Stats = new Dictionary<Stat, int>();
            Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
            Stats.Add(Stat.Defence, Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5);
            Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);
            Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
            Stats.Add(Stat.SpDefence, Mathf.FloorToInt((Base.SpDefence * Level) / 100f) + 5);

            MaxHp = Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10;
        }

        public int MaxHp { get; private set; }

        public int GetStat(Stat stat) {
            int statVal = Stats[stat];
            
            // TODO Stat boost
            int boost = StatBoosts[stat];
            var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

            if (boost >= 0 ) {
                statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
            } else {
                statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
            }

            return statVal;
        }

        public void ApplyBoosts(List<StatBoost> statBoosts) {
            foreach (StatBoost statBoost in statBoosts) {
                var stat = statBoost.stat;
                var boost = statBoost.boost;

                StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

                if (boost > 0) {
                    StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
                } else {
                    StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
                }

                Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
            }
        }

        public int Attack {
            // Pokémon original formula
            get { return GetStat(Stat.Attack); }
        }

        public int Defence {
            // Pokémon original formula
            get { return GetStat(Stat.Defence); }
        }

        public int Speed {
            // Pokémon original formula
            get { return GetStat(Stat.Speed); }
        }

        public int SpAttack {
            // Pokémon original formula
            get { return GetStat(Stat.SpAttack); }
        }

        public int SpDefence {
            // Pokémon original formula
            get { return GetStat(Stat.SpDefence); }
        }

        public DamageDetails TakeDamage(Move move, Pokemon attacker) {
            float critical = 1f;
            if (Random.value * 100f <= 6.25f) {
                critical = 2f;
            }

            // Pokémon original formula
            float TypeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, Base.MainType) * TypeChart.GetEffectiveness(move.Base.Type, Base.SubType);

            var DamageDetails = new DamageDetails() {
                TypeEffectiveness = TypeEffectiveness,
                Critical = critical,
                Fainted = false
            };

            float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
            float defence = (move.Base.Category == MoveCategory.Special) ? SpDefence : Defence;

            float modifiers = Random.Range(0.85f, 1f) * TypeEffectiveness * critical;
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * move.Base.Power * ((float) attack / defence ) + 2;
            int damage = Mathf.FloorToInt(d * modifiers);

            UpdateHP(damage);

            return DamageDetails;
        }

        public Move GetRandomMove() {
            int r = Random.Range(0, Moves.Count);

            return Moves[r];
        }

        public void OnBattleOver() {
            ResetStatBoost();
        }

        public void SetStatus(ConditionID conditionId) {
            Status = ConditionsDB.Conditions[conditionId];
            Status?.OnStart?.Invoke(this);
            StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");
        }

        public void CureStatus() {
            Status = null;
        }

        public void UpdateHP(int damage) {
            HP = Mathf.Clamp(HP - damage, 0, MaxHp);
            HPChanged = true;
        }

        public bool OnBeforeMove() {
            if (Status?.OnBeforeMove != null) {
                return Status.OnBeforeMove(this);
            }

            return true;
        }

        public void OnAfterTurn() {
            Status?.OnAfterTurn?.Invoke(this);
        }

    }

    public class DamageDetails {

        public bool Fainted { get; set; }
        public float Critical { get; set; }
        public float TypeEffectiveness { get; set; }

    }

}