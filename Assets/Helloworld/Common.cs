using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif


public static class Common
{   
    public static Dictionary<string, AssetBundle> AssetBundleDict = new Dictionary<string, AssetBundle>();
    private static Dictionary<string, Dictionary<string, UnityEngine.Object>> ResourceCache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    public static void Debug(string msg)
    {   
        UnityEngine.Debug.Log(msg);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    public static void Error(string msg)
    {
        UnityEngine.Debug.LogError(msg);
    }

    
    /// <summary>
    /// 加载ab包
    /// </summary>
    /// <param name="abName"></param>
    public static bool LoadAssetBundle(string abName)
    {
        AssetBundle assetBundle = null;
        string path = Path.Combine(AppHotfixResPath, abName);
        AssetBundleDict.TryGetValue(path, out assetBundle);

        if(assetBundle == null)
        {
#if UNITY_EDITOR
            string[] realPath = AssetDatabase.GetAssetPathsFromAssetBundle(abName);
            foreach(string tmpResourcePath in realPath)
            {
                Common.Debug($"Editor Load AB {abName}, Asset -> {tmpResourcePath}");
                string assetName = Path.GetFileNameWithoutExtension(tmpResourcePath);
                UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(tmpResourcePath);
                
                AddResource(abName, tmpResourcePath, resource);
            }

            return true;
#endif
            _LoadAssetBundle(abName, path);
        }

        return true;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="abName"></param>
    public static bool _LoadAssetBundle(string abName, string path)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        if(assetBundle == null)
            return false;

        AssetBundleDict[path] = assetBundle;

        // 把每一个对象资源都加载进来放到cache里面去
        string[] assetPaths = assetBundle.GetAllAssetNames();
        foreach(string resPath in assetPaths)
        {
            Common.Debug($"Load AB {abName}, Asset -> {resPath}");
            UnityEngine.Object asset = assetBundle.LoadAsset(resPath);
            // 完整的路径作为key
            AddResource(abName, resPath, asset);
        }

        return true;
    }


    public static string AppHotfixResPath
    {
        get
        {
            string game = Application.productName;
            string path = AppResPath;
            if (Application.isMobilePlatform)
            {
                path = $"{Application.persistentDataPath}/{game}/";
            }
            return path;
        }
    }


    public static string AppResPath
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="assetName"></param>
    /// <param name="resource"></param>
    public static void AddResource(string bundleName, string assetName, UnityEngine.Object resource)
    {
        assetName = assetName.ToLower();
        Dictionary<string, UnityEngine.Object> dict;
        if (!ResourceCache.TryGetValue(assetName, out dict))
        {
            dict = new Dictionary<string, UnityEngine.Object>();
            ResourceCache[bundleName.ToLower()] = dict;
        }

        dict[assetName] = resource;
    }

}

