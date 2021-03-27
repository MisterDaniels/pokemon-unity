using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Core.Mechanic {

    public class DayAndNightCycle : MonoBehaviour {

        [SerializeField] private Gradient lightColor;
        [SerializeField] private Light2D globalLight;

        private int days;
        private float time = 50;
        private bool canChangeDay = true;

        public int Days => days;

        public delegate void OnDayChanged();
        public OnDayChanged DayChanged;

        private void Update() {
            if (time > 500) {
                time = 0;
            }

            if ((int) time == 250 && canChangeDay) {
                canChangeDay = false;
                DayChanged();
                days++;
            }

            if ((int) time == 250) {
                canChangeDay = true;
            }

            time += Time.deltaTime;
            globalLight.color = lightColor.Evaluate(time * 0.002f);
        }

    }

}