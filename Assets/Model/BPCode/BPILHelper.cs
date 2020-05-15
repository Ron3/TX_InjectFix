using System;
using UnityEngine;

namespace ETModel
{
    public static class BPILHelper
    {
        /// <summary>
        /// 所有初始化的接口
        /// </summary>
        /// 
        public static void BPInitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
			// 值类型绑定
			// appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        	// appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
        	// appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());


			appdomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();            
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Collider2D>();
			appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Collider2D>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Collision2D>();
			// appdomain.DelegateManager.RegisterMethodDelegate<Spine.TrackEntry>();
			appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
			appdomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
			appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Int16>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.String>();
			
			// myDictionary = myDictionary.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, o => o.Value);
			// Dictionary<int, string> myDictionary = new Dictionary<int, string>();
			// myDictionary.OrderByDescending(o => o.Key);
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.Int32>();
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.String>();
			
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Int32>();
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
			
			// List<KeyValuePair<int, string>> lst = new List<KeyValuePair<int, string>>(myDictionary);
			// lst.Sort((s1, s2) => s2.Value.CompareTo(s1.Value));
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.String>>>((act) =>
			{
				return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.String>>((x, y) =>
				{
					return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.Collections.Generic.KeyValuePair<System.Int32, System.String>, System.Int32>)act)(x, y);
				});
			});
			
			// dic 转 list 排序后，再存入 dic 中
			// Dictionary<int, List<int>> itemDic = new Dictionary<int, List<int>>();
			// List<KeyValuePair<int, List<int>>> newList = new List<KeyValuePair<int, List<int>>>(itemDic);
			// newList.Sort((s1, s2) => s2.Key.CompareTo(s1.Key));
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>, System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>>>((act) =>
			{
				return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>>((x, y) =>
				{
					return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>, System.Int32>)act)(x, y);
				});
			});
			
			// dic 转 list 排序后，再存入 dic 中
			// Dictionary<int, List<BPItemGroupValue>> itemDic = new Dictionary<int, List<BPItemGroupValue>>();
			// List<KeyValuePair<int, List<BPItemGroupValue>>> newList = new List<KeyValuePair<int, List<BPItemGroupValue>>>(itemDic);
			// newList.Sort((s1, s2) => s2.Key.CompareTo(s1.Key));
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>>((act) =>
			{
				return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>((x, y) =>
				{
					return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Int32>)act)(x, y);
				});
			});
			
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>, System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>>>((act) =>
			{
				return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>>((x, y) =>
				{
					return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.String>>, System.Int32>)act)(x, y);
				});
			});
			
			// Dictionary<int, Dictionary<int, List<BPItemGroupValue>>> 用到
			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>, System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>>>((act) =>
			{
				return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>>((x, y) =>
				{
					return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>, System.Int32>)act)(x, y);
				});
			});
			
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.String>, List<object>>();
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<System.Int32>>, System.Collections.Generic.List<System.Int32>>();
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>();
			// appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
			
// this.itemDicDic = new Dictionary<int, Dictionary<int, List<BPItemGroupValue>>>();
// Dictionary<int, List<BPItemGroupValue>> itemDic = null;
        
			// appdomain.DelegateManager.RegisterDelegateConvertor<Spine.AnimationState.TrackEntryDelegate>((act) =>
			// {
			// 	return new Spine.AnimationState.TrackEntryDelegate((trackEntry) =>
			// 	{
			// 		((Action<Spine.TrackEntry>)act)(trackEntry);
			// 	});
			// });

			appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
			{
				return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
				{
					return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<ETModel.BPCallback>((act) =>
			{
				return new ETModel.BPCallback(() =>
				{
					((Action)act)();
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<ETModel.BPCallback<System.Int32>>((act) =>
			{
				return new ETModel.BPCallback<System.Int32>((arg1) =>
				{
					((Action<System.Int32>)act)(arg1);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<ETModel.BPCallback<System.String>>((act) =>
			{
				return new ETModel.BPCallback<System.String>((arg1) =>
				{
					((Action<System.String>)act)(arg1);
				});
			});

			
			#region ========FairyGUI========

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback0>((act) =>
			{
				return new FairyGUI.EventCallback0(() =>
				{
					((Action)act)();
				});
			});

			appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.EventContext>();

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback1>((act) =>
			{
				return new FairyGUI.EventCallback1((context) =>
				{
					((Action<FairyGUI.EventContext>)act)(context);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.PlayCompleteCallback>((act) =>
			{
				return new FairyGUI.PlayCompleteCallback(() =>
				{
					((Action)act)();
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.ListItemRenderer>((act) =>
			{
				return new FairyGUI.ListItemRenderer((index, obj) =>
				{
					((Action<int, FairyGUI.GObject>)act)(index, obj);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.GTweenCallback>((act) =>
			{
				return new FairyGUI.GTweenCallback(() =>
				{
					((Action)act)();
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.GTweenCallback1>((act) =>
			{
				return new FairyGUI.GTweenCallback1((tweener) =>
				{
					((Action<FairyGUI.GTweener>)act)(tweener);
				});
			});

			appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.GTweener>();

			appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, FairyGUI.GObject>();
			
			#endregion


			#region ==========NodeCanvas==========
			appdomain.DelegateManager.RegisterMethodDelegate<NodeCanvas.Tasks.Actions.BPActionShell>();
			appdomain.DelegateManager.RegisterMethodDelegate<NodeCanvas.Tasks.Actions.BPActionShell, System.String>();

			appdomain.DelegateManager.RegisterFunctionDelegate<NodeCanvas.Framework.BPConditionShell, System.Boolean>();
			
			appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Component, NodeCanvas.Framework.IBlackboard, NodeCanvas.Framework.Status>();
			
			#endregion
        }
    }
}
