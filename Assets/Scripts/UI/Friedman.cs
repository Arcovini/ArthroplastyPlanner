using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class Friedman : MonoBehaviour
    {
        private void OnEnable()
        {
            UIDocument document = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            VisualElement axial = root.Q<VisualElement>("Axial");
            VisualElement coronal = root.Q<VisualElement>("Coronal");

            // TODO: change to automatically set according to parent's position and scale
            Point axialP0  = new Point(new Vector2(300, 150), 10.0f, Color.blue);
            Point axialP1  = new Point(new Vector2(600, 150), 10.0f, Color.red);
            Link axialLink = new Link(axialP0, axialP1);
            axial.Add(axialLink);

            Point coronalP0  = new Point(new Vector2(300, 150), 10.0f, Color.blue);
            Point coronalP1  = new Point(new Vector2(600, 150), 10.0f, Color.red);
            Link coronalLink = new Link(coronalP0, coronalP1);
            coronal.Add(coronalLink);
        }
    }   
}