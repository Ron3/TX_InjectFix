
using UnityEngine;
using IFix.Core;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;



/// <summary>
/// 程序开始跑的
/// </summary>
public class Init : MonoBehaviour 
{
    TestView ctr = new TestView("tx", "view");
    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        // 1, 先初始化fairyGUI
        FairyGUIHelper.Init();

        // 2, 创建fairyGUI的界面
        ctr.Show();

        // 3, 
        Common.Debug($"init start");

    }


    public void Update() 
    {
        
    }
}
