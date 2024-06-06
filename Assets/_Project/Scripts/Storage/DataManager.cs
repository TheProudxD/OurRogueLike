using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Storage.Static;
using UnityEngine;

namespace StorageService
{
    public class DataManager
    {
        public event Action<PlayerData> DataLoaded;

        public const string REGISTRY_DATA_KEY = "registry";
        public const string CHANGE_NAME_KEY = "changename";
        public const string MAX_LEVEL_DATA_KEY = "maxleveldata";
        private const string DYNAMIC_USER_DATA_KEY = "userdata";
        private const string DEFAULT_PLAYER_NAME = "Player";

        private readonly GameData _gameData = new();
        private PlayerData _playerData;
        private IStaticStorageService StaticStorageService { get; }
        private IDynamicStorageService DynamicStorageService { get; }

        public DataManager(IStaticStorageService staticStorageService, IDynamicStorageService dynamicStorageService)
        {
            StaticStorageService = staticStorageService;
            DynamicStorageService = dynamicStorageService;
        }

        private async Task SetName(string newName)
        {
            var uploadParams = new Dictionary<string, string>
            {
                { "playername", newName },
                { "action", CHANGE_NAME_KEY },
                { "playerid", SystemPlayerData.Instance.uid.ToString() },
            };

            await DynamicStorageService.Upload(uploadParams, result =>
            {
                if (result)
                {
                    _playerData.Name = newName;
                    Debug.Log($"Renamed Successfully to {newName}");
                }
                else
                {
                    Debug.Log("Error while renaming");
                }
            });
        }

        public async Task DownloadMaxLevel() =>
            await StaticStorageService.Download(MAX_LEVEL_DATA_KEY, data =>
            {
                if (data is null)
                    throw new Exception("File is not found");

                Debug.Log("MaxLevel: " + data.MaxLevel);
                _gameData.MaxLevel = data.MaxLevel;
            });

        public async Task<int> GetMaxLevel()
        {
            if (_gameData is { MaxLevel: > 0 })
                return _gameData.MaxLevel;

            await DownloadMaxLevel();

            return _gameData.MaxLevel;
        }

        public string GetName() => _playerData.Name;

        public async Task<PlayerData> GetDynamicData()
        {
            if (_playerData != null)
                return _playerData;

            var downloadParams = new Dictionary<string, string>
            {
                { "action", DYNAMIC_USER_DATA_KEY },
                { "playerid", SystemPlayerData.Instance.uid.ToString() },
            };

            var callbackData = await DynamicStorageService.Download(downloadParams);
            _playerData = new PlayerData
            {
                AmountHardResources = callbackData["HardCurrency"],
                AmountSoftResources = callbackData["SoftCurrency"],
                MaxPassedLevel = callbackData["lvl"],
                Name = callbackData["Name"],
                AmountEnergy = callbackData["Energy"],
            };

            DataLoaded?.Invoke(_playerData);

            if (_playerData.Name == DEFAULT_PLAYER_NAME)
            {
                await SetName("Player " + SystemPlayerData.Instance.uid);
            }

            if (_playerData.MaxPassedLevel > _gameData.MaxLevel)
            {
                Debug.LogError("Max level less than current!");
            }

            Debug.Log(_playerData.ToString());

            await Task.CompletedTask;

            return _playerData;
        }
    }
}