using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using System;
using UnityEngine;
using User;
using VContainer;

namespace Session.Scheme.Block
{
    [DisallowMultipleComponent]
    public class SchemeBlockFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(WindowFactory windowService, BlockConfigs blockConfigs, WorldUIControllerService worldUIControllerService, SchemeBlockFactory schemeBlockFactory)
        {
            _windowService = windowService;
            _blockConfigs = blockConfigs;
            _worldUIControllerService = worldUIControllerService;
            _schemeBlockFactory = schemeBlockFactory;
        }

        private WindowFactory _windowService;
        private BlockConfigs _blockConfigs;
        private WorldUIControllerService _worldUIControllerService;
        private SchemeBlockFactory _schemeBlockFactory;

        [Header("Base")]
        [SerializeField] private string _blockName;
        [SerializeField] private bool _singleInstance;
        public bool SingleInstance => _singleInstance;
        public string BlockName => _blockName;
        [SerializeField] private BaseWindow _settingsWindowPrefab;

        [Header("Essentials")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BlockLabel _label;
        [SerializeField] private Transform _menuPoint;
        [SerializeField] private Transform _inputPoint;
        [SerializeField] private Transform[] _outputPoints;
        [SerializeField] private Vector3[] _connectorOffsets;

        public BlockInputTrigger BlockInputTrigger { get; private set; }
        public BlockOutputButton[] BlockOutputButtons { get; private set; }
        public BlockSettingsButton BlockSettingsButton { get; private set; }

        public BlockDeleteButton BlockDeleteButton { get; private set; }

        public IBlock Model { get; set; }

        // Configs
        public BoxCollider2D Collider { get; private set; }
        public Rigidbody2D Rigidbody => _rigidbody;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public BlockLabel Label { get { return _label; } set { _label = value; } }

        public DraggableBlockButton DraggableBlockButton { get; private set; }

        private void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Array.Resize(ref _connectorOffsets, _outputPoints.Length);
        }

        private void Start()
        {
            _label.OnChanged += OnLabelChanged;

            Collider = GetComponent<BoxCollider2D>();

            DraggableBlockButton = gameObject.AddComponent<DraggableBlockButton>();
            DraggableBlockButton.ConstructManually(_blockConfigs);

            if (_settingsWindowPrefab != null && _menuPoint != null)
            {
                BlockSettingsButton = Instantiate(
                    _blockConfigs.SettingsButtonPrefab,
                    _menuPoint)
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

            BlockDeleteButton = Instantiate(_blockConfigs.DeleteButtonPrefab, _menuPoint);
            BlockDeleteButton.ConstructManualy(_schemeBlockFactory, Model);

            _label.SetText(_label.GetText());
            SetDestroyWaiting(_schemeBlockFactory.DestroyWaiting);
        }

        private void OnLabelChanged(Vector2 size)
        {
            SpriteRenderer.size = size;
            Collider.size = size;
            _inputPoint.transform.localPosition = new Vector2(_inputPoint.transform.localPosition.x, size.y / 2);
            _menuPoint.transform.localPosition = new Vector2(size.x / 2, size.y / 2);

            for (int i = 0; i < _outputPoints.Length; i++)
            {
                Vector2 previousLocalPosition = _outputPoints[i].localPosition;

                bool xRightPadding = previousLocalPosition.x > 0;
                bool xCenterPadding = previousLocalPosition.x == 0;
                bool yUpPadding = previousLocalPosition.y > 0;
                bool yCenterPadding = previousLocalPosition.y == 0;

                Vector2 finalPosition = new();

                if (!xCenterPadding)
                {
                    if (xRightPadding)
                        finalPosition = new Vector2(size.x / 2, finalPosition.y);
                    else
                        finalPosition = new Vector2(-size.x / 2, finalPosition.y);
                }

                if (!yCenterPadding)
                {
                    if (yUpPadding)
                        finalPosition = new Vector2(finalPosition.x, size.y / 2);
                    else
                        finalPosition = new Vector2(finalPosition.x, -size.y / 2);
                }

                _outputPoints[i].transform.localPosition = finalPosition;
            }
        }

        public void SetDestroyWaiting(bool value)
        {
            if (BlockSettingsButton != null)
                BlockSettingsButton.gameObject.SetActive(!value);
            if (BlockDeleteButton != null)
            BlockDeleteButton.gameObject.SetActive(value);
        }

        private void OnDestroy()
        {
            Destroy(_menuPoint.gameObject);
            Destroy(_inputPoint.gameObject);
            foreach (Transform t in _outputPoints) Destroy(t.gameObject);

            _label.OnChanged -= OnLabelChanged;
        }
    }
}