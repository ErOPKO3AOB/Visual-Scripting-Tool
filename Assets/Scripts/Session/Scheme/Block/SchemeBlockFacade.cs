using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using System;
using TMPro;
using UnityEngine;
using User;
using VContainer;

namespace Session.Scheme.Block
{
    [DisallowMultipleComponent]
    public class SchemeBlockFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(WindowService windowService, BlockConfigs blockConfigs, WorldUIControllerService worldUIControllerService)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
            _worldUIControllerService = worldUIControllerService;
        }

        [Header("Base")]
        [SerializeField] private string _blockName;
        [SerializeField] private bool _singleInstance;
        public bool SingleInstance => _singleInstance;
        public string BlockName => _blockName;
        [SerializeField] private BaseWindow _settingsWindowPrefab;

        [Header("Essentials")]
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Transform _settingsPoint;
        [SerializeField] private Transform _inputPoint;
        [SerializeField] private Transform[] _outputPoints;
        [SerializeField] private Vector3[] _connectorOffsets;

        public BlockInputTrigger BlockInputTrigger { get; private set; }
        public BlockOutputButton[] BlockOutputButtons { get; private set; }
        public BlockSettingsButton BlockSettingsButton { get; private set; }

        private WindowService _windowService;
        private BlockConfigs _blockConfigs;
        private WorldUIControllerService _worldUIControllerService;

        public IBlock Model { get; set; }

        // Configs
        public Collider2D Collider { get; private set; }
        public Rigidbody2D Rigidbody {  get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public TMP_Text Label { get { return _label; } set { _label = value; } }

        public DraggableBlockButton DraggableBlockButton { get; private set; }

        private void OnValidate()
        {
            Array.Resize(ref  _connectorOffsets, _outputPoints.Length);
        }

        private void Start()
        {
            Collider = GetComponent<Collider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            DraggableBlockButton = gameObject.AddComponent<DraggableBlockButton>();
            DraggableBlockButton.ConstructManually(_blockConfigs);

            if (_settingsWindowPrefab != null && _settingsPoint != null)
            {
                if (_windowService == null) Debug.Log("Window service is null!");
                if (_blockConfigs == null) Debug.Log("Block config is null!");

                BlockSettingsButton = Instantiate(
                    _blockConfigs.SettingsButtonPrefab, 
                    _settingsPoint)
                    .GetComponent<BlockSettingsButton>();
                
                BlockSettingsButton.ConstructManualy(_windowService, _settingsWindowPrefab, Model);
            }

            BlockInputTrigger = Instantiate(_blockConfigs.InputTriggerPrefab, _inputPoint);
            BlockInputTrigger.ConstructManualy(Model, _worldUIControllerService);

            BlockOutputButtons = new BlockOutputButton[_outputPoints.Length];

            for (int i = 0; i < _outputPoints.Length; i++)
            {
                BlockOutputButtons[i] = Instantiate(_blockConfigs.OutputButtonPrefab, _outputPoints[i]);
                BlockOutputButtons[i].ConstructManually(_blockConfigs, Model, _connectorOffsets[i]);
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