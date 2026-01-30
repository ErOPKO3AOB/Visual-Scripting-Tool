using Session.Scheme.Block;
using Session.Scheme.Block.Button;
using Session.Scheme.Windows;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GlobalServices.ProjectLifetime
{
    [CreateAssetMenu(fileName = nameof(ProjectConfig), menuName = "ProjectConfigs" + "/" + nameof(ProjectConfig))]
    public class ProjectConfig : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField] private MainScenesConfig mainScenes;
        public MainScenesConfig MainScenes { get { return mainScenes; } }
        [Header("Data")]
        [SerializeField] private DataConfigs _dataConfigs;
        public DataConfigs DataConfigs { get { return _dataConfigs; } }

        [Header("Blocks")]
        [SerializeField] private BlockConfigs _blockConfigs;
        public BlockConfigs BlockConfigs { get { return _blockConfigs; } }

        [Header("User")]
        [SerializeField] private CameraSettings _cameraSettings;
        public CameraSettings CameraSettings { get { return _cameraSettings; } }
        [SerializeField] private InputSettings _inputSettings;
        public InputSettings InputSettings { get { return _inputSettings; } }
    }

    [Serializable]
    public class MainScenesConfig
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _workspaceScene;
        public string WorkspaceScene { get { return _workspaceScene.name; } }
#endif
    }

    [Serializable]
    public class DataConfigs
    {
        [Header("Settings")]
        [SerializeField] private string _settingsSaveFolderName;
        public string SettingsSaveFolderPath { get { return Path.Combine(Application.persistentDataPath, _settingsSaveFolderName); } }

        [SerializeField] private string _settingsFileName;
        public string SettingsFileName { get { return _settingsFileName; } }

        [Header("Sessions")]
        [SerializeField] private string _sessionsSaveFolderName;
        public string SessionsSaveFolderPath { get { return Path.Combine(Application.persistentDataPath, _sessionsSaveFolderName); } }
    }

    [Serializable]
    public class BlockConfigs
    {
        [Header("Block Units")]
        [SerializeField] private SchemeBlockFacade[] _blockFacades;
        public SchemeBlockFacade[] BlockFacades { get { return BlockFacades; } }

        [Header("Drag")]
        [SerializeField] private Color _draggingColorAffect;
        public Color DraggingColorAffect { get { return _draggingColorAffect; } }

        [SerializeField] private float _draggingSizeAffect;
        public float DraggingSizeAffect { get { return _draggingSizeAffect; } }

        [SerializeField] private string _draggableObjectTag;
        public string DraggableObjectTag { get { return _draggableObjectTag; } }

        [Header("Block Points")]
        [SerializeField] private string _clickableObjectTag;
        public string ClickableObjectTag { get { return _clickableObjectTag; } }

        [SerializeField] private BlockButton _settingsButtonPrefab;
        public BlockButton SettingsButtonPrefab { get { return _settingsButtonPrefab; } }
        //[SerializeField] private BlockButton _inputButtonPrefab;
        //public BlockButton InputButtonPrefab { get { return _inputButtonPrefab; } }
        //[SerializeField] private BlockButton _outputButtonPrefab;
        //public BlockButton OutputButtonPrefab { get { return _outputButtonPrefab; } }

        [Header("Block windows")]
        [SerializeField] private BaseWindowUI[] _windowPrefabsUI;
        public BaseWindowUI[] WindowPrefabsUI { get { return _windowPrefabsUI; } }
    }

    [Serializable]
    public class CameraSettings
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _camPrefab;
        public GameObject CamPrefab => _camPrefab;

        [Header("Movement")]
        [SerializeField] private float _moveSensitivity = 1f;
        public float MoveSensitivity => _moveSensitivity;

        [SerializeField] private float _moveSmoothTime = 0.1f;
        public float MoveSmoothTime => _moveSmoothTime;

        [Header("Zoom")]
        [SerializeField] private float _zoomSensitivity = 1f;
        public float ZoomSensitivity => _zoomSensitivity;

        [SerializeField] private float _zoomSmoothTime = 0.1f;
        public float ZoomSmoothTime => _zoomSmoothTime;
    }

    [Serializable]
    public class InputSettings
    {
        [SerializeField] private GameObject _playerInputPrefab;
        public GameObject PlayerInputPrefab { get { return _playerInputPrefab; } }
    }
}