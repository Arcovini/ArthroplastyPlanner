using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class ModelController
    {
        public static GameObject model = null;

        public const float translationStepSize = 0.1f;
        public const float rotationStepSize = 1.0f;

        public const float fixedTranslationStepSize = 1.0f;
        public const float fixedRotationStepSize = 1.0f;

        public static void Translate(Vector3 dir) => model.transform.Translate(dir * translationStepSize * Time.deltaTime, Space.World);
        public static void Rotate(Vector3 axis) => model.transform.Rotate(axis * rotationStepSize * Time.deltaTime, Space.Self);
       
        public static void FixedStepTranslate(Vector3 dir) => model.transform.Translate(dir * fixedTranslationStepSize, Space.World);
        public static void FixedStepRotate(Vector3 axis) => model.transform.Rotate(axis * fixedRotationStepSize, Space.Self);

    }
}