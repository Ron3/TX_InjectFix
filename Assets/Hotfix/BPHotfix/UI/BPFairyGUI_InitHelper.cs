using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ETModel;
using FairyGUI;

namespace ETHotfix
{
	/// <summary>
	/// FairyGUI的初始化
	/// </summary>
	public static class BPFairyGUI_InitHelper
	{	
		/// <summary>
		/// FairyGUI里的包名(只是包名, 不需要路径)
		/// </summary>
		public static string packageName = BPG_UI.PACKET_NAME;

		/// <summary>
		/// 初始化FairyGUI相关的东西
		/// </summary>
		/// <param name="path"></param>
		public static void InitFairyGUI(string path=BPG_UI.PACKET_PATH)
		{
			// 1, 这里先计算屏幕比例, 如果超过屏幕的2:1.那么就用1334 X 618那个比例
			// float screenRate = Math.Max(Screen.width / Screen.height, Screen.height / Screen.width);
			// if(screenRate >= 1.99f)
			// {
			// 	BPG_UI._FAIRY_GUI_WIDTH = 1334;
			// 	BPG_UI._FAIRY_GUI_HIGHT = 618;
			// }
			// else
			// {
			// 	BPG_UI._FAIRY_GUI_WIDTH = 1334;
			// 	BPG_UI._FAIRY_GUI_HIGHT = 750;
			// }

			// 2, 设置全局缩放比例
			GRoot.inst.SetContentScaleFactor(BPG_UI.FAIRY_GUI_WIDTH, BPG_UI.FAIRY_GUI_HIGHT, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
			
			if(ETModel.Define.IsEditorMode == true)
			{
				Log.Debug("采用路径加载FairyGUI包资源");
				UIPackage.AddPackage(path);
			}
			else
			{
				Log.Debug("采用ab包加载FairyGUI包资源");
				string descBundleName = "pixelgamefgui_des.unity3d";
				string resBundleName = "pixelgamefgui_res.unity3d";
				ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle(descBundleName);
				ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle(resBundleName);
				AssetBundle descBundleObj = ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAssetBundle(descBundleName);
				AssetBundle resBundleObj = ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAssetBundle(resBundleName);
				Log.Debug("descBundleObj ==> " + descBundleObj.name);
				Log.Debug("resBundleObj ==> " + resBundleObj.name);
				UIPackage packageObj = UIPackage.AddPackage(descBundleObj, resBundleObj);
				if(packageObj == null){
					Log.Error("fairyGUI加载 ab包失败 ..");
				}
				else{
					Log.Debug("fairyGUI加载 ab包成功 ..");
				}
			}
			
			// 2.1 注册我们自己的loader
			UIObjectFactory.SetLoaderExtension(typeof(BPGLoader));

			// 3, UIConfig的默认配置
            SetFairyGUIDefault();

			// 4, fairyGUI的Unity层设置. fairyGUI的摄像机只显示它自己的层
			GameObject fairyGUIStage = GameObject.Find("Stage");
			fairyGUIStage.layer = BPG_UI.FAIRY_GUI_LAYER_INDEX;

			GameObject grootObj = GameObject.Find("Stage/GRoot");
			grootObj.layer = BPG_UI.FAIRY_GUI_LAYER_INDEX;

			GameObject stageCamera = GameObject.Find("Stage Camera");
			Camera fairyGUICamera = stageCamera.GetComponent<Camera>();

			fairyGUICamera.cullingMask = 1 << BPG_UI.FAIRY_GUI_LAYER_INDEX;

			Log.Debug("初始化fairyGUI成功.........");
		}

		/// <summary>
		/// 初始化FairyGUI.主要是Jeff封装的
		/// </summary>
		public static void SetFairyGUIDefault()
        {
            // Log.Debug("---- BPUIManagerComponent.SetFairyGUIDefault");
            //Use the font names directly 
            UIConfig.defaultFont = "Noto Sans CJK";
            UIConfig.verticalScrollBar = UIPackage.GetItemURL(BPG_UI.PACKET_NAME, "ScrollBar_VT");
            UIConfig.horizontalScrollBar = UIPackage.GetItemURL(BPG_UI.PACKET_NAME, "ScrollBar_HZ");
			Log.Debug("verticalScrollBar ==> " + UIConfig.verticalScrollBar);
			Log.Debug("horizontalScrollBar ==> " + UIConfig.horizontalScrollBar);
            // UIConfig.popupMenu = UIPackage.GetItemURL("Basics", "PopupMenu");

            // Unity版本要求一个AudioClip对象，如果是使用库里面的资源，那么可以使用：
            // UIConfig.buttonSound = (NAudioClip)UIPackage.GetItemAsset(packageName, "click");
			UIConfig.buttonSound = (NAudioClip)UIPackage.GetItemAsset(packageName, "click");
			if (UIConfig.buttonSound == null)
            {
                Log.Error("BPUI_InitHelper load button sound error!");
            }
			
            //全局音量    
            UIConfig.buttonSoundVolumeScale = 1f;
        }
	}
}
