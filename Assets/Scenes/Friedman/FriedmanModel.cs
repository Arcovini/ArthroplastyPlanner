using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace AP
{
    public class FriedmanModel : MonoBehaviour
    {
        private DICOM dicom = null;

        public void OpenFileExplorer()
        {
            string path = EditorUtility.OpenFolderPanel("Select a folder to open", "", "");
    
            if(String.IsNullOrEmpty(path))
                return;

            FriedmanEvents.LoadDicom?.Invoke(path);
        }

        public void LoadDicom(string path)
        {
            if(this.dicom is not null)
                this.dicom.Dispose();

            this.dicom = Loader.LoadDicom(path);

            FriedmanEvents.SetSliceViews?.Invoke(this.dicom.Volume);
        }

        // public void SetSlicePosition(Slice slice, float value)
        // {
        //     // Position the ray at the center of the volume and have it facing the plane's normal direction
        //     Ray r = new Ray();
        //     r.origin = Dicom.Volume.Position;
        //     r.direction = slice.Transform.forward;
            
        //     // Get the distance to the closest point inside the volume
        //     float t = 0.0f;
        //     Dicom.Volume.Collider.bounds.IntersectRay(r, out t);

        //     // This distance is the maximum travel distance we can go within the volume in that direction
        //     float distance = Mathf.Abs(t);

        //     // Place the origin at the border of the volume
        //     r.origin = r.origin - r.direction * distance;

        //     // Set the slice position inside the volume according to the value (0-1) in relation with the maximum travel distance
        //     slice.Position = r.GetPoint(2.0f * distance * Mathf.Lerp(0.01f, 0.99f, value));
        // }
    }
}