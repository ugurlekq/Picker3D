using UnityEngine;

namespace Runtime.Commands.Level
{
    public class OnLevelDestroyerCommand
    {
        private Transform _levelHolder;
        public OnLevelDestroyerCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        public void Execute()
        {
            //level Destroy
            if(_levelHolder.transform.childCount <= 0) return;
            Object.Destroy(_levelHolder.transform.GetChild(0).gameObject);
        }
    }
}