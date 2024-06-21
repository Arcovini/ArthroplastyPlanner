using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    using MC = ModelController;

    public class UIManager : MonoBehaviour
    {
        public class UIButtonAction
        {
            public Action Clicked = null;
            public Action Held = null;

            public UIButtonAction(Action clicked = null, Action held = null)
            {
                this.Clicked = clicked;
                this.Held = held;
            }
        }
        
        private float timer = 0.0f;
        private float threshold = 0.15f;
        private bool isButtonPressed = false;

        // TODO: refactor
        private void OnEnable()
        {
            UIDocument document = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            // Rotation
            Button buttonPitchLeft = root.Q<Button>("buttonPitchLeft");
            Button buttonPitchRight = root.Q<Button>("buttonPitchRight");
            Button buttonYawLeft = root.Q<Button>("buttonYawLeft");
            Button buttonYawRight = root.Q<Button>("buttonYawRight");
            Button buttonRollLeft = root.Q<Button>("buttonRollLeft");
            Button buttonRollRight = root.Q<Button>("buttonRollRight");

            buttonPitchLeft. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.left),    ()=>MC.Rotate(Vector3.left)),    TrickleDown.TrickleDown);
            buttonPitchRight.RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.right),   ()=>MC.Rotate(Vector3.right)),   TrickleDown.TrickleDown);
            buttonYawLeft.   RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.down),    ()=>MC.Rotate(Vector3.down)),    TrickleDown.TrickleDown);
            buttonYawRight.  RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.up),      ()=>MC.Rotate(Vector3.up)),      TrickleDown.TrickleDown);
            buttonRollLeft.  RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.forward), ()=>MC.Rotate(Vector3.forward)), TrickleDown.TrickleDown);
            buttonRollRight. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.back),    ()=>MC.Rotate(Vector3.back)),    TrickleDown.TrickleDown);

            buttonPitchLeft. RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonPitchRight.RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonYawLeft.   RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonYawRight.  RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonRollLeft.  RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonRollRight. RegisterCallback<MouseUpEvent>(OnButtonRelease);

            // Translation
            Button buttonDepthForward = root.Q<Button>("buttonDepthForward");
            Button buttonDepthBackwards = root.Q<Button>("buttonDepthBackwards");
            Button buttonHorizontalLeft = root.Q<Button>("buttonHorizontalLeft");
            Button buttonHorizontalRight = root.Q<Button>("buttonHorizontalRight");
            Button buttonVerticalUp = root.Q<Button>("buttonVerticalUp");
            Button buttonVerticalDown = root.Q<Button>("buttonVerticalDown");

            buttonDepthForward.   RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.forward), ()=>MC.Translate(Vector3.forward)), TrickleDown.TrickleDown);
            buttonDepthBackwards. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.back),    ()=>MC.Translate(Vector3.back)),    TrickleDown.TrickleDown);
            buttonHorizontalLeft. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.left),    ()=>MC.Translate(Vector3.left)),    TrickleDown.TrickleDown);
            buttonHorizontalRight.RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.right),   ()=>MC.Translate(Vector3.right)),   TrickleDown.TrickleDown);
            buttonVerticalUp.     RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.up),      ()=>MC.Translate(Vector3.up)),      TrickleDown.TrickleDown);
            buttonVerticalDown.   RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepTranslate(Vector3.down),    ()=>MC.Translate(Vector3.down)),    TrickleDown.TrickleDown);

            buttonDepthForward.   RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonDepthBackwards. RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonHorizontalLeft. RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonHorizontalRight.RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonVerticalUp.     RegisterCallback<MouseUpEvent>(OnButtonRelease);
            buttonVerticalDown.   RegisterCallback<MouseUpEvent>(OnButtonRelease);
        }

        private void OnButtonPressed(MouseDownEvent e, UIButtonAction action)
        {
            this.isButtonPressed = true;
            StartCoroutine(ButtonHandler(action));
        }

        private void OnButtonRelease(MouseUpEvent e)
        {
            this.isButtonPressed = false;
        }

        private IEnumerator ButtonHandler(UIButtonAction action)
        {
            while(this.isButtonPressed)
            {
                this.timer += Time.deltaTime;
                
                if(this.timer > this.threshold)
                    action.Held?.Invoke();
                
                yield return null;
            }

            if(this.timer < this.threshold)
                action.Clicked?.Invoke();

            this.timer = 0.0f;
        }
    }
}