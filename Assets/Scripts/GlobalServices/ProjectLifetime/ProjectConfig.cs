using NUnit.Framework;
using Session.Scheme;
using Session.Scheme.Block;
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
        [SerializeField] private SceneAsset _menuScene;
        public string MenuScene { get { return _menuScene.name; } }
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
        [SerializeField] private SchemeBlockFacade _startBlock;
        public SchemeBlockFacade StartBlock { get { return _startBlock; } }

        [SerializeField] private Color _draggingColorAffect;
        public Color DraggingColorAffect { get { return _draggingColorAffect; } }

        [SerializeField] private float _draggingSizeAffect;
        public float DraggingSizeAffect { get { return _draggingSizeAffect; } }

        [SerializeField] private string _draggableObjectTag;
        public string DraggableObjectTag { get { return _draggableObjectTag; } }

        [Header("UI Settings Windows")]
        [SerializeField] private SettingsBaseWindowUI[] _variableItemPrefabs;
        public SettingsBaseWindowUI[] VariableItemPrefabs { get { return _variableItemPrefabs; } }
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

        [Header("Zoom")]
        [SerializeField] private float _zoomSensitivity = 1f;
        public float ZoomSensitivity => _zoomSensitivity;
    }

    [Serializable]
    public class InputSettings
    {
        [SerializeField] private GameObject _playerInputPrefab;
        public GameObject PlayerInputPrefab { get { return _playerInputPrefab; } }
    }
}