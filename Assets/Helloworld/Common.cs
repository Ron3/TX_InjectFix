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
    public static Dictionary<string, AssetBundle> AssetBundleDictForNameKey = new Dictionary<string, AssetBundle>();
    public static Dictionary<string, Dictionary<string, UnityEngine.Object>> ResourceCache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

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
    /// 获取已经加载的ab包
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public static AssetBundle GetAssetBundle(string abName)
    {
        AssetBundle bundle = null; 
        AssetBundleDictForNameKey.TryGetValue(abName, out bundle);
        return bundle;
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
        Common.Debug($"real load ab. path ==> {path}");
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        if(assetBundle == null)
        {
            Common.Debug($"real load ab failed");
            return false;
        }

        
        AssetBundleDict[path] = assetBundle;
        AssetBundleDictForNameKey[abName] = assetBundle;

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


    /// <summary>
    /// 加载资源的总入口
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetAsset(string bundleName, string prefab)
    {
        // by Ron 2019-06-26. 底层全部转成小写. 
        prefab = prefab.ToLower();
        bundleName = bundleName.ToLower();
        UnityEngine.Object resource = null;

        // 1, 找到缓存dict, 如果没有不要抛出异常
        Dictionary<string, UnityEngine.Object> dict;
        ResourceCache.TryGetValue(bundleName, out dict);

        // 2, 从缓存中加载资源
        if (dict != null && dict.TryGetValue(prefab, out resource) == false)
        {
            // 2019-06-14 兼容原来不带路径与后缀的查找
            // 如果匹配不到绝对路径，则查找不带路径与后缀的文件名
            foreach(string nameKey in dict.Keys)
            {
                if(Path.GetFileNameWithoutExtension(nameKey).Equals(prefab))
                {
                    dict.TryGetValue(nameKey, out resource);
                    break;
                }
            }
        }

        return resource;
    }
}

