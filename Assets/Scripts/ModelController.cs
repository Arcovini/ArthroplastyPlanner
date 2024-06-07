using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class ModelController
    {
        public static GameObject model = null;

        public static void Translate(Vector3 dir, float stepSize = 0.1f) => model.transform.Translate(dir * stepSize, Space.World);
        public static void Rotate(Vector3 axis, float stepSize = 3.0f) => model.transform.Rotate(axis * stepSize, Space.Self);
    }
}