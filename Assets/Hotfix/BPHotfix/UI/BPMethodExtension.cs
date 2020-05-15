using FairyGUI;
using ETModel;
using UnityEngine;


namespace ETHotfix
{
    public static class BPMehtondExtension
    {
        #region ========实现某些辅助函数========
        /// <summary>
		/// 从FairyGUI的view里面.递归遍历得到对应名字的child(只会遇到第一个就返回)
		/// </summary>
		/// <param name="comp"></param>
		/// <param name="childName"></param>
		/// <returns></returns>
		private static GObject _GetChildFromComp(GComponent comp, string childName)
		{
			GObject[] childArray = comp.GetChildren();
			return _GetChildFromComp(childArray, childName);
		}

		/// <summary>
		/// 从FairyGUI的view里面.递归遍历得到对应名字的child(只会遇到第一个就返回)
		/// </summary>
		/// <param name="childArray"></param>
		/// <param name="childName"></param>
		/// <returns></returns>
		private static GObject _GetChildFromComp(GObject[] childArray, string childName)
		{
			if(childArray == null){
				return null;
			}
			
			foreach(GObject obj in childArray)
			{
				if(obj.name == childName) {
					return obj;
				}
				else if(obj.GetType() == typeof(GComponent)){
					// Log.Debug("递归遍历====> " + obj.name);
					GComponent comp = obj as GComponent;
					GObject resultObj = _GetChildFromComp(comp.GetChildren(), childName);
					if(resultObj != null) {
						return resultObj;
					}
				}
			}
			
			return null;
		}
        #endregion


        /// <summary>
        /// 递归遍历.获取第一个是childName的GObject
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GObject BPGetChild(this GComponent parent, string childName)
        {
            // return BPCommon.GetChildFromComp(parent, childName);
            return _GetChildFromComp(parent, childName);
        }
        
        /// <summary>
        /// 获取在ab包中的loader.url
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static string BPGetLoaderUrlFromAB(this GLoader loader, string abName, string resName)
        {
            // return BPCommon.GetLoaderUrl(abName, resName);
            return BPUICommon.GetLoaderUrlFromABPackage(abName, resName);
        }

        /// <summary>
        /// 从fairyGUI中获取loader.url
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="resName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string BPGetLoaderURLFromFG(this GLoader loader, string resName, string packageName=BPG_UI.PACKET_NAME)
        {
            return BPUICommon.GetLoaderUrlFromFGPackage(resName, packageName);
        }

        /// <summary>
        /// 获取在ab包中的loader.url
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static string BPGetLoaderUrlFromAB(this BPGLoader loader, string abName, string resName)
        {
            // return BPCommon.GetLoaderUrl(abName, resName);
            return BPUICommon.GetLoaderUrlFromABPackage(abName, resName);
        }


        /// <summary>
        /// 从fairyGUI包中加载loader.url
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="resName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string BPGetLoaderUrlFromFG(this BPGLoader loader, string resName, string packageName=BPG_UI.PACKET_NAME)
        {
            return BPUICommon.GetLoaderUrlFromFGPackage(resName, packageName);
        }

        /// <summary>
        /// 直接拉伸到全屏的. 其实就是修改了设计分辨率.原来是1334 X 750的.这次直接修改掉
        /// 
        /// 2019-11-19
        /// 多分辨率适配算法V2.0
        /// 1, 所有界面都做成全屏.设计分辨率都是1334 X 750( 这个也叫幕布 )
        /// 2, 接着在幕布里面埋一个bpContentView. 所有的内容都在这里contentView里
        /// 3, 底层统一修改幕布大小,让其铺满全屏. 接着根据缩放分辨率, 统一对bpCotentView进行缩放
        /// 4, bpContentView在幕布里需要做好关联. 设置好中心锚点. 在修改幕布 && 缩放的时候. 位置将会自动对齐
        /// </summary>
        /// <param name="view"></param>
        public static void BPFairyGUIMakeFullScreen(this GComponent view)
        {
            int screenWidth = UnityEngine.Screen.width;
            int screenHeight = UnityEngine.Screen.height;
            
            int designWith = (int)(screenWidth / BPG_UI.FairyGUIScale);
            int desginHeight = (int)(screenHeight / BPG_UI.FairyGUIScale);
            view.SetSize(designWith, desginHeight);
            Log.Debug($"FullScreen ==> {designWith},{desginHeight}, {BPG_UI.FairyGUIScale}, {view.width}, {view.height}");

            // 2, =====================2019-11-18 适配contentView =====================
            GObject tempContent = view.BPGetChild("bpContentView");
            if(tempContent != null)
                BPFairyGUIScaleContentView(tempContent as GComponent);
        }

        /// <summary>
        /// 按照命名约定.缩放 bpContentView
        /// </summary>
        /// <param name="contentView"></param>
        public static void BPFairyGUIScaleContentView(this GComponent contentView)
        {
            if(contentView == null)
                return;

            // 屏幕越窄. screenRatio值越大. 只有超过设计比例的,才需要进行压缩
            float designRatio = BPG_UI.FAIRY_GUI_WIDTH * 1.0f / BPG_UI.FAIRY_GUI_HIGHT;
            float screenRatio = BPUICommon.GetScreenRatio();
            if(designRatio >= screenRatio)
            {
                return;
            }
            
            // 所有scale, 都统一
            float realPhysicsHeight = BPG_UI.FAIRY_GUI_HIGHT * BPG_UI.FairyGUIScale;
            float scale = UnityEngine.Screen.height * 1.0f / realPhysicsHeight;
            contentView.SetScale(scale, scale);
        }
    }
}
