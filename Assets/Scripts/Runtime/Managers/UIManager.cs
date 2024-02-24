using System;
using Runtime.Commands.Level;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitizalize;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed += OnlevelFailed;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void OnlevelFailed()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
        }

        private void OnLevelSuccessful()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
        }

        private void OnLevelInitizalize(byte arg0)
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
            UISignals.Instance.onSetLevelValue?.Invoke((byte)CoreGameSignals.Instance.onGetLevelValue?.Invoke());
        }

        private void OnReset()
        {
            CoreUISignals.Instance.onCloseAllPanels?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);

        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitizalize;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed -= OnlevelFailed;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        public void Play()
        {
            UISignals.Instance.onPlay?.Invoke();
            CoreUISignals.Instance.onClosePanel?.Invoke(1 );
            InputSignals.Instance.onEnableInput?.Invoke();
            //CameraSignals.Instance.onSetCameraTarget?.Invoke();
        }

        public void NextLevel()
        {
            CoreGameSignals.Instance.OnNextLevel?.Invoke();
        }

        public void RestartLevel()
        {
            CoreGameSignals.Instance.OnRestartLevel?.Invoke();
        }
    }
}