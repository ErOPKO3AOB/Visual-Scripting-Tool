using Session.Scheme.Windows;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Block
{
    public class SchemeBlockFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(IActionProvider model, WindowService windowService)
        {
            Model = model;
            WindowService = windowService;
        }

        [SerializeField] private string _blockName;
//        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private BaseWindowUI _settingsWindowPrefab;

        public string BlockName { get { return _blockName; } }

        public IActionProvider Model { get; private set; }
        public WindowService WindowService { get; private set; }

        private void Start()
        {
            //if (_openSettingsButton != null && _settingsWindow != null)
            //{
            //    _openSettingsButton.onClick.AddListener(() =>
            //    {
            //        WindowService.OpenWindow(_settingsWindow.WindowName);
            //    });
            //}
        }

        private void OnDestroy()
        {
            //if (_openSettingsButton != null && _settingsWindow != null)
            //{
            //    _openSettingsButton.onClick.RemoveAllListeners();
            //}
        }
    }
}