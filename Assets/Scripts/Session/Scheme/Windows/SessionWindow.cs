using UnityEngine;
using VContainer;

namespace Session.Scheme.Windows
{
    public class SessionWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowService)
        {
            _windowService = windowService;
        }

        [SerializeField] private Transform _content;
        [SerializeField] private DownMenuItem _downMenuPrefab;

        private WindowFactory _windowService;

        private void Start()
        {
            _windowService.OpenWindow(_downMenuPrefab.WindowName, _content);
        }
    }
}