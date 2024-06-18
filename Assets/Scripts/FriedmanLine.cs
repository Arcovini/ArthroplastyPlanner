using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class FriedmanLine : MonoBehaviour
    {
        private void OnEnable()
        {
            UIDocument document = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            VisualElement sagital = root.Q<VisualElement>("Sagital");
            DragAndDropManipulator p0 = new(root.Q<VisualElement>("P0"));
            DragAndDropManipulator p1 = new(root.Q<VisualElement>("P1"));

            sagital.Add(new Line(new Vector2(200, 500), new Vector2(500, 600), Color.blue, 5));
        }
    }   
}