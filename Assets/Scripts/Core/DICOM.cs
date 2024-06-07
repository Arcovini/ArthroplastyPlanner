using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using System;
using System.Linq;

namespace AP
{
    [RequireComponent(typeof(Volume))]
    public class DICOM : MonoBehaviour
    {
        // Patient
        [ReadOnly] public string PatientName;
        [ReadOnly] public string PatientID;
        [ReadOnly] public string PatientSex;

        // Image Plane
        [ReadOnly] public string SliceThickness;
        [ReadOnly] public string[] ImagePositionPatient;
        [ReadOnly] public string[] ImageOrientationPatient;
        [ReadOnly] public string[] PixelSpacing;

        // VOI LUT
        [ReadOnly] public float WindowWidth;
        [ReadOnly] public float WindowCenter;

        // Modality LUT
        [ReadOnly] public float RescaleSlope;
        [ReadOnly] public float RescaleIntercept;

        [System.NonSerialized] public DicomFile[] Files;
        [System.NonSerialized] public Volume Volume;

        public void Init(DicomFile[] files)
        {
            Files = files;

            // Preamble
            DicomFile file = Files.First();
            PatientName = file.Dataset.GetSingleValue<string>(DicomTag.PatientName);
            PatientID = file.Dataset.GetSingleValue<string>(DicomTag.PatientID);
            PatientSex = file.Dataset.GetSingleValue<string>(DicomTag.PatientSex);
            SliceThickness = file.Dataset.GetSingleValue<string>(DicomTag.SliceThickness);
            ImagePositionPatient = file.Dataset.GetValues<string>(DicomTag.ImagePositionPatient);
            ImageOrientationPatient = file.Dataset.GetValues<string>(DicomTag.ImageOrientationPatient);
            PixelSpacing = file.Dataset.GetValues<string>(DicomTag.PixelSpacing);
            WindowWidth = file.Dataset.GetSingleValue<float>(DicomTag.WindowWidth);
            WindowCenter = file.Dataset.GetSingleValue<float>(DicomTag.WindowCenter);
            RescaleSlope = file.Dataset.GetSingleValue<float>(DicomTag.RescaleSlope);
            RescaleIntercept = file.Dataset.GetSingleValue<float>(DicomTag.RescaleIntercept);
            
            LoadVolume();
        }

        private void LoadVolume()
        {
            Volume = GetComponent<Volume>();

            // Pixel Data
            DicomPixelData pixelData = DicomPixelData.Create(Files.First().Dataset);
            IPixelData dataInferface = PixelDataFactory.Create(pixelData, 0);

            Volume.VoxelWidth = pixelData.Width;
            Volume.VoxelHeight = pixelData.Height;
            Volume.VoxelLength = Files.Length;

            if(dataInferface is GrayscalePixelDataS16)
                LoadGrayscalePixelData();

            Volume.SetTransform();
        }

        private void LoadGrayscalePixelData()
        {
            int x = Volume.VoxelWidth;
            int y = Volume.VoxelHeight;
            int z = Volume.VoxelLength;

            Volume.Texture = new Texture3D(x, y, z, TextureFormat.RGB24, true);
            Color[] colors = new Color[x * y * z];

            z = 0;
            foreach(DicomFile file in Files)
            {
                DicomPixelData pixelData = DicomPixelData.Create(file.Dataset);
                IPixelData dataInferface = PixelDataFactory.Create(pixelData, 0);

                Color[] pixelColor = new Color[x * y];
                short[] data = (dataInferface as GrayscalePixelDataS16).Data;

                // Value range
                float minRange = WindowCenter - 0.5f * WindowWidth;
                float maxRange = WindowCenter + 0.5f * WindowWidth;

                for(int i = 0; i < data.Length; i++)
                {
                    // Convert data to Hounsfield Units (HU)
                    float hounsfield = (RescaleSlope * data[i]) + RescaleIntercept;

                    // Adjust range
                    float scale = (hounsfield + 0.5f * WindowWidth) / WindowWidth;

                    // Grayscale value
                    pixelColor[i] = new Color(scale, scale, scale);
                }

                Array.Copy(pixelColor, 0, colors, x * y * z, x * y);
                z++;   
            }

            Volume.Texture.SetPixels(colors);
            Volume.Texture.Apply();
        }
    }
}
