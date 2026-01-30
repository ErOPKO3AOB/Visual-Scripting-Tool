using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Block
{
    [DisallowMultipleComponent]
    public class SchemeBlockFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(/*IActionProvider model,*/ WindowService windowService, BlockConfigs blockConfigs)
        {
            //_model = model;
            _windowService = windowService;
            _blockConfigs = blockConfigs;
        }

        [Header("Base")]
        [SerializeField] private string _blockName;
        public string BlockName { get { return _blockName; } }
        [SerializeField] private BaseWindowUI _settingsWindowPrefab;

        [Header("Essentials")]
        [SerializeField] private Transform _settingsPoint;
        [SerializeField] private Transform _inputPoint;
        [SerializeField] private Transform[] _outputPoints;

        public IActionProvider _model;
        private WindowService _windowService;
        private BlockConfigs _blockConfigs;

        private void Start()
        {
            if (_settingsWindowPrefab != null && _settingsPoint != null)
            {
                if (_windowService == null) Debug.Log("Window service is null!");
                if (_blockConfigs == null) Debug.Log("Block config is null!");

                BlockButton settingsButton = Instantiate(
                    _blockConfigs.SettingsButtonPrefab, 
                    _settingsPoint)
                    .GetComponent<BlockButton>();
                
                settingsButton.ConstructManualy(_windowService, _settingsWindowPrefab);
            }

            //Instantiate(_blockConfigs.InputButtonPrefab, _inputPoint);

            //for (int i = 0; i < _outputPoints.Length; i++)
            //{
            //    Instantiate(_blockConfigs.OutputButtonPrefab, _outputPoints[i]);
            //}
        }

        private void OnDestroy()
        {

        }
    }
}