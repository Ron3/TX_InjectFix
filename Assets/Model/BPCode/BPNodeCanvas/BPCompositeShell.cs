using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
    [Name("BPCompositeShell", 0)]
    [Category("黑珍珠")]
    [Description("")]
    // [Icon("Sequencer")]
    [Icon("")]
    [Color("483D8B")]
    public class BPCompositeShell : BTComposite
    {
        [RequiredField]
        public int actionId;
        // public string actionName;

		// 自定义数据
        public string metaData;

#if UNITY_EDITOR
		// 描述
		[HideInInspector]
		public string desc;
#endif

        [HideInInspector]
        public System.Func<UnityEngine.Component, IBlackboard, Status> onExecute;

        [HideInInspector]
        public System.Action onReset;

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


        /// <summary>
        /// 
        /// </summary>
        protected override void OnReset()
        {
            if(this.onReset != null)
            {
                this.onReset();
            }
        }


#if UNITY_EDITOR
		/// <summary>
		/// 2020-04-10 by Ron
		/// 直接魔改friendlyName
		/// </summary>
		/// <value></value>
		public override string name
		{
			get{
				return System.Convert.ToString(this.enumActionId);
			}
		}

		/// <summary>
		/// 魔改
		/// </summary>
		/// <value></value>
		public override string description
		{
			get{
				// Debug.LogError($"===> {this.desc}");
				return this.desc;
			}
		}

		protected override void OnNodeInspectorGUI() 
		{
			try
			{
				base.OnNodeInspectorGUI();
                
				BP_NODE_CANVAS_ACTION_ID chooseActionId = (BP_NODE_CANVAS_ACTION_ID)UnityEditor.EditorGUILayout.EnumPopup(this.enumActionId);
				int newActionId = System.Convert.ToInt32(chooseActionId);
				if(this.actionId != newActionId)
				{
					this.actionId = newActionId;
					ETModel.BPNodeCanvasTreeConfig configInfo = null;
					ETModel.NodeCanvasConfig.configDict.TryGetValue(this.actionId, out configInfo);
					this.metaData = configInfo.ExampleMetaData;
					this.desc = configInfo.Description;
				}
			}
			catch(System.Exception e)
			{

			}
			
		}

		/// <summary>
		/// 获取枚举的id
		/// </summary>
		/// <value></value>
		public BP_NODE_CANVAS_ACTION_ID enumActionId
		{
			get
			{
				return (BP_NODE_CANVAS_ACTION_ID)System.Enum.ToObject(typeof(BP_NODE_CANVAS_ACTION_ID), this.actionId);
			}
		}
#endif

    }
}
