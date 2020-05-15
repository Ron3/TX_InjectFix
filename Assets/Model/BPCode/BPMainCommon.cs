using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
// using FairyGUI;
using LitJson;
using System.Collections;
using UnityEngine.SceneManagement;


namespace ETModel
{
	public static class BPMainCommon
	{
		// /// <summary>
		// /// 加载资源
		// /// </summary>
		// /// <param name="bundleName"></param>
		// /// <param name="prefabName"></param>
		// /// <param name="resourceName"></param>
		// /// <returns></returns>
		// public static UnityEngine.Object BPLoadRes(string bundleName, string prefabName, string resourceName="")
		// {
		// 	return Resources.Load(resourceName);
		// }`


		/// <summary>
		/// 获取当前语言
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentLanguageStr()
		{
			// return "en";
			
			var str = "en";
			switch (Application.systemLanguage)
			{
				case SystemLanguage.French:
					str = "fr";
					break;
				case SystemLanguage.English:
					str = "en";
					break;
				case SystemLanguage.Afrikaans:
					break;
				case SystemLanguage.Arabic:
					break;
				case SystemLanguage.Basque:
					break;
				case SystemLanguage.Belarusian:
					break;
				case SystemLanguage.Bulgarian:
					break;
				case SystemLanguage.Catalan:
					break;
				case SystemLanguage.Czech:
					break;
				case SystemLanguage.Danish:
					break;
				case SystemLanguage.Dutch:
					break;
				case SystemLanguage.Estonian:
					break;
				case SystemLanguage.Faroese:
					break;
				case SystemLanguage.Finnish:
					break;
				case SystemLanguage.German:
					str = "de";
					break;
				case SystemLanguage.Greek:
					break;
				case SystemLanguage.Hebrew:
					break;
				// case SystemLanguage.Hugarian:
				// 	break;
				case SystemLanguage.Icelandic:
					break;
				case SystemLanguage.Indonesian:
					break;
				case SystemLanguage.Italian:
					break;
				case SystemLanguage.Japanese:
					str = "jp";
					break;
				case SystemLanguage.Korean:
					str = "ko";
					break;
				case SystemLanguage.Latvian:
					break;
				case SystemLanguage.Lithuanian:
					break;
				case SystemLanguage.Norwegian:
					break;
				case SystemLanguage.Polish:
					break;
				case SystemLanguage.Portuguese:
					str = "pt";
					break;
				case SystemLanguage.Romanian:
					break;
				case SystemLanguage.Russian:
					str = "ru";
					break;
				case SystemLanguage.SerboCroatian:
					break;
				case SystemLanguage.Slovak:
					break;
				case SystemLanguage.Slovenian:
					break;
				case SystemLanguage.Spanish:
					str = "es";
					break;
				case SystemLanguage.Swedish:
					break;
				case SystemLanguage.Thai:
					str = "th";
					break;
				case SystemLanguage.Turkish:
					break;
				case SystemLanguage.Ukrainian:
					break;
				case SystemLanguage.Vietnamese:
					break;
				case SystemLanguage.Chinese:
				case SystemLanguage.ChineseSimplified:
					str = "cn";
					break;
				case SystemLanguage.ChineseTraditional:
					str = "tw";
					break;
				case SystemLanguage.Unknown:
					break;
				// default:
				// 	throw new ArgumentOutOfRangeException();
			}
			return str;
		}
	

