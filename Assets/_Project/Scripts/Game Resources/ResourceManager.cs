using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Storage.Static;
using StorageService;
using UnityEngine;
using Zenject;

namespace ResourceService
{
    public class ResourceManager
    {
        public event Action<ResourceType, int, int> ResourceChanged;
        private readonly IDynamicStorageService _dynamicStorageService;
        private Dictionary<ResourceType, Resource> _resources;
        private PlayerData _playerData;

        [Inject]
        private ResourceManager(IDynamicStorageService dynamicStorageService)
        {
            _dynamicStorageService = dynamicStorageService;
        }

        public void Initialize(int defaultSoft, int defaultHard, PlayerData playerData)
        {            
            _playerData = playerData;
            Resource[] resources =
            {
                new(ResourceType.SoftCurrency, defaultSoft),
                new(ResourceType.HardCurrency, defaultHard)
            };

            _resources = resources.ToDictionary(r => r.Type);

            ResourceChanged += OnResourceChanged;

            foreach (var resource in resources)
            {
                resource.Changed += (oldValue, newValue) =>
                {
                    ResourceChanged?.Invoke(resource.Type, oldValue, newValue);
                };
            }
        }

        public void AddResource(ResourceType type, int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative");
            var resource = _resources[type];
            resource.Amount += value;
        }

        public void SpendResource(ResourceType type, int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative");
            var resource = _resources[type];
            resource.Amount -= value;
        }

        private async void SaveSoftCurrency(int newValue)
        {
            var uploadParams = new Dictionary<string, string>
            {
                { "playercurrency", newValue.ToString() },
                { "action", "changesoftcurrency" },
                { "playerid", SystemPlayerData.Instance.uid.ToString() },
            };

            await _dynamicStorageService.Upload(uploadParams, result =>
            {
                if (result)
                {
                    _playerData.AmountSoftResources = newValue;
                    Debug.Log($"Soft Currency saved Successfully to {newValue}");
                }
                else
                {
                    Debug.Log("Error while saving Soft Currency");
                }
            });
        }

        private async void SaveHardCurrency(int newValue)
        {
            var uploadParams = new Dictionary<string, string>
            {
                { "playercurrency", newValue.ToString() },
                { "action", "changehardcurrency" },
                { "playerid", SystemPlayerData.Instance.uid.ToString() },
            };

            await _dynamicStorageService.Upload(uploadParams, result =>
            {
                if (result)
                {
                    _playerData.AmountHardResources = newValue;
                    Debug.Log($"Hard Currency saved Successfully to {newValue}");
                }
                else
                {
                    Debug.Log("Error while saving Hard Currency");
                }
            });
        }

        public bool HasResource(ResourceType type, int value) =>
            _resources[type].Amount >= value;

        public int GetResourceValue(ResourceType type) =>
            _resources[type].Amount;

        private void OnResourceChanged(ResourceType resType, int oldV, int newV)
        {
            switch (resType)
            {
                case ResourceType.SoftCurrency:
                    SaveSoftCurrency(newV);
                    break;
                case ResourceType.HardCurrency:
                    SaveHardCurrency(newV);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resType), resType, null);
            }

            //Debug.Log($"Changed. Resource type: {resType}). Old value - {oldV}, new value - {newV}");
        }

        ~ResourceManager() =>
            ResourceChanged -= OnResourceChanged;
    }
}