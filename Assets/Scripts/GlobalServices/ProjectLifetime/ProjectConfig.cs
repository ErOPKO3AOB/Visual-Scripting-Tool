using Session.Scheme.Block;
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
    }

    [Serializable]
    public class MainScenesConfig
    {
        [SerializeField] private SceneAsset _menuScene;
        public string MenuScene { get { return _menuScene.name; } }
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
        [SerializeField] private SchemeBlockFacade _startBlock;
        public SchemeBlockFacade StartBlock { get { return _startBlock; } }
        //[SerializeField] private SchemeBlockFacade _endBlock;
    }
}