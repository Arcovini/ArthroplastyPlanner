using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    [RequireComponent(typeof(Camera))]
    public class ArcballCamera : MonoBehaviour
    {
        // TODO: refactor
        [SerializeField] private Transform target;
        [Space(20)]

        [Header("Settings")]
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float rotationSpeed = 1.0f;
        [SerializeField] private float boomSpeed = 3.0f;
        [SerializeField] private float scrollingSpeed = 100.0f;
        [SerializeField] private float minDollyDist = 0.1f;
        [SerializeField] private float minFOV = 20.0f;
        [SerializeField] private float maxFOV = 120.0f;
        [SerializeField] private float fovStepSize = 1.0f;
        
        private const float eps = 0.01f;

        private Camera cam;
        private Vector3 lastMouseScreenPos;
        private Vector3 sphericalCoordinates;

        private bool isLeftMouseButtonPressed = false;
        private bool isMiddleMouseButtonPressed = false;

        private void Start()
        {
            this.cam = GetComponent<Camera>();
            
            transform.position = new Vector3(this.radius, 0.0f, 0.0f);
            transform.LookAt(this.target);
            
            this.sphericalCoordinates = GetSphericalCoordinates(transform.position);
        }

        private void LateUpdate()
        {
            // TODO: change to a state machine
            // TODO: change to new input system (event based)
            Zoom();
            Rotate();
            Dolly();
            Boom();
        }

        private Vector3 GetSphericalCoordinates(Vector3 cartesian)
        {
            float r = Mathf.Sqrt(Mathf.Pow(cartesian.x, 2) + Mathf.Pow(cartesian.y, 2) + Mathf.Pow(cartesian.z, 2));
            float phi = Mathf.Atan2(cartesian.z, cartesian.x);
            float theta = Mathf.Acos(cartesian.y / r);

            return new Vector3(r, phi, theta);
        }

        private Vector3 GetCartesianCoordinates(Vector3 spherical)
        {
            float x = spherical.x * Mathf.Sin(spherical.z) * Mathf.Cos(spherical.y);
            float y = spherical.x * Mathf.Cos(spherical.z);
            float z = spherical.x * Mathf.Sin(spherical.z) * Mathf.Sin(spherical.y);

            return new Vector3(x, y, z);
        }

        private void Zoom()
        {
            if(Input.GetKey(KeyCode.Equals))
                this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - this.fovStepSize, this.minFOV, this.maxFOV);
            if(Input.GetKey(KeyCode.Minus))
                this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView + this.fovStepSize, this.minFOV, this.maxFOV);
        }

        private void Rotate()
        {
            if(Input.GetMouseButtonDown(0))
            {
                this.isLeftMouseButtonPressed = true;
                this.lastMouseScreenPos = Input.mousePosition;
            }

            if(Input.GetMouseButtonUp(0))
                this.isLeftMouseButtonPressed = false;

            if(this.isLeftMouseButtonPressed)
            {
                float dx = (this.lastMouseScreenPos.x - Input.mousePosition.x) * this.rotationSpeed;
                float dy = (this.lastMouseScreenPos.y - Input.mousePosition.y) * this.rotationSpeed;

                // Radius
                this.sphericalCoordinates.x = this.radius;

                // Phi
                this.sphericalCoordinates.y += dx * Time.deltaTime;

                // Theta (clamp to avoid turning the camera upside down)
                this.sphericalCoordinates.z = Mathf.Clamp(this.sphericalCoordinates.z - dy * Time.deltaTime, eps, Mathf.PI - eps);

                transform.position = GetCartesianCoordinates(this.sphericalCoordinates) + this.target.position;
                transform.LookAt(target);

                this.lastMouseScreenPos = Input.mousePosition;
            }
        }

        private void Dolly()
        {
            float dist = Vector3.Distance(this.cam.transform.position, this.target.transform.position);
            this.radius = dist;

            if(dist < this.minDollyDist)
            {
                this.radius = this.minDollyDist;
                dist = 0.0f;
            }

            float relativeSpeed = Mathf.Min(Mathf.Pow(dist, 2.0f), this.scrollingSpeed);
            this.cam.transform.Translate(new Vector3(0.0f, 0.0f, Input.mouseScrollDelta.y * relativeSpeed * Time.deltaTime), Space.Self);
        }

        private void Boom()
        {
            if(Input.GetMouseButtonDown(2))
            {
                this.isMiddleMouseButtonPressed = true;
                this.lastMouseScreenPos = Input.mousePosition;
            }

            if(Input.GetMouseButtonUp(2))
                this.isMiddleMouseButtonPressed = false;

            if(this.isMiddleMouseButtonPressed)
            {
                float dx = (this.lastMouseScreenPos.x - Input.mousePosition.x);
                float dy = (this.lastMouseScreenPos.y - Input.mousePosition.y);

                Vector3 dir = new Vector3(dx, dy, 0.0f);
                
                Vector3 from = this.cam.transform.position;
                this.cam.transform.Translate(dir * this.boomSpeed * Time.deltaTime , Space.Self);
                Vector3 to = this.cam.transform.position;

                this.target.transform.Translate(to - from, Space.World);

                this.lastMouseScreenPos = Input.mousePosition;
            }
        }
    }
}