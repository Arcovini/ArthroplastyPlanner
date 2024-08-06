using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace AP
{
    public class LoadButtonView : IDisposable
    {
        public Button Button;

        public LoadButtonView(Button button)
        {
            Button = button;
            Button.clicked += OnButtonClicked;
        }

        public void Dispose()
        {
            Button.clicked -= OnButtonClicked;
        }

        private void OnButtonClicked() => FriedmanEvents.OpenFileExplorer?.Invoke();
    }
}