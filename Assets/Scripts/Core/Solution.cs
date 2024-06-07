using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor;
using FellowOakDicom;


namespace AP
{
    public class Solution : MonoBehaviour
    {
        // TODO: remove
        [SerializeField] private UIDocument document;
        [SerializeField] private GameObject volumePrefab;
        [SerializeField] private Camera cam;

        private Renderer renderer;

        private void OnEnable()
        {
            VisualElement root = this.document.rootVisualElement;
            UnityEngine.UIElements.Button buttonOpenFolder = root.Q<UnityEngine.UIElements.Button>("ButtonOpenFolder");
            buttonOpenFolder.clicked += () => OpenFolder();
        }

        private void OpenFolder()
        {
            string path = EditorUtility.OpenFolderPanel("Select a folder to open", "", "");

            if(String.IsNullOrEmpty(path))
                return;

            DicomFile[] files = Loader.OpenFolder(path);
        
            var volume = GameObject.Instantiate(Resources.Load("Prefabs/Volume")) as GameObject;
            var dicom = volume.GetComponent<DICOM>();
            var o = volume.GetComponent<Volume>();
            dicom.Init(files);
            
            // TODO: renderizar para textura, organizar o project manager, adicionar o slice para ir na direcao do plano

            var slicePlane = GameObject.Instantiate(Resources.Load("Prefabs/SlicePlane")) as GameObject;
            float maxSize = Mathf.Max(dicom.Volume.PhysicalWidth, dicom.Volume.PhysicalHeight);
            slicePlane.transform.localScale = new Vector3(maxSize, maxSize, 1.0f).MillimetersToMeters();

            var collider = volume.GetComponent<Collider>();
            Vector3 min = collider.bounds.min;
            Vector3 max = collider.bounds.max;

            renderer = slicePlane.GetComponent<Renderer>();
            var mat = volume.transform.localToWorldMatrix;
            renderer.material.SetTexture("_VolumeTex", o.Texture);
            renderer.material.SetMatrix("_WorldToVolume", Matrix4x4.Inverse(mat));
            renderer.material.SetVector("AABBmin", min);
            renderer.material.SetVector("AABBmax", max);

            //AssetDatabase.CreateAsset(dicom.Volume.Texture, "Assets/DICOM.asset");
            // Renderer renderer = plane.GetComponent<Renderer>();
            // renderer.material.SetTexture("_VolumeTex", dicom.Volume.Texture);
            // renderer.material.SetMatrix("_WorldToVolume", plane.transform.worldToLocalMatrix);
        }
    }
}