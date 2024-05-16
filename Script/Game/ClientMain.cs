using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Unity.Collections.LowLevel.Unsafe;
//using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZG;

public class ClientMain : MonoBehaviour
{
    public enum SceneName
    {
        JyScenes,
        FightBigMap,
        FightBattle,
    }

    [Serializable]
    public struct AssetPath
    {
        [Flags]
        public enum Flag
        {
            SharedLanguage = 0x01,
        }

        [Mask] public Flag flag;
        public string value;

        public AssetPath(string value, Flag flag = 0)
        {
            this.value = value;
            this.flag = flag;
        }
    }

    public static ClientMain Instance;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        Instance = this;
        //wwwSimulation = GetComponent<WWWSimulation>();
        // wwwDrawCards = GetComponent<WWWDrawCards>();
        wwwReadServer = GetComponent<WWWReadServer>();
    }


    public bool isServer = false;


    public AssetPath[] assetPaths = new AssetPath[]
    {
        new AssetPath("SandBox"),
        new AssetPath("Scenes/Others/Others"),
        new AssetPath("Maps/Normal/Normal", AssetPath.Flag.SharedLanguage)
    };

    public WWWReadServer wwwReadServer;
    //public WWWSimulation wwwSimulation;
    //public WWWDrawCards wwwDrawCards;

    private uint __userId;

    public uint UserId_ReadOnly
    {
        get => __userId;
        private set{}
    }
    

    public int version;
    public string path;
    public string scenePath;
    public string mapPath;
    public string defaultSceneName;

    public string languagePackageResourcePath = "Main/Canvas";
    public UnityEngine.Video.VideoPlayer videoPlayer;

    public UnityEngine.Events.UnityEvent onSupported;

    public event Func<IEnumerator> onStart;

    public event Action onSceneLoaded;

    /*private WWWGameData __gameData;

    public WWWGameData gameData
    {
        get
        {
            if (__gameData == null)
                __gameData = GetComponent<WWWGameData>();

            return __gameData;
        }
    }*/

    IEnumerator Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        while (!GameConstantManager.isInit)
        {
            yield return null;
        }

        yield return null;

        if (onSupported != null)
            onSupported.Invoke();

        AssetBundle assetBundle;
        using (var www = UnityEngine.Networking.UnityWebRequest.Get(
                   GameAssetManager.GetStreamingAssetsURL(
                       Path.Combine(Path.GetDirectoryName(languagePackageResourcePath), GameLanguage.overrideLanguage.ToLower()))))
        {
            var languagePackageRequest = www.SendWebRequest();

            bool isWillPlay = videoPlayer != null, isPlaying = isWillPlay && videoPlayer.playOnAwake;
            while (isPlaying)
            {
                yield return null;

                if (Input.GetMouseButtonDown(0))
                {
                    videoPlayer.Stop();

                    break;
                }

                if (videoPlayer.isPlaying)
                    isWillPlay = false;
                else if (!isWillPlay)
                    isPlaying = false;
            }

            if (!languagePackageRequest.isDone)
                yield return languagePackageRequest;

            byte[] bytes = www.downloadHandler?.data;
            assetBundle = AssetBundle.LoadFromMemory(bytes);
        }

        //var languagePackagePrefab = assetBundle.LoadAsset<GameObject>(Path.GetFileName(languagePackageResourcePath));

        //var languagePackage = Instantiate(languagePackagePrefab);

        //Destroy(languagePackagePrefab);

        //assetBundle.Unload(false);

        //Destroy(assetBundle);

        //DontDestroyOnLoad(languagePackage);

        /*var startChecker = languagePackage.GetComponentInChildren<GameStartChecker>();
        if (this.onStart == null)
        {
            WWWGameData gameData = this.gameData;
            if (gameData != null)
            {
                bool result = false, isContinue = true;
                do
                {
                    while (!isContinue)
                        yield return null;

                    isContinue = false;

                    yield return gameData.YieldVersion(version, (x, url) =>
                    {
                        if (!x)
                        {
                            startChecker.ShowDisconnect(() => isContinue = true);

                            return;
                        }

                        if (string.IsNullOrEmpty(url))
                        {
                            result = true;

                            return;
                        }

                        startChecker.ShowNewVersion(url);
                    });

                } while (!result);
            }
        }
        else
        {
            var onStarts = this.onStart.GetInvocationList();
            foreach (var onStart in onStarts)
                yield return ((Func<IEnumerator>)onStart)();
        }

        startChecker.Clear();*/


        //gameClient.onReconnet += startChecker.ShowReconnect;

        //world.GetExistingSystemManaged<Unity.Animation.StateMachine.StateMachineSystemGroup>().Enabled = false;

        //var assetManager = GameAssetManager.instance;
        //assetManager.onConfirmCancel += __OnConfirmCancel;//todo

        yield return GameAssetManager.instance.Init(defaultSceneName, scenePath, path, GameConstantManager.Get(GameConstantManager.KEY_CDN_URL));

        if (onSceneLoaded != null)
            onSceneLoaded();
    }

    public string language
    {
        get
        {
            var result = PlayerPrefs.GetString(GameLanguage.NAME_SPACE);
            if (string.IsNullOrEmpty(result))
            {
                result = GameLanguage.overrideLanguage;
            }

            return result;
        }
    }

    public void LoadAssets()
    {
    }

    public void Get(SceneName sceneName)
    {
        //Debug.LogError("成功");
        GameAssetManager.instance.LoadScene("Scenes/Others/" + sceneName.ToString() + ".scene", () =>
        {
            //StartCoroutine(wwwSimulation.YieldUser("None", Application.identifier, (WWWSimulation.User user) =>
            //{
            //    __userId = user.id;
                
                
            //    Debug.LogError("user Id:" + user.id);
            //    Debug.LogError("user Status:" + user.status);

            //    JyClientMain.Instance.OnBegin();
            //}));
        });
    }

    public void LoadScene(SceneName sceneName, System.Action<float> onProgressValue = null)
    {
        string language = this.language;
        int assetPathCount = this.assetPaths == null ? 0 : this.assetPaths.Length;
        var assetPaths = new GameAssetManager.AssetPath[assetPathCount];
        for (int i = 0; i < assetPathCount; ++i)
        {
            ref readonly var source = ref this.assetPaths[i];
            ref var destination = ref assetPaths[i];
            destination.value = source.value;

            if ((source.flag & AssetPath.Flag.SharedLanguage) != AssetPath.Flag.SharedLanguage)
            {
                destination.filePrefix = language;
                destination.urlPrefix = language;
            }
        }


        string assetURL = GameConstantManager.Get(GameConstantManager.KEY_CDN_URL);
        if (assetURL != null)
            assetURL = $"{assetURL}/{Application.platform}";

        GameAssetManager.instance.LoadAssets(false, () => { Get(sceneName); }, assetURL, assetPaths, null);
    }

    IEnumerator LoadSceneAsync(string sceneName, System.Action<float> onProgressValue)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (onProgressValue != null)
                onProgressValue.Invoke(progress);

            yield return null;
        }
    }
}