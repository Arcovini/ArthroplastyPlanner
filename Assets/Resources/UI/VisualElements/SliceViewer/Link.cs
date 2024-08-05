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

        public void SetVisualElements()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(styleSheet));
            AddToClassList(styleClass);

            StartPoint = new Point(StartPointColor, OutlineColor, Radius, Thickness);
            EndPoint = new Point(EndPointColor, OutlineColor, Radius, Thickness);

            StartPoint.AddToClassList(startPointStyleClass);
            EndPoint.AddToClassList(endPointStyleClass);

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
            painter.MoveTo(StartPoint.Position);
            painter.LineTo(EndPoint.Position);
            painter.Stroke();
            painter.ClosePath();
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

                link.generateVisualContent += link.OnGenerateVisualContent;
            }
        }
    }
}