using FairyGUI;


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
    }
}


