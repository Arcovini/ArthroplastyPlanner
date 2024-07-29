using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class FriedmanView : MonoBehaviour
    {
        private LoadButtonView loadButtonView;
        private ConfirmButtonView confirmButtonView;
        private SliceViewerView axialSliceView;
        private SliceViewerView coronalSliceView;

        public void Awake()
        {
            VisualElement root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

            var loadButton = root.Q<Button>("LoadButton");
            var confirmButton = root.Q<Button>("ConfirmButton");
            var axialSliceViewer = root.Q<VisualElement>("AxialSliceViewer");
            var coronalSliceViewer = root.Q<VisualElement>("CoronalSliceViewer");

            this.loadButtonView = new LoadButtonView(loadButton);
            this.confirmButtonView = new ConfirmButtonView(confirmButton);
            this.axialSliceView = new SliceViewerView(axialSliceViewer);
            this.coronalSliceView = new SliceViewerView(coronalSliceViewer);
        }

        public void SetSliceViews(Volume volume)
        {   
            Plane XZ = new Plane(volume.BoundsMax.x - volume.BoundsMin.x, volume.BoundsMax.z - volume.BoundsMin.z);
            Plane ZY = new Plane(volume.BoundsMax.z - volume.BoundsMin.z, volume.BoundsMax.y - volume.BoundsMin.y);

            Slice axialSlice = new Slice(volume, XZ.Width, XZ.Height, volume.Position, new Vector3(90.0f, 0.0f, 0.0f));
            Slice coronalSlice = new Slice(volume, ZY.Width, ZY.Height, volume.Position, new Vector3(0.0f, 90.0f, 0.0f));

            this.axialSliceView.SetView(axialSlice);
            this.coronalSliceView.SetView(coronalSlice);
        }
    }
}