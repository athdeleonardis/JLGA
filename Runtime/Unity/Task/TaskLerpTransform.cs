using UnityEngine;

namespace JLGA.Unity.Task
{
    public class TaskLerpTransform : Task
    {
        [SerializeField] private float _timeToLerp;
        [SerializeField] private Transform _transformToLerp;
        [SerializeField] private Transform _transformA;
        [SerializeField] private Transform _transformB;

        private Vector3 _positionA;
        private Vector3 _positionB;
        private Quaternion _rotationA;
        private Quaternion _rotationB;
        private float _time;

        #region Task

        public override void BeginTask()
        {
            _InitializeLerp();
            enabled = true;
        }

        protected override void _EndTaskInternal()
        {
            enabled = false;
        }

        #endregion

        #region Unity

        void Update()
        {
            if (_time < _timeToLerp)
            {
                float lerpFactor = _time / _timeToLerp;
                _UpdateTransform(lerpFactor);
                _time += Time.deltaTime;
            }
            else
            {
                _UpdateTransform(1);
                EndTask();
            }
        }

        #endregion

        #region TaskLerpTransform

        private void _InitializeLerp()
        {
            _positionA = _transformA.position;
            _positionB = _transformB.position;
            _rotationA = _transformA.rotation;
            _rotationB = _transformB.rotation;
            _time = 0;
        }

        private void _UpdateTransform(float lerpFactor)
        {
            Vector3 position = Vector3.Lerp(_positionA, _positionB, lerpFactor);
            Quaternion rotation = Quaternion.Slerp(_rotationA, _rotationB, lerpFactor);
            _transformToLerp.transform.position = position;
            _transformToLerp.transform.rotation = rotation;
        }

        #endregion
    }
}
