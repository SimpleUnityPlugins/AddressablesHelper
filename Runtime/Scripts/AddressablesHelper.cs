using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace SUP.AddressablesHelper {
    public sealed class AddressablesHelper {
        #region Fields & Properties

        private static AddressablesHelper _instance;
        private readonly Dictionary<Type, List<IResourceLocation>> _iResourcesLocations;

        public static bool IsReady { get; private set; } = false;

        public static AddressablesHelper Instance {
            get {
                if (_instance != null) return _instance;
                Debug.LogError("Please initialize the addressables helper.");
                return null;
            }
        }

        #endregion

        #region Custom functions

        #region Init Functions

        public static void Init(Dictionary<string, Type> labelAndTypes) {
            _instance = new AddressablesHelper(labelAndTypes);
        }

        private AddressablesHelper(Dictionary<string, Type> addressableLabelsAndType) {
            _iResourcesLocations = new Dictionary<Type, List<IResourceLocation>>();
            LoadResourcesLocations(addressableLabelsAndType);
        }

        private async void LoadResourcesLocations(Dictionary<string, Type> labelAndTypes) {
            foreach (var labelAndType in labelAndTypes) {
                var resourceLocationsList = await Addressables.LoadResourceLocationsAsync(labelAndType.Key, labelAndType.Value).Task;
                _iResourcesLocations.Add(labelAndType.Value, resourceLocationsList.ToList());
            }

            IsReady = true;
        }

        #endregion


        #region Single Asset Loading

        public WaitForAddressablesHelper LoadAsset<T>(string assetName, Action<T> onComplete) {
            if (!IsReady) {
                Debug.LogError("Addressables helper is not yet initialized. Check the status using AddressablesHelper.IsReady");
                return null;
            }

            var type = typeof(T);
            if (!_iResourcesLocations.ContainsKey(type)) {
                Debug.LogError($"No label found: {type}");
                return null;
            }

            var waitForAddressablesHelper = new WaitForAddressablesHelper();
            var iResourceLocation = _iResourcesLocations[type].Find(location => location.ToString().Contains(assetName));

            var operationHandle = Addressables.LoadAssetAsync<T>(iResourceLocation);
            operationHandle.Completed += handle => {
                var loadedAsset = handle.Result;
                onComplete.Invoke(loadedAsset);
                Addressables.Release(loadedAsset);
                waitForAddressablesHelper.StopWaiting();
            };

            return waitForAddressablesHelper;
        }

        #endregion

        #region Multiple Assets Loading

        public WaitForAddressablesHelper LoadAssets<T>(IEnumerable<string> assetNames, Action<IEnumerable<T>> onComplete) {
            if (!IsReady) {
                Debug.LogError("Addressables helper is not yet initialized. Check the status using AddressablesHelper.IsReady");
                return null;
            }

            var type = typeof(T);
            if (!_iResourcesLocations.ContainsKey(type)) {
                Debug.LogError($"No label found: {type}");
                return null;
            }

            var waitForAddressablesHelper = new WaitForAddressablesHelper();
            IEnumerable<IResourceLocation> iResourceLocations = assetNames.SelectMany(name => _iResourcesLocations[type].Where(location => location.ToString().Contains(name))).ToList();

            var operationHandle = Addressables.LoadAssetsAsync<T>(iResourceLocations, null);
            operationHandle.Completed += handle => {
                var loadedAsset = handle.Result;
                onComplete.Invoke(loadedAsset);
                Addressables.Release(loadedAsset);
                waitForAddressablesHelper.StopWaiting();
            };
            return waitForAddressablesHelper;
        }

        #endregion

        #endregion


        #region Default Static Functions

        #region By Address

        public static WaitForAddressablesHelper LoadAssetByAddress<T>(string address, Action<T> onComplete) {
            var waitForAddressablesHelper = new WaitForAddressablesHelper();
            var operationHandle = Addressables.LoadAssetAsync<T>(address);
            operationHandle.Completed += handle => {
                var loadedAsset = handle.Result;
                onComplete.Invoke(loadedAsset);
                Addressables.Release(loadedAsset);
                waitForAddressablesHelper.StopWaiting();
            };

            return waitForAddressablesHelper;
        }

        #endregion

        #region By Labels

        public static WaitForAddressablesHelper LoadAssetsByLabel<T>(string label, Action<IEnumerable<T>> onComplete) {
            return LoadAssetsByLabels(new[] {label}, onComplete);
        }

        public static WaitForAddressablesHelper LoadAssetsByLabels<T>(IEnumerable<string> label, Action<IEnumerable<T>> onComplete) {
            var waitForAddressablesHelper = new WaitForAddressablesHelper();
            var operationHandle = Addressables.LoadAssetsAsync<T>(label, null, Addressables.MergeMode.Union);
            operationHandle.Completed += handle => {
                var loadedAsset = handle.Result;
                onComplete.Invoke(loadedAsset);
                Addressables.Release(loadedAsset);
                waitForAddressablesHelper.StopWaiting();
            };

            return waitForAddressablesHelper;
        }

        #endregion

        #endregion
    }
}