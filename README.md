# Addressables Helper

Helper class for Unity's Addressables Asset System.

## Installation

Refer this link for installation guide: https://docs.unity3d.com/Manual/upm-ui-giturl.html

# References

## Static Methods

### Using asset addresses

1. Load addressable asset using address.

```csharp
    private IEnumerator LoadAsset() {
        GameObject gamePrefab;
        yield return AddressablesHelper.LoadAssetByAddress<GameObject>("GamePrefabs/Game1.prefab", gameObj => gamePrefab = gameObj);
    }
```

2. Load multiple addressable assets using addresses.

```csharp
    private IEnumerator LoadAssets() {
        var assetAddresses = new[] {"GamePrefabs/Game1.prefab", "GamePrefabs/Game2.prefab", "GamePrefabs/Game3.prefab"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByAddress<GameObject>(assetAddresses, gameObjCollections => gamePrefabs = gameObjCollections.ToList());
    }
```

---

### Using asset labels

1. Load multiple addressable assets using asset label.

```csharp
    private IEnumerator LoadAssets() {
        var assetLabels = "GamePrefabs";
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByLabel<GameObject>(assetLabels, gameObjCollections => gamePrefabs = gameObjCollections.ToList());
    }
```

2. Load multiple addressable assets using multiple labels.

```csharp
    private IEnumerator LoadAssets() {
        var assetLabels = new[] {"GamePrefabs", "Prefabs"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByLabels<GameObject>(assetLabels, gameObjCollections => gamePrefabs = gameObjCollections.ToList());
    }
```

---

## Helper Functions

To use the following functions you must initialize the addressables helper class with <b> asset label & asset type </b> as shown below. This is an async task to initialize the class on app/game start.

### Initialize addressables helper

```csharp
    private void InitAddressablesHelper() {
        var labelsAndTypeDict = new Dictionary<string, Type> {
            {"GamePrefabs", typeof(GameObject)}, // {AssetLabel , TypeOfAsset}
            {"Sprites", typeof(Sprite)},
            {"SpriteAtlas", typeof(SpriteAtlas)},
            {"Json", typeof(TextAsset)},
            {"Materials", typeof(Material)}
        };
        AddressablesHelper.Init(labelsAndTypeDict);
    }
```

Before using the following functions please ensure the addressables helper class is initailized using,

```csharp
   AddressablesHelper.IsReady;
```

### 1. Load using asset name (No need to pass the full asset address, matches the given string to existing asset addresses)

```csharp
    private IEnumerator LoadAssets() {
        GameObject gamePrefab;
        yield return AddressablesHelper.Instance.LoadAsset<GameObject>("Game1", gameObj => gamePrefab = gameObj);
    }
```

### 2. Load multiple assets using asset names (No need to pass the full asset addresses, matches the given string to existing asset addresses)

```csharp
    private IEnumerator LoadAssets() {
        var gameObjectNames = new[] {"Game1", "Game2"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.Instance.LoadAssets<GameObject>(gameObjectNames, gameObjCollections => gamePrefabs = gameObjCollections.ToList());
    }
```

---

## Demo 

```csharp
    private void Start() {
        InitAddressablesHelper();
        StartCoroutine(LoadAssets());
    }

    private void InitAddressablesHelper() {
        var labelsAndTypeDict = new Dictionary<string, Type> {
            {"GamePrefabs", typeof(GameObject)},
            {"Sprites", typeof(Sprite)},
            {"SpriteAtlas", typeof(SpriteAtlas)},
            {"Json", typeof(TextAsset)},
            {"Materials", typeof(Material)}
        };
        AddressablesHelper.Init(labelsAndTypeDict);
    }


    private IEnumerator LoadAssets() {
        yield return AddressablesHelper.IsReady; // Waits until addressables helper is initialized.
        var gameObjectNames = new[] {"Game1", "Game2"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.Instance.LoadAssets<GameObject>(gameObjectNames, gameObjCollections => gamePrefabs = gameObjCollections.ToList());
    }
```



