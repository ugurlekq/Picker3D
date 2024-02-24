using System;
using Runtime.Enums;
using Runtime.Managers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Handlers
{
    public class UIEventSubscriber : MonoBehaviour
    {
        #region Self Variables

        #endregion

        #region Serialized Variables

        [SerializeField] private UIEventSubscriptionTypes type;
        [SerializeField] private Button button;

        #endregion

        #region Private Variables

        private UIManager _manager; //

        #endregion

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            _manager = FindObjectOfType<UIManager>();
        }

        private void SubscribeEvents()
        {
            button.clicked += type switch
            {
                UIEventSubscriptionTypes.OnPlay => _manager.Play,
                UIEventSubscriptionTypes.OnNextLevel => _manager.NextLevel,
                UIEventSubscriptionTypes.OnRestartLevel => _manager.RestartLevel,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void UnSubscribeEvents()
        {
            button.clicked -= type switch
            {
                UIEventSubscriptionTypes.OnPlay => _manager.Play,
                UIEventSubscriptionTypes.OnNextLevel => _manager.NextLevel,
                UIEventSubscriptionTypes.OnRestartLevel => _manager.RestartLevel,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}