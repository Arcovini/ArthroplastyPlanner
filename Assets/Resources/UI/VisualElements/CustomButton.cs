using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System;

namespace AP.CustomVisualElements
{
    public class CustomButton : Button, IDisposable
    {
        public Action OnPressed;
        public Action OnHeld;

        public float TimerThreshold;

        private float timer = 0.0f;
        private bool isHeld = false;

        public CustomButton()
        {
            RegisterCallback<MouseDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            RegisterCallback<MouseUpEvent>(OnButtonReleased);
        }

        public void Dispose()
        {
            UnregisterCallback<MouseDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            UnregisterCallback<MouseUpEvent>(OnButtonReleased);
        }

        private async void OnButtonPressed(MouseDownEvent e)
        {
            this.isHeld = true;
            await ActionHandler();
        }

        private void OnButtonReleased(MouseUpEvent e)
        {
            this.isHeld = false;
        }

        private async Task ActionHandler()
        {
            while(this.isHeld)
            {
                this.timer += Time.deltaTime;

                if(this.timer > TimerThreshold)
                    OnHeld?.Invoke();

                await Task.Yield();
            }

            if(this.timer < TimerThreshold)
                OnPressed?.Invoke();

            this.timer = 0.0f;
        }

        public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription timerThreshold = new UxmlFloatAttributeDescription { name = "timer-threshold", defaultValue = 0.3f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                CustomButton button = ve as CustomButton;

                button.name = "CustomButton";
                button.TimerThreshold = timerThreshold.GetValueFromBag(bag, cc);
            }
        }
    }
}