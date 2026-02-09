using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class ConsoleWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        private WindowFactory _windowService;
        private BlockConfigs _blockConfigs;

        [SerializeField] private Transform _content;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            _closeButton.onClick.AddListener(() =>
            {
                _windowService.CloseWindow(WindowName);
            });
        }

        public void SpawnOutuptMessage(string message)
        {
            MessageItem messageItem = BuildMessageItem();
            messageItem.BuildOutputMessage(message);
        }

        public void GetInput(string variableName, InputBlock inputBlock)
        {
            MessageItem messageItem = BuildMessageItem();
            messageItem.BuildInputMessage(variableName, inputBlock);
        }

        private MessageItem BuildMessageItem()
        {
            return (MessageItem)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[5].WindowName, _content.transform);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}