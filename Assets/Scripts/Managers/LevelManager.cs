using System;
using Cinemachine;
using Data.ValueObjects;
using Commands.Level;
using Data.UnityObjects;
using Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variable

        #region Serialized Variables

        [SerializeField] private Transform LevelHolder;
        [SerializeField] private byte totalLevelCount;

        #endregion

        #region private Variables

        private OnLevelLoaderCommand _levelLoaderCommand; //Observer Pattern
        private OnLevelDestroyerCommand _levelDestroyerCommand;

        private byte _currentLevel;
        private LevelData _levelData;

        #endregion

        #endregion

        private void Awake()
        {
            _levelData = GetLevelData();
            _currentLevel = GetActiveData();
            Init();
        }

        private void Init()
        {
            _levelLoaderCommand = new OnLevelLoaderCommand(LevelHolder);
            _levelDestroyerCommand = new OnLevelDestroyerCommand(LevelHolder);
        }

        private byte GetActiveData()
        {
            throw new NotImplementedException();
        }

        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
        }

        private byte GetActiveLevel()
        {
            return (byte)_currentLevel;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute; //Command Pattern
            CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
            CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue; //Observer Pattern
            CoreGameSignals.Instance.OnNextLevel += OnNextLevel;
            CoreGameSignals.Instance.OnRestartLevel += OnRestartLevel;

        }
       
        
        private byte OnGetLevelValue()
        {
            return (byte)_currentLevel;
        }

        private void OnNextLevel()
        {
            _currentLevel++;
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }
        public void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute; //Command Pattern
            CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
            CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue; //Observer Pattern
            CoreGameSignals.Instance.OnNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.OnRestartLevel -= OnRestartLevel;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void Start()
        {
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte) (_currentLevel % totalLevelCount));
            //UISignals
        }
    }
}
