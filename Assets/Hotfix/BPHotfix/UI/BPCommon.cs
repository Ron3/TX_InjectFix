using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using FairyGUI;
using LitJson;

namespace ETHotfix
{
	public static class BPCommon
	{
		public static string nextSceneName;
		
		public static readonly StringBuilder stringBuilder = new StringBuilder();
		
		#region ============================== 加载程序文本 ==============================
		private static readonly Dictionary<string, string> textDic = new Dictionary<string, string>();
		
		// 读取配置文件，将文件信息保存到字典里    
		public static void LoadLocalText()
		{
			// var textAsset = BPLoadRes("", "", "Res/text_" + GetCurrentLanguageStr()) as TextAsset;
			// if (textAsset == null)
			// {
			// 	return;
			// }
			// var text = textAsset.text;    
			string path = $"Assets/Res/text/text_{GetCurrentLanguageStr()}.txt";
			var text = BPUtility.LoadTextFromAB(BPG_UI.AB_NAME_TEXT, path);
			if (text == null)
			{
				return;
			}
			
			string[] lines = text.Split('\n');
			foreach (string line in lines)    
			{    
				if (line == null)    
				{    
					continue;    
				}    
				string[] keyAndValue = line.Split('=');    
				textDic.Add(keyAndValue[0], keyAndValue[1]);
			}     
		}
		
		public static string GetLocalTextByKey(string key)    
		{    
			if (textDic.ContainsKey(key) == false)    
			{    
				return "";    
			}
			textDic.TryGetValue(key, out string value);    
			return value;    
		}  
		#endregion

