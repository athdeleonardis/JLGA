using JLGA.Architecture.VisualNovel.VNScript.Listener;
using UnityEngine;

namespace JLGA.Unity.VisualNovel.VNScript.Listener
{
    public class VNListeners : MonoBehaviour, IVNListeners<AVNActor, AVNDisplay>
    {
        [SerializeField] private AVNActorSelector _actorSelector;
        [SerializeField] private AVNDisplaySelector _displaySelector;
        [SerializeField] private AVNControlledDisplaySelector _controlledDisplaySelector;
        [SerializeField] private VNFlags _flags;
        [SerializeField] private AVNState _state;

        public IVNActorSelector<AVNActor> ActorSelector => _actorSelector;
        public IVNDisplaySelector<AVNDisplay> DisplaySelector => _displaySelector;
        public IVNControlledDisplaySelector<AVNDisplay> ControlledDisplaySelector => _controlledDisplaySelector;
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
            _state.Cleanup();
        }
    }
}
