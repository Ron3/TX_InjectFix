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
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;


// 跑不同仔细看文档Doc/example.md
public class Helloworld : MonoBehaviour 
{
    // check and load patchs
    void Start () 
    {
        VirtualMachine.Info = (s) => UnityEngine.Debug.Log(s);

        string assetBundleName = "code.unity3d";
        string dllPath = "Assets/IFix/Resources/Assembly-CSharp.patch.bytes";
        Common.LoadAssetBundle(assetBundleName);
        UnityEngine.TextAsset patch = Common.GetAsset(assetBundleName, dllPath) as UnityEngine.TextAsset;
        if (patch != null)
        {
            UnityEngine.Debug.Log("loading Assembly-CSharp.patch ...");
            var sw = Stopwatch.StartNew();
            PatchManager.Load(new MemoryStream(patch.bytes));
            UnityEngine.Debug.Log("patch Assembly-CSharp.patch, using " + sw.ElapsedMilliseconds + " ms");
        }
        else
        {
            Common.Error("load failed ...");
        }

        // TODO Ron,我们暂时不需要测试firstpass.dll
        //try to load patch for Assembly-CSharp-firstpass.dll
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
        UnityEngine.Debug.Log("10 + 9 = " + calc.Add(10, 9));
        //test calc.Sub
        UnityEngine.Debug.Log("10 - 2 = " + calc.Sub(10, 2));

        var anotherClass = new AnotherClass(1);
        //AnotherClass in Assembly-CSharp-firstpass.dll
        var ret = anotherClass.Call(i => i + 1);
        UnityEngine.Debug.Log("anotherClass.Call, ret = " + ret);

        //test for InjectFix/Fix(Android) InjectFix/Fix(IOS) Menu for unity 2018.3 or newer
#if UNITY_2018_3_OR_NEWER
#if UNITY_IOS
        UnityEngine.Debug.Log("UNITY_IOS");
#endif
#if UNITY_EDITOR
        UnityEngine.Debug.Log("UNITY_EDITOR");
#endif
#if UNITY_ANDROID
        UnityEngine.Debug.Log("UNITY_ANDROID");
#endif
#endif

        // 
        // StartCoroutine(this.DownloadFile("https://news.163.com/20/0513/10/FCGJENDE0001899O.html"));

        // UnityEngine.Debug.Log("test async errror");
        // Method1();
        // Method2();

        BPActor bpActor = this.GetComponent<BPActor>();
        Common.Debug($"bpActor.GetHp2222 ==> {bpActor.GetHp2()}");
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    // [IFix.Patch]
    public IEnumerator DownloadFile(string url)
    {
        // UnityEngine.Debug.Log($"downloadFile 2");

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 10;
        yield return request.SendWebRequest();
        if(request.error != null)     
        {
            UnityEngine.Debug.Log($"加载出错： {request.error}, url is: {url}");
            request.Dispose();
            yield break;
        }
        
        if(request.isDone)
        {
            UnityEngine.Debug.Log($"done ======> {request.downloadHandler.data}");

            string path = "xxxxx";
            File.WriteAllBytes(path, request.downloadHandler.data);
            request.Dispose();
            yield break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    // [IFix.Patch]
    public static async void Method1()
    {  
        await Task.Run(() =>
        {  
            for (int i = 0; i < 2; i++)
            {  
                UnityEngine.Debug.Log("Method 1 change 2!");
            }  
        });  
    }  
    
    
    /// <summary>
    /// 
    /// </summary>
    // [IFix.Patch]
    public static void Method2()
    {  
        for (int i = 0; i < 2; i++)
        {  
            UnityEngine.Debug.Log(" Method 2 change 2!");
        }  
    }
}
