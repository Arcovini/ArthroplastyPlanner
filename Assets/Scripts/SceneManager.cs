using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class SceneManager : MonoBehaviour
    {
        // TODO: refactor
        [SerializeField] private GameObject model;
        private UIManager uiManager;

        private void Awake()
        {
            ModelController.model = this.model;
            this.uiManager = GetComponent<UIManager>();
        }
    }
}