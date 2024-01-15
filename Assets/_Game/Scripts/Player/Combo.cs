using System;
using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    [Serializable]
    public class Combo
    {
        public int comboid;
        
        public float appearrange, appearspeed, hitrange;

        public bool hitToFall;

        public Transform hitCenter;
    }
}