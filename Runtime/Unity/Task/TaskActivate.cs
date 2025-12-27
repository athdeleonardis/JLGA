using UnityEngine;

namespace JLGA.Unity.Task
{
    public class TaskActivate : Task
    {
        [SerializeField] private bool _isActivate = true;
        [SerializeField] private GameObject[] _gameObjects;
        [SerializeField] private MonoBehaviour[] _monoBehaviours;

        #region Task

        public override void BeginTask()
        {
            _SetActive(_isActivate);
            EndTask();
        }

        protected override void _EndTaskInternal() { }

        #endregion

        #region TaskActivate

        private void _SetActive(bool isActive)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.SetActive(isActive);
            }

            foreach (MonoBehaviour monoBehaviour in _monoBehaviours)
            {
                monoBehaviour.enabled = isActive;
            }
        }

        #endregion
    }
}
