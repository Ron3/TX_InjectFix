using UnityEngine;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using NodeCanvas.Tasks.Actions;


namespace ETHotfix
{
    [ObjectSystem]
    public class FindActionTaskAwakeSystem : AwakeSystem<FindActionTaskComponent, GameObject>
    {
        public override void Awake(FindActionTaskComponent self, GameObject go) 
        {
            self.Awake(go);
        }
    }


    /// <summary>
    /// 找到这个gameObject上的行为树.
    /// 1, 扫描它的hotfixShell
    /// 2, 桥接到真正的逻辑里
    /// </summary>
    public class FindActionTaskComponent : ETHotfix.Component
    {
        public readonly List<BPActionShell> actionList = new List<BPActionShell>();
        public readonly List<BPConditionShell> conditionList = new List<BPConditionShell>();
        public readonly List<BPCompositeShell> compositeList = new List<BPCompositeShell>();
        public readonly List<BPDecoratorShell> decoratorList = new List<BPDecoratorShell>();

        private bool isInit = false;
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        public void Awake(GameObject go)
        {
            this.Init(go);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Init(GameObject go)
        {
            if(go == null)
                return false;

            if(this.isInit == true)
                return true;

            // TODO Ron. 这里这样写gameobject,应该是不对的.
            BehaviourTreeOwner btOwner = go.GetComponent<BehaviourTreeOwner>();
            if(btOwner == null)
                return false;
            
            this.FindTask(btOwner.behaviour);
            
            // 2, 寻找子树
            // Log.Debug($"subGraph start~~~");
            IEnumerable<Graph> subGraphList = btOwner.behaviour.GetAllInstancedNestedGraphs();
            foreach(Graph subGraph in subGraphList)
            {
                this.FindTask(subGraph);
                // Log.Debug($"subGraph ==> {subGraph.name}");
            }

            this.isInit = true;
            return true;
        }


        private void FindTask(Graph graph)
        {
            // 1, actionTask
            IEnumerable<BPActionShell> tmpActionShellList  = graph.GetAllTasksOfType<BPActionShell>();
            foreach(BPActionShell shell in tmpActionShellList)
            {
                actionList.Add(shell);
                // Log.Debug($"shell.name ==> {shell.name}");
            }
            // Log.Debug($"找到action个数===> {actionList.Count}");

            // 2, 条件判定
            IEnumerable<BPConditionShell> tmpConditionList  = graph.GetAllTasksOfType<BPConditionShell>();
            foreach(BPConditionShell shell in tmpConditionList)
            {
                conditionList.Add(shell);
                // Log.Debug($"shell.name ==> {shell.name}");
            }

            // 3, 组合
            IEnumerable<BPCompositeShell> tmpCompositeList  = graph.GetAllNodesOfType<BPCompositeShell>();
            foreach(BPCompositeShell shell in tmpCompositeList)
            {
                compositeList.Add(shell);
                // Log.Debug($"shell.name 33 ==> {shell.name}");
            }

            // 4, 装饰器
            IEnumerable<BPDecoratorShell> tmpDecoratorList  = graph.GetAllNodesOfType<BPDecoratorShell>();
            foreach(BPDecoratorShell shell in tmpDecoratorList)
            {
                decoratorList.Add(shell);
                // Log.Debug($"shell.name 44 ==> {shell.name}");
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public override void Dispose()
        {
            Log.Debug($"FindActionDispose ===> {this.GetHashCode()}");
            this.actionList.Clear();
            this.conditionList.Clear();
            this.compositeList.Clear();
            this.decoratorList.Clear();
            this.isInit = false;

            base.Dispose();
        }
    }
}




