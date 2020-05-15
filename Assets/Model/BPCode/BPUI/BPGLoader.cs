using UnityEngine;
using FairyGUI.Utils;
using ETModel;

namespace FairyGUI
{
    /// <summary>
    /// 说明: 根据fairyGUI的文档说明,如果要加载fairyGUI外部的资源
    /// 那么就需要重写下面2个函数.
    /// 在这里,我们采用的是根据url来区分是加载的是ab包,还是它以前的(命名约定方式来实现)
    /// 命名约定 ==> ab://ab包名字/资源名字
    /// 举例子:   ab://icon.unity3d/monster_header_10001
    /// 关于texture资源释放问题.交给ET底层的ResourceComponent去做.
    /// 原理是,当你加载某个texture的时候,你先需要加载ab包.而ET底层对ab包采用引用计数器来做的
    /// 所以当ui关闭的时候,需要调用unload(ab) 就应该可以了
    /// </summary>
    public class BPGLoader : GLoader
    {
        /// <summary>
        /// 外部资源的命名前缀约定
        /// </summary>
        string externalResourcePrefix = "ab://";


        protected override void LoadExternal()
        {
            /*
            开始外部载入，地址在url属性
            载入完成后调用OnExternalLoadSuccess
            载入失败调用OnExternalLoadFailed
            注意：如果是外部载入，在载入结束后，调用OnExternalLoadSuccess或OnExternalLoadFailed前，
            比较严谨的做法是先检查url属性是否已经和这个载入的内容不相符。
            如果不相符，表示loader已经被修改了。
            这种情况下应该放弃调用OnExternalLoadSuccess或OnExternalLoadFailed。
            */

            if(this.isLoadFromAB() == true)
            {
                // 注意,上层在用loader.url = "ab://xxx/xxx" 的时候.是需要做LoadBundle相应的工作
                string abName = this.getABName();
                string resourceName = this.getResourceName();
                // Log.Debug("abName ==> " + abName + " resourceName ==> " + resourceName);

                ResourcesComponent resourceComp = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                UnityEngine.Object obj = resourceComp.GetAsset(abName, resourceName);
                // resourceComp.LoadBundle(abName);
                // resourceComp.UnloadBundle(abName);

                // 加载成功
                Texture2D tex = obj as Texture2D;
                if (tex != null)
                    onExternalLoadSuccess(new NTexture(tex));
                else
                    onExternalLoadFailed();
            }
            else
            {
                Texture2D tex = (Texture2D)Resources.Load(this.url, typeof(Texture2D));
                if (tex != null)
                    onExternalLoadSuccess(new NTexture(tex));
                else
                    onExternalLoadFailed();    
            }
        }

        protected override void FreeExternal(NTexture texture)
        {
            // 释放外部载入的资源
            
            // 190814 by Ron
            // 关于texture资源释放问题.交给ET底层的ResourceComponent去做.
            // 原理是,当你加载某个texture的时候,你先需要加载ab包.而ET底层对ab包采用引用计数器来做的
            // 所以当ui关闭的时候,需要调用unload(ab) 就应该可以了
        }
        

        /// <summary>
        /// 根据url判定是否从ab包中加载
        /// </summary>
        /// <returns></returns>
        public bool isLoadFromAB()
        {
            return this.url.StartsWith(this.externalResourcePrefix) == true;
        }

        /// <summary>
        /// 根据传入的url.获取a包名字
        /// </summary>
        /// <returns></returns>
        public string getABName()
        {
            if(this.url.StartsWith(this.externalResourcePrefix) == false)
                return "";

            string path = this.url.Substring(this.externalResourcePrefix.Length);
            int index = path.LastIndexOf("/");
            if(index < 0)
                return "";

            return path.Substring(0, index);
        }

        /// <summary>
        /// 获取资源的名
        /// </summary>
        /// <returns></returns>
        public string getResourceName()
        {
            if(this.url.StartsWith(this.externalResourcePrefix) == false)
                return "";

            int index = this.url.LastIndexOf("/");
            if(index < 0)
                return "";

            return this.url.Substring(index+ "/".Length);
        }
    }
}


