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
    unsafe class FairyGUI_UIConfig_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(FairyGUI.UIConfig);

            field = type.GetField("defaultFont", flag);
            app.RegisterCLRFieldGetter(field, get_defaultFont_0);
            app.RegisterCLRFieldSetter(field, set_defaultFont_0);
            field = type.GetField("verticalScrollBar", flag);
            app.RegisterCLRFieldGetter(field, get_verticalScrollBar_1);
            app.RegisterCLRFieldSetter(field, set_verticalScrollBar_1);
            field = type.GetField("horizontalScrollBar", flag);
            app.RegisterCLRFieldGetter(field, get_horizontalScrollBar_2);
            app.RegisterCLRFieldSetter(field, set_horizontalScrollBar_2);
            field = type.GetField("buttonSound", flag);
            app.RegisterCLRFieldGetter(field, get_buttonSound_3);
            app.RegisterCLRFieldSetter(field, set_buttonSound_3);
            field = type.GetField("buttonSoundVolumeScale", flag);
            app.RegisterCLRFieldGetter(field, get_buttonSoundVolumeScale_4);
            app.RegisterCLRFieldSetter(field, set_buttonSoundVolumeScale_4);


        }



        static object get_defaultFont_0(ref object o)
        {
            return FairyGUI.UIConfig.defaultFont;
        }
        static void set_defaultFont_0(ref object o, object v)
        {
            FairyGUI.UIConfig.defaultFont = (System.String)v;
        }
        static object get_verticalScrollBar_1(ref object o)
        {
            return FairyGUI.UIConfig.verticalScrollBar;
        }
        static void set_verticalScrollBar_1(ref object o, object v)
        {
            FairyGUI.UIConfig.verticalScrollBar = (System.String)v;
        }
        static object get_horizontalScrollBar_2(ref object o)
        {
            return FairyGUI.UIConfig.horizontalScrollBar;
        }
        static void set_horizontalScrollBar_2(ref object o, object v)
        {
            FairyGUI.UIConfig.horizontalScrollBar = (System.String)v;
        }
        static object get_buttonSound_3(ref object o)
        {
            return FairyGUI.UIConfig.buttonSound;
        }
        static void set_buttonSound_3(ref object o, object v)
        {
            FairyGUI.UIConfig.buttonSound = (FairyGUI.NAudioClip)v;
        }
        static object get_buttonSoundVolumeScale_4(ref object o)
        {
            return FairyGUI.UIConfig.buttonSoundVolumeScale;
        }
        static void set_buttonSoundVolumeScale_4(ref object o, object v)
        {
            FairyGUI.UIConfig.buttonSoundVolumeScale = (System.Single)v;
        }


    }
}
