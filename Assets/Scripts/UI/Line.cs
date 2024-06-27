using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class Line : VisualElement
    {
        private Point p0;
        private Point p1;

        private Color color;
        private float thickness;

        public Line(Point p0, Point p1, Color color, float thickness)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.color = color;
            this.thickness = thickness;

            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;

            painter.strokeColor = this.color;
            painter.lineWidth = this.thickness;
            painter.lineJoin = LineJoin.Miter;
            painter.lineCap = LineCap.Butt;

            painter.BeginPath();
            painter.MoveTo(p0.position);
            painter.LineTo(p1.position);
            painter.Stroke();
        }
    }
}