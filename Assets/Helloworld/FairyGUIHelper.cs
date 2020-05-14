using FairyGUI;
using UnityEngine;


public static class  FairyGUIHelper
{
    public readonly static string desAbName = "tx_injectfixfgui_des.unity3d";
    public readonly static string resAbName = "tx_injectfixfgui_res.unity3d";

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        Common.LoadAssetBundle(desAbName);
        Common.LoadAssetBundle(resAbName);

        GRoot.inst.SetContentScaleFactor(1334, 750, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
			
// #if UNITY_EDITOR

//             Common.Debug("Editor模式. 采用资源加载");
//             string path = "Assets/Res/fairygui/tx";
//             UIPackage.AddPackage(path);
// #else           
        
            Common.Debug("采用ab包加载FairyGUI包资源");
            string descBundleName = "tx_injectfixfgui_des.unity3d";
            string resBundleName = "tx_injectfixfgui_res.unity3d";
            Common.LoadAssetBundle(descBundleName);
            Common.LoadAssetBundle(resBundleName);
            AssetBundle descBundleObj = Common.GetAssetBundle(descBundleName);
            AssetBundle resBundleObj = Common.GetAssetBundle(resBundleName);
            Common.Debug("descBundleObj ==> " + descBundleObj.name);
            Common.Debug("resBundleObj ==> " + resBundleObj.name);
            UIPackage packageObj = UIPackage.AddPackage(descBundleObj, resBundleObj);
            if(packageObj == null){
                Common.Debug("fairyGUI加载 ab包失败 ..");
            }
            else{
                Common.Debug("fairyGUI加载 ab包成功 ..");
            }
// #endif        
        
        // 2.1 注册我们自己的loader
        
        // 3, UIConfig的默认配置
        // SetFairyGUIDefault();

        // // 4, fairyGUI的Unity层设置. fairyGUI的摄像机只显示它自己的层
        // GameObject fairyGUIStage = GameObject.Find("Stage");
        // fairyGUIStage.layer = BPG_UI.FAIRY_GUI_LAYER_INDEX;

        // GameObject grootObj = GameObject.Find("Stage/GRoot");
        // grootObj.layer = BPG_UI.FAIRY_GUI_LAYER_INDEX;

        // GameObject stageCamera = GameObject.Find("Stage Camera");
        // Camera fairyGUICamera = stageCamera.GetComponent<Camera>();

        // fairyGUICamera.cullingMask = 1 << BPG_UI.FAIRY_GUI_LAYER_INDEX;

        // Log.Debug("初始化fairyGUI成功.........");

        Common.Debug("FairyGUI初始化成功......");
    }
}


