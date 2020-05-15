using UnityEngine;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;


namespace ETHotfix
{
    public class NodeCanvasHelper
    {
        /// <summary>
        /// TODO Ron 2020-04- 这个是半成品.等fay那边确定后.才修改这一段代码
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="go"></param> 挂着行为的树的obj
        public static bool Init(BPEntity entity, GameObject go)
        {
            // TODO Ron.稍后再改.应该是不用传递gameObject过来的.但是目前又不知道如何根据Entity查找到GameObject
            if(entity == null || go == null)
                return false;

            entity.go = go;

            // 1, 扫描
            FindActionTaskComponent findComp = entity.Ensure<FindActionTaskComponent, GameObject>(go);
            // ctr.Init(go);

            // 2, 创建对应的热更新执行的类实例
            BindActionTaskComponent bindComp = entity.Ensure<BindActionTaskComponent>();
            // Log.Debug($"entity.InstanceId ===> {entity.InstanceId}, findComp===>{findComp.GetHashCode()}, bindComp ==>{bindComp.GetHashCode()} ");

            return true;
        }

        public static bool ReInit(BPEntity entity, GameObject go)
        {
            // TODO Ron如果是存在.那么就删掉
            if(entity == null || go == null)
                return false;
            
            entity.EnsureRemove<FindActionTaskComponent>();
            entity.EnsureRemove<BindActionTaskComponent>();
            
            // 然后再次初始化
            return Init(entity, go);
        }
    }
}
