using UnityEngine;
using FairyGUI;
using System;
using ETModel;
using System.Collections.Generic;


namespace ETHotfix
{	
	public class BPUIBaseComponent: ETHotfix.Component
	{
		#region =============基类成员=============
		protected string packageName;
		protected string xmlSettingName;
		protected string needLoadABName;

		// 支持底层传入多个ab包加载
		protected List<string> assetBundleList = null;

		// 对应FariyGUI的GComponent. 配置文件最终初始化得到的view对象
		public GComponent view;

        // Fairygui的窗口。如果是弹出窗体，使用Window展示。否则使用GRoot展示
        // protected ExWindow Window;

		#endregion
		
		/// <summary>
		/// 获得物理屏幕的宽高比
		/// </summary>
		/// <returns></returns>
		protected float GetWidthHeightRatio()
		{
			float width = Screen.width;
			float height = Screen.height;
			return Math.Max(width / height, height / width);
		}

		/// <summary>
		/// 加载FairyGUI的界面
		/// </summary>
		/// <param name="strPackageName"></param>
		/// <param name="strUIName"></param>
		/// <param name="needLoadABName"></param>
		protected void Awake(string strPackageName, string strUIName, string needLoadABName="", bool isPopup=false)
		{
			this.packageName = strPackageName;
			// this.uiName = strUIName;
			this.xmlSettingName = strUIName;
			this.needLoadABName = needLoadABName;

			// 1, 加载fgui的xml配置
			GObject panel = UIPackage.CreateObject(packageName, this.xmlSettingName);

			// 2, 如果依然不行.那么则跳出错误
			if (panel == null)
            {
                Log.Error(this.GetType() + "/Awake() 获取不到FairyGui对象！确认项目配置正确后，请检查包名：" + this.packageName + "，组件名:" + this.xmlSettingName);
                return;
            }

			this.view = panel as GComponent;
			if(this.view == null)
			{
				Log.Error("转成componentView失败 =======>");
			}

			// 2019-09-28 直接修改分辨率.拉伸到全屏
			if(this.xmlSettingName.StartsWith("UI_") == true) 
			{
				// Log.Debug($"this.xmlSettingName ==> {this.xmlSettingName}");
				this.view.BPFairyGUIMakeFullScreen();
			}
			
			// 初始化界面之前.先加载响应的ab包
			this.LoadAssetBundle();

			// 初始化界面
			this.InitUI();

			// 初始完后.Add到场景上
			if(isPopup == false)
				GRoot.inst.AddChild(this.view);
			else
				GRoot.inst.ShowPopup(this.view);
				
			this.OnShowEvent();
		}


		/// <summary>
		/// 支持加载多个ab包
		/// </summary>
		/// <param name="strPackageName"></param>
		/// <param name="strUIName"></param>
		/// <param name="argAssetBundleList"></param>
		/// <param name="isPopup"></param>
		protected void Awake(string strPackageName, string strUIName, List<string> argAssetBundleList, bool isPopup=false)
		{
			this.assetBundleList = argAssetBundleList;
			this.Awake(strPackageName, strUIName);
		}


		/// <summary>
		/// 重写父类的释放方法
		/// </summary>
		public override void Dispose()
		{	
			// 这个与OnlyCloseView也不冲突. 如果先调用OnlyCloseView
			// 然后在释放掉这个组件本身,在调用这里,也是没问题的. RemoveChild底层找不到,就不会做任何事
			if(this.view != null)
			{
				GRoot.inst.RemoveChild(this.view);
				this.view.Dispose();
				this.view = null;
				
				// 释放掉ab包
				this.UnLoadAssetBundle();
				this.assetBundleList = null;
			}
			
			// 调用父类的析构函数
			base.Dispose();
		}


		/// <summary>
		/// 只是关闭view.但不会释放资源view
		/// </summary>
		public void OnlyCloseView()
		{
			GRoot.inst.RemoveChild(this.view);
		}
		

		/// <summary>
		/// 初始化UI - 底层重写这个方法.
		/// </summary>
		public virtual void InitUI()
		{
			// Log.Debug("BPController InitUI");
		}


		/// <summary>
		/// 已经把界面add到场景中.
		/// </summary>
		public virtual void OnShowEvent()
		{
			// Log.Debug("OnShowEvent InitUI");

			// 2019-09-10 Fay: 修改显示层
			// this.ChangeLayer(BPMainControllerComponent.currentFairyGUILayer);
		}
		

		/// <summary>
		/// 由于fairyGUi需要引用到ab里的图素(texture)
		/// 那么,在调用InitUI之前.框架会自动调用这个函数.
		/// 并且在释放的时候,会调用unLoad
		/// </summary>
		public virtual void LoadAssetBundle()
		{
			ETModel.ResourcesComponent comp = ETModel.Game.Scene.GetComponent<ETModel.ResourcesComponent>();
			if(this.needLoadABName.IsNullOrEmpty() == false)
			{
				comp.LoadBundle(this.needLoadABName);
			}

			if(this.assetBundleList != null && this.assetBundleList.Count > 0)
			{
				foreach (string assetBundleName in this.assetBundleList)
				{		
					comp.LoadBundle(assetBundleName);
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public virtual void UnLoadAssetBundle()
		{
			ETModel.ResourcesComponent comp = ETModel.Game.Scene.GetComponent<ETModel.ResourcesComponent>();
			if(this.needLoadABName.Length > 0)
			{
				comp.UnloadBundle(this.needLoadABName);
			}

			if(this.assetBundleList != null && this.assetBundleList.Count > 0)
			{
				foreach (string assetBundleName in this.assetBundleList)
				{		
					Log.Debug($"base class UnLoadAssetBundle ===> {assetBundleName}");
					comp.UnloadBundle(assetBundleName);
				}
			}
		}


		public float width
		{
			get {
				if(this.view != null)
					return this.view.width;
				
				return 0f;
			}
		}


		public float height
		{
			get {
				if(this.view != null)
					return this.view.height;
				
				return 0f;
			}
		}


		/// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="xv"></param>
        /// <param name="yv"></param>
        /// <param name="zv"></param>
        public virtual void SetPos(float xv, float yv, float zv)
        {
            this.view.SetPosition(xv, yv, zv);
        }


		/// <summary>
		/// 修改层
		/// </summary>
		/// <param name="layer"></param>
		public virtual void ChangeLayer(int layer)
		{
			if(this.view == null)
				return;

			GameObject go = this.view.displayObject.gameObject;
			if(go == null)
				return;

			// Log.Debug("go.name ==> " + go.name);
			go.layer = layer;

			// 遍历修改子的
			foreach(Transform tran in go.GetComponentsInChildren<Transform>())
			{
				tran.gameObject.layer = layer;
			}
		}


		/// <summary>
        /// 隐藏view
        /// </summary>
        /// <param name="isHide"></param>
        public void HideView(bool isHide=true)
        {
            if(isHide == true)
            {
                this.view.visible = false;
            }
            else
            {
                this.view.visible = true;
            }    
        }


		/// <summary>
		/// 设置是否穿透.
		/// false表示能穿透
		/// </summary>
		/// <param name="opaque"></param>
		public void SetOpaque(bool opaque=true)
		{
			this.view.opaque = opaque;
		}
	}
}