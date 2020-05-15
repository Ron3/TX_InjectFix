using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.BehaviourTrees
{
    [Name("BPDecoratorShell", 0)]
    [Category("黑珍珠")]
    [Description("")]
    [Icon("")]
    [Color("1E90FF")]
    public class BPDecoratorShell : BTDecorator
    {
        [RequiredField]
        public int actionId;
        // public string actionName;

		// 自定义数据
        public string metaData;

        public System.Func<UnityEngine.Component, IBlackboard, Status> onExecute;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="blackboard"></param>
        /// <returns></returns>
        protected override Status OnExecute(UnityEngine.Component agent, IBlackboard blackboard)
        {
            if(this.onExecute != null)
            {
                return this.onExecute(agent, blackboard);
            }
            else
            {
                return Status.Failure;
            }
        }
    }
}
