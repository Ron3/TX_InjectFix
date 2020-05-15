#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 这一段代码写的真蛋疼.因为Model层又无法引用Editor层的东西.
/// 而原来的解析又是写在Editor里面的.现在,想在编辑时刻,获取example meta data都获取不了.
/// 只能在这里又写一段一样的
/// 如果到时路径什么的有修改.就导致2边都要改.蛋疼
/// </summary>
namespace ETModel
{
    public class NodeCanvasConfig
    {
        // 定义2个dict. 这样做是为了id/name 均唯一
        // 如果有重复.则会报异常
        public static Dictionary<long, BPNodeCanvasTreeConfig> configDict = new Dictionary<long, BPNodeCanvasTreeConfig>();
        public static Dictionary<string, BPNodeCanvasTreeConfig> configNameDict = new Dictionary<string, BPNodeCanvasTreeConfig>();

        /// <summary>
        /// 读取行为树配置节点.然后生成枚举
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        static void DoCreateEnum()
        {
            configDict.Clear();
            configNameDict.Clear();

            string path = System.Environment.CurrentDirectory;
            path = Path.Combine(path, "Assets/Res/Config/BPNodeCanvasTreeConfig.txt");
            // Debug.Log($"读取行为树的配置 Path ==> {path}");

            string data = "";
            System.IO.StreamReader fileObj = new System.IO.StreamReader(path);  
            while((data = fileObj.ReadLine()) != null)  
            {
                data = data.Trim();
                if(data.Length >= 2)
                {
                    BPNodeCanvasTreeConfig config = LitJson.JsonMapper.ToObject<BPNodeCanvasTreeConfig>(data);
                    configDict.Add(config.Id, config);
                    configNameDict.Add(config.Name, config);
                }
            }

            fileObj.Close();

            // DoWriteEnumToFile();
        }
    }
}
#endif

