using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class UIManager : MonoBehaviour
    {        
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

            buttonPitchLeft.clicked += () => ModelController.Rotate(Vector3.left);
            buttonPitchRight.clicked += () => ModelController.Rotate(Vector3.right);
            buttonYawLeft.clicked += () => ModelController.Rotate(Vector3.down);
            buttonYawRight.clicked += () => ModelController.Rotate(Vector3.up);
            buttonRollLeft.clicked += () => ModelController.Rotate(Vector3.forward);
            buttonRollRight.clicked += () => ModelController.Rotate(Vector3.back);

            // Translation
            Button buttonDepthForward = root.Q<Button>("buttonDepthForward");
            Button buttonDepthBackwards = root.Q<Button>("buttonDepthBackwards");
            Button buttonHorizontalLeft = root.Q<Button>("buttonHorizontalLeft");
            Button buttonHorizontalRight = root.Q<Button>("buttonHorizontalRight");
            Button buttonVerticalUp = root.Q<Button>("buttonVerticalUp");
            Button buttonVerticalDown = root.Q<Button>("buttonVerticalDown");

            buttonDepthForward.clicked += () => ModelController.Translate(Vector3.forward);
            buttonDepthBackwards.clicked += () => ModelController.Translate(Vector3.back);
            buttonHorizontalLeft.clicked += () => ModelController.Translate(Vector3.left);
            buttonHorizontalRight.clicked += () => ModelController.Translate(Vector3.right);
            buttonVerticalUp.clicked += () => ModelController.Translate(Vector3.up);
            buttonVerticalDown.clicked += () => ModelController.Translate(Vector3.down);
        }
    }
}