#define QUICK_INIT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using ETModel;
using UnityEngine.UI;
using System.Xml;
using FairyGUI;
using LitJson;
// using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class BPTitleComponentAwakeSystem : AwakeSystem<BPTitleComponent>
    {
        public override void Awake(BPTitleComponent self)
        {
            self.Awake();
        }
    }

    public class BPTitleComponent : BPUIBaseComponent
    {
        // private GameObject loginButton;
        // private GameObject testButton1;
        // private GameObject testButton2;
        // private GameObject ronTestButton;
        // bool isShowLog = false;
        // private BPRonTest  ronTestObj = new BPRonTest();

        public void Awake()
        {
            // 1, 构造参数

            // 2, 构造FairyGUI的UI
            base.Awake(BPG_UI.PACKET_NAME, BPG_UI.UI_TITLE_PANEl);
        }

        /// <summary>
        /// 初始化fairyGUI的
        /// </summary>
        public override void InitUI()
        {
            GButton ronTestBtn = this.view.BPGetChild("ronTestBtn").asButton;
            ronTestBtn.onClick.Add(this.OnRonTestBtnClick);

            GButton jeffTestBtn = this.view.BPGetChild("jeffTestBtn").asButton;
            jeffTestBtn.onClick.Add(this.OnJeffTestBtnClick);

            GButton fayTestBtn = this.view.BPGetChild("fayTestBtn").asButton;
            fayTestBtn.onClick.Add(this.OnFayTestBtnClick);
        }

        
        public void Update()
        {
            
        }


        public void OnRonTestBtnClick(EventContext context)
        {
            // Log.Debug("OnRonTestBtnClick ~~~");
            GButton btn = context.sender as GButton;
            BPRonTest test = new BPRonTest();
            test.StartTest(btn);
        }

        public void OnJeffTestBtnClick(EventContext context)
        {
            GButton btn = context.sender as GButton;
        }

        public void OnFayTestBtnClick(EventContext context)
        {
            GButton btn = context.sender as GButton;
        }
    }

}
