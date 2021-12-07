using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunGenDataTypes
{
    [Serializable]public class LocalGridDataType
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
            WaypointObject = newWaypoint;
            Traversable = walkable;
        }

        public bool ReturnTraversable()
        {
            return Traversable;
        }

        public GameObject ReturnWaypoint()
        {
            return WaypointObject;
        }
    }
}
