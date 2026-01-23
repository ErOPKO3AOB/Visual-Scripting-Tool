using GlobalServices.ProjectLifetime;
using Session.Scheme.Variables;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Windows
{
    public class ConsoleWindow : BaseWindowUI
    {
        [Inject]
        public void Construct(WindowService windowService, BlockConfigs blockConfigs)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        private WindowService _windowService;
        private BlockConfigs _blockConfigs;

        [SerializeField] private Transform _content;
        
        public void SpawnOutuptMessage(string message)
        {
            MessageItem messageItem = BuildMessageItem();
            messageItem.BuildOutputMessage(message);
        }

        public void SpawnInputMessage(ref SchemeVariableBase variable)
        {
            MessageItem messageItem = BuildMessageItem();
            messageItem.BuildInputMessage();
        }

        private MessageItem BuildMessageItem()
        {
            return (MessageItem)_windowService.OpenWindow(_blockConfigs.WindowPrefabsUI[5].WindowName, _content.transform);
        }
    }
}