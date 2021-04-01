using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Characters;

namespace Core.Mechanic {

    public interface IPlayerTriggerable {

        void OnPlayerTriggered(PlayerController player);

    }

}