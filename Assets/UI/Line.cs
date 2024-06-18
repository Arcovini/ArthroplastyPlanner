using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class Line : VisualElement
    {
        private Vector3 start;
        private Vector3 end;

        private Color color;
        private float thickness;

        private MeshWriteData mesh;

        public Line(Vector3 p0, Vector3 p1, Color color, float thickness)
        {
            this.start = p0;
            this.end = p1;
            this.color = color;
            this.thickness = thickness;

            generateVisualContent += OnGenerateVisualContent;
        }

        public void UpdatePosition(Vector3 p0, Vector3 p1)
        {
            // Update line position
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            float angleDeg = Vector2.Angle(this.start, this.end);

            this.mesh = context.Allocate(4, 6);
            
            Vertex[] vertices = new Vertex[4];
            vertices[0].position = this.start - new Vector3(0, this.thickness / 2, 0);
            vertices[1].position = this.start + new Vector3(0, this.thickness / 2, 0);
            vertices[2].position = this.end + new Vector3(0, this.thickness / 2, 0);
            vertices[3].position = this.end - new Vector3(0, this.thickness / 2, 0);

            for(int index = 0; index < vertices.Length; index++)
            {
                vertices[index].position += Vector3.forward * Vertex.nearZ;
                vertices[index].tint = this.color;
            }

            mesh.SetAllVertices(vertices);
            mesh.SetAllIndices(new ushort[] { 0, 3, 1, 1, 3, 2 });
        }
    }
}