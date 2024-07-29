using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace AP.CustomVisualElements
{
    public class Link : VisualElement
    {
        public Color StartPointColor { get; set; }
        public Color EndPointColor { get; set; } 
        public Color OutlineColor { get; set; }
        public float Radius { get; set; }
        public float Thickness { get; set; }

        public Point StartPoint;
        public Point EndPoint;

        private const string styleSheet = "UI/VisualElements/SliceViewer/LinkStyleSheet";
        private const string styleClass = "link";
        private const string startPointStyleClass = "startPoint";
        private const string endPointStyleClass = "endPoint";

        public Link()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(styleSheet));
            AddToClassList(styleClass);
        }

        public void SetVisualElements()
        {
            StartPoint = new Point(StartPointColor, OutlineColor, Radius, Thickness);
            EndPoint = new Point(EndPointColor, OutlineColor, Radius, Thickness);

            StartPoint.AddManipulator(new DragAndDropManipulator(StartPoint));
            EndPoint.AddManipulator(new DragAndDropManipulator(EndPoint));

            StartPoint.AddToClassList(startPointStyleClass);
            EndPoint.AddToClassList(endPointStyleClass);

            StartPoint.style.width = 2 * Radius;
            StartPoint.style.height = 2 * Radius;
            EndPoint.style.width = 2 * Radius;
            EndPoint.style.height = 2 * Radius;

            Add(StartPoint);
            Add(EndPoint);
        }
            
        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;

            painter.strokeColor = OutlineColor;
            painter.lineWidth = Thickness;
            painter.lineJoin = LineJoin.Miter;
            painter.lineCap = LineCap.Butt;

            painter.BeginPath();
            painter.MoveTo(new Vector2(0, 0));
            painter.LineTo(new Vector2(0, 0));
            painter.Stroke();
        }

        public new class UxmlFactory : UxmlFactory<Link, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlColorAttributeDescription startPointColor = new UxmlColorAttributeDescription { name = "start-point-color", defaultValue = Color.red };
            UxmlColorAttributeDescription endPointColor = new UxmlColorAttributeDescription { name = "end-point-color", defaultValue = Color.blue };
            UxmlColorAttributeDescription outlineColor = new UxmlColorAttributeDescription { name = "outline-color", defaultValue = Color.white };
            UxmlFloatAttributeDescription radius = new UxmlFloatAttributeDescription { name = "radius", defaultValue = 10.0f };
            UxmlFloatAttributeDescription thickness = new UxmlFloatAttributeDescription { name = "thickness", defaultValue = 3.0f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                Link link = ve as Link;
                link.Clear();

                link.name = "Link";
                link.StartPointColor = startPointColor.GetValueFromBag(bag, cc);
                link.EndPointColor = endPointColor.GetValueFromBag(bag, cc);
                link.OutlineColor = outlineColor.GetValueFromBag(bag, cc);
                link.Radius = radius.GetValueFromBag(bag, cc);
                link.Thickness = thickness.GetValueFromBag(bag, cc);

                link.SetVisualElements();
            }
        }
    }
}