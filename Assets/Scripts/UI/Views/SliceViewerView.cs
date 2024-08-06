using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace AP
{
    public class SliceViewerView : IDisposable
    {
        public VisualElement SliceView;
        public Slider Slider;

        public Slice Slice = null;
        public SliceCamera Camera = null;

        public SliceViewerView(VisualElement root)
        {
            SliceView = root.Q<VisualElement>("Slice");
            Slider = root.Q<Slider>("Slider");

            SliceView.RegisterCallback<WheelEvent>(OnMouseScroll);
            Slider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);
        }

        public void SetView(Slice slice)
        {
            Slice = slice;
            SetSlicePosition(Slider.value);

            var camObj = GameObject.Instantiate(Resources.Load("Prefabs/SliceCamera")) as GameObject;

            Camera = camObj.GetComponent<SliceCamera>();
            Camera.Init(Slice);

            SliceView.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(Camera.RenderTexture));
        }

        public void OnSliderValueChanged(ChangeEvent<float> e)
        {
            if(Slice is not null)
                SetSlicePosition(e.newValue);
        }

        private void SetSlicePosition(float value)
        {
            // Position the ray at the center of the volume and have it facing the plane's normal direction
            Ray r = new Ray();
            r.origin = Slice.TargetVolume.Position;
            r.direction = Slice.Transform.forward;
            
            // Get the distance to the closest point inside the volume
            float t = 0.0f;
            Slice.TargetVolume.Collider.bounds.IntersectRay(r, out t);

            // This distance is the maximum travel distance we can go within the volume in that direction
            float distance = Mathf.Abs(t);

            // Place the origin at the border of the volume
            r.origin = r.origin - r.direction * distance;

            // Set the slice position inside the volume according to the value (0-1) in relation with the maximum travel distance
            Slice.Position = r.GetPoint(2.0f * distance * Mathf.Clamp(value, 0.01f, 0.99f));
        }

        private void OnMouseScroll(WheelEvent e)
        {
            if(Camera is not null)
            {
                float xoffset =  (2 * e.localMousePosition.x - SliceView.resolvedStyle.width) / SliceView.resolvedStyle.width;
                float yoffset = -(2 * e.localMousePosition.y - SliceView.resolvedStyle.height) / SliceView.resolvedStyle.height;
                
                Camera.Zoom(new Vector2(xoffset, yoffset), e.delta.y);
            }
        }

        public void Dispose()
        {
            SliceView.UnregisterCallback<WheelEvent>(OnMouseScroll);
            Slider.UnregisterCallback<ChangeEvent<float>>(OnSliderValueChanged);
        }
    }
}