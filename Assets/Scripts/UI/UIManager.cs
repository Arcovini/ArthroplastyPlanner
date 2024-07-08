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
        private float threshold = 0.40f;
        private bool isButtonPressed = false;

        //variables of the labels
        float pitchDistance = 0;
        private Label labelPitchDistance = null;
        float yawDistance = 0;
        private Label labelYawDistance = null;
        float RollDistance = 0;
        private Label labelRollDistance = null;
        // TODO: refactor
        private void OnEnable()
        {
            UIDocument document = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;



            labelPitchDistance = root.Q<Label>("labelPitchDistance");
            labelYawDistance = root.Q<Label>("labelYawDistance");
            labelRollDistance = root.Q<Label>("labelRollDistance");


            // Rotation
            Button buttonPitchLeft = root.Q<Button>("buttonPitchLeft");
            Button buttonPitchRight = root.Q<Button>("buttonPitchRight");
            Button buttonYawLeft = root.Q<Button>("buttonYawLeft");
            Button buttonYawRight = root.Q<Button>("buttonYawRight");
            Button buttonRollLeft = root.Q<Button>("buttonRollLeft");
            Button buttonRollRight = root.Q<Button>("buttonRollRight");

            //buttonPitchLeft. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=>MC.FixedStepRotate(Vector3.left),    ()=>MC.Rotate(Vector3.left)),    TrickleDown.TrickleDown);
            buttonPitchLeft.RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(() => { MC.FixedStepRotate(Vector3.left); pitchUpdateEvent(true, 1f) ; }, () => { MC.Rotate(Vector3.left);  pitchUpdateEvent(true, 0.01f);}), TrickleDown.TrickleDown);
            buttonPitchRight.RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=> { MC.FixedStepRotate(Vector3.right); pitchUpdateEvent(false,1f); },   ()=> { MC.Rotate(Vector3.right); pitchUpdateEvent(false,01f); }),   TrickleDown.TrickleDown);
            buttonYawLeft.   RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=> { MC.FixedStepRotate(Vector3.down); yawUpdateEvent(true, 1f); },    ()=>{ MC.Rotate(Vector3.down); yawUpdateEvent(true, 0.01f); }),    TrickleDown.TrickleDown);
            buttonYawRight.  RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=> { MC.FixedStepRotate(Vector3.up); yawUpdateEvent(false, 1f); },      ()=>{ MC.Rotate(Vector3.up); yawUpdateEvent(false, 0.01f); }),      TrickleDown.TrickleDown);
            buttonRollLeft.  RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=> { MC.FixedStepRotate(Vector3.forward); RollUpdateEvent(true, 1f);}, ()=>{ MC.Rotate(Vector3.forward); RollUpdateEvent(true, 0.01f); }), TrickleDown.TrickleDown);
            buttonRollRight. RegisterCallback<MouseDownEvent, UIButtonAction>(OnButtonPressed, new UIButtonAction(()=> { MC.FixedStepRotate(Vector3.back); RollUpdateEvent(false, 1f); },    ()=>{ MC.Rotate(Vector3.back); RollUpdateEvent(false, 0.01f); }),    TrickleDown.TrickleDown);

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

        private void pitchUpdateEvent(bool isLeft, float increaseRatio) {
            if (isLeft)
            {
                pitchDistance -= 1.0f * increaseRatio;
            }
            else
            {
                pitchDistance += 1.0f * increaseRatio;
            }
            Vector3 modelEulerAngles = MC.model.transform.eulerAngles;
            float continuousPitchAngle = ConvertToContinuousAngle(modelEulerAngles.x);
            labelPitchDistance.text = ((int)continuousPitchAngle).ToString() + "°";
        }

        private void yawUpdateEvent(bool isDown, float increaseRatio)
        {
            if (isDown)
            {
                yawDistance -= 1.0f * increaseRatio;
            }
            else
            {
                yawDistance += 1.0f * increaseRatio;
            }
            Vector3 modelEulerAngles = MC.model.transform.eulerAngles;
            float continuousyawAngle = ConvertToContinuousAngle(modelEulerAngles.y);
            labelYawDistance.text = ((int)continuousyawAngle).ToString() + "°";
        }

        private void RollUpdateEvent(bool isForward, float increaseRatio)
        {
            if (isForward)
            {
                RollDistance -= 1.0f * increaseRatio;
            }
            else
            {
                RollDistance += 1.0f * increaseRatio;
            }
            Vector3 modelEulerAngles = MC.model.transform.eulerAngles;
            float continuousRollAngle = ConvertToContinuousAngle(modelEulerAngles.z);
            labelRollDistance.text = ((int)continuousRollAngle).ToString() + "°";
        }

        private float ConvertToContinuousAngle(float angle)
        {
            angle = angle % 360f; // Ensure the angle is within -360 to 360 degrees
            if (angle > 180f)
                angle -= 360f; // Convert 181-360 to -179 to 0
            else if (angle < -180f)
                angle += 360f; // Convert -181 to -360 to 179 to 0
            return angle;
        }

    }
}