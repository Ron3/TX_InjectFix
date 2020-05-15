using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace NodeCanvas.Tasks.Actions
{
	[Category("黑珍珠")]
    [Description("BPActionShell")]
	public class BPActionShell : ActionTask
	{
		[RequiredField]
		[ReadOnly]
        public int actionId;

		// 自定义数据
        public string metaData;
        
#if UNITY_EDITOR
		// 描述
		private string desc;
#endif

		// 桥接函数
		[HideInInspector]
		public System.Func<string> onInit;
		// public System.Func<BPActionShell, string> onInit;
		
		[HideInInspector]
		public System.Action onExecute;
        // public System.Action<BPActionShell> onExecute;
		
		[HideInInspector]
		public System.Action onUpdate;
        // public System.Action<BPActionShell> onUpdate;
		
		[HideInInspector]
		public System.Action onPause;
        // public System.Action<BPActionShell> onPause;
		
		[HideInInspector]
		public System.Action onStop;
        // public System.Action<BPActionShell> onStop;

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

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			if(this.onExecute != null)
			{
				this.onExecute();
			}
			else
			{
				EndAction(true);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			if(this.onUpdate != null)
			{
				this.onUpdate();
			}
		}

		//Called when the task is disabled.
		protected override void OnStop()
		{
			if(this.onStop != null)	
			{
				this.onStop();
			}
		}

		//Called when the task is paused.
		protected override void OnPause()
		{
			if(this.onPause != null)
			{
				this.onPause();
			}	
		}


		// /// <summary>
		// /// BehaviorTree会调用OnReset.Action 是调用OnStop, Node才是调用OnReset 
		// /// </summary>
		// private void Reset() 
		// {
		// 	Debug.Log("重置TaskAction~~~~");
		// }

		
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