		#region ============================== Json 辅助方法 ==============================

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jsonData"></param>
		/// <param name="keyStr"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int GetIntWithKey(JsonData jsonData, string keyStr, int defaultValue)
		{
			JsonData temp = null;
			try 
			{
				temp = jsonData[keyStr];
			}
			catch (System.Exception e)
			{
				Log.Warning($"---- BPCommon GetIntWithKey 没有 {keyStr} key " + e);
			}
			
			return temp == null ? defaultValue : int.Parse(temp.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="jsonData"></param>
		/// <param name="keyStr"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetStringWithKey(JsonData jsonData, string keyStr, string defaultValue)
		{
			JsonData temp = null;
			try 
			{
				temp = jsonData[keyStr];
			} 
			catch (System.Exception e)
			{
				Log.Warning($"---- BPCommon GetStringWithKey 没有 {keyStr} key " + e);
			}
			
			return temp == null ? defaultValue : temp.ToString();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="jsonData"></param>
		/// <param name="keyStr"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float GetFloatWithKey(JsonData jsonData, string keyStr, float defaultValue)
		{
			JsonData temp = null;
			try 
			{
				temp = jsonData[keyStr];
			} 
			catch (System.Exception e) 
			{
				Log.Warning($"---- BPCommon GetFloatWithKey 没有 {keyStr} key " + e);
			}
			
			return temp == null ? defaultValue : float.Parse(temp.ToString());
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="jsonData"></param>
		/// <param name="keyStr"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double GetDoubleWithKey(JsonData jsonData, string keyStr, double defaultValue)
		{
			JsonData temp = null;
			try
			{
				temp = jsonData[keyStr];
			} catch (System.Exception e) 
			{
				Log.Warning($"---- BPCommon GetDoubleWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getDoubleWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : double.Parse(temp.ToString());
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jsonData"></param>
		/// <param name="keyStr"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool GetBoolWithKey(JsonData jsonData, string keyStr, bool defaultValue)
		{
			JsonData temp = null;
			try 
			{
				temp = jsonData[keyStr];
			} 
			catch (System.Exception e) 
			{
				Log.Warning($"---- BPCommon GetBoolWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getBoolWithKey " + e);
				// throw e;
			}
				
			return temp == null ? defaultValue : bool.Parse(temp.ToString());
		}

		#endregion

		
		/// <summary>
		/// 获取当前时间戳.返回秒
		/// </summary>
		/// <returns></returns>
		public static int GetNowTimeForSec()
		{
			// return System.DateTime.Now.Second;
			TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);  
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return (int)(a/1000);
		}
		

		/// <summary>
		/// 找到最上层父物体
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform FindTopParent(Transform transform)
		{
			while (true)
			{
				if (transform.parent == null) 
					return transform;

				transform = transform.parent;
			}
		}


		/// <summary>
		/// 获取不要Destroy的对象
		/// </summary>
		/// <returns></returns>
		public static GameObject[] GetDontDestroyOnLoadObjects()
		{
			GameObject tempGameObj = null;
			try
			{
				tempGameObj = new GameObject();
				UnityEngine.Object.DontDestroyOnLoad(tempGameObj);
				UnityEngine.SceneManagement.Scene dontDestroyOnLoad = tempGameObj.scene;
				UnityEngine.Object.DestroyImmediate(tempGameObj);
				tempGameObj = null;
		
				return dontDestroyOnLoad.GetRootGameObjects();
			}
			finally
			{
				if(tempGameObj != null)
					UnityEngine.Object.DestroyImmediate(tempGameObj);
			}
		}


		/// <summary>
		/// 是否已经初始化过游戏了
		/// </summary>
		/// <returns></returns>
		public static bool IsAlreadInitGame()
		{
			GameObject[] resultArray = GetDontDestroyOnLoadObjects();
			
			// foreach (GameObject gameObj in resultArray)
			// {
			// 	Log.Debug("dont gameObj ==> " + gameObj);
			// }

			if(resultArray.Length > 0)
				return true;

			return false;
		}


		/// <summary>
		/// 字符串转成byte
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] StringToByte(string data)
		{
			if(data == null)
				return null;

			return data.ToByteArray();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static string ByteToString(byte[] byteArray)
		{
			if (byteArray == null)
				return string.Empty;
			
			return Encoding.UTF8.GetString(byteArray);
		}


		/// <summary>
		/// base64编码
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Base64encode(byte[] byteArray)
		{
			return System.Convert.ToBase64String(byteArray);
		}


		/// <summary>
		/// base64解码
		/// </summary>
		/// <returns></returns>
		public static byte[] Base64decode(string base64Str)
		{
			return System.Convert.FromBase64String(base64Str);
		}
	}
}