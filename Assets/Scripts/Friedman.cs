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

            VisualElement sagital = root.Q<VisualElement>("Sagital");

            Point p0  = new Point(new Vector2(200, 500), 50.0f, Color.blue);
            Point p1  = new Point(new Vector2(500, 500), 50.0f, Color.red);
            Link link = new Link(p0, p1);

            sagital.Add(link);
        }
    }   
}