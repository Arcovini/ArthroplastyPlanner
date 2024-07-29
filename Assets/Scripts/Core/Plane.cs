using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class Plane
    {
        public float Width = 1.0f;
        public float Height = 1.0f;

        public Plane()
        {
            
        }

        public Plane(float width = 1.0f, float height = 1.0f)
        {
            Width = width;
            Height = height;
        }
    }
}