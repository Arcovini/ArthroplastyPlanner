using UnityEngine;
using UnityEngine.UIElements;
using System;
using FellowOakDicom;

namespace AP
{
    public class FriedmanPresenter : MonoBehaviour
    {
        [System.NonSerialized] public FriedmanModel Model;
        [System.NonSerialized] public FriedmanView View;

        private void Awake()
        {
            Model = GetComponent<FriedmanModel>();
            View = GetComponent<FriedmanView>();
        }

        private void Start()
        {
            FriedmanEvents.OpenFileExplorer += Model.OpenFileExplorer;
            FriedmanEvents.LoadDicom += Model.LoadDicom;

            FriedmanEvents.SetSliceViews += View.SetSliceViews;
        }

        private void OnDisable()
        {
            FriedmanEvents.OpenFileExplorer -= Model.OpenFileExplorer;
            FriedmanEvents.LoadDicom -= Model.LoadDicom;

            FriedmanEvents.SetSliceViews -= View.SetSliceViews;
        }
    }
}