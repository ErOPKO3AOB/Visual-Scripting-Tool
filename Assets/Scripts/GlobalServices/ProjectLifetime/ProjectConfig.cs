using Session.Scheme.Block;
using Session.Scheme.Block.Button;
using Session.Scheme.Connector;
using Session.Scheme.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GlobalServices.ProjectLifetime
{
    [CreateAssetMenu(fileName = nameof(ProjectConfig), menuName = "ProjectConfigs" + "/" + nameof(ProjectConfig))]
    public class ProjectConfig : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField] private MainScenesConfig mainScenes;

        [Header("Blocks")]
        [SerializeField] private BlockConfigs _blockConfigs;

        [Header("User")]
        [SerializeField] private CameraSettings _cameraSettings;
        [SerializeField] private InputSettings _inputSettings;

        public MainScenesConfig MainScenes => mainScenes;
        public BlockConfigs BlockConfigs => _blockConfigs;
        public CameraSettings CameraSettings => _cameraSettings;
        public InputSettings InputSettings => _inputSettings;

#if UNITY_EDITOR
        private void OnValidate()
        {
            MainScenes.OnValidate();
        }
#endif
    }

    [Serializable]
    public class MainScenesConfig
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _workspaceScene;
#endif
        [SerializeField] private string _workscapeSceneName;

        public string WorkspaceScene 
        { 
            get 
            { 
#if UNITY_EDITOR
                return _workspaceScene.name;
#else
                return _workscapeSceneName;
#endif
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _workscapeSceneName = _workspaceScene.name;
        }
#endif
    }

    [Serializable]
    public class BlockConfigs
    {
        [Header("Block Units")]
        [SerializeField] private List<SchemeBlockFacade> _blockFacades;
        [SerializeField] private ActionConnecorFacade _actionConnecorFacadePrefab;

        [Header("Sprites")]
        [SerializeField] private Sprite _boxSprite;
        [SerializeField] private Sprite _circleSprite;
        [SerializeField] private Sprite _rhombSprite;
        [SerializeField] private Sprite _parallelogramSprite;

        [Header("Drag")]
        [SerializeField] private Color _draggingColorAffect;
        [SerializeField] private float _draggingSizeAffect;
        [SerializeField] private float _dragSensitivity = 4f;

        [Header("Block Buttons")]
        [SerializeField] private Color _holdingColorAffect;
        [SerializeField] private BlockSettingsButton _settingsButtonPrefab;
        [SerializeField] private BlockInputTrigger _inputTriggerPrefab;
        [SerializeField] private BlockOutputButton _outputButtonPrefab;
        [SerializeField] private DraggableConnectorPoint _draggableConnectorPointPrefab;

        [Header("Block windows")]
        [SerializeField] private List<BaseWindow> _windowPrefabsUI;

        public List<SchemeBlockFacade> BlockFacades => _blockFacades;
        public ActionConnecorFacade ActionConnecorFacadePrefab => _actionConnecorFacadePrefab;
        public Sprite BoxSprite => _boxSprite;
        public Sprite CircleSprite => _circleSprite;
        public Sprite RhombSprite => _rhombSprite;
        public Sprite ParallelogramSprite => _parallelogramSprite;
        public Color DraggingColorAffect => _draggingColorAffect;
        public float DraggingSizeAffect => _draggingSizeAffect;
        public float DragSensitivity => _dragSensitivity;
        public Color HoldingColorAffect => _holdingColorAffect;
        public BlockSettingsButton SettingsButtonPrefab => _settingsButtonPrefab;
        public BlockInputTrigger InputTriggerPrefab => _inputTriggerPrefab;
        public BlockOutputButton OutputButtonPrefab => _outputButtonPrefab;
        public DraggableConnectorPoint DraggableConnectorPointPrefab => _draggableConnectorPointPrefab; 
        public List<BaseWindow> WindowPrefabsUI => _windowPrefabsUI;
    }

    [Serializable]
    public class CameraSettings
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _camPrefab;

        [Header("Movement")]
        [SerializeField] private float _moveSensitivity = 1f;
        [SerializeField] private float _moveSmoothTime = 0.1f;

        [Header("Zoom")]
        [SerializeField] private float _zoomSensitivity = 1f;
        [SerializeField] private float _zoomSmoothTime = 0.1f;
        
        public GameObject CamPrefab => _camPrefab;
        public float MoveSensitivity => _moveSensitivity;
        public float MoveSmoothTime => _moveSmoothTime;
        public float ZoomSensitivity => _zoomSensitivity;
        public float ZoomSmoothTime => _zoomSmoothTime;
    }

    [Serializable]
    public class InputSettings
    {
        [SerializeField] private GameObject _playerInputPrefab;

        public GameObject PlayerInputPrefab => _playerInputPrefab;
    }
}