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
    }
}