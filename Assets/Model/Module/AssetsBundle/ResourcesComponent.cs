using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETModel
{
	public class ABInfo : Component
	{
		private int refCount;
		public string Name { get; }

		public int RefCount
		{
			get
			{
				return this.refCount;
			}
			set
			{
				//Log.Debug($"{this.Name} refcount: {value}");
				this.refCount = value;
			}
		}

		public AssetBundle AssetBundle { get; }

		public ABInfo(string name, AssetBundle ab)
		{
			this.Name = name;
			this.AssetBundle = ab;
			this.RefCount = 1;
			//Log.Debug($"load assetbundle: {this.Name}");
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			//Log.Debug($"desdroy assetbundle: {this.Name}");

			this.AssetBundle?.Unload(true);
		}
	}
	
	// 用于字符串转换，减少GC
	public static class AssetBundleHelper
	{
		public static readonly Dictionary<int, string> IntToStringDict = new Dictionary<int, string>();
		
		public static readonly Dictionary<string, string> StringToABDict = new Dictionary<string, string>();

		public static readonly Dictionary<string, string> BundleNameToLowerDict = new Dictionary<string, string>() 
		{
			{ "StreamingAssets", "StreamingAssets" }
		};
		
		// 缓存包依赖，不用每次计算
		public static Dictionary<string, string[]> DependenciesCache = new Dictionary<string, string[]>();

		public static string IntToString(this int value)
		{
			string result;
			if (IntToStringDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value.ToString();
			IntToStringDict[value] = result;
			return result;
		}
		
		public static string StringToAB(this string value)
		{
			string result;
			if (StringToABDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value + ".unity3d";
			StringToABDict[value] = result;
			return result;
		}

		public static string IntToAB(this int value)
		{
			return value.IntToString().StringToAB();
		}
		
		public static string BundleNameToLower(this string value)
		{
			string result;
			if (BundleNameToLowerDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value.ToLower();
			BundleNameToLowerDict[value] = result;
			return result;
		}
		
		public static string[] GetDependencies(string assetBundleName)
		{
			string[] dependencies = new string[0];
			if (DependenciesCache.TryGetValue(assetBundleName,out dependencies))
			{
				return dependencies;
			}
			if (!Define.IsAsync)
			{
#if UNITY_EDITOR
				dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
#endif
			}
			else
			{
				dependencies = ResourcesComponent.AssetBundleManifestObject.GetAllDependencies(assetBundleName);
			}
			DependenciesCache.Add(assetBundleName, dependencies);
			return dependencies;
		}

		public static string[] GetSortedDependencies(string assetBundleName)
		{
			Dictionary<string, int> info = new Dictionary<string, int>();
			List<string> parents = new List<string>();
			CollectDependencies(parents, assetBundleName, info);
			string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
			return ss;
		}

		public static void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
		{
			parents.Add(assetBundleName);
			string[] deps = GetDependencies(assetBundleName);
			foreach (string parent in parents)
			{
				if (!info.ContainsKey(parent))
				{
					info[parent] = 0;
				}
				info[parent] += deps.Length;
			}


			foreach (string dep in deps)
			{
				if (parents.Contains(dep))
				{
					throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
				}
				CollectDependencies(parents, dep, info);
			}
			parents.RemoveAt(parents.Count - 1);
		}
	}
	

	public class ResourcesComponent : Component
	{
		public static AssetBundleManifest AssetBundleManifestObject { get; set; }

		private readonly Dictionary<string, Dictionary<string, UnityEngine.Object>> resourceCache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

		private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (var abInfo in this.bundles)
			{
				abInfo.Value?.AssetBundle?.Unload(true);
			}

			this.bundles.Clear();
			this.resourceCache.Clear();
		}


		/// <summary>
		/// 2019-06-26 如果不从缓存里面加载.则从ab包里面加载
		/// </summary>
		/// <param name="bundleName"></param>
		/// <param name="prefab"></param>
		/// <returns></returns>
		///  public T LoadAsset<T>(string name) where T : Object;
		public T LoadResourceDirectFromAB<T>(string bundleName, string path)  where T : UnityEngine.Object
		{
            path = path.ToLower();
			T resource = null;

#if UNITY_EDITOR

			string[] realPathArray = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName.ToLower());
			// Log.Debug("path 111 ==>" + path + " realPathArray==> " + realPathArray.Length);
			foreach (string nameKey in realPathArray)
			{
				string realPath = nameKey.ToLower();
				// Log.Debug("realPath 222 ===>  " + realPath);
				if(realPath == path || Path.GetFileNameWithoutExtension(realPath).Equals(path))
				{
					// Log.Debug("Real  path ==> " + realPath);
					return AssetDatabase.LoadAssetAtPath<T>(realPath);
				}
			}

			return null;
#endif			
			
			AssetBundle bundle = this.GetAssetBundle(bundleName);
			if(bundle == null)
			{
				throw new Exception($"LoadResourceDirectFromAB. not found asset 2222 : {bundleName} {path}");
			}

			// Log.Debug("LoadResourceDirectFromAB path ==> " + path);
			resource = bundle.LoadAsset<T>(path);
			
			// 2,这里要兼容原来ET的,如果没路径.就纯靠名字来匹配.
			if(resource == null)
			{
				string[] assetPaths = bundle.GetAllAssetNames();
				foreach(string nameKey in assetPaths)
				{
					// Log.Debug("LoadResourceDirectFromAB nameKey 111 ==> " + nameKey);
					if(Path.GetFileNameWithoutExtension(nameKey).Equals(path) == true)
					{
						// Log.Debug("LoadResourceDirectFromAB nameKey 222 ==> " + nameKey);
						resource = bundle.LoadAsset<T>(nameKey);
						break;
					}
				}
			}

			return resource;
		}


		public UnityEngine.Object GetAsset(string bundleName, string prefab)
		{
			// by Ron 2019-06-26. 底层全部转成小写. 
            prefab = prefab.ToLower();
			UnityEngine.Object resource = null;

			// 1, 找到缓存dict, 如果没有不要抛出异常
            Dictionary<string, UnityEngine.Object> dict;
			this.resourceCache.TryGetValue(bundleName.BundleNameToLower(), out dict);
			
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

			// by Ron 2019-06-26.如果最终都没从缓存里面找到.就直接从ab包加载
			if(resource != null)
			{
				return resource;
			}
			
			return this.LoadResourceDirectFromAB<UnityEngine.Object>(bundleName, prefab);
		}

		/// <summary>
		/// Fay: 考虑不用框架提供的方法，因为那个不能直接用底层的LoadAsset<T>这个方法（比如：不能直接将一个图片加载成Sprite）
		/// </summary>
		/// <param name="bundleName"></param>
		/// <param name="path"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetAssetByType<T>(string bundleName, string path) where T : UnityEngine.Object
		{
			return this.LoadResourceDirectFromAB<T>(bundleName, path);
		}

		/// <summary>
		/// 加载拥有子集的资源
		/// </summary>
		/// <param name="bundleName"></param>
		/// <param name="path"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public UnityEngine.Object[] GetAllAsset(string bundleName, string path)
		{
			path = path.ToLower();
#if UNITY_EDITOR

			string[] realPathArray = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName.ToLower());
			// Log.Debug("path 111 ==>" + path + " realPathArray==> " + realPathArray.Length);
			foreach (string nameKey in realPathArray)
			{
				string realPath = nameKey.ToLower();
				// Log.Debug("GetAllAsset -> realPath 222 ===>  " + realPath);
				if(realPath == path || Path.GetFileNameWithoutExtension(realPath).Equals(path))
				{
					// Log.Debug("Match! GetAllAsset -> Real  path ==> " + realPath);
					return AssetDatabase.LoadAllAssetsAtPath(realPath);
				}
			}

			return null;
#endif	

			AssetBundle bundle = this.GetAssetBundle(bundleName);
			if(bundle == null)
			{
				throw new Exception($"GetAllAsset. not found asset 3333 : {bundleName} {path}");
			}

			// Log.Debug("LoadResourceDirectFromAB path ==> " + path);
			UnityEngine.Object[] resources = bundle.LoadAssetWithSubAssets(path);
			
			return resources;
		}

		public void UnloadBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();

			string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);

			//Log.Debug($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				this.UnloadOneBundle(dependency);
			}
		}

		private void UnloadOneBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();

			ABInfo abInfo;
			if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				throw new Exception($"not found assetBundle: {assetBundleName}");
			}
			
			//Log.Debug($"---------- unload one bundle {assetBundleName} refcount: {abInfo.RefCount - 1}");

			--abInfo.RefCount;
            
			if (abInfo.RefCount > 0)
			{
				return;
			}


			this.bundles.Remove(assetBundleName);
			abInfo.Dispose();
			//Log.Debug($"cache count: {this.cacheDictionary.Count}");
		}

		/// <summary>
		/// 同步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <param name="isCacheRes"></param>
		/// <returns></returns>
		public void LoadBundle(string assetBundleName, bool isCacheRes=true)
		{
			// Log.Debug("LoadBundle assetBundleName ==> " + assetBundleName);
			assetBundleName = assetBundleName.ToLower();
			string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
			//Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				if (string.IsNullOrEmpty(dependency))
				{
					continue;
				}
				this.LoadOneBundle(dependency, isCacheRes);
			}
        }

		public void AddResource(string bundleName, string assetName, UnityEngine.Object resource)
		{
            //Debug.Log("AddResource -> " + bundleName + ", cache asset -> " + assetName);
            assetName = assetName.ToLower();
            Dictionary<string, UnityEngine.Object> dict;
			if (!this.resourceCache.TryGetValue(bundleName.BundleNameToLower(), out dict))
			{
				dict = new Dictionary<string, UnityEngine.Object>();
				this.resourceCache[bundleName] = dict;
			}

			dict[assetName] = resource;
		}

		public void LoadOneBundle(string assetBundleName, bool isCacheRes=true)
		{
			//Log.Debug($"---------------load one bundle {assetBundleName}");
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				++abInfo.RefCount;
				return;
			}

			if (!Define.IsAsync)
			{
				string[] realPath = null;
#if UNITY_EDITOR
				realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
				if(isCacheRes == true)
				{
					foreach (string s in realPath)
					{
						string assetName = Path.GetFileNameWithoutExtension(s);
						UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
						

						// 2019-06-14 ET原版是以assetName为Key
						//AddResource(assetBundleName, assetName, resource);
						// 2019-06-14 Fay改成以完整路径为Key
						// Debug.Log("get asset path == " + AssetDatabase.GetAssetPath(resource));
						AddResource(assetBundleName, s, resource);
					}
				}

				this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
				return;
			}

			string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
			AssetBundle assetBundle = null;
			if (File.Exists(p))
			{
				assetBundle = AssetBundle.LoadFromFile(p);
			}
			else
			{
				p = Path.Combine(PathHelper.AppResPath, assetBundleName);
				assetBundle = AssetBundle.LoadFromFile(p);
			}

			if (assetBundle == null)
			{
				throw new Exception($"assets bundle not found: {assetBundleName}");
			}

			if (!assetBundle.isStreamedSceneAssetBundle)
			{
				// by Ron 2019-06-25
				if(isCacheRes == true)
				{
					// 异步load资源到内存cache住
					string[] assetPaths = assetBundle.GetAllAssetNames();
					foreach(string path in assetPaths)
					{
						// Debug.Log("Load AB (" + assetBundleName + ") Asset -> " + path);
						UnityEngine.Object asset = assetBundle.LoadAsset(path);
						// 2019-06-14 Fay: 将完整路径做为key
						AddResource(assetBundleName, path, asset);
					}
				}
				else
				{
					// Log.Debug("Don't cache assetBundleName ===> " + assetBundleName);
				}
                // 异步load资源到内存cache住
                //UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
                //foreach (UnityEngine.Object asset in assets)
                //{
                //    AddResource(assetBundleName, asset.name, asset);
                //}
            }

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
		}

		/// <summary>
		/// 异步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public async Task LoadBundleAsync(string assetBundleName)
		{
            assetBundleName = assetBundleName.ToLower();
			string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
            // Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
            foreach (string dependency in dependencies)
			{
				if (string.IsNullOrEmpty(dependency))
				{
					continue;
				}
				await this.LoadOneBundleAsync(dependency);
			}
        }

		public async Task LoadOneBundleAsync(string assetBundleName)
		{
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				++abInfo.RefCount;
				return;
			}

            //Log.Debug($"---------------load one bundle {assetBundleName}");
            if (!Define.IsAsync)
			{
				string[] realPath = null;
#if UNITY_EDITOR
				realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
				foreach (string s in realPath)
				{
					string assetName = Path.GetFileNameWithoutExtension(s);
					UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
					AddResource(assetBundleName, assetName, resource);
				}

				this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
				return;
			}

			string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
			AssetBundle assetBundle = null;
			if (!File.Exists(p))
			{
				p = Path.Combine(PathHelper.AppResPath, assetBundleName);
			}
			
			using (AssetsBundleLoaderAsync assetsBundleLoaderAsync = ComponentFactory.Create<AssetsBundleLoaderAsync>())
			{
				assetBundle = await assetsBundleLoaderAsync.LoadAsync(p);
			}

			if (assetBundle == null)
			{
				throw new Exception($"assets bundle not found: {assetBundleName}");
			}

			if (!assetBundle.isStreamedSceneAssetBundle)
			{
				// 异步load资源到内存cache住
				UnityEngine.Object[] assets;
				using (AssetsLoaderAsync assetsLoaderAsync = ComponentFactory.Create<AssetsLoaderAsync, AssetBundle>(assetBundle))
				{
					assets = await assetsLoaderAsync.LoadAllAssetsAsync();
				}
				foreach (UnityEngine.Object asset in assets)
				{
                    AddResource(assetBundleName, asset.name, asset);
                }
			}

			this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
		}

		public string DebugString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (ABInfo abInfo in this.bundles.Values)
			{
				sb.Append($"{abInfo.Name}:{abInfo.RefCount}\n");
			}
			return sb.ToString();
		}

		/// <summary>
		/// by Ron.我们自己封装的接口.
		/// </summary>
		/// <returns></returns>
		public AssetBundle GetAssetBundle(string assetBundleName)
		{
			ABInfo abInfo;
			this.bundles.TryGetValue(assetBundleName, out abInfo);
			if(abInfo == null)
			{
				this.bundles.TryGetValue(assetBundleName.ToLower(), out abInfo);
			}

			if(abInfo == null)
				return null;

			return abInfo.AssetBundle;
		}

		
		public int Mult(int a, int b)
		{
			return a * b;
		}
	}
}
