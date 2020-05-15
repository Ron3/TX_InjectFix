using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using ETModel;
using UnityEngine.UI;
using FairyGUI;
using System.Text;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

namespace ETHotfix
{
    public class BPRonTest
    {

        public static bool isOK = false;
        
        public Dictionary<int, BPEntity> entityDict = new Dictionary<int, BPEntity>();

        /// <summary>
        /// 测试入口
        /// </summary>
        /// <param name="btn"></param>
        public void StartTest(GButton btn)
        {
            // this._TestMainUI();
            // this._TestNodeCanvas();
            // this._TestNodeCanvasGetArgs();
            // this._TestLoadNodeCanvas();
            // this._TestDesignerNodeCanvas();         // 测试策划的行为树
            // this._TestGlobalParameters();           // 测试全局变量
            this._TestCalc();

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public void _TestNodeCanvasGetArgs()
        {
            // 这样是获取NPC Blackboard Variables
            GameObject npc = GameObject.Find("NPC");
            BehaviourTreeOwner btOwner = npc.GetComponent<BehaviourTreeOwner>();
            Variable val = btOwner.blackboard.GetVariable("myInteger");
            System.Int32 intval = Convert.ToInt32(val.value);
            Log.Debug($"intval ==> {intval}");

            // 这样是获取Graph Blackboard Variables
            val = btOwner.behaviour.blackboard.GetVariable("myInteger");
            intval = Convert.ToInt32(val.value);
            Log.Debug($"intval 222 ==> {intval}");
            
            // IEnumerable<IBlackboard> blackboards = btOwner.blackboard.GetAllParents(false);
            // foreach(IBlackboard bb in blackboards)
            // {
            //     bb.GetVariable("");
            // }
        }


        /// <summary>
        /// 测试行为树桥接到hotfix层的
        /// </summary>
        public void _TestNodeCanvas()
        {
            GameObject npc = GameObject.Find("NPC");
            BehaviourTreeOwner btOwner = npc.GetComponent<BehaviourTreeOwner>();
            // Log.Debug($"btOwner ==> {btOwner.name}");
            // btOwner.StartBehaviour();

            BPEntity entity = Game.Scene.Ensure<BPEntity>();
            NodeCanvasHelper.Init(entity, npc);
            
            btOwner.RestartBehaviour();
        }


        /// <summary>
        /// 测试主界面
        /// </summary>
        public void _TestMainUI()
        {
            ETHotfix.BPTitleComponent comp = Game.Scene.GetComponent<ETHotfix.BPTitleComponent>();
            GButton btn = comp.view.BPGetChild("ronTestBtn").asButton;

            Log.Debug($"_TestMainUI~~~ {btn.name}");
        }


        /// <summary>
        /// 测试加载行为树
        /// </summary>
        public void _TestLoadNodeCanvas()
        {
            // 经过测试.要么给全路径,要么给名字(不带后缀)
            string bundleName = "bttree.unity3d";
            // string resPath = "assets/res/nodecanvasassets/npcai.asset";
            string resPath = "npcai";
            ResourcesComponent resComp = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resComp.LoadBundle(bundleName);

            UnityEngine.Object tree = resComp.GetAsset(bundleName, resPath);
            
            Log.Debug($"btTree333 ==> {tree.GetHashCode()}");
            NodeCanvas.BehaviourTrees.BehaviourTree btTree = tree as NodeCanvas.BehaviourTrees.BehaviourTree;
            if(btTree != null)
                Log.Debug($"btTree666 ==> {btTree.GetHashCode()}, ===> {btTree.name}");
        }


        /// <summary>
        /// 测试策划的行为树
        /// </summary>
        public  void _TestDesignerNodeCanvas()
        {
            // 1, 由于策划的行为树是我不知道他gameObject叫什么的.那我目前就扫描场景中的所有GameObject吧
            // 并且只会找到是活跃的GameObject
            // TODO Ron. Conditional暂时理解成,只有条件为True,才会进入子节点.并且返回子节点的状态
            string btTreeTag = "btTree";
            GameObject[] allBtTreeGameObj = UnityEngine.GameObject.FindGameObjectsWithTag(btTreeTag);
            for(int index = 0; index < allBtTreeGameObj.Length; ++ index)
            {
                GameObject go = allBtTreeGameObj[index];
                if(go.GetComponent<BehaviourTreeOwner>() == null)
                    continue;

                // TODO Ron 如果从工作流角度出发.这里的Entity怎么来? 我暂时存在dict里, 正常来说,按照ET ECS,是应该是插入到某个实体里的
                Log.Debug($"拥有行为树的GameObject ==> {go.name}");
                BPEntity entity =  new BPEntity();
                this.entityDict.Add(entity.GetHashCode(), entity);
                NodeCanvasHelper.Init(entity, go);

                BehaviourTreeOwner btOwner = go.GetComponent<BehaviourTreeOwner>();
                btOwner.RestartBehaviour();
            }
        }


        /// <summary>
        /// 测试全局黑板变量
        /// </summary>
        public void _TestGlobalParameters()
        {
            GameObject npc = GameObject.Find("NPC");
            GameObject demo = GameObject.Find("Demo");
            GlobalBlackboard bbNpc = npc.GetComponent<GlobalBlackboard>();
            GlobalBlackboard demoNpc = demo.GetComponent<GlobalBlackboard>();

            string argName = "isOk";
            Variable isNpcOK = bbNpc.GetVariable(argName);
            isNpcOK.SetValueBoxed(true);

            Variable isDemoOk = demoNpc.GetVariable(argName);
            Debug.Log($"isDemoOk ==> {isDemoOk.GetValueBoxed()}");
        }

        /// <summary>
        /// 测试跑100W次
        /// </summary>
        public void _TestCalc()
        {
            ResourcesComponent resComp = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            long beginTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;
            for(int i = 0; i < 1000000; ++i)            
            {
                resComp.Mult(2, 2);
            }

            long now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;
            Debug.Log($"hotfix cost time 444 ==> {now - beginTime}");
        }
    }
}
