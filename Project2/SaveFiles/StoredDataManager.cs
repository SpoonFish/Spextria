using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace Spextria.StoredData
{
    class StoredDataManager
    {
        private SaveFile DefaultSaveFile;
        private SettingsData DefaultSettings;
        public SettingsData Settings;
        public SaveFile CurrentSaveFile;
        public List<SaveFile> SaveFiles;
        public int CurrentSaveNumber;
        private const string SETTINGS_SAVE_PATH = "StoredData/Settings.json";
        private const string SAVE_FILE_PATH = "StoredData/SaveFiles/";
        public StoredDataManager()
        {
            DefaultSettings = new SettingsData()
            {
                MusicVolume = 80,
                SoundVolume = 100,
                AttackTowardsMouse = false
            };
            //SaveSettings(DefaultSettings);
            // Settings = LoadSettings();

            DefaultSaveFile = new SaveFile()
            {
                Name = "EMPTY",
                New = true,
                Coins = 0,
                CurrentLevel = 1,
                ShowIntro = true,
                Checkpoint = 0,
                Character = "solum",
                PlanetsUnlocked = 1,
                Purchases = new List<string>() { "steel_fists" },
                Cutscenes = new List<string>() { }
            };
            //SaveSettings(DefaultSettings);
            if (!Directory.Exists("StoredData"))
                Directory.CreateDirectory("StoredData");
            if (!Directory.Exists("StoredData/SaveFiles"))
                Directory.CreateDirectory("StoredData/SaveFiles");

            if (!File.Exists("StoredData/Settings.json"))
            {
                Settings = new SettingsData() { MusicVolume = 80, SoundVolume = 100, AttackTowardsMouse = false};
                SaveSettings();
            }
            if (!File.Exists("StoredData/SaveFiles/Save1.json"))
            { 
                DebugSaveFile(1);
            }
            if (!File.Exists("StoredData/SaveFiles/Save2.json"))
            {
                DebugSaveFile(2);
            }
            if (!File.Exists("StoredData/SaveFiles/Save3.json"))
            {
                DebugSaveFile(3);
            }

            Settings = LoadSettings();
            SaveFiles = new List<SaveFile>();
            for (int i = 0; i < 3; i++)
                SaveFiles.Add(LoadFile(i+1));
        }
        public void SaveSettings()
        {
            string serializedText = JsonSerializer.Serialize<SettingsData>(Settings);
            Trace.WriteLine(serializedText);
            File.WriteAllText(SETTINGS_SAVE_PATH, serializedText);
        }
        public bool CheckSkillUnlock(string skillName)
        {
            return CurrentSaveFile.Purchases.Contains(skillName);
        }

        public SettingsData LoadSettings()
        {
            var settingsData = File.ReadAllText(SETTINGS_SAVE_PATH);
            return JsonSerializer.Deserialize<SettingsData>(settingsData);

        }

        public void ResetSettings()
        {
            string serializedText = JsonSerializer.Serialize<SettingsData>(DefaultSettings);
            Trace.WriteLine(serializedText);
            File.WriteAllText(SETTINGS_SAVE_PATH, serializedText);
            Settings = LoadSettings();
        }


        public void DebugSaveFile(int number)
        {
            string path = SAVE_FILE_PATH + $"Save{number}.json";
            string serializedText = JsonSerializer.Serialize<SaveFile>(DefaultSaveFile);
            Trace.WriteLine(serializedText);
            File.WriteAllText(path, serializedText);
        }
        public void CreateFile(string name)
        {

            int number = CurrentSaveNumber;
            string path = SAVE_FILE_PATH + $"Save{number}.json";
            SaveFile NewFile = DefaultSaveFile;
            NewFile.Name = name;
            NewFile.New = false;
            string serializedText = JsonSerializer.Serialize<SaveFile>(NewFile);
            Trace.WriteLine(serializedText);
            File.WriteAllText(path, serializedText);
            SaveFiles[number-1] = LoadFile(number);
        }

        public void SaveFile()
        {
            string path = SAVE_FILE_PATH + $"Save{CurrentSaveNumber}.json";
            string serializedText = JsonSerializer.Serialize<SaveFile>(CurrentSaveFile);
            Trace.WriteLine(serializedText);
            File.WriteAllText(path, serializedText);
        }

        public SaveFile LoadFile(int number)
        {
            string path = SAVE_FILE_PATH + $"Save{number}.json";
            var saveFileData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SaveFile>(saveFileData);

        }

        public void LoadSelectedFile(int number)
        {
            CurrentSaveNumber = number;
            string path = SAVE_FILE_PATH + $"Save{number}.json";
            var saveFileData = File.ReadAllText(path);
            CurrentSaveFile = JsonSerializer.Deserialize<SaveFile>(saveFileData);

        }

    }
}
