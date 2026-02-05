using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using TMPro;
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
        [SerializeField] private BaseWindow _settingsWindowPrefab;

        [Header("Essentials")]
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Transform _settingsPoint;
        [SerializeField] private Transform _inputPoint;
        [SerializeField] private Transform[] _outputPoints;

        public IActionProvider _model;
        private WindowService _windowService;
        private BlockConfigs _blockConfigs;

        // Configs
        public SpriteRenderer SpriteRenderer { get; private set; }
        public TMP_Text Label { get { return _label; } set { _label = value; } }

        private void OnValidate()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            gameObject.AddComponent<DraggableBlockButton>().ConstructManually(_blockConfigs);

            if (_settingsWindowPrefab != null && _settingsPoint != null)
            {
                if (_windowService == null) Debug.Log("Window service is null!");
                if (_blockConfigs == null) Debug.Log("Block config is null!");

                BlockSettingsButton settingsButton = Instantiate(
                    _blockConfigs.SettingsButtonPrefab, 
                    _settingsPoint)
                    .GetComponent<BlockSettingsButton>();
                
                settingsButton.ConstructManualy(_windowService, _settingsWindowPrefab, _model, _blockConfigs);
            }

            BlockInputButton blockInputButton = Instantiate(_blockConfigs.InputButtonPrefab, _inputPoint);

            for (int i = 0; i < _outputPoints.Length; i++)
            {
                BlockOutputButton blockOutputButton = Instantiate(_blockConfigs.OutputButtonPrefab, _outputPoints[i]);
                blockOutputButton.ConstructManually(_blockConfigs, _model);
            }
        }

        private void OnDestroy()
        {
            Destroy(_settingsPoint.gameObject);
            Destroy(_inputPoint.gameObject);
            foreach (Transform t in _outputPoints) Destroy(t.gameObject);
        }
    }
}