using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGenDataTypes
{
    public class LocalGridDataType
    {
        private GameObject waypointObject;
        private bool traversable;

        public LocalGridDataType(GameObject newWaypoint, bool walkable)
        {
            waypointObject = newWaypoint;
            traversable = walkable;
        }
    }
}
