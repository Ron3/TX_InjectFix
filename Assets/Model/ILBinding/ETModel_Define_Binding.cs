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
    unsafe class ETModel_Define_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Define);

            field = type.GetField("IsEditorMode", flag);
            app.RegisterCLRFieldGetter(field, get_IsEditorMode_0);
            app.RegisterCLRFieldSetter(field, set_IsEditorMode_0);
            field = type.GetField("IsILRuntime", flag);
            app.RegisterCLRFieldGetter(field, get_IsILRuntime_1);
            app.RegisterCLRFieldSetter(field, set_IsILRuntime_1);


        }



        static object get_IsEditorMode_0(ref object o)
        {
            return ETModel.Define.IsEditorMode;
        }
        static void set_IsEditorMode_0(ref object o, object v)
        {
            ETModel.Define.IsEditorMode = (System.Boolean)v;
        }
        static object get_IsILRuntime_1(ref object o)
        {
            return ETModel.Define.IsILRuntime;
        }
        static void set_IsILRuntime_1(ref object o, object v)
        {
            ETModel.Define.IsILRuntime = (System.Boolean)v;
        }


    }
}