		public static string GetCurrentLanguageStr()
		{			
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
		public static int GetIntWithKey(JsonData jsonData, string keyStr, int defaultValue)
		{
			JsonData temp = null;
			try {
				temp = jsonData[keyStr];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon GetIntWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getIntWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : int.Parse(temp.ToString());
		}
		
		public static string GetStringWithKey(JsonData jsonData, string keyStr, string defaultValue)
		{
			JsonData temp = null;
			try {
				temp = jsonData[keyStr];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon GetStringWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getStringWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : temp.ToString();
		}
		
		public static float GetFloatWithKey(JsonData jsonData, string keyStr, float defaultValue)
		{
			JsonData temp = null;
			try {
				temp = jsonData[keyStr];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon GetFloatWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getFloatWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : float.Parse(temp.ToString());
		}
		
		public static double GetDoubleWithKey(JsonData jsonData, string keyStr, double defaultValue)
		{
			JsonData temp = null;
			try {
				temp = jsonData[keyStr];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon GetDoubleWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getDoubleWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : double.Parse(temp.ToString());
		}
		
		public static bool GetBoolWithKey(JsonData jsonData, string keyStr, bool defaultValue)
		{
			JsonData temp = null;
			try {
				temp = jsonData[keyStr];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon GetBoolWithKey 没有 {keyStr} key " + e);
				// Debug.LogWarning("---- BPCommon getBoolWithKey " + e);
				// throw e;
			}
			
			return temp == null ? defaultValue : bool.Parse(temp.ToString());
		}
		#endregion
		
		#region ============================== 根据 JsonData 创建游戏对象 ==============================
		public static T CreateBPModel<T>(JsonData jsonData, string typeName, bool isCheck = true)
		{
			Log.Debug("---- BPCommon CreateBPModel");
			// 先判断表资源是否有这个 Id 的数据
			if (isCheck)
			{
				var infoId = GetIntWithKey(jsonData, BPG_COMMON.KEY_INFO_ID, 0);
				object info = null;
				// switch (typeName)
				// {
				// 	case "ETHotfix.BPItem":
				// 		info = BPInfoManager.GetItemInfoById(infoId);
				// 		break;
				// 	case "ETHotfix.BPMonster":
				// 		info = BPInfoManager.GetMonsterInfoById(infoId);
				// 		break;
				// 	case "ETHotfix.BPSkill":
				// 		info = BPInfoManager.GetSkillInfoById(infoId);
				// 		break;
				// }
				
				if (info == null)
				{
					return default(T);
				}
			}

			var model = CreateBPModelFromJsonData<T>(jsonData, typeName);
			return model;
		}
		
		public static T CreateBPModelFromJsonData<T>(JsonData jsonData, string typeName)
		{
			Log.Debug("---- BPCommon CreateBPModelFromJsonData");
			Type type = Type.GetType(typeName);
			object instance = Activator.CreateInstance(type);
			MethodInfo mi = type.GetMethod("InitFromJsonData");
			if (mi != null)
			{
				// object[] param0 = new object[0];
				// object[] param1 = new object[1];
				// param1[0] = jsonData;
				mi.Invoke(instance, new object[]{jsonData});
			}
			return (T)instance;
		}

        #endregion
        
        #region ============================== 根据 JsonData 创建游戏对象数组 ==============================
		public static List<T> CreateBPModelListFromJsonData<T>(JsonData jsonData, string typeName, string key, List<T> defaultList)
		{
			Log.Debug("---- BPCommon CreateBPModelListFromJsonData");
			JsonData keyJsonData = null;
			try {
				keyJsonData = jsonData[key];
			} catch (System.Exception e) {
				Log.Warning($"---- BPCommon CreateBPModelListFromJsonData 没有 {key} key " + e);
			}
			
			if (keyJsonData == null)
			{
				Log.Warning("---- BPCommon CreateBPModelListFromJsonData keyJsonData 空");
				return defaultList;
			}
            
			var newList = new List<T>();
			foreach (JsonData tempJsonData in keyJsonData)
			{
				var temp = CreateBPModel<T>(tempJsonData, typeName);
				if (temp != null)
				{
					newList.Add(temp);
				}
			}
			return newList;
		}

        #endregion
		
		#region ============================== 根据游戏对象获得 JsonData ==============================
		public static JsonData GetJsonDataByBPBaseModelList<T>(List<T> tempList)
		{
			// Log.Debug("---- BPCommon GetJsonDataByBPBaseModelList");
			// if (tempList == null || tempList.Count == 0)
			// {
			// 	return null;
			// }
			
			// var allJsonData = new JsonData();
			// foreach (var temp in tempList)
			// {
			// 	var baseModel = temp as BPBaseModel;
			// 	var jsonData = baseModel.GetJsonData();
			// 	allJsonData.Add(jsonData);
			// }

			// return allJsonData;
            return null;
		}

		#endregion
		
// 		public static UnityEngine.Object BPLoadRes(string bundleName, string prefabName, string resourceName="")
// 		{
// // #if RES_LOAD
// // 			Log.Debug("BPCommon ==> BPLoadRes ");
// 			return Resources.Load(resourceName);
// // #else
// 			// return ETModel.Game.Scene.GetComponent<ETModel.ResourcesComponent>().GetAsset(bundleName, prefabName);
// // #endif
// 		}


// 		public static UnityEngine.Sprite BPLoadRes_Ex(string bundleName, string prefabName, string resourceName="")
// 		{
// // #if RES_LOAD
// // 			Log.Debug("BPCommon ==> BPLoadRes ");
// 			return Resources.Load<UnityEngine.Sprite>(resourceName);
// // #else
// 			// return ETModel.Game.Scene.GetComponent<ETModel.ResourcesComponent>().GetAsset(bundleName, prefabName);
// // #endif
// 		}

		
		/// <summary>  
		/// 将文件读取到字符串中  
		/// </summary>  
		/// <param name="filePath">文件的绝对路径</param>  
		public static string FileToString(string filePath)  
		{  
			return FileToString(filePath, Encoding.Default);  
		}  
		
		/// <summary>  
		/// 将文件读取到字符串中
		/// </summary>  
		/// <param name="filePath">文件的绝对路径</param>  
		/// <param name="encoding">字符编码</param>  
		public static string FileToString(string filePath, Encoding encoding)  
		{  
			Log.Debug("---- BPCommon FileToString filePath = " + filePath);
			//创建流读取器  
			var reader = new StreamReader(filePath, encoding);  
			try  
			{  
				//读取流  
				return reader.ReadToEnd();  
			}  
			catch  
			{  
				return string.Empty;  
			}  
			finally  
			{  
				//关闭流读取器  
				reader.Close();  
			}  
		}

		public static string LoadFile(string fileName)
		{
			Log.Debug("---- BPCommon LoadFile fileName = " + fileName);
			var filePath = Path.Combine(BPG_COMMON.BPSavePath, fileName);
			return FileToString(filePath);
		}

		public static void SaveFile(string fileName, string text)
		{
			Log.Debug("---- BPCommon SaveFile Application.dataPath = " + Application.dataPath);
			Log.Debug("---- BPCommon SaveFile Application.streamingAssetsPath = " + Application.streamingAssetsPath);
			Log.Debug("---- BPCommon SaveFile Application.persistentDataPath = " + Application.persistentDataPath);
			Log.Debug("---- BPCommon SaveFile Application.temporaryCachePath = " + Application.temporaryCachePath);

			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			
			var filePath = Path.Combine(BPG_COMMON.BPSavePath, fileName);
			
			// WriteAllText 创建一个新文件，在其中写入指定的字符串，然后关闭文件。如果目标文件已存在，则覆盖该文件。
			File.WriteAllText(filePath, text);
		}
		

		// /// <summary>
		// /// 获取FairyGUI的图片路径.让GLoader可以加载.     ui://包名/图片名
		// /// </summary>
		// /// <param name="imageName"></param>
		// /// <param name="packageName"></param>
		// /// <returns></returns>
		// public static string GetFariyGUIPath(string imageName, string packageName="")
		// {
		// 	return "ui://" + packageName + "/" + imageName;
		// }

	
		
		/// <summary>
		/// 设置 FairyGUI.GObject 中心点坐标
		/// </summary>
		/// <param name="gObject"></param>
		/// <param name="posX"></param>
		/// <param name="posY"></param>
		/// <returns></returns>
		public static void SetFariyGUIGObjectCenterXY(FairyGUI.GObject gObject, float posX, float posY)
		{
			// Log.Debug("----BPCommon SetFariyGUIGObjectCenterXY");
			if (gObject == null)
			{
				return;
			}
			
			gObject.SetXY(posX - gObject.width / 2, posY - gObject.height / 2);
			// Log.Debug($"----BPCommon SetFariyGUIGObjectCenterXY centerXY x = {posX - gObject.width / 2}, y = {posY - gObject.height / 2}");
		}
		
		/// <summary>
		/// 获得 FairyGUI 将世界空间的坐标转换为屏幕坐标
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Vector2 GetFariyGUIWorldToScreenPosition(Camera camera, Vector3 position)
		{
			// Log.Debug("----BPCommon GetFariyGUIWorldToScreenPosition position = " + position);
			if (camera == null || position == null)
			{
				return Vector2.zero;
			}
			
			// var screenPos = Camera.main.WorldToScreenPoint(position); 
			var screenPos = camera.WorldToScreenPoint(position); 
			//原点位置转换
			screenPos.y = Screen.height - screenPos.y; 
			var pos = GRoot.inst.GlobalToLocal(screenPos);
			// Log.Debug($"----BPController_Battle_Panel GetFariyGUIWorldToScreenPosition screenPos == {screenPos}, pos = {pos}");
			return pos;
		}

		/// <summary>
		/// 创建一个 组合多个组件 的 大组件
		/// </summary>
		/// <param name="spacing"></param>
		/// <param name="alignment"></param>
		/// <param name="gObjectList"></param>
		/// <returns></returns>
		public static GComponent CreateGComponentWithSpacing(float spacing, int alignment, List<GObject>gObjectList)
		{
			Log.Debug($"----BPCommon CreateGComponentWithSpacing spacing = {spacing}");
			if (gObjectList == null || gObjectList.Count == 0)
			{
				return null;
			}

			if (alignment == BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_TOP ||
			    alignment == BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_CENTER ||
			    alignment == BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_BOTTOM)
			{
				return CreateGComponentHorizontallyWithSpacing(spacing, alignment, gObjectList);
			}

			return CreateGComponentVerticalWithSpacing(spacing, alignment, gObjectList);
		}

		/// <summary>
		/// 创建一个水平 组合多个组件 的 大组件
		/// </summary>
		/// <param name="spacing"></param>
		/// <param name="alignment"></param>
		/// <param name="gObjectList"></param>
		/// <returns></returns>
		public static GComponent CreateGComponentHorizontallyWithSpacing(float spacing, int alignment, List<GObject>gObjectList)
		{
			Log.Debug($"----BPCommon CreateGComponentHorizontallyWithSpacing spacing = {spacing}");
			if (gObjectList == null || gObjectList.Count == 0)
			{
				return null;
			}

			// 算出宽高
			float width = 0;
			float height = 0;
			int len = gObjectList.Count;
			GObject tempGObject;
			for (int i = 0; i < len; i++)
			{
				tempGObject = gObjectList[i];
				width += tempGObject.actualWidth;
				width += spacing;
				height = Math.Max(height, tempGObject.actualHeight);
			}
			
			// 减去最后的间隔
			width -= spacing;
			
			var allGComponent = new GComponent();
			allGComponent.SetSize(width, height);
			// 设置组件点击不穿透。
			allGComponent.opaque = true;
			
			// var gGraph = new GGraph();
			// gGraph.DrawRect(width, height, 0, Color.red, Color.green);
			// gGraph.alpha = 0.5f;
			// allGComponent.AddChild(gGraph);
			
			float offsetX = 0;
			for (int i = 0; i < len; i++)
			{
				tempGObject = gObjectList[i];
				allGComponent.AddChild(tempGObject);
				
				float offsetY = 0;
				switch (alignment)
				{
					case BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_TOP:
						offsetY = 0;
						break;

					case BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_CENTER:
						offsetY = (height - tempGObject.actualHeight)/2;
						break;

					case BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_BOTTOM:
						offsetY = height - tempGObject.actualHeight;
						break;

					default:
						offsetY = height/2;
						break;
				}

				tempGObject.SetXY(offsetX, offsetY);
				
				offsetX += tempGObject.actualWidth;
				offsetX += spacing;
			}

			return allGComponent;
		}

		/// <summary>
		/// 创建一个垂直 组合多个组件 的 大组件
		/// </summary>
		/// <param name="spacing"></param>
		/// <param name="alignment"></param>
		/// <param name="gObjectList"></param>
		/// <returns></returns>
		public static GComponent CreateGComponentVerticalWithSpacing(float spacing, int alignment, List<GObject>gObjectList)
		{
			Log.Debug($"----BPCommon CreateGComponentVerticalWithSpacing spacing = {spacing}");
			if (gObjectList == null || gObjectList.Count == 0)
			{
				return null;
			}

			// 算出宽高
			float width = 0;
			float height = 0;
			int len = gObjectList.Count;
			GObject tempGObject;
			for (int i = 0; i < len; i++)
			{
				tempGObject = gObjectList[i];
				height += tempGObject.actualHeight;
				height += spacing;
				width = Math.Max(width, tempGObject.actualWidth);
			}
			
			// 减去最后的间隔
			height -= spacing;
			
			var allGComponent = new GComponent();
			allGComponent.SetSize(width, height);
			// 设置组件点击不穿透。
			allGComponent.opaque = true;
			
			// var gGraph = new GGraph();
			// gGraph.DrawRect(width, height, 0, Color.red, Color.blue);
			// gGraph.alpha = 0.5f;
			// allGComponent.AddChild(gGraph);
			
			float offsetY = 0;
			for (int i = 0; i < len; i++)
			{
				tempGObject = gObjectList[i];
				allGComponent.AddChild(tempGObject);
				
				float offsetX = 0;
				switch (alignment)
				{
					case BPG_COMMON.UI_ALIGNMENT_VERTICAL_LEFT:
						offsetX = 0;
						break;

					case BPG_COMMON.UI_ALIGNMENT_VERTICAL_CENTER:
						offsetX = (width - tempGObject.actualWidth)/2;
						break;

					case BPG_COMMON.UI_ALIGNMENT_VERTICAL_RIGHT:
						offsetX = width - tempGObject.actualWidth;
						break;

					default:
						offsetX = width/2;
						break;
				}

				tempGObject.SetXY(offsetX, offsetY);
				
				offsetY += tempGObject.actualHeight;
				offsetY += spacing;
			}

			return allGComponent;
		}


		/// <summary>
		/// 将"1,1;1,1;2,2"转成[1,1][1,1][2,2]这样的数组返回
		/// </summary>
		/// <returns></returns>	
        public static List<Dictionary<int, int>> splitStringToIntArrayBySemicolon(string tmpString)
		{
			if(string.IsNullOrEmpty(tmpString) == true){
                return null;
			}

			// 去除最后的空格
            tmpString = tmpString.Trim();

            // 切割字符串,并且去除最后的,;
            int realSize = 0;
            string[] tmpArray = tmpString.Split(new char[2]{',', ';'});
            for (int index = tmpArray.Length - 1; index >= 0; --index)
            {
                string lastChar = tmpArray[index];
                if(lastChar == ";" || lastChar == ","){
                    continue;
                }
                else
                {
                    realSize = index;
                    break;
                }
            }
            
			// 得到结果
            List<Dictionary<int, int>> resultList = new List<Dictionary<int, int>>();
            for(int index = 0; index < realSize; index += 2)
			{		
                // Log.Debug("realSize ==> " + realSize + "index ==> " + index + "Error  CardId===> " + tmpArray[index] + " num ==> " + tmpArray[index+1]);
				int key = int.Parse(tmpArray[index]);
				int val = int.Parse(tmpArray[index+1]);
				
                Dictionary<int, int> dic = new Dictionary<int, int>();
                dic.Add(key, val);
                resultList.Add(dic);
			}
			
            return resultList;
		}
		
		/// <summary>
		/// 将字符串 按切割字符 切割为数组：1,2,3 =》 [1,2,3]
		/// </summary>
		/// <returns></returns>	
		public static string[] SplitStringToArray(string tmpString, char splitChar = ',')
		{
			if(string.IsNullOrEmpty(tmpString)){
				return null;
			}

			// 去除最后的空格
			tmpString = tmpString.Trim();
			var tmpArray = tmpString.Split(splitChar);
			return tmpArray;
		}
		
		/// <summary>
		/// 将字符串 按切割字符 切割为数组列表：1,2,3;a,b,c; =》 list [1,2,3][a,b,c]
		/// </summary>
		/// <returns></returns>	
		public static List<string[]> SplitStringToList(string tmpString, char splitChar1 = ';', char splitChar2 = ',')
		{
			var tempArray = SplitStringToArray(tmpString, splitChar1);
			if (tempArray == null || tempArray.Length == 0) return null;
			
			var resultList = new List<string[]>();
			for (var i = 0; i < tempArray.Length; i++)
			{
				var tempStr = tempArray[i];
				var cellArray = SplitStringToArray(tempStr, splitChar2);
				if (cellArray == null || cellArray.Length == 0) continue;
				resultList.Add(cellArray);
			}

			return resultList;
		}

		/// <summary>
		/// 判断字符串是否是数字
		/// </summary>
		public static bool IsNumber(string temp)
		{
			if (string.IsNullOrWhiteSpace(temp)) return false;
//			const string pattern = "^[0-9]*$";
//			var rx = new Regex(pattern);
//			return rx.IsMatch(temp);
			
			//判断整数
			var regex = new Regex(@"^\d+$");
			//判断小数
			var regex1 = new Regex(@"^\d+(\.\d+)?$");
			return regex.IsMatch(temp) || regex1.IsMatch(temp);
		}

		#region ============================== 战斗公式相关 ==============================

		public static float BattleFormula_1(float[] valueList)
		{
			if (valueList == null || valueList.Length != 3)
			{
				return 0;
			}
			
			var AAA = valueList[0];
			var BBB = valueList[1];
			var CCC = valueList[2];
			
			Log.Debug($"---- BPCommon BattleFormula_1 AAA = {AAA} BBB = {BBB} CCC = {CCC}");
			
			return AAA * BBB + CCC;
		}
		
		public static float BattleFormula_2(float[] valueList)
		{
			if (valueList == null || valueList.Length != 6)
			{
				return 0;
			}
			
			var AAA = valueList[0];
			var BBB = valueList[1];
			var CCC = valueList[2];
			var DDD = valueList[3];
			var EEE = valueList[4];
			var FFF = valueList[5];
			
			Log.Debug($"---- BPCommon BattleFormula_1 AAA = {AAA} BBB = {BBB} CCC = {CCC} DDD = {DDD} EEE = {EEE} FFF = {FFF}");
			
			return AAA * CCC + DDD + BBB * (AAA * EEE + FFF);
		}
		

		/// <summary>
		/// 驳接地图. 以map1世界坐标为基准,然后在指定方向拼接地图2.
		/// 方向请参考BPGlobal中的定义
		/// UI_ALIGNMENT_HORIZONTAL_TOP = 0
		/// UI_ALIGNMENT_HORIZONTAL_BOTTOM = 2
		/// UI_ALIGNMENT_VERTICAL_LEFT = 3;
		/// UI_ALIGNMENT_VERTICAL_RIGHT = 5;
		/// </summary>
		/// <param name="map1"></param>
		/// <param name="map2"></param>
		/// <param name="direction"></param>
		/// <param name="maskPrefixName"></param>
		public static void ConnectMap(GameObject map1, GameObject map2, int direction, string maskPrefixName)
		{
			if(map1 == null || map2 == null) 
				return;

			Transform t1;
			Transform t2;

			// [上] 这种是地图1的(1, 2) 与 地图2的(4, 3)
			if(direction == BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_TOP)
			{
				t1 = map1.transform.Find(maskPrefixName + "1");
				t2 = map2.transform.Find(maskPrefixName + "4");
			}
			// [下] 这种是地图1的(3, 4) 与地图2的(2, 1)
			else if(direction == BPG_COMMON.UI_ALIGNMENT_HORIZONTAL_BOTTOM)
			{
				t1 = map1.transform.Find(maskPrefixName + "3");
				t2 = map2.transform.Find(maskPrefixName + "2");
			}
			// [左] 这种是地图1的(2, 3) 与 地图2的(1, 4)
			else if(direction == BPG_COMMON.UI_ALIGNMENT_VERTICAL_LEFT)
			{
				t1 = map1.transform.Find(maskPrefixName + "2");
				t2 = map2.transform.Find(maskPrefixName + "1");
			}
			// [右] 这种是地图1的(1, 4) 与 地图2的(2, 3)
			else
			{
				t1 = map1.transform.Find(maskPrefixName + "1");
				t2 = map2.transform.Find(maskPrefixName + "2");
			}

			if(t1 == null || t2 == null) return;

			// 计算出地图2的移动位置
			float x = t1.position.x - t2.position.x;
			float y = t1.position.y - t2.position.y;
			map2.transform.position = new Vector3(map2.transform.position.x + x, map2.transform.position.y + y, 0);
		}

		/// <summary>
		/// 替换建筑
		/// </summary>
		/// <param name="old"></param>
		/// <param name="newBuild"></param>
		public static void ReplaceBuilding(GameObject old, GameObject newBuild)
		{
			if(old == null || newBuild == null)
			{
				return;
			}

			newBuild.transform.SetParent(old.transform.parent);
			newBuild.transform.position = new Vector3(old.transform.position.x, old.transform.position.y);
		}

		#endregion

		/// <summary>
		/// 自己写一个转int32的
		/// </summary>
		/// <param name="byteArray"></param>
		public static Int32 ToInt32(byte[] byteArray)
		{
			if(byteArray == null || byteArray.Length != 4)
			{
				return 0;
			}

			byte a = byteArray[0];
			byte b = byteArray[1];
			byte c = byteArray[2];
			byte d = byteArray[3];

			return (a << 24) + (b << 12) + (c << 8) + d;
		}

		#region ==============fairyGUI by Ron==============
		/// <summary>
		/// 屏幕宽度转成fairyGUI的宽度
		/// </summary>
		/// <param name="screenWidth"></param>
		/// <returns></returns>
		public static float ScreenWidthToFairyGUIWidth(float screenWidth)
		{
			return screenWidth / BPG_UI.FairyGUIScale;
		}

		/// <summary>
		/// 虽然现在的比例都一样.但还是先写多一个.
		/// 以后如果改了比例,那么直接改掉这个函数就可以了
		/// </summary>
		/// <param name="screenHeight"></param>
		/// <returns></returns>
		public static float ScreenHeightToFairyGUIHeight(float screenHeight)
		{
			return screenHeight / BPG_UI.FairyGUIScale;
		}

		/// <summary>
		/// 在iPad那样的比例平板里.为了看起来界面是在左下角为(0, 0)的感觉.
		/// 直接偏移view的y过去.这里就是计算在fariyGUI里,应该下移几个单位
		/// </summary>
		/// <returns></returns>
		public static float GetMoveDownHeight()
		{
			float height = Screen.height - (BPG_UI.FAIRY_GUI_HIGHT * BPG_UI.FairyGUIScale);
			height = Math.Max(1, height);
			if(height <= 1)
				return 0;

			return height / BPG_UI.FairyGUIScale;
		}

		/// <summary>
		/// 获取这个控件.在屏幕上下居中的坐标y
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float GetCenterPosXByGObject(GObject obj)
		{
			if(obj == null)
				return 0;

			float width = BPG_UI.FAIRY_GUI_WIDTH;
			float posX = (width - obj.width) / 2.0f;

			// 加上锚点的运算
			return posX + obj.pivot.x * obj.width;
		}

		/// <summary>
		/// 获取这个控件.在屏幕上下居中的坐标y
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float GetCenterPosYByGObject(GObject obj)
		{
			if(obj == null)
				return 0;

			float height = BPG_UI.FAIRY_GUI_HIGHT;
			float posY = (height - obj.height) / 2.0f;

			// 加上锚点的运算
			return posY + obj.pivot.y * obj.height;
		}

		/// <summary>
		/// 获取这个控件.在屏幕上下居中的坐标y
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float GetBottomPosYByGObject(GObject obj)
		{
			if(obj == null)
				return 0;

			float height = BPG_UI.FAIRY_GUI_HIGHT;
			float posY = (height - obj.height);

			// 加上锚点的运算
			return posY + obj.pivot.y * obj.height;
		}
		

		/// <summary>
        /// 游戏时长转换
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static string Timestamp2String(float gameTime)
        {
            if(gameTime <= 0)
                return "-- --";
            
            int hour = (int)gameTime / 3600;
            int min = (int)((gameTime - hour * 3600) / 60);
            int sec = (int)(gameTime - hour * 3600 - min * 60);

            string result = $"{hour}";
            if(min < 10)
            {
                result += $" : 0{min}";
            }
            else
            {
                result += $" : {min}";
            }

            if(sec < 10)
            {
                result += $" : 0{sec}";
            }
            else
            {
                result += $" : {sec}";
            }

            return result;
        }

		#endregion
	}
}

