using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP.CustomVisualElements
{
    public class Point : VisualElement
    {
        public Color Color;
        public Color OutlineColor;
        public float Radius;
        public float OutlineThickness;

        public Point(Color color, Color outlineColor, float radius, float outlineThickness)
        {   
            name = "Point";
            
            Color = color;
            OutlineColor = outlineColor;
            Radius = radius;
            OutlineThickness = outlineThickness;

            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;

            painter.fillColor = OutlineColor;
            painter.BeginPath();
            painter.Arc(new Vector2(Radius, Radius), Radius + OutlineThickness, 0.0f, 360.0f);
            painter.Fill();

            painter.fillColor = Color;
            painter.BeginPath();
            painter.Arc(new Vector2(Radius, Radius), Radius, 0.0f, 360.0f);
            painter.Fill();
        }
    }
}