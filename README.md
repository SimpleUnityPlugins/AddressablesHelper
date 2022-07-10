# Addressables Helper

Helper class for Unity's Addressables Asset System.

---

## Installation

Refer this link for installation guide:ls
https://docs.unity3d.com/Manual/upm-ui-giturl.html
---

# References

## Static Methods

### Using asset addresses

1. Load addressables asset using address.

```csharp
    private IEnumerator LoadAsset() {
        GameObject gamePrefab;
        yield return AddressablesHelper.LoadAssetByAddress<GameObject>("GamePrefabs/Game1.prefab", gameObj => gamePrefab = gameObj);
    }
```

2. Load multiple addressables assets using address.

```csharp
    private IEnumerator LoadAssets() {
        var assetAddresses = new[] {"GamePrefabs/Game1.prefab", "GamePrefabs/Game2.prefab", "GamePrefabs/Game3.prefab"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByAddress<GameObject>(assetAddresses, gameObjs => gamePrefabs = gameObjs.ToList());
    }
```

---

### Using asset labels

1. Load multiple addressables assets using label.

```csharp
    private IEnumerator LoadAssets() {
        var assetLabels = "GamePrefabs";
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByLabel<GameObject>(assetLabels, gameObjs => gamePrefabs = gameObjs.ToList());
    }
```

2. Load multiple addressables assets using multiple label.

```csharp
    private IEnumerator LoadAssets() {
        var assetLabels = new[] {"GamePrefabs", "Prefabs"};
        List<GameObject> gamePrefabs;
        yield return AddressablesHelper.LoadAssetsByLabels<GameObject>(assetLabels, gameObjs => gamePrefabs = gameObjs.ToList());
    }
```

---

## Helper Functions

### Initialize addressables helper class with labels & type of asset.

```csharp
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
```

### Load using asset name (No need to pass full asset address)

```csharp
    private IEnumerator LoadAssets() {
        GameObject gamePrefab;
        yield return AddressablesHelper.Instance.LoadAsset<GameObject>("Game1", gameObj => gamePrefab = gameObj);
    }
```

### Load using asset name (Matches the given string to existing addresses)

```csharp
    private IEnumerator LoadAsset() {
        GameObject gamePrefab;
        yield return AddressablesHelper.Instance.LoadAsset<GameObject>("Game1", gameObj => gamePrefab = gameObj);
    }
```

### Load multiple assets using name (Matches the given string to existing addresses)

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



