using System;
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
    public class HotfixTaskFactory
    {
        /// <summary>
        /// 绑定action
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="actionTask"></param>
        /// <returns></returns>
        public static Component Create(Entity parent, BPActionShell actionTask)
        {
            if(parent == null || actionTask == null || actionTask.actionId <= 0)
                return null;

            try
            {
                ConfigComponent configComp = Game.Scene.GetComponent<ConfigComponent>();
                BPNodeCanvasTreeConfig config = (BPNodeCanvasTreeConfig)configComp.Get(typeof(BPNodeCanvasTreeConfig), actionTask.actionId);
                
                Type type = Type.GetType($"ETHotfix.{config.ComponentName}");
                Component component = Game.ObjectPool.Fetch(type);
                
                Game.EventSystem.Awake(component, parent, actionTask, config);

                return component;
            }
            catch(Exception e)
            {
                Log.Error($"[行为树配置表,并不存在这个Id], actionId ===> {actionTask.actionId}");
                Log.Error(e);
            }

            return null;
        }


        /// <summary>
        /// 绑定任务条件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="conditionTask"></param>
        /// <returns></returns>
        public static Component Create(Entity parent, BPConditionShell conditionTask)
        {
            if(parent == null || conditionTask == null || conditionTask.actionId <= 0)
                return null;

            try
            {
                // Log.Debug($"conditionTask ==> {conditionTask.actionId}");
                ConfigComponent configComp = Game.Scene.GetComponent<ConfigComponent>();
                BPNodeCanvasTreeConfig config = (BPNodeCanvasTreeConfig)configComp.Get(typeof(BPNodeCanvasTreeConfig), conditionTask.actionId);
                
                Type type = Type.GetType($"ETHotfix.{config.ComponentName}");
                Component component = Game.ObjectPool.Fetch(type);
                
                Game.EventSystem.Awake(component, parent, conditionTask, config);

                return component;    
            }
            catch(Exception e)
            {
                Log.Error($"[行为树配置表,并不存在这个Id], actionId ===> {conditionTask.actionId}");
                Log.Error(e);
            }

            return null;
        }


        /// <summary>
        /// 绑定复核
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="compositeTask"></param>
        /// <returns></returns>
        public static Component Create(Entity parent, BPCompositeShell compositeTask)
        {
            if(parent == null || compositeTask == null || compositeTask.actionId <= 0)
                return null;

            try
            {
                ConfigComponent configComp = Game.Scene.GetComponent<ConfigComponent>();
                BPNodeCanvasTreeConfig config = (BPNodeCanvasTreeConfig)configComp.Get(typeof(BPNodeCanvasTreeConfig), compositeTask.actionId);
                
                Type type = Type.GetType($"ETHotfix.{config.ComponentName}");
                Component component = Game.ObjectPool.Fetch(type);
                
                Game.EventSystem.Awake(component, parent, compositeTask, config);

                return component;    
            }
            catch(Exception e)
            {
                Log.Error($"[行为树配置表,并不存在这个Id], actionId ===> {compositeTask.actionId}");
                Log.Error(e);
            }

            return null;
        }


        /// <summary>
        /// 绑定装饰器
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="decoratorTask"></param>
        /// <returns></returns>
        public static Component Create(Entity parent, BPDecoratorShell decoratorTask)
        {
            if(parent == null || decoratorTask == null || decoratorTask.actionId <= 0)
                return null;

            try
            {
                ConfigComponent configComp = Game.Scene.GetComponent<ConfigComponent>();
                BPNodeCanvasTreeConfig config = (BPNodeCanvasTreeConfig)configComp.Get(typeof(BPNodeCanvasTreeConfig), decoratorTask.actionId);
                
                Type type = Type.GetType($"ETHotfix.{config.ComponentName}");
                Component component = Game.ObjectPool.Fetch(type);
                
                Game.EventSystem.Awake(component, parent, decoratorTask, config);

                return component;    
            }
            catch(Exception e)
            {
                Log.Error($"[行为树配置表,并不存在这个Id], actionId ===> {decoratorTask.actionId}");
                Log.Error(e);
            }

            return null;
        }
    }
}
