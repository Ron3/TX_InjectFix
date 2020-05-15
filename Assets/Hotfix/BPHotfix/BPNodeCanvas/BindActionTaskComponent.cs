using UnityEngine;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using NodeCanvas.Tasks.Actions;

// TODO Ron 2020-04-08 如果把父实体(在这里也就是NPC所在的Entity传入进来)
// 而它的子是有可能持有它的.那么就出现这种互相持有的情况. C#里这种能释放???


namespace ETHotfix
{

    [ObjectSystem]
    public class BindActionTaskComponentAwakeSystem : AwakeSystem<BindActionTaskComponent>
    {
        public override void Awake(BindActionTaskComponent self)
        {
            self.Awake();
        }
    }

    
    public class BindActionTaskComponent : ETHotfix.Component
    {
        private Dictionary<BPActionShell, Component> actionDict = new Dictionary<BPActionShell, Component>();
        private Dictionary<BPConditionShell, Component> conditionDict = new Dictionary<BPConditionShell, Component>();
        private Dictionary<BPCompositeShell, Component> compositeDict = new Dictionary<BPCompositeShell, Component>();
        private Dictionary<BPDecoratorShell, Component> decoratorDict = new Dictionary<BPDecoratorShell, Component>();

        /// <summary>
        /// 
        /// </summary>
        public void Awake() 
        {
            // Log.Debug($"this.Parent.InstanceId =====> {this.Parent.InstanceId}");
            // 目前这种方法不一定是完美的.因为按照结构,就是parent里.找回那4条表.然后在做后面的逻辑
            Entity entity = this.EntityParent;
            if(entity == null)
                return;

            FindActionTaskComponent findActionTaskComp = entity.GetComponent<FindActionTaskComponent>();
            if(findActionTaskComp == null)
                return;

            this.BindHotfixActionTask(findActionTaskComp.actionList);
            this.BindHotifxConditionTask(findActionTaskComp.conditionList);
            this.BindHotfixCompositeTask(findActionTaskComp.compositeList);
            this.BindHotfixDecoratorTask(findActionTaskComp.decoratorList);
        }


        /// <summary>
        /// 绑定hotfix ActionTask
        /// </summary>
        /// <param name="actionTaskList"></param>
        public void BindHotfixActionTask(List<BPActionShell> actionTaskList)
        {
            if(actionTaskList == null)
                return;
            
            for(int index = 0; index < actionTaskList.Count; ++index)
            {
                BPActionShell actionTask = actionTaskList[index];
                Component logicComponent = HotfixTaskFactory.Create(this.EntityParent, actionTask);
                if(logicComponent != null)
                    this.actionDict.Add(actionTask, logicComponent);
            }

            Log.Debug($"桥接action个数是 ===>{this.actionDict.Count}");
        }


        /// <summary>
        /// 绑定条件判定
        /// </summary>
        /// <param name="conditionList"></param>
        public void BindHotifxConditionTask(List<BPConditionShell> conditionList)
        {
            if(conditionList == null)
                return;

            for(int index = 0; index < conditionList.Count; ++index)
            {
                BPConditionShell conditionShell = conditionList[index];
                Component conditionComp = HotfixTaskFactory.Create(this.EntityParent, conditionShell);
                if(conditionComp != null)
                    this.conditionDict.Add(conditionShell, conditionComp);
            }
        }


        /// <summary>
        /// 绑定复合判定
        /// </summary>
        /// <param name="compositeList"></param>
        public void BindHotfixCompositeTask(List<BPCompositeShell> compositeList)
        {
            if(compositeList == null)
                return;

            // Log.Debug($"compositeList.Count ==> {compositeList.Count}");
            for(int index = 0; index < compositeList.Count; ++index)
            {
                BPCompositeShell compositeShell = compositeList[index];
                Component compositeComp = HotfixTaskFactory.Create(this.EntityParent, compositeShell);
                if(compositeComp != null)
                    this.compositeDict.Add(compositeShell, compositeComp);
            }
        }


        /// <summary>
        /// 绑定装饰器
        /// </summary>
        /// <param name="decoratorList"></param>
        public void BindHotfixDecoratorTask(List<BPDecoratorShell> decoratorList)
        {
            if(decoratorList == null)
                return;

            // Log.Debug($"decoratorList.Count ==> {decoratorList.Count}");

            for(int index = 0; index < decoratorList.Count; ++index)
            {
                BPDecoratorShell decoratorShell = decoratorList[index];
                Component comp = HotfixTaskFactory.Create(this.EntityParent, decoratorShell);
                if(comp != null)
                    this.decoratorDict.Add(decoratorShell, comp);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public Entity EntityParent
        {
            // TODO Ron. 暂时先这样写.这个parent还不是十分确定的.具体的等fay在使用的时候.在回来改
            get{
                Entity entity = this.Parent as Entity;
                return entity;
            }
        }


        public override void Dispose()
        {
            Log.Debug($"BindActionD Dispose ===> {this.GetHashCode()}");

            this.actionDict.Clear();
            this.conditionDict.Clear();
            this.compositeDict.Clear();
            this.decoratorDict.Clear();

            base.Dispose();
        }
    }
}
