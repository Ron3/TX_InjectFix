using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ETEditor
{
    // : SaveAssetsProcessor
    public class CreateNodeCanvasEnum 
    {
        // 定义2个dict. 这样做是为了id/name 均唯一
        // 如果有重复.则会报异常
        static Dictionary<long, BPNodeCanvasTreeConfig> configDict = new Dictionary<long, BPNodeCanvasTreeConfig>();
        static Dictionary<string, BPNodeCanvasTreeConfig> configNameDict = new Dictionary<string, BPNodeCanvasTreeConfig>();

        /// <summary>
        /// 读取行为树配置节点.然后生成枚举
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        static void DoCreateEnum()
        {
            configDict.Clear();
            configNameDict.Clear();

            // Debug.Log($"Current ====> {System.Environment.CurrentDirectory}");
            string path = System.Environment.CurrentDirectory;
            path = Path.Combine(path, "Assets/Res/Config/BPNodeCanvasTreeConfig.txt");
            // Debug.Log($"读取行为树的配置 Path ==> {path}");

            string data = "";
            System.IO.StreamReader fileObj = new System.IO.StreamReader(path);  
            while((data = fileObj.ReadLine()) != null)  
            {
                // Debug.Log($"data ===> {data}");
                data = data.Trim();
                if(data.Length >= 2)
                {
                    BPNodeCanvasTreeConfig config = LitJson.JsonMapper.ToObject<BPNodeCanvasTreeConfig>(data);
                    configDict.Add(config.Id, config);
                    configNameDict.Add(config.Name, config);
                    // Debug.Log($"行为树配置 Id ==> {config.Id}, ===>{config.Name}");
                }
            }

            fileObj.Close();

            DoWriteEnumToFile();
        }


        /// <summary>
        /// 写枚举到文件
        /// TODO Ron. 以后恐怕是要把条件判定的分开
        /// </summary>
        static void DoWriteEnumToFile()
        {
            string resultData = "";
            resultData += "// 该文件的代码是自动生成的.只要修改任何一个脚本.unityEditor重新编译.就会根据策划的行为树配置文件.重新生成对应的枚举";
            resultData += "\n";
            resultData += "// 如果因为该文件编译出错.可以直接删除.";
            resultData += "\n";
            resultData += "#if UNITY_EDITOR";
            resultData += "\n";
            resultData += "\n";
            resultData += "public enum BP_NODE_CANVAS_ACTION_ID";
            resultData += "\n";
            resultData += "{";
            resultData += "\n";

            // 首先写入一个占位的
            resultData += "    None = 0,";
            resultData += "\n";

            List<long> keyList = new List<long>();
            foreach(long configId in configDict.Keys)
            {
                keyList.Add(configId);
            }
            keyList.Sort();

            // 先写actionTask
            foreach(long configId in keyList)
            {
                BPNodeCanvasTreeConfig configObj = null;
                configDict.TryGetValue(configId, out configObj);
                resultData += $"    {configObj.Name} = {configId},";
                resultData += "\n";
            }
            
            resultData += "\n";
            resultData += "}";
            resultData += "\n";

            // 在写条件判定的 TODO Ron 以后需要在打开
            // resultData += "public enum BP_NODE_CANVAS_CONDITION_ID";
            // resultData += "\n";
            // resultData += "{";
            // resultData += "\n";
            // foreach(long configId in keyList)
            // {
            //     if(configId < 10000)
            //         continue;
                
            //     BPNodeCanvasTreeConfig configObj = null;
            //     configDict.TryGetValue(configId, out configObj);
            //     resultData += $"    {configObj.Name} = {configId},";
            //     resultData += "\n";
            // }
            // resultData += "}";
            resultData += "\n";
            resultData += "\n";
            resultData += "#endif";
            resultData += "\n";

            string path = System.Environment.CurrentDirectory;
            path = Path.Combine(path, "Assets/Model/BPCode/BPNodeCanvas/BPNodeCanvasEnum.cs");
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);  
            writer.Write(resultData);
            writer.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        static string[] OnWillSaveAssets(string[] paths)
        {
            // Debug.Log("OnWillSaveAssets");
            // foreach (string path in paths)
            //     Debug.Log(path);
            // return paths;
            return null;
        }
    }
}



