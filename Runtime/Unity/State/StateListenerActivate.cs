using UnityEngine;

namespace JLGA.Unity.State
{
    public class StateListenerActivate : StateListener
    {
        [SerializeField] private bool isActivate;
        [SerializeField] private GameObject[] _gameObjectsToActivate;
        [SerializeField] private MonoBehaviour[] _monoBehavioursToEnable;

        public override void OnStateAdded()
        {
            _Activate(isActivate);
        }

        public override void OnStateRemoved()
        {
            _Activate(!isActivate);
        }

        private void _Activate(bool isActivate)
        {
            foreach (GameObject gameObject in _gameObjectsToActivate)
            {
                gameObject.SetActive(isActivate);
            }
            foreach (MonoBehaviour monoBehaviour in _monoBehavioursToEnable)
            {
                monoBehaviour.enabled = isActivate;
            }
        }
    }
}
