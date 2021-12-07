using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGenDataTypes
{
    public class LocalGridDataType
    {
        private GameObject waypointObject;
        public GameObject WaypointObject
        {
            get
            {
                return waypointObject;
            }
            set
            {
                waypointObject = value;
            }
        }
        private bool traversable;
        public bool Traversable
        {
            get
            {
                return traversable;
            }
            set
            {
                traversable = value;
            }
        }

        public LocalGridDataType(GameObject newWaypoint, bool walkable)
        {
            waypointObject = newWaypoint;
            traversable = walkable;
        }
    }
}
