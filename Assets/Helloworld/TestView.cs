using UnityEngine;
using FairyGUI;


public class TestView
{
    public string xmlSettingName;
    public string packageName;
    public GComponent view;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xmlName"></param>
    public TestView(string packageName, string xmlName)
    {
        this.xmlSettingName = xmlName;
        this.packageName = packageName;
    }


    /// <summary>
    /// 
    /// </summary>
    public void Show()
    {
        // 1, 加载fgui的xml配置
        GObject panel = UIPackage.CreateObject(packageName, this.xmlSettingName);

        // 2, 如果依然不行.那么则跳出错误
        if (panel == null)
        {
            Common.Error(this.GetType() + "/Awake() 获取不到FairyGui对象！确认项目配置正确后，请检查包名：" + this.packageName + "，组件名:" + this.xmlSettingName);
            return;
        }

        this.view = panel as GComponent;
        if(this.view == null)
        {
            Common.Error("转成componentView失败 =======>");
        }

        // 初始化界面
        this.InitUI();

        GRoot.inst.AddChild(this.view);

        // // 初始完后.Add到场景上
        // if(isPopup == false)
        //     GRoot.inst.AddChild(this.view);
        // else
        //     GRoot.inst.ShowPopup(this.view);
    }


    /// <summary>
    /// 
    /// </summary>
    public void InitUI()
    {
        GButton btn = this.view.GetChild("btn1").asButton;
        btn.onClick.Add(this.OnBtnClick);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnBtnClick(EventContext context)
    {
        
    }
}

