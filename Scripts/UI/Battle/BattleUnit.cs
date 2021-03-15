using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;
using DG.Tweening;

namespace UI.Battle {

    public class BattleUnit : MonoBehaviour {

        [SerializeField] PokemonBase _base;
        [SerializeField] int level;
        [SerializeField] bool isPlayerUnit;

        public Pokemon Pokemon { get; set; }

        private Image image;
        private Vector3 originalPos;
        private Color originalColor;

        private void Awake() {
            image = GetComponent<Image>();
            originalPos = image.transform.localPosition;
            originalColor = image.color;
        }

        public void Setup(Pokemon pokemon) {
            Pokemon = pokemon;

            if (isPlayerUnit) {
                image.sprite = Pokemon.Base.BackSprite;
            } else {
                image.sprite = Pokemon.Base.FrontSprite;
            }

            image.color = originalColor;

            PlayEnterAnimation();
        }

        public void PlayEnterAnimation() {
            if (isPlayerUnit) {
                image.transform.localPosition = new Vector3(-500f, originalPos.y);
            } else {
                image.transform.localPosition = new Vector3(500f, originalPos.y);
            }

            image.transform.DOLocalMoveX(originalPos.x, 1f);
        }

        public IEnumerator PlayAttackAnimation() {
            var sequence = DOTween.Sequence();

            if (isPlayerUnit) {
                sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
            } else {
                sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 25f, 0.25f));
            }

            sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
            yield return new WaitForSeconds(1f);
        }

        public void PlayHitAnimation() {
            var sequence = DOTween.Sequence();

            sequence.Append(image.DOColor(Color.gray, 0.1f));
            sequence.Append(image.DOColor(originalColor, 0.1f));
        }

        public void PlayFaintAnimation() {
            var sequence = DOTween.Sequence();

            sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 50f, 0.5f));

            sequence.Join(image.DOFade(0f, 0.5f));
        }

    }

}