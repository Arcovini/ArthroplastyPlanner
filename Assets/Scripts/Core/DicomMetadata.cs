using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class DicomMetadata : MonoBehaviour
    {
        [Header("Metadata")]

        [Header("Patient")]
        [ReadOnly] public string PatientName;
        [ReadOnly] public string PatientID;
        [ReadOnly] public string PatientSex;

        [Header("Image Plane")]
        [ReadOnly] public float SliceThickness;
        [ReadOnly] public float[] ImagePositionPatient;
        [ReadOnly] public float[] ImageOrientationPatient;
        [ReadOnly] public float[] PixelSpacing;

        [Header("VOI LUT")]
        [ReadOnly] public float WindowWidth;
        [ReadOnly] public float WindowCenter;

        [Header("Modality LUT")]
        [ReadOnly] public float RescaleSlope;
        [ReadOnly] public float RescaleIntercept;
        
        [Header("Volume")]
        [ReadOnly] public int VoxelWidth;
        [ReadOnly] public int VoxelHeight;
        [ReadOnly] public int VoxelLength;
    
        [ReadOnly] public float PhysicalWidth;
        [ReadOnly] public float PhysicalHeight;
        [ReadOnly] public float PhysicalLength;

        public void SetDicomData(DICOM dicom)
        {
            PatientName = dicom.PatientName;
            PatientID = dicom.PatientID;
            PatientSex = dicom.PatientSex;

            SliceThickness = dicom.SliceThickness;
            ImagePositionPatient = dicom.ImagePositionPatient;
            ImageOrientationPatient = dicom.ImageOrientationPatient;
            PixelSpacing = dicom.PixelSpacing;

            WindowWidth = dicom.WindowWidth;
            WindowCenter = dicom.WindowCenter;

            RescaleSlope = dicom.RescaleSlope;
            RescaleIntercept = dicom.RescaleIntercept;
        }

        public void SetVolumeData(Volume volume)
        {
            VoxelWidth = volume.VoxelWidth;
            VoxelHeight = volume.VoxelHeight;
            VoxelLength = volume.VoxelLength;

            PhysicalWidth = volume.PhysicalWidth;
            PhysicalHeight = volume.PhysicalHeight;
            PhysicalLength = volume.PhysicalLength;
        }
    }
}