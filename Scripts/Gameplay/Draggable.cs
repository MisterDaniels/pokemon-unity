using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Core.Mechanic {

    public interface Draggable {

        Item GetItem();

        void SetItem(Item item);

    }

}