using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using System;

namespace AP
{
    public class Volume : IDisposable
    {
        public Texture3D Texture = null;

        public int VoxelWidth;
        public int VoxelHeight;
        public int VoxelLength;
    
        public float PhysicalWidth;
        public float PhysicalHeight;
        public float PhysicalLength;

        public Transform Transform
        {
            get { return this.boundingBox.transform; }
            private set {}
        }

        public Vector3 Position
        {
            get { return this.boundingBox.transform.position; }
            set { this.boundingBox.transform.position = value; }
        }

        public Collider Collider => this.boundingBox.GetComponent<Collider>();
        public Vector3 BoundsMin => this.boundingBox.GetComponent<Collider>().bounds.min;
        public Vector3 BoundsMax => this.boundingBox.GetComponent<Collider>().bounds.max;

        private GameObject boundingBox;

        public Volume(int voxelWidth, int voxelHeight, int voxelLength, DICOM dicom)
        {
            VoxelWidth = voxelWidth;
            VoxelHeight = voxelHeight;
            VoxelLength = voxelLength;

            float pixelRowSpacing = dicom.PixelSpacing[0];
            float pixelColumnSpacing = dicom.PixelSpacing[1];
            float sliceThickness = dicom.SliceThickness;

            PhysicalHeight = pixelRowSpacing * voxelHeight + pixelRowSpacing;
            PhysicalWidth = pixelColumnSpacing * voxelWidth + pixelColumnSpacing;
            PhysicalLength = sliceThickness * voxelLength;

            // TODO: add conversor to mm
            PhysicalHeight /= 1000.0f;
            PhysicalWidth  /= 1000.0f;
            PhysicalLength /= 1000.0f;

            SetBoundingBox(dicom);

            // Metadata
            DicomMetadata metadata = this.boundingBox.GetComponent<DicomMetadata>();
            metadata.SetDicomData(dicom);
            metadata.SetVolumeData(this);
        }

        public void SetBoundingBox(DICOM dicom)
        {
            this.boundingBox = GameObject.Instantiate(Resources.Load("Prefabs/DicomVolume")) as GameObject;

            // TODO: add position?

            // Scale
            this.boundingBox.transform.localScale = new Vector3(PhysicalWidth, PhysicalHeight, PhysicalLength);

            // TODO: refactor
            // Rotation
            var orientation = dicom.ImageOrientationPatient;

            Vector3 rowOrientation = new Vector3(orientation[0], orientation[1], orientation[2]);
            Vector3 columnOrientation = new Vector3(orientation[3], orientation[4], orientation[5]);
            Vector3 depthOrientation = Vector3.Cross(rowOrientation, columnOrientation);

            Matrix4x4 localToWorld = new Matrix4x4();
            localToWorld.SetColumn(0, new Vector4(rowOrientation.x, rowOrientation.y, rowOrientation.z, 0.0f));
            localToWorld.SetColumn(1, new Vector4(columnOrientation.x, columnOrientation.y, columnOrientation.z, 0.0f));
            localToWorld.SetColumn(2, new Vector4(depthOrientation.x, columnOrientation.y, columnOrientation.z, 0.0f));
            localToWorld.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            Vector3 forward;
            forward.x = localToWorld.m02;
            forward.y = localToWorld.m12;
            forward.z = localToWorld.m22;

            Vector3 upwards;
            upwards.x = localToWorld.m01;
            upwards.y = localToWorld.m11;
            upwards.z = localToWorld.m21;

            this.boundingBox.transform.rotation = Quaternion.LookRotation(forward, upwards);

            Physics.SyncTransforms();
        }

        public void Dispose()
        {
            GameObject.Destroy(this.boundingBox);
        }
    }
}