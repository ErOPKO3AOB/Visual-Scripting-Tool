using UnityEngine;
using VContainer;

namespace Session.Scheme.Block
{
    public class SchemeBlockFacade : MonoBehaviour
    {
        [SerializeField] private string _blockName;
        public string BlockName { get { return _blockName; } } 

        [Inject]
        public void Construct(IActionProvider model)
        {
            Model = model;
        }

        public IActionProvider Model { get; private set; }
    }
}