using GlobalServices.ProjectLifetime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableListWindowUI : BaseWindowUI
    {
        [Inject]
        public void Construct(BlockConfigs blockConfigs)
        {
            _blockConfigs = blockConfigs;
        }

        private BlockConfigs _blockConfigs;

        [Header("UI")]
        [SerializeField] private Button _addNewVariableButton;
        [SerializeField] private LayoutElement _content;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            if (_blockConfigs == null) { Debug.Log("_blockConfigs are null!"); }
            else { Debug.Log("_blockConfigs ARE HERE!"); }

            _addNewVariableButton.onClick.AddListener(() =>
            {
                //Instantiate(_blockConfigs., _content.transform);
            });

            _closeButton.onClick.AddListener(() => { Destroy(gameObject); });
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        public override void OnEndEdit()
        {
            throw new System.NotImplementedException();
        }
    }
}