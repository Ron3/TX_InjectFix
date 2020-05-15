using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using System.Xml;
using System;
using System.Globalization;
using System.IO;

namespace ETHotfix
{
    public static class BPUtility
    {
        /// <summary>
        /// 根据数字图片，生成一个数字的gameobject
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberBundleName"></param>
        /// <param name="numberResourcePath"></param>
        /// <returns></returns>
        public static GameObject CreateNumberGameObject(int number, string numberBundleName, string numberResourcePath, int sortingOrder=0)
        {
            // [0] - Texture2D整条数字, [1] - [11] Sprite数字0-9
            UnityEngine.Object[] numberObjects = BPUtility.LoadObjectWithSubAssetsFromAB(numberBundleName, numberResourcePath, false);
            GameObject numberGO = new GameObject("number_" + number);
            number = Math.Abs(number);
            int tempNum = number;
            int counter = ("" + number).Length;
            float singleWidth = ((Sprite)numberObjects[1]).bounds.size.x;
            float totalWidth = counter * singleWidth;
            float tempX = totalWidth/2 - singleWidth/2;
            while(tempNum > 0)
            {
                int subNum = tempNum % 10;
                tempNum /= 10;

                Sprite subNumSprite = (Sprite)numberObjects[subNum + 1];
                GameObject subNumGO = new GameObject("" + subNum);
                subNumGO.AddComponent<SpriteRenderer>().sprite = subNumSprite;
                subNumGO.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                subNumGO.transform.parent = numberGO.transform;
                subNumGO.transform.position = new Vector3(tempX, 0, 0);
                tempX -= singleWidth;
            }            

            return numberGO;
        }

        /// <summary>
        /// 从指定的Parent的Object找到指定name的子Object
        /// </summary>
        /// <returns>The child by name.</returns>
        /// <param name="parentObject">Parent object.</param>
        /// <param name="childName">Child name.</param>
        public static GameObject GetChildByName(GameObject parentObject, string childName)
        {
            foreach (Transform child in parentObject.transform)
            {
                if (child.name.Equals(childName))
                { 
                    return child.gameObject;
                }else
                {
                    GameObject newChild = GetChildByName(child.gameObject, childName);
                    if(newChild != null)
                    {
                        return newChild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 得到所有的子结点
        /// </summary>
        /// <returns>The all child.</returns>
        /// <param name="parentObject">Parent object.</param>
        public static List<GameObject> GetAllChilds(this GameObject parentObject)
        {
            List<GameObject> childs = new List<GameObject>();
            foreach (Transform child in parentObject.transform)
            {
                childs.Add(child.gameObject);
            }

            return childs;
        }

        /// <summary>
        /// 修改当下一层子结点的sprite renderer的sortingOrder
        /// </summary>
        /// <param name="sortingOrderOffset"></param>
        public static void ChangeSpriteRendererSortingLayer(this GameObject go, int sortingOrderOffset)
        {
            var renderers = go.GetComponentsInChildren<SpriteRenderer>();
            foreach(var renderer in renderers)
            {
                renderer.sortingOrder += sortingOrderOffset;
            }
        }

        /// <summary>
        /// 修改所有子结点的sprite renderer的sortingOrder
        /// </summary>
        /// <param name="go"></param>
        /// <param name="sortingOrderOffset"></param>
        public static void ChangeSpriteRendererSortingLayerDeep(this GameObject go, int sortingOrderOffset)
        {
            go.ChangeSpriteRendererSortingLayer(sortingOrderOffset);
            List<GameObject> childs = go.GetAllChilds();
            foreach(GameObject child in childs)
            {
                child.ChangeSpriteRendererSortingLayerDeep(sortingOrderOffset);
            }
        }
        

        /// <summary>
        /// 得到所有的子结点
        /// </summary>
        /// <returns>The all child.</returns>
        /// <param name="parentObject">Parent object.</param>
        public static List<GameObject> GetAllChild(GameObject parentObject)
        {
            List<GameObject> childs = new List<GameObject>();
            foreach (Transform child in parentObject.transform)
            {
                childs.Add(child.gameObject);
            }

            return childs;
        }

        /// <summary>
        /// 从ABq民加载GameObject
        /// </summary>
        /// <returns>The game object from ab.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="objectName">Object name.</param>
        public static GameObject LoadGameObjectFromAB(string assetBundleName, string objectName, bool isUnloadResource = false)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle(assetBundleName);

            GameObject theObject = null;
            try
            {
                var theAsset = resourcesComponent.GetAsset(assetBundleName, objectName);
                Debug.Log(" LoadGameObjectFromAB, bundleGameObject == " + theAsset);
                GameObject bundleGameObject = (GameObject)theAsset;
                theObject = UnityEngine.Object.Instantiate(bundleGameObject);

            }catch(Exception e)
            {
                Debug.Log("LoadGameObjectFromAB -> " + e);
            }

            if (isUnloadResource)
            {
                resourcesComponent.UnloadBundle(assetBundleName);
            }

            return theObject;
        }

        public static T LoadObjectFromAB<T>(string assetBundleName, string objectName, bool isUnloadResource = true) where T : UnityEngine.Object
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle(assetBundleName);

            T bundleGameObject = default;
            try
            {
                
                // bundleGameObject = (T)resourcesComponent.GetAsset(assetBundleName, objectName);
                // 2019-08-20 传入类型
                bundleGameObject = resourcesComponent.GetAssetByType<T>(assetBundleName, objectName);

                //theObject = UnityEngine.Object.Instantiate(bundleGameObject);

            }
            catch (Exception e)
            {
                Debug.Log("LoadObjectFromAB -> " + e);
            }

            if (isUnloadResource)
            {
                resourcesComponent.UnloadBundle(assetBundleName);
            }

            return bundleGameObject;
        }

        public static UnityEngine.Object[] LoadObjectWithSubAssetsFromAB(string assetBundleName, string objectName, bool isUnloadResource = true)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle(assetBundleName);

            UnityEngine.Object[] bundleGameObject = default;
            try
            {
                bundleGameObject = resourcesComponent.GetAllAsset(assetBundleName, objectName);

                //theObject = UnityEngine.Object.Instantiate(bundleGameObject);

            }
            catch (Exception e)
            {
                Debug.Log("LoadObjectWithSubAssetsFromAB -> " + e);
            }

            if (isUnloadResource)
            {
                resourcesComponent.UnloadBundle(assetBundleName);
            }

            return bundleGameObject;
        }

        /// <summary>
        /// 从AB文件加载文本
        /// </summary>
        /// <returns>The text from ab.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="objectName">Object name.</param>
        public static string LoadTextFromAB(string assetBundleName, string objectName, bool isUnloadResource=true)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle(assetBundleName);

            TextAsset bundleGameObject = resourcesComponent.GetAsset(assetBundleName, objectName) as TextAsset;

            //GameObject theObject = UnityEngine.Object.Instantiate(bundleGameObject);

            if(isUnloadResource)
            {
                resourcesComponent.UnloadBundle(assetBundleName);
            }

            //Debug.Log("LoadTextFromAB, bundle name = " + assetBundleName + ", object name = " + objectName + ", objet -> " + bundleGameObject);
            //Debug.Log("type == " + resourcesComponent.GetAsset(assetBundleName, objectName).GetType());
            if(bundleGameObject == null)
            {
                return null;
            }else
            {
                return bundleGameObject.text;
            }
        }


        // /// <summary>
        // /// aes之后的测试代码
        // /// </summary>
        // /// <param name="assetBundleName"></param>
        // /// <param name="objectName"></param>
        // /// <param name="isUnloadResource"></param>
        // /// <returns></returns>
        // public static XmlDocument LoadXmlDocumentFromAB(string assetBundleName, string objectName, bool isUnloadResource = true)
        // {
        //     bool isAES = false;
        //     if(assetBundleName == BPModel.AB_NAME_MAP && objectName.StartsWith("Assets/Res/Map/") == true)
        //     {
        //         isAES = true;
        //         string key = "Assets/Res";
        //         objectName = objectName.Substring(key.Length);
        //         objectName = "Assets/ResEncrypt" + objectName;
        //         Log.Debug("objectName 111 ==> " + objectName);
                
        //     }
        //     else
        //     {
        //         isAES = false;
        //     }

        //     string text = LoadTextFromAB(assetBundleName, objectName, isUnloadResource);
        //     if(text == null)
        //     {
        //         return new XmlDocument();
        //         //return null;
        //     }
        //     XmlDocument xml = new XmlDocument();
        //     if(isAES == false)
        //     {
        //         xml.LoadXml(text);
        //     }
        //     else
        //     {
        //         Log.Debug("base64 111 ==> " + text);
        //         text = BPAESHelper.Decrypt_Base64(text);
        //         Log.Debug("base64 222 ==> " + text);
        //         xml.LoadXml(text);
                
        //     }
        //     return xml;
        // }


        public static XmlDocument LoadXmlDocumentFromAB(string assetBundleName, string objectName, bool isUnloadResource = true, bool isEncrypt=false)
        {
            string text = LoadTextFromAB(assetBundleName, objectName, isUnloadResource);
            if(text == null)
            {
                return new XmlDocument();
                //return null;
            }

            // 2019-06-18 by Ron. 
            if(isEncrypt == true)
            {
                text = BPAESHelper.Decrypt_Base64(text);
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            return xml;
        }

        /// <summary>
        /// 从xml element解出int值，如果没有value，则为默认值
        /// </summary>
        /// <returns>The int from element.</returns>
        /// <param name="element">Element.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static int ParseIntFromElement(XmlElement element, string attributeName, int defaultValue)
        {
            int finalValue = defaultValue;
            try { 
                string value = element.GetAttribute(attributeName);
                if(value.Equals(""))
                {
                    finalValue = defaultValue;
                }else
                {
                    finalValue = int.Parse(value);
                }
            }catch(System.Exception e)
            {
                Debug.Log("Exception caught: {0}" + e.Message);
            }
            finally
            {

            }

            return finalValue;
        }

        public static bool IsOnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static int GetAsInt(this LitJson.JsonData jsonData, string name, int defaultValue)
        {
            try
            {
                if (jsonData == null || name == null)
                {
                    return defaultValue;
                }

                LitJson.JsonData value = jsonData[name];
                if (value == null || (value.IsDouble == false && value.IsInt == false && value.IsLong == false))
                {
                    return defaultValue;
                }

                if(value.IsDouble)
                {
                    return (int)((double)value);
                }else if(value.IsLong)
                {
                    return (int)((long)value);
                }else
                {
                    return (int)value;
                }
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                Debug.Log("Json.GetAsInt Exception = " + e + ", value = " + jsonData[name] + ", type = " + jsonData[name].GetJsonType());
                return defaultValue;
            }
        }

        public static float GetAsFloat(this LitJson.JsonData jsonData, string name, float defaultValue)
        {
            try
            {
                if (jsonData == null || name == null)
                {
                    return defaultValue;
                }

                LitJson.JsonData value = jsonData[name];
                if (value == null || (value.IsDouble == false && value.IsInt == false && value.IsLong == false))
                {
                    return defaultValue;
                }

                if(value.IsInt)
                {
                    return (float)((int)value);
                }else if(value.IsLong)
                {
                    return (float)((long)value);
                }else
                {
                    return (float)((double)value);
                }
                
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                Debug.Log("Json.GetAsFloat Exception = " + e + ", value.GetJsonType == " + jsonData[name].GetJsonType());
                return defaultValue;
            }
        }

        public static double GetAsDouble(this LitJson.JsonData jsonData, string name, double defaultValue)
        {
            try
            {
                if (jsonData == null || name == null)
                {
                    return defaultValue;
                }

                LitJson.JsonData value = jsonData[name];
                if (value == null || (value.IsDouble == false && value.IsInt == false && value.IsLong == false))
                {
                    return defaultValue;
                }

                return (double)value;
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                Debug.Log("Json.GetAsDouble Exception = " + e);
                return defaultValue;
            }
        }

        public static bool GetAsBool(this LitJson.JsonData jsonData, string name, bool defaultValue)
        {
            try
            {
                if (jsonData == null || name == null)
                {
                    return defaultValue;
                }

                LitJson.JsonData value = jsonData[name];
                if (value == null)
                {
                    return defaultValue;
                }

                if(value.IsInt || value.IsDouble || value.IsLong)
                {
                    return ((int)value) != 0;
                }else if(value.IsString)
                {
                    if ((string)value == "1" || string.Equals((string)value, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        value = bool.TrueString;
                    }
                    else if ((string)value == "0" || string.Equals((string)value, "false", StringComparison.OrdinalIgnoreCase))
                    {
                        value = bool.FalseString;
                    }
                }else if(value.IsBoolean == false)
                {
                    return defaultValue;
                }
                return (bool)value;
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                Debug.Log("Json.GetAsBool Exception = " + e);
                return defaultValue;
            }
        }

        public static string GetAsString(this LitJson.JsonData jsonData, string name, string defaultValue)
        {
            try
            {
                if (jsonData == null || name == null)
                {
                    return defaultValue;
                }

                LitJson.JsonData value = jsonData[name];
                if (value == null || value.IsString == false)
                {
                    return defaultValue;
                }

                return (string)value;
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                Debug.Log("Json.GetAsString Exception = " + e);
                return defaultValue;
            }
        }

        public static T GetAttributeAs<T>(this XmlElement element, string name, T defaultValue) where T : IConvertible
        {
            try
            {
                if (element == null)
                {
                    return defaultValue;
                }

                string value = element.GetAttribute(name);
                if (value == null)
                {
                    return defaultValue;
                }

                if(value == "" && typeof(T) != typeof(string))
                {
                    return defaultValue;
                }

                // Special case for enum
                //if (typeof(T).IsEnum)
                //{
                //    return value.ToEnum<T>();
                //}

                // Special case for bool
                if (typeof(T) == typeof(bool))
                {
                    if (value == "1" || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "是", StringComparison.OrdinalIgnoreCase))
                    {
                        value = bool.TrueString;
                    }
                    else if (value == "0" || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "否", StringComparison.OrdinalIgnoreCase))
                    {
                        value = bool.FalseString;
                    }
                }

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }catch(Exception e)
            {
                Debug.Log("name -> " + name);
                Debug.Log(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// 解析类似"1,2,3"等用逗号隔开的int数组
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAttributeEnum<T>(this XmlElement element, string name, Dictionary<string, T> mapping, T defaultValue)
        {
            try
            {
                if (element == null)
                {
                    return defaultValue;
                }

                string key = element.GetAttribute(name);
                if (key == null)
                {
                    return defaultValue;
                }

                if(mapping.ContainsKey(key) == false)
                {
                    return defaultValue;
                }
                return mapping[key];
            }catch(Exception e)
            {
                Debug.Log("name -> " + name);
                Debug.Log(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// 解析类似"1,2,3"等用逗号隔开的float数组
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float[] GetAttributeFloatArray(this XmlElement element, string name, float[] defaultValue, char separator=',')
        {
            try
            {
                if (element == null)
                {
                    return defaultValue;
                }

                string value = element.GetAttribute(name);
                if (value == null)
                {
                    return defaultValue;
                }

                if(value == "" && value.Contains(",") == false)
                {
                    return defaultValue;
                }

                string[] strArray = value.Split(separator);
                if(strArray == null || strArray.Length == 0){
                    return defaultValue;
                }

                float[] floatArray = new float[strArray.Length];
                for(int i = 0 ; i < strArray.Length ; i ++)
                {
                    floatArray[i] = Convert.ToSingle(strArray[i]);
                }

                return floatArray;
            }catch(Exception e)
            {
                Debug.Log("name -> " + name);
                Debug.Log(e);
                return defaultValue;
            }
        }


        /// <summary>
        /// 解析类似"1,2,3"等用逗号隔开的int数组
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int[] GetAttributeIntArray(this XmlElement element, string name, int[] defaultValue, char separator=',')
        {
            try
            {
                if (element == null)
                {
                    return defaultValue;
                }

                string value = element.GetAttribute(name);
                if (value == null)
                {
                    return defaultValue;
                }

                if(value == "")
                {
                    return defaultValue;
                }

                if(value.Contains(",") == false)
                {
                    // Debug.Log("123123, value = " + value);
                    return new int[]{Convert.ToInt32(value)};
                }

                string[] strArray = value.Split(separator);
                if(strArray == null || strArray.Length == 0){
                    return defaultValue;
                }

                int[] intArray = new int[strArray.Length];
                for(int i = 0 ; i < strArray.Length ; i ++)
                {
                    intArray[i] = Int32.Parse(strArray[i]);
                }

                return intArray;
            }catch(Exception e)
            {
                Debug.Log("In Exception, GetAttributeIntArray -> name -> " + name + ", value = " + element.GetAttribute(name));
                Debug.Log(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// 解析类似"1,2;1,3"等用逗号隔开的int数组
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int[][] GetAttributeInt2Array(this XmlElement element, string name, int[][] defaultValue, char separator1=';', char separator2=',')
        {
            try
            {
                if (element == null)
                {
                    return defaultValue;
                }

                string value = element.GetAttribute(name);
                if (value == null)
                {
                    return defaultValue;
                }

                if(value == "")
                {
                    return defaultValue;
                }

                return GetInt2ArrayFromString(value);
            }catch(Exception e)
            {
                Debug.Log("In Exception, GetAttributeInt2Array -> name -> " + name + ", value = " + element.GetAttribute(name));
                Debug.Log(e);
                return defaultValue;
            }
        }

        public static int[][] GetInt2ArrayFromString(string str, char separator1=';', char separator2=',')
        {
            if(str == null || str.Length == 0)
            {
                return null;
            }

            int[][] result = null;
            if(str.IndexOf(separator1) < 0)
            {
                int[] tempResult = GetIntArrayFromString(str, separator2);
                result = new int[1][]{tempResult};
            }else
            {
                string[] tempUnlokInfoStrArray = str.Split(separator1);
                result = new int[tempUnlokInfoStrArray.Length][];
                for(int i = 0 ; i < result.Length ; i ++)
                {
                    string tempUnlockInfoStr = tempUnlokInfoStrArray[i];
                    int[] tempResult = GetIntArrayFromString(tempUnlockInfoStr, separator2);
                    result[i] = tempResult;
                }
            }

            return result;
        }

        public static int[] GetIntArrayFromString(string str, char separator=',')
        {
            if(str == null || str.Length == 0){
                return null;
            }
            string[] strArray = str.Split(separator);
            if(strArray == null || strArray.Length == 0){
                return null;
            }

            int[] intArray = new int[strArray.Length];
            for(int i = 0 ; i < strArray.Length ; i ++)
            {
                intArray[i] = Int32.Parse(strArray[i]);
            }
            return intArray;
        }

        public static float[] GetFloatArrayFromString(string str, char separator=',')
        {
            if(str == null || str.Length == 0){
                return null;
            }
            string[] strArray = str.Split(separator);
            if(strArray == null || strArray.Length == 0){
                return null;
            }

            float[] floatArray = new float[strArray.Length];
            for(int i = 0 ; i < strArray.Length ; i ++)
            {
                // Debug.Log("str array [" + i + "]=" + strArray[i]);
                floatArray[i] = Convert.ToSingle(strArray[i]);
            }
            return floatArray;
        }

        public static XmlElement GetFirstElementByTagName(this XmlDocument xml, string name)
        {
            XmlNodeList nodeList = xml.GetElementsByTagName(name);
            if(nodeList == null || nodeList.Count < 1)
            {
                return null;
            }else
            {
                return (XmlElement)nodeList[0];
            }
        }

        public static XmlElement GetChildNodeByName(this XmlElement element, string name) 
        {
            if (element == null)
            {
                return null;
            }

            XmlNodeList nodeList = element.ChildNodes;
            foreach(XmlNode node in nodeList)
            {
                if(node.Name.Equals(name))
                {
                    return (XmlElement) node;
                }
            }
            return null;
        }


        /// <summary>
        /// 从xml element解出boolean值，如果没有value，则为默认值
        /// </summary>
        /// <returns>The int from element.</returns>
        /// <param name="element">Element.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static bool ParseBoolFromElement(XmlElement element, string attributeName, bool defaultValue)
        {
            bool finalValue = defaultValue;
            try
            {
                string value = element.GetAttribute(attributeName);
                if (value.Equals(""))
                {
                    finalValue = defaultValue;
                }
                else
                {
                    if(value.ToLower().Equals("true"))
                    {
                        finalValue = true;
                    }else
                    {
                        finalValue = false;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Exception caught: {0}" + e.Message);
            }
            finally
            {

            }

            return finalValue;
        }

        public static float CalculateDistanceF(float sx, float sy, float tx, float ty)
        {
            return (Mathf.Abs(sx - tx) + Mathf.Abs(sy - ty));
        }

        public static int CalculateDistance(float sx, float sy, float tx, float ty)
        {
            return (int)(Mathf.Abs(sx - tx) + Mathf.Abs(sy - ty));
        }

        public static int CalculateDistance(int sx, int sy, int tx, int ty)
        {
            return Math.Abs(sx - tx) + Math.Abs(sy - ty);
        }

        public static int CalculateDistance(int p1, int p2)
        {
            return Math.Abs(p1 - p2);
        }

        private static long Jan1st1970Ms = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc).Ticks;
        public static long CurrentTimeMillis()
        {
            return (System.DateTime.UtcNow.Ticks - Jan1st1970Ms) / 10000 ;
        }

        public static void LogXmlNode(XmlNode node)
        {
            if (node == null)
            {
                Debug.Log("logXmlElement, node == null!");
                return;
            }


            string attStr = "Attribute: ";
            XmlAttributeCollection xmlAttributeCollection = node.Attributes;
            foreach(XmlAttribute att in xmlAttributeCollection)
            {
                attStr += "[" + att.Name + "]=" + att.Value + ", ";
            }
            Debug.Log("node's name = " + node.Name + ", child node num = " + node.ChildNodes.Count + ", " + attStr);

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                Debug.Log("---- " + node.Name + "'s childNode[" + i + "] ---- ");
                LogXmlNode(node.ChildNodes[i]);
            }
        }


        /// <summary>
        /// 得到指定文件下指定后缀的所有文件的路径
        /// </summary>
        /// <returns>The all file path by suffix.</returns>
        /// <param name="dirPath">Dir path.</param>
        /// <param name="suffix">Suffix.</param>
        public static List<string> GetAllFilePathBySuffix(string dirPath, string suffix)
        {
            string fullPath = dirPath;
            List<string> allPath = new List<string>();

            //获取指定路径下面的所有资源文件  
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                //FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);      // 循环目录
                FileInfo[] files = direction.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

                //Debug.Log(files.Length);


                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        continue;
                    }

                    string fileFullPath = dirPath + files[i].Name;
                    //string relativePath = fileFullPath.Substring(fileFullPath.IndexOf("Assets", StringComparison.OrdinalIgnoreCase));
                    //relativePath.Replace('\\', '/');
                    //relativePath = "/" + relativePath;
                    string targetPath = fileFullPath;
                    allPath.Add(targetPath);
                    Debug.Log("Target Path: " + targetPath);
                    //Debug.Log( "FullName:" + files[i].FullName );  
                    //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  
                }
            }

            return allPath;
        }

        public static string TransTimeSecondIntToString(long second)
        {
            string str = "";
            try
            {
                long hour = second / 3600;
                long min = second % 3600 / 60;
                long sec = second % 60;
                if (hour < 10)
                {
                    str += "0" + hour.ToString();
                }
                else
                {
                    str += hour.ToString();
                }
                str += ":";
                if (min < 10)
                {
                    str += "0" + min.ToString();
                }
                else
                {
                    str += min.ToString();
                }
                str += ":";
                if (sec < 10)
                {
                    str += "0" + sec.ToString();
                }
                else
                {
                    str += sec.ToString();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Catch:" + ex.Message);
            }
            return str;
        }

        public static LitJson.JsonData LoadJsonDataFromPath(string path)
        {
            string str = System.IO.File.ReadAllText(path);
            if(str == null)
            {
                return null;
            }
            return LitJson.JsonMapper.ToObject(str);
        }

        /// <summary>
        /// 将JsonData保存到output指定的路径里
        /// </summary>
        /// <param name="jsonData">Json data.</param>
        /// <param name="outputPath">Output path.</param>
        public static void Save(this LitJson.JsonData jsonData, string outputPath)
        {
            string jsonStr = jsonData.ToJson();
            //jsonStr = JsonUtility.ToJson(jsonData, true);
            //Debug.Log("333 jsonStr == " + JsonTree(jsonStr));
            //jsonStr = JsonTree(jsonStr);

            System.IO.FileInfo file = new System.IO.FileInfo(outputPath);
            file.Directory.Create();

            System.IO.File.WriteAllText(outputPath, jsonStr);
        }

        //private static string ConvertJsonString(string str)
        //{
        //    //格式化json字符串
        //    JsonSerializer serializer = new JsonSerializer();
        //    TextReader tr = new StringReader(str);
        //    JsonTextReader jtr = new JsonTextReader(tr);
        //    object obj = serializer.Deserialize(jtr);
        //    if (obj != null)
        //    {
        //        StringWriter textWriter = new StringWriter();
        //        JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
        //        {
        //            Formatting = Formatting.Indented,
        //            Indentation = 4,
        //            IndentChar = ' '
        //        };
        //        serializer.Serialize(jsonWriter, obj);
        //        return textWriter.ToString();
        //    }
        //    else
        //    {
        //        return str;
        //    }
        //}
    }
}