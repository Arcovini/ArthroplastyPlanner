using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace AP
{
    public class Link : VisualElement
    {
        public Action DrawLine;

        private Point p0;
        private Point p1;
        private Line line;

        public Link(Point p0, Point p1)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p0.link = this;
            this.p1.link = this;

            this.line = new Line(p0, p1, Color.white, 5.0f);

            Add(line);
            Add(p0);
            Add(p1);

            DrawLine += () => this.line.MarkDirtyRepaint();
        }
    }
}