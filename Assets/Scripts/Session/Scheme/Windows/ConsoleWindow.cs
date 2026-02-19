using Session.Scheme.Block.Types;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class ConsoleWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService, SchemeConsoleService consoleService)
        {
            _windowService = windowService;
            _consoleService = consoleService;
        }

        private WindowFactory _windowService;
        private SchemeConsoleService _consoleService;

        [SerializeField] private MessageItem _messageItemPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _clearButton;

        public override void SetSender(object sender)
        {
            base.SetSender(sender);

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < _consoleService.Messages.Count; i++)
            {
                OnMessageSpawn(_consoleService.Messages[i]);
            }

            _closeButton.onClick.AddListener(() =>
            {
                _windowService.CloseWindow(this);
            });

            _clearButton.onClick.AddListener(() =>
            {
                ClearAllMessages();
            });
            ;
            _consoleService.OnMessageSpawn += OnMessageSpawn;
            _consoleService.OnInputRequest += OnInputRequest;
        }

        private void OnMessageSpawn(string message)
        {
            MessageItem messageItem = (MessageItem)_windowService.OpenWindow(_messageItemPrefab, _content);
            messageItem.BuildOutputMessage(message);
        }

        private void OnInputRequest(string variableName, InputBlock inputBlock)
        {
            MessageItem messageItem = (MessageItem)_windowService.OpenWindow(_messageItemPrefab, _content);
            messageItem.BuildInputMessage(variableName, inputBlock);
        }

        private void ClearAllMessages()
        {
            _consoleService.ClearAllMessages();

            List<BaseWindow> windows = _windowService.ActiveWindows.FindAll(w => w.WindowName == _messageItemPrefab.WindowName);

            for (int i = 0; i < windows.Count; i++)
            {
                _windowService.CloseWindow(windows[i]);
            }
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            _clearButton.onClick.RemoveAllListeners();

            _consoleService.OnMessageSpawn -= OnMessageSpawn;
            _consoleService.OnInputRequest -= OnInputRequest;
        }
    }
}