using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class PlaygroundPresenter : MonoBehaviour
    {
        [System.NonSerialized] public PlaygroundModel Model;
        [System.NonSerialized] public PlaygroundView View;

        public void Awake()
        {
            Model = GetComponent<PlaygroundModel>();
            View = GetComponent<PlaygroundView>();
        }

        public void OnEnable()
        {
            PlaygroundEvents.PrintMessage += Model.PrintMessage;
        }

        public void OnDisable()
        {
            PlaygroundEvents.PrintMessage -= Model.PrintMessage;
        }
    }
}