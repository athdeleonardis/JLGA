using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNSound : MonoBehaviour, IVNSound
    {
        [SerializeField] private string _soundName;

        public string VNListenerName => _soundName;

        public void SetVNListenerName(string name)
        {
            _soundName = name;
        }

        public abstract bool Load(string soundName);
        public abstract void Play();
        public abstract void Loop();
        public abstract void Pause();
        public abstract void Unpause();
        public abstract void Cleanup();
    }
}
