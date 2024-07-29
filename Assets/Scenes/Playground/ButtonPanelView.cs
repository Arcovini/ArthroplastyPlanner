using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class ButtonPanelView
    {
        public CustomVisualElements.CustomButton CustomButton;
        public Slider Slider;

        public ButtonPanelView(VisualElement root)
        {
            CustomButton = root.Q<CustomVisualElements.CustomButton>("CustomButton");
            Slider = root.Q<Slider>("Slider");

            CustomButton.OnPressed += OnButtonPressed;
            CustomButton.OnHeld += OnButtonHeld;

            Slider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChange);
        }

        private void OnButtonPressed()
        {
            PlaygroundEvents.PrintMessage?.Invoke("Called from Model - Button pressed!");
        }

        private void OnButtonHeld()
        {
            Debug.LogWarning("WARNING: Called from View - Button held!");
        }

        private void OnSliderValueChange(ChangeEvent<float> e)
        {
            Debug.Log(e.newValue);
        }
    }
}