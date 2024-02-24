using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Signals;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Controllers.UI
{
    public class UIPanelControllers : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<Transform> layers = new List<Transform>();


        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanel;
        }

        private void OnCloseAllPanel()
        {
            foreach (var layer in layers)
            {
                if (layers.Count <= 0) return;
#if UNITY_EDITOR
                {
                    DestroyImmediate(layer.GetChild(0).gameObject);
#endif
                    Destroy(layer.GetChild(0).gameObject);
                }
            }
        }

        private void OnOpenPanel(UIPanelTypes panelType, int value)
        {
            OnClosePanel(value);
            Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"), layers[value]);
        }
        
        private void OnClosePanel(int value)
        {
            if (layers[value].childCount <= 0) return;
        #if UNITY_EDITOR
            {
                DestroyImmediate(layers[value].GetChild(0).gameObject);
        #endif
                Destroy(layers[value].GetChild(0).gameObject);
            }
        }
        
        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}