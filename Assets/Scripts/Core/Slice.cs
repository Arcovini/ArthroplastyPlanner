using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AP
{
    public class Slice : Plane, IDisposable
    {
        public Transform Transform
        {
            get { return this.plane.transform; }
            private set {}
        }

        public Vector3 Position
        {
            get { return this.plane.transform.position; }
            set { this.plane.transform.position = value; }
        }

        public Volume TargetVolume;

        private GameObject plane;

        public Slice(Volume volume) : base()
        {
            TargetVolume = volume;

            this.plane = GameObject.Instantiate(Resources.Load("Prefabs/Slice")) as GameObject;
            
            SetMaterial();
        }

        public Slice(Volume volume, float width, float height) : base(width, height)
        {
            TargetVolume = volume;

            this.plane = GameObject.Instantiate(Resources.Load("Prefabs/Slice")) as GameObject;
            this.plane.transform.localScale = new Vector3(width, height, 1.0f);
            
            SetMaterial();
        }

        public Slice(Volume volume, float width, float height, Vector3 position, Vector3 orientation) : base(width, height)
        {
            TargetVolume = volume;

            this.plane = GameObject.Instantiate(Resources.Load("Prefabs/Slice")) as GameObject;
            this.plane.transform.position = position;
            this.plane.transform.localScale = new Vector3(width, height, 1.0f);
            this.plane.transform.rotation = Quaternion.Euler(orientation.x, orientation.y, orientation.z);

            SetMaterial();
        }

        public void SetMaterial()
        {
            Material mat = this.plane.GetComponent<Renderer>().material;

            Texture volumeTex = TargetVolume.Texture;
            Matrix4x4 worldToVolume = Matrix4x4.Inverse(TargetVolume.Transform.localToWorldMatrix);

            mat.SetTexture("_VolumeTex", volumeTex);
            mat.SetMatrix("_WorldToVolume", worldToVolume);
        }

        public void Dispose() => GameObject.Destroy(this.plane);
    }
}