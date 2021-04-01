using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {

    public class EssentialObjects : MonoBehaviour {

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

    }

}