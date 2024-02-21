using System;
using UnityEngine;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float horizontalSpeed;
        public Vector2 clampValues;
        public float clampSpeed;
        
    }
    
}