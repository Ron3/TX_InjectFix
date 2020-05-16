/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms. 
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */

using UnityEngine;
using IFix.Core;
using System.IO;
using System.Diagnostics;

// 跑不同仔细看文档Doc/example.md
public class Helloworld : MonoBehaviour 
{
    public TestView view = null;

    // check and load patchs
    void Start () 
    {
        this.StartAsync().Coroutine();
    }

    private async ETModel.ETVoid StartAsync()
    {
        DontDestroyOnLoad(gameObject);
		// ETModel.Game.EventSystem.Add(ETModel.DLLType.Model, typeof(Helloworld).Assembly);

        // 1, 初始化ET相关的一些东西
        ETModel.ResourcesComponent resCmop = ETModel.Game.Scene.AddComponent<ETModel.ResourcesComponent>();

        // 2, 初始化FairyGUI
        FairyGUIHelper.Init();
        
        // 3, 把FairyGUI显示出来
        this.view = new TestView("tx", "view");
        this.view.Show();
        
        
        VirtualMachine.Info = (s) => ETModel.Log.Debug(s);
        //try to load patch for Assembly-CSharp.dll
        var patch = Resources.Load<TextAsset>("Assembly-CSharp.patch");
        if (patch != null)
        {
            ETModel.Log.Debug("loading Assembly-CSharp.patch ...");
            var sw = Stopwatch.StartNew();
            PatchManager.Load(new MemoryStream(patch.bytes));
            ETModel.Log.Debug("patch Assembly-CSharp.patch, using " + sw.ElapsedMilliseconds + " ms");
        }
        
        // try to load patch for Assembly-CSharp-firstpass.dll
        // patch = Resources.Load<TextAsset>("Assembly-CSharp-firstpass.patch");
        // if (patch != null)
        // {
        //     UnityEngine.Debug.Log("loading Assembly-CSharp-firstpass ...");
        //     var sw = Stopwatch.StartNew();
        //     PatchManager.Load(new MemoryStream(patch.bytes));
        //     UnityEngine.Debug.Log("patch Assembly-CSharp-firstpass, using " + sw.ElapsedMilliseconds + " ms");
        // }

        test();
    }


    [IFix.Patch]
    void test()
    {
        var calc = new IFix.Test.Calculator();
        //test calc.Add
        ETModel.Log.Debug("10 + 9 = " + calc.Add(10, 9));
        //test calc.Sub
        ETModel.Log.Debug("10 - 2 = " + calc.Sub(10, 2));

        // var anotherClass = new AnotherClass(1);
        // //AnotherClass in Assembly-CSharp-firstpass.dll
        // var ret = anotherClass.Call(i => i + 1);
        // UnityEngine.Debug.Log("anotherClass.Call, ret = " + ret);

        //test for InjectFix/Fix(Android) InjectFix/Fix(IOS) Menu for unity 2018.3 or newer
#if UNITY_2018_3_OR_NEWER
#if UNITY_IOS
        ETModel.Log.Debug("UNITY_IOS");
#endif
#if UNITY_EDITOR
        ETModel.Log.Debug("UNITY_EDITOR");
#endif
#if UNITY_ANDROID
        ETModel.Log.Debug("UNITY_ANDROID");
#endif
#endif
    }


    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if(this.view != null)
        {
            this.view.Update();
        }
    }
}

