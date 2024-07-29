using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class PlaygroundView : MonoBehaviour
    {
        public ButtonPanelView ButtonPanelView;

        private CustomVisualElements.Point p;

        public void Awake()
        {
            VisualElement root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

            VisualElement buttonPanel = root.Q<VisualElement>("ButtonPanel");

            ButtonPanelView = new ButtonPanelView(buttonPanel);
        }
    }
}