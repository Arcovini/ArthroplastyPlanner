using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public interface ISliceCameraCommand
    {
        public void Execute();
        public void Undo();
    }

    public class ZoomCommand : ISliceCameraCommand
    {
        private Vector3 previousPosition;

        private SliceCamera camera;
        private Vector2 mousePos;

        private const float scaleStepSize = 0.9f;
        private const float distStepSize = 0.5f;

        private static int steps = 0;

        public ZoomCommand(SliceCamera camera, Vector2 mousePos)
        {
            this.camera = camera;
            this.mousePos = mousePos;
        }

        public void Execute()
        {
            // Vector2 tangentDir = this.mousePos * this.camera.Size;
            // Vector3 targetPosition = this.camera.InitialPosition + (this.camera.Transform.right * tangentDir.x) + (this.camera.Transform.up * tangentDir.y);

            // this.previousPosition = this.camera.Position;
            // this.camera.Position = Vector3.Lerp(this.camera.Position, targetPosition, distStepSize);

            // Target position in world space coordinates
            // Vector3 targetPosition = this.camera.Position + (this.camera.Transform.right * targetOffset.x) + (this.camera.Transform.up * targetOffset.y);

            // TODO: change to lerp
            this.camera.Size = this.camera.Size * scaleStepSize;
        }

        public void Undo()
        {

            this.camera.Size = this.camera.Size / scaleStepSize;
        }
    }

    public class SliceCamera : MonoBehaviour
    {
        public float Size
        {
            get { return this.camera.orthographicSize; }
            set { this.camera.orthographicSize = value; }
        }

        public Transform Transform
        {
            get { return this.camera.transform; }
            private set {}
        }

        public Vector3 Position
        {
            get { return this.camera.transform.position; }
            set { this.camera.transform.position = value; }
        }

        public RenderTexture RenderTexture
        {
            get { return this.camera.targetTexture; }
            set { this.camera.targetTexture = value; }
        }

        public float InitialSize { get; private set; }
        public Vector3 InitialPosition { get; private set; }

        private new Camera camera;
        private Stack<ISliceCameraCommand> commandBuffer;

        private void Awake()
        {
            this.camera = GetComponent<Camera>();
            this.commandBuffer = new Stack<ISliceCameraCommand>();
        }

        public void Init(Slice slice)
        {
            Size = 0.5f * slice.Width;
            InitialSize = Size;

            Position = slice.Position - 2.0f * slice.Transform.forward;
            InitialPosition = Position;

            Transform.LookAt(slice.Transform);
            
            RenderTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        }

        public void Zoom(Vector2 mousePos, float delta)
        {
            // Zoom in
            if(delta < 0.0f)
            {
                if(this.commandBuffer.Count == 10)
                    return;

                ISliceCameraCommand command = new ZoomCommand(this, mousePos);
                command.Execute();
                this.commandBuffer.Push(command);
            }
            // Zoom out
            else
            {
                if(this.commandBuffer.Count == 0)
                    return;

                ISliceCameraCommand command = this.commandBuffer.Peek();
                command.Undo();
                this.commandBuffer.Pop();
            }
        }
    }
}