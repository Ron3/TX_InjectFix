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
    unsafe class NodeCanvas_Tasks_Actions_BPActionShell_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(NodeCanvas.Tasks.Actions.BPActionShell);

            field = type.GetField("onExecute", flag);
            app.RegisterCLRFieldGetter(field, get_onExecute_0);
            app.RegisterCLRFieldSetter(field, set_onExecute_0);
            field = type.GetField("metaData", flag);
            app.RegisterCLRFieldGetter(field, get_metaData_1);
            app.RegisterCLRFieldSetter(field, set_metaData_1);
            field = type.GetField("onInit", flag);
            app.RegisterCLRFieldGetter(field, get_onInit_2);
            app.RegisterCLRFieldSetter(field, set_onInit_2);
            field = type.GetField("onStop", flag);
            app.RegisterCLRFieldGetter(field, get_onStop_3);
            app.RegisterCLRFieldSetter(field, set_onStop_3);
            field = type.GetField("actionId", flag);
            app.RegisterCLRFieldGetter(field, get_actionId_4);
            app.RegisterCLRFieldSetter(field, set_actionId_4);


        }



        static object get_onExecute_0(ref object o)
        {
            return ((NodeCanvas.Tasks.Actions.BPActionShell)o).onExecute;
        }
        static void set_onExecute_0(ref object o, object v)
        {
            ((NodeCanvas.Tasks.Actions.BPActionShell)o).onExecute = (System.Action)v;
        }
        static object get_metaData_1(ref object o)
        {
            return ((NodeCanvas.Tasks.Actions.BPActionShell)o).metaData;
        }
        static void set_metaData_1(ref object o, object v)
        {
            ((NodeCanvas.Tasks.Actions.BPActionShell)o).metaData = (System.String)v;
        }
        static object get_onInit_2(ref object o)
        {
            return ((NodeCanvas.Tasks.Actions.BPActionShell)o).onInit;
        }
        static void set_onInit_2(ref object o, object v)
        {
            ((NodeCanvas.Tasks.Actions.BPActionShell)o).onInit = (System.Func<System.String>)v;
        }
        static object get_onStop_3(ref object o)
        {
            return ((NodeCanvas.Tasks.Actions.BPActionShell)o).onStop;
        }
        static void set_onStop_3(ref object o, object v)
        {
            ((NodeCanvas.Tasks.Actions.BPActionShell)o).onStop = (System.Action)v;
        }
        static object get_actionId_4(ref object o)
        {
            return ((NodeCanvas.Tasks.Actions.BPActionShell)o).actionId;
        }
        static void set_actionId_4(ref object o, object v)
        {
            ((NodeCanvas.Tasks.Actions.BPActionShell)o).actionId = (System.Int32)v;
        }


    }
}
