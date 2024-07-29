using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AP
{
    public class FriedmanEvents
    {
        // Model Events
        public static Action OpenFileExplorer;
        public static Action<string> LoadDicom;
        public static Action<Slice, float> SetSlicePosition;

        // View Events
        public static Action<Volume> SetSliceViews;
    }
}