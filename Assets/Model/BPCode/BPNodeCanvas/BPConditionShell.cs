using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Framework
{

	[Category("黑珍珠")]
	[Description("BPCondtionShell")]
	public class BPConditionShell : ConditionTask
	{
		[RequiredField]
        public int actionId;
        // public string actionName;

		// 自定义数据
        public string metaData;

#if UNITY_EDITOR
		// 描述
		private string desc;
#endif		

		// 桥接函数
		[HideInInspector]
		public System.Func<string> onInit;
		// public System.Func<BPConditionShell, string> onInit;

		[HideInInspector]
		public System.Action onEnable;
        // public System.Action<BPConditionShell> onEnable;

		[HideInInspector]
		public System.Action onDisable;
        // public System.Action<BPConditionShell> onDisable;

		[HideInInspector]
		public System.Func<bool> onCheck;
        // public System.Func<BPConditionShell, bool> onCheck;


		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			if(this.onInit != null)
			{
				return this.onInit();
			}
			else
			{
				return null;
			}
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable()
		{
			if(this.onEnable != null)
			{
				this.onEnable();
			}
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable()
		{
			if(this.onDisable != null)
			{
				this.onDisable();
			}
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			if(this.onCheck != null)
			{
				return this.onCheck();
			}
			else
			{
				return true;
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
				return this.desc;
			}
		}

		protected override void OnTaskInspectorGUI() 
		{
			try
			{
				base.OnTaskInspectorGUI();
			
				UnityEditor.EditorGUILayout.BeginHorizontal();

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
				// Debug.Log($"this.actionId ==> {this.actionId}");

				UnityEditor.EditorGUILayout.EndHorizontal();
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