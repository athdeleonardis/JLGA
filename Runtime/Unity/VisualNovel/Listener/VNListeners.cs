using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public class VNListeners : MonoBehaviour, IVNListeners<AVNActor, AVNDisplay, AVNSound>
    {
        [SerializeField] private AVNActorSelector _actorSelector;
        [SerializeField] private AVNDisplaySelector _displaySelector;
        [SerializeField] private AVNDisplaySelector _controlledDisplaySelector;
        [SerializeField] private AVNSoundSelector _soundSelector;
        [SerializeField] private VNFlags _flags;
        [SerializeField] private AVNState _state;

        public IVNListenerSelector<AVNActor> ActorSelector => _actorSelector;
        public IVNListenerSelector<AVNDisplay> DisplaySelector => _displaySelector;
        public IVNListenerSelector<AVNDisplay> ControlledDisplaySelector => _controlledDisplaySelector;
        public IVNListenerSelector<AVNSound> SoundSelector => _soundSelector;
        public IVNFlags Flags => _flags;
        public IVNState State => _state;

        public void Initialize()
        {
            _state.Initialize();
        }

        public void Cleanup()
        {
            _actorSelector.Cleanup();
            _displaySelector.Cleanup();
            _controlledDisplaySelector.Cleanup();
            _soundSelector.Cleanup();
            _state.Cleanup();
        }
    }
}
