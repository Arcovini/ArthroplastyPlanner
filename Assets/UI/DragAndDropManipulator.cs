using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AP
{
    public class DragAndDropManipulator : PointerManipulator
    {
        private Vector2 targetStartPosition;
        private Vector3 pointerStartPosition;

        private VisualElement root;

        public DragAndDropManipulator(VisualElement target)
        {
            this.target = target;
            this.root = target.parent;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            this.target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            this.target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            this.target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            this.target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        }

        public void PointerDownHandler(PointerDownEvent e)
        {
            this.targetStartPosition = this.target.transform.position;
            this.pointerStartPosition = e.position;
            e.target.CapturePointer(e.pointerId);
        }

        public void PointerMoveHandler(PointerMoveEvent e)
        {
            if(e.target.HasPointerCapture(e.pointerId))
            {
                Vector3 pointerDelta = e.position - this.pointerStartPosition;

                this.target.transform.position = new Vector2(
                    targetStartPosition.x + pointerDelta.x,
                    targetStartPosition.y + pointerDelta.y
                );
            }
        }

        public void PointerUpHandler(PointerUpEvent e)
        {
            e.target.ReleasePointer(e.pointerId);
        }
    }
}