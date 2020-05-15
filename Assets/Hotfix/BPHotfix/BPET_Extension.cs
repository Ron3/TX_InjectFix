using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;
    
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;
// using Object = UnityEngine.Object;
namespace ETHotfix
{
    public static class BPETExtension
    {
        public static T Ensure<T>(this ETHotfix.Entity entity) where T : ETHotfix.Component, new()
        {
            if(entity != null)
            {
                T t = entity.GetComponent<T>();
                if(t == null)
                    t = entity.AddComponent<T>();

                return t;
            }

            return null;
        }


        /// <summary>
        /// 辅助带一个参数的
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="arg1"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static T Ensure<T, T2>(this ETHotfix.Entity entity, T2 arg1) where T : ETHotfix.Component, new()
        {
            if(entity != null)
            {
                T t = entity.GetComponent<T>();
                if(t == null)
                    t = entity.AddComponent<T, T2>(arg1);

                return t;
            }

            return null;
        }


        /// <summary>
        /// 确定删除某个组件
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="T"></typeparam>
        public static void EnsureRemove<T>(this ETHotfix.Entity entity) where T: ETHotfix.Component
        {
            if(entity.GetComponent<T>() != null)
                entity.RemoveComponent<T>();
        }
    }

}

