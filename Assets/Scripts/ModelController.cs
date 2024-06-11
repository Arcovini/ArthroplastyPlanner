using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class ModelController
    {
        public static GameObject model = null;

        public const float translationStepSize = 5.0f;
        public const float rotationStepSize = 30.0f;

        public const float fixedTranslationStepSize = 0.5f;
        public const float fixedRotationStepSize = 3.0f;

        public static void Translate(Vector3 dir) => model.transform.Translate(dir * translationStepSize * Time.deltaTime, Space.World);
        public static void Rotate(Vector3 axis) => model.transform.Rotate(axis * rotationStepSize * Time.deltaTime, Space.Self);
       
        public static void FixedStepTranslate(Vector3 dir) => model.transform.Translate(dir * fixedTranslationStepSize, Space.World);
        public static void FixedStepRotate(Vector3 axis) => model.transform.Rotate(axis * fixedRotationStepSize, Space.Self);

    

    }
}