using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using FellowOakDicom;

namespace AP
{
    public class Loader
    {
        public static DicomFile[] LoadFiles(string path)
        {
            DicomFile[] files = null;
            
            try
            {
                FileInfo[] fileInfo = new DirectoryInfo(path).GetFiles("*.dcm");
                files = new DicomFile[fileInfo.Length];

                for(int i = 0; i < fileInfo.Length; i++)
                    files[i] = DicomFile.Open(fileInfo[i].FullName);
            }
            catch(Exception e)
            {
                // TODO: Add runtime handler
                Debug.Log(e);
            }

            return files;
        }

        public static DICOM LoadDicom(string path) => new DICOM(LoadFiles(path));
    }
}