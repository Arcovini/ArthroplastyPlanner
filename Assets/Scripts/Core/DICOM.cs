using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using System;
using System.Globalization;
using System.Linq;

namespace AP
{
    public class DICOM : IDisposable
    {
        // Patient
        public string PatientName;
        public string PatientID;
        public string PatientSex;

        // Image Plane
        public float SliceThickness;
        public float[] ImagePositionPatient;
        public float[] ImageOrientationPatient;
        public float[] PixelSpacing;

        // VOI LUT
        public float WindowWidth;
        public float WindowCenter;

        // Modality LUT
        public float RescaleSlope;
        public float RescaleIntercept;
        
        public DicomFile[] Files;
        public Volume Volume;

        public DICOM(DicomFile[] files)
        {
            Files = files;

            // Preamble
            DicomFile file = Files.First();

            PatientName = file.Dataset.GetSingleValue<string>(DicomTag.PatientName);
            PatientID = file.Dataset.GetSingleValue<string>(DicomTag.PatientID);
            PatientSex = file.Dataset.GetSingleValue<string>(DicomTag.PatientSex);

            SliceThickness = StringToFloat(file.Dataset.GetSingleValue<string>(DicomTag.SliceThickness));
            ImagePositionPatient = StringArrayToFloat(file.Dataset.GetValues<string>(DicomTag.ImagePositionPatient));
            ImageOrientationPatient = StringArrayToFloat(file.Dataset.GetValues<string>(DicomTag.ImageOrientationPatient));
            PixelSpacing = StringArrayToFloat(file.Dataset.GetValues<string>(DicomTag.PixelSpacing));

            WindowWidth = file.Dataset.GetSingleValue<float>(DicomTag.WindowWidth);
            WindowCenter = file.Dataset.GetSingleValue<float>(DicomTag.WindowCenter);

            RescaleSlope = file.Dataset.GetSingleValue<float>(DicomTag.RescaleSlope);
            RescaleIntercept = file.Dataset.GetSingleValue<float>(DicomTag.RescaleIntercept);

            LoadVolume();
        }

        private void LoadVolume()
        {
            // Pixel Data
            DicomPixelData pixelData = DicomPixelData.Create(Files.First().Dataset);
            IPixelData dataInferface = PixelDataFactory.Create(pixelData, 0);

            var width = pixelData.Width;
            var height = pixelData.Height;
            var length = Files.Length;

            Volume = new Volume(width, height, length, this);

            if(dataInferface is GrayscalePixelDataS16)
                LoadGrayscalePixelData();
        }

        private void LoadGrayscalePixelData()
        {
            int x = Volume.VoxelWidth;
            int y = Volume.VoxelHeight;
            int z = Volume.VoxelLength;

            Volume.Texture = new Texture3D(x, y, z, TextureFormat.RGB24, true);
            Color[] colors = new Color[x * y * z];

            // Window range
            float minRange = WindowCenter - 0.5f * WindowWidth;
            float maxRange = WindowCenter + 0.5f * WindowWidth;

            z = 0;
            foreach(DicomFile file in Files)
            {
                DicomPixelData pixelData = DicomPixelData.Create(file.Dataset);
                IPixelData dataInferface = PixelDataFactory.Create(pixelData, 0);

                Color[] pixelColor = new Color[x * y];
                short[] data = (dataInferface as GrayscalePixelDataS16).Data;

                for(int i = 0; i < data.Length; i++)
                {
                    // Convert data to Hounsfield Units (HU)
                    float hounsfield = Mathf.Clamp((RescaleSlope * data[i]) + RescaleIntercept, minRange, maxRange);

                    // Adjust range to grayscale
                    float scale = AdjustRange(hounsfield, minRange, maxRange);
         
                    // Grayscale value
                    pixelColor[i] = new Color(scale, scale, scale);
                }

                Array.Copy(pixelColor, 0, colors, x * y * z, x * y);
                z++;   
            }

            Volume.Texture.SetPixels(colors);
            Volume.Texture.Apply();
        }

        public void Dispose() => Volume.Dispose();

        private float AdjustRange(float x, float minRange, float maxRange)
        {
            float min = Mathf.Abs(minRange);
            float max = Mathf.Abs(maxRange);
            float y = x * (1.0f / (min + max)) + (min / (min + max));

            return Mathf.Clamp(y, 0, 1);
        }

        // TODO: move this elsewhere
        private float StringToFloat(string value)
        {
            return Convert.ToSingle(value, CultureInfo.InvariantCulture);
        }
    
        // TODO: move this elsewhere
        private float[] StringArrayToFloat(string[] values)
        {
            float[] floatValues = new float[values.Length];

            for(int i = 0; i < values.Length; i++)
                floatValues[i] = Convert.ToSingle(values[i], CultureInfo.InvariantCulture);

            return floatValues;
        }
    }
}
