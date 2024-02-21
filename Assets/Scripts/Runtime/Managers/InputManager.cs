using System.Collections.Generic;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager
    {
        #region Self Variables

        #region Private Variables

        private InputData _data;
        private bool _IsAvailableTouch, _isFirstTimeTouch, _isTouching;

        private float _currentVelccity;
        private float3 _moveVector;
        private Vector2? _mousePosition;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
        }

        public void OnDisableInput()
        {
            _IsAvailableTouch = false;
        }

        private void OnEnableInput()
        {
            _IsAvailableTouch = true;
        }

        private void OnReset()
        {
            _IsAvailableTouch = false;
            //_isFirstTimeTouch = false;
            _isTouching = false;
        }

        public void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            if (!_IsAvailableTouch) return;
            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
            {
                _isTouching = false;
                InputSignals.Instance.onInputReleased?.Invoke(); //birileri beni dinliyor mu?
                Debug.LogWarning("Executed ---> OnInputTaken");
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                Debug.LogWarning("Executed ---> OnInputTaken");
                if (!_isFirstTimeTouch)
                {
                    _isFirstTimeTouch = true;
                    InputSignals.Instance.onFirstTimeTouchToken?.Invoke();
                    Debug.LogWarning("Executed ---> OnFirstTimeTouchTaken");
                }

                _mousePosition = (Vector2)Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                if (_isTouching)
                {
                    if (_mousePosition != null)
                    {
                        Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                        if (mouseDeltaPos.x > _data.horizontalSpeed)
                        {
                            _moveVector.x = _data.horizontalSpeed / 10f * mouseDeltaPos.x;
                        }
                        else if (mouseDeltaPos.x < -_data.horizontalSpeed / 10f * mouseDeltaPos.x) ;
                        else
                            _moveVector.x -= Mathf.SmoothDamp(-_moveVector.x, 0f, ref _currentVelccity,
                                _data.clampSpeed); //Direk hız 0 olmasın yavaş yavaş 0 olsun.
                    }

                    _mousePosition = (Vector2)Input.mousePosition;
                    InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                    {
                        HorizontalValue = _moveVector.x,
                        ClampValues = (float2)_data.clampValues
                    });

                }
            }
        }

        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = (Vector2)Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
