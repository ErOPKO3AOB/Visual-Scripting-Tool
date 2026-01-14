using GlobalServices.ProjectLifetime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class VariableListUI : SettingsBaseWindowUI
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
            _addNewVariableButton.onClick.AddListener(() =>
            {
                //Instantiate(, _content.transform);
            });

            _closeButton.onClick.AddListener(() => { Destroy(gameObject); });
        }

        private void OnDestroy()
        {
            _addNewVariableButton.onClick.RemoveAllListeners();
        }

        public override void OnEndEdit()
        {
            throw new System.NotImplementedException();
        }
    }
}