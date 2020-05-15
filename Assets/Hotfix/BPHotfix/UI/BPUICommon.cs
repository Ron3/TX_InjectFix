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
	public static class BPUICommon
	{
        // /// <summary>
		// /// 初始化fgui的按钮
		// /// 在按钮里面搞了一个icon_loader
		// /// </summary>
		// /// <param name="btn"></param>
		// /// <param name="iconName"></param>
		// public static void InitFairyGUIBtnIcon(GButton btn, string iconName="btn_return_icon")
		// {
		// 	if(btn == null)
		// 		return;

		// 	string url = BPCommon.GetFariyGUIPath(iconName, BPG_UI.PACKET_NAME);
		// 	GLoader iconLoader = btn.GetChild("icon_loader") as GLoader;
		// 	if(iconLoader == null)
		// 		return;

        //     Log.Debug($"InitFairyGUIBtnIcon=====>{url}");
		// 	iconLoader.url = url;
		// }

        /// <summary>
        /// 获取物理屏幕宽/高比
        /// </summary>
        public static float GetScreenRatio()
        {
            return UnityEngine.Screen.width * 1.0f / UnityEngine.Screen.height;
        }

        /// <summary>
        /// 适配屏幕.如果返回true.表示做了适配.如返回false,则没做任何操作
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static float AdapteScreen(GComponent view)
        {
            if(view == null)
                return 1.0f;

            float designRatio = BPG_UI.FAIRY_GUI_WIDTH * 1.0f / BPG_UI.FAIRY_GUI_HIGHT;
            float screenRatio = GetScreenRatio();
            if(designRatio >= screenRatio)
            {
                return 1.0f;
            }

            // 需要适配.这种不能直接改幕布大小.只能改scale
            float realPhysicsHeight = view.height * BPG_UI.FairyGUIScale;
            Log.Debug($"realPhysicsHeight ==> {realPhysicsHeight}, FairyGUIScale ==> {BPG_UI.FairyGUIScale}");
            float scale = UnityEngine.Screen.height * 1.0f / realPhysicsHeight;
            view.SetScale(scale, scale);
            Log.Debug($"2222 scale =======> {scale}");
            return scale;
        }

        /// <summary>
		/// 获取FairyGUI的图片路径.让GLoader可以加载.     ui://包名/图片名
		/// </summary>
		/// <param name="imageName"></param>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public static string GetLoaderUrlFromFGPackage(string imageName, string packageName=BPG_UI.PACKET_NAME)
		{
			return "ui://" + packageName + "/" + imageName;
		}

        /// <summary>
		/// 获取loader的url
		/// </summary>
		/// <param name="abName"></param>
		/// <param name="imgName"></param>
		/// <returns></returns>
		public static string GetLoaderUrlFromABPackage(string abName, string imgName)
		{
			return string.Format("ab://{0}/{1}", abName, imgName);
		}


        /// <summary>
		/// 创建fairyGUI的对象
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="resName"></param>
		/// <returns></returns>
		public static GObject CreateFairyGUIObj(string packageName, string resName)
		{
			return UIPackage.CreateObject(packageName, resName);
		}

        /// <summary>
		/// 根据对象,和指定的锚点.获取fairyGUI的舞台坐标.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="pivotX"></param>
		/// <param name="pivotY"></param>
		/// <returns></returns>
		public static Vector2 GetStagePositionByPivot(GObject obj, float pivotX, float pivotY)
		{
			if(obj == null)
				return Vector2.zero;

			Vector2 pos = new Vector2(obj.position.x, obj.position.y);
			
			// 先求出左上角.在舞台的坐标
			if(obj.pivotAsAnchor == true)
			{
				pos.x -= obj.width * obj.pivotX;
				pos.y -= obj.height * obj.pivotY;
			}

			pos.x += obj.width * pivotX;
			pos.y += obj.height * pivotY;
			return pos;
		}

        /// <summary>
		/// 根据比例.获得屏幕位置
		/// </summary>
		/// <param name="rate"></param>
		/// <returns></returns>
		public static float GetHeightByRate(float rate)
		{
			return BPG_UI.GetFGUIHeightByRate(rate);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rate"></param>
		/// <returns></returns>
		public static float GetWidthByRate(float rate)
		{
			return BPG_UI.GetFGUIWidthByRate(rate);
		}
    }
}
