using UnityEngine;

namespace Runtime.Commands.Level
{
    public class OnLevelLoaderCommand
    {
        private Transform _LevelHolder;
        public OnLevelLoaderCommand(Transform levelHolder)
        {
            _LevelHolder = levelHolder;
        }

        public void Execute(byte levelIndex)
        {
            Object.Instantiate(Resources.Load<GameObject>($"PreFabs/LevelPrefabs/Level {levelIndex}"));
        }
    }
}