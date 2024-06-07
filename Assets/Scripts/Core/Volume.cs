using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using System;
using System.Globalization;

namespace AP
{
    public class Volume : MonoBehaviour
    {
        public Texture3D Texture = null;

        [ReadOnly] public int VoxelWidth;
        [ReadOnly] public int VoxelHeight;
        [ReadOnly] public int VoxelLength;
        
        [ReadOnly] public float PhysicalWidth;
        [ReadOnly] public float PhysicalHeight;
        [ReadOnly] public float PhysicalLength;
    
        public void SetTransform()
        {
            DICOM dicom = GetComponent<DICOM>();

            float pixelRowSpacing = Convert.ToSingle(dicom.PixelSpacing[0], CultureInfo.InvariantCulture);
            float pixelColumnSpacing = Convert.ToSingle(dicom.PixelSpacing[1], CultureInfo.InvariantCulture);
            float thickness = Convert.ToSingle(dicom.SliceThickness, CultureInfo.InvariantCulture);

            PhysicalHeight = pixelRowSpacing * VoxelHeight + pixelRowSpacing;
            PhysicalWidth = pixelColumnSpacing * VoxelWidth + pixelColumnSpacing;
            PhysicalLength = thickness * VoxelLength;

            // Scale
            transform.localScale = new Vector3(PhysicalWidth, PhysicalHeight, PhysicalLength).MillimetersToMeters();

            // Rotation
            int length = dicom.ImageOrientationPatient.Length;
            float[] orientation = new float[length];
            
            for(int i = 0; i < length; i++)
                orientation[i] = Convert.ToSingle(dicom.ImageOrientationPatient[i], CultureInfo.InvariantCulture);

            Vector3 rowOrientation = new Vector3(orientation[0], orientation[1], orientation[2]);
            Vector3 columnOrientation = new Vector3(orientation[3], orientation[4], orientation[5]);
            Vector3 depthOrientation = Vector3.Cross(rowOrientation, columnOrientation);

            // DICOM LPS Y axis swapped with Unity's Z axis
            Matrix4x4 localToWorld = new Matrix4x4();
            localToWorld.SetColumn(0, new Vector4(rowOrientation.x, rowOrientation.z, rowOrientation.y, 0.0f));
            localToWorld.SetColumn(1, new Vector4(columnOrientation.x, columnOrientation.z, columnOrientation.y, 0.0f));
            localToWorld.SetColumn(2, new Vector4(depthOrientation.x, columnOrientation.z, columnOrientation.y, 0.0f));
            localToWorld.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            Vector3 forward;
            forward.x = localToWorld.m02;
            forward.y = localToWorld.m12;
            forward.z = localToWorld.m22;

            Vector3 upwards;
            upwards.x = localToWorld.m01;
            upwards.y = localToWorld.m11;
            upwards.z = localToWorld.m21;

            transform.rotation = Quaternion.LookRotation(forward, upwards);
        }
    }
}