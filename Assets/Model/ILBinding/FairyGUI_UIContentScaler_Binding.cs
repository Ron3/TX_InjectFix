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
    unsafe class FairyGUI_UIContentScaler_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(FairyGUI.UIContentScaler);

            field = type.GetField("scaleFactor", flag);
            app.RegisterCLRFieldGetter(field, get_scaleFactor_0);
            app.RegisterCLRFieldSetter(field, set_scaleFactor_0);


        }



        static object get_scaleFactor_0(ref object o)
        {
            return FairyGUI.UIContentScaler.scaleFactor;
        }
        static void set_scaleFactor_0(ref object o, object v)
        {
            FairyGUI.UIContentScaler.scaleFactor = (System.Single)v;
        }


    }
}
