using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace AP.CustomVisualElements
{
    public class Point : VisualElement, IDisposable
    {
        public Color Color;
        public Color OutlineColor;
        public float Radius;
        public float OutlineThickness;

        public Vector2 Position
        {
            get { return new Vector2(transform.position.x + resolvedStyle.left + Radius, transform.position.y + resolvedStyle.top + Radius); }
            set {}
        }

        private Vector3 startPosition;
        private Vector3 pointerStartPosition;

        public Point(Color color, Color outlineColor, float radius, float outlineThickness)
        {   
            name = "Point";
            
            Color = color;
            OutlineColor = outlineColor;
            Radius = radius;
            OutlineThickness = outlineThickness;          
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
            RegisterCallback<PointerUpEvent>(OnPointerUp);

            generateVisualContent += OnGenerateVisualContent;
        }

        public void Dispose()
        {
            UnregisterCallback<PointerDownEvent>(OnPointerDown);
            UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }
        
        private void OnPointerDown(PointerDownEvent e)
        {
            this.startPosition = transform.position;
            this.pointerStartPosition = e.position;

            e.target.CapturePointer(e.pointerId);
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if(e.target.HasPointerCapture(e.pointerId))
            {
                Vector3 previousTransform = transform.position;

                Vector3 pointerDelta = e.position - this.pointerStartPosition;
                transform.position = new Vector2(this.startPosition.x + pointerDelta.x, this.startPosition.y + pointerDelta.y);
                
                // Check if point is out of bounds
                if(Position.x < 0.0f || Position.y < 0.0f || Position.x > parent.resolvedStyle.width || Position.y > parent.resolvedStyle.height)
                    transform.position = previousTransform;
                
                parent.MarkDirtyRepaint();
            }
        }

        private void OnPointerUp(PointerUpEvent e)
        {
            e.target.ReleasePointer(e.pointerId);
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            style.width = 2 * Radius;
            style.height = 2 * Radius;  

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