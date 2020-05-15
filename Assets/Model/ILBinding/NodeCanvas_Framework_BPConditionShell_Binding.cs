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
    unsafe class NodeCanvas_Framework_BPConditionShell_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(NodeCanvas.Framework.BPConditionShell);

            field = type.GetField("onCheck", flag);
            app.RegisterCLRFieldGetter(field, get_onCheck_0);
            app.RegisterCLRFieldSetter(field, set_onCheck_0);
            field = type.GetField("actionId", flag);
            app.RegisterCLRFieldGetter(field, get_actionId_1);
            app.RegisterCLRFieldSetter(field, set_actionId_1);


        }



        static object get_onCheck_0(ref object o)
        {
            return ((NodeCanvas.Framework.BPConditionShell)o).onCheck;
        }
        static void set_onCheck_0(ref object o, object v)
        {
            ((NodeCanvas.Framework.BPConditionShell)o).onCheck = (System.Func<System.Boolean>)v;
        }
        static object get_actionId_1(ref object o)
        {
            return ((NodeCanvas.Framework.BPConditionShell)o).actionId;
        }
        static void set_actionId_1(ref object o, object v)
        {
            ((NodeCanvas.Framework.BPConditionShell)o).actionId = (System.Int32)v;
        }


    }
}
