using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class Point : VisualElement
    {
        // Dirty hack against Unity's event system
        public Link link = null;
        
        public Vector3 position => transform.position + new Vector3(this.radius, this.radius, 0.0f);

        private Vector3 startPosition;
        private Vector3 pointerStartPosition;

        private float radius = 1.0f;
        private float outlineThickness = 5.0f;
        private Color color;

        public Point(Vector2 position, float radius, Color color)
        {
            transform.position = position - new Vector2(radius, radius);
            this.radius = radius;
            this.color = color;

            this.style.position = Position.Absolute;
            this.style.width = 2 * radius;
            this.style.height = 2 * radius;

            RegisterCallback<PointerDownEvent>(PointerDownHandler);
            RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            RegisterCallback<PointerUpEvent>(PointerUpHandler);

            generateVisualContent += OnGenerateVisualContent;
        }

        private void PointerDownHandler(PointerDownEvent e)
        {
            this.startPosition = transform.position;
            this.pointerStartPosition = e.position;
            e.target.CapturePointer(e.pointerId);
        }

        private void PointerMoveHandler(PointerMoveEvent e)
        {
            if(e.target.HasPointerCapture(e.pointerId))
            {
                Vector3 pointerDelta = e.position - this.pointerStartPosition;

                transform.position = new Vector2(
                    this.startPosition.x + pointerDelta.x,
                    this.startPosition.y + pointerDelta.y
                );

                link.DrawLine?.Invoke();
            }
        }
        
        private void PointerUpHandler(PointerUpEvent e)
        {
            e.target.ReleasePointer(e.pointerId);
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            Painter2D painter = context.painter2D;

            painter.fillColor = Color.white;
            painter.BeginPath();
            painter.Arc(new Vector2(this.radius, this.radius), this.radius + this.outlineThickness, 0.0f, 360.0f);
            painter.Fill();

            painter.fillColor = this.color;
            painter.BeginPath();
            painter.Arc(new Vector2(this.radius, this.radius), this.radius, 0.0f, 360.0f);
            painter.Fill();
        }
    }
}