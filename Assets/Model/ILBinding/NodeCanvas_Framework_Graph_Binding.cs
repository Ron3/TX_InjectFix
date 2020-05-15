using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class NodeCanvas_Framework_Graph_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(NodeCanvas.Framework.Graph);
            args = new Type[]{};
            method = type.GetMethod("GetAllInstancedNestedGraphs", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAllInstancedNestedGraphs_0);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(NodeCanvas.Tasks.Actions.BPActionShell)};
            if (genericMethods.TryGetValue("GetAllTasksOfType", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<NodeCanvas.Tasks.Actions.BPActionShell>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, GetAllTasksOfType_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(NodeCanvas.Framework.BPConditionShell)};
            if (genericMethods.TryGetValue("GetAllTasksOfType", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<NodeCanvas.Framework.BPConditionShell>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, GetAllTasksOfType_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(NodeCanvas.BehaviourTrees.BPCompositeShell)};
            if (genericMethods.TryGetValue("GetAllNodesOfType", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<NodeCanvas.BehaviourTrees.BPCompositeShell>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, GetAllNodesOfType_3);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(NodeCanvas.BehaviourTrees.BPDecoratorShell)};
            if (genericMethods.TryGetValue("GetAllNodesOfType", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<NodeCanvas.BehaviourTrees.BPDecoratorShell>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, GetAllNodesOfType_4);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("get_blackboard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_blackboard_5);


        }


        static StackObject* GetAllInstancedNestedGraphs_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllInstancedNestedGraphs();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAllTasksOfType_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllTasksOfType<NodeCanvas.Tasks.Actions.BPActionShell>();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAllTasksOfType_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllTasksOfType<NodeCanvas.Framework.BPConditionShell>();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAllNodesOfType_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllNodesOfType<NodeCanvas.BehaviourTrees.BPCompositeShell>();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAllNodesOfType_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAllNodesOfType<NodeCanvas.BehaviourTrees.BPDecoratorShell>();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_blackboard_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.Graph instance_of_this_method = (NodeCanvas.Framework.Graph)typeof(NodeCanvas.Framework.Graph).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.blackboard;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
