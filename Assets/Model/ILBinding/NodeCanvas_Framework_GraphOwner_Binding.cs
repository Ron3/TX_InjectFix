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
    unsafe class NodeCanvas_Framework_GraphOwner_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(NodeCanvas.Framework.GraphOwner);
            args = new Type[]{};
            method = type.GetMethod("RestartBehaviour", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RestartBehaviour_0);
            args = new Type[]{};
            method = type.GetMethod("get_blackboard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_blackboard_1);


        }


        static StackObject* RestartBehaviour_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.GraphOwner instance_of_this_method = (NodeCanvas.Framework.GraphOwner)typeof(NodeCanvas.Framework.GraphOwner).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RestartBehaviour();

            return __ret;
        }

        static StackObject* get_blackboard_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NodeCanvas.Framework.GraphOwner instance_of_this_method = (NodeCanvas.Framework.GraphOwner)typeof(NodeCanvas.Framework.GraphOwner).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.blackboard;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
