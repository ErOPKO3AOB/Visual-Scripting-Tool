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
        [Header("Data config")]
        [SerializeField] private DataConfigs _dataConfigs;
        public DataConfigs DataConfigs { get { return _dataConfigs; } }
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
}