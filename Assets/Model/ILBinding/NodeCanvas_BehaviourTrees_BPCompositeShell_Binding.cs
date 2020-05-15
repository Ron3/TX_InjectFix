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
    unsafe class NodeCanvas_BehaviourTrees_BPCompositeShell_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(NodeCanvas.BehaviourTrees.BPCompositeShell);

            field = type.GetField("onExecute", flag);
            app.RegisterCLRFieldGetter(field, get_onExecute_0);
            app.RegisterCLRFieldSetter(field, set_onExecute_0);
            field = type.GetField("actionId", flag);
            app.RegisterCLRFieldGetter(field, get_actionId_1);
            app.RegisterCLRFieldSetter(field, set_actionId_1);


        }



        static object get_onExecute_0(ref object o)
        {
            return ((NodeCanvas.BehaviourTrees.BPCompositeShell)o).onExecute;
        }
        static void set_onExecute_0(ref object o, object v)
        {
            ((NodeCanvas.BehaviourTrees.BPCompositeShell)o).onExecute = (System.Func<UnityEngine.Component, NodeCanvas.Framework.IBlackboard, NodeCanvas.Framework.Status>)v;
        }
        static object get_actionId_1(ref object o)
        {
            return ((NodeCanvas.BehaviourTrees.BPCompositeShell)o).actionId;
        }
        static void set_actionId_1(ref object o, object v)
        {
            ((NodeCanvas.BehaviourTrees.BPCompositeShell)o).actionId = (System.Int32)v;
        }


    }
}
