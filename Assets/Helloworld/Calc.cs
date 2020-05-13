/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms. 
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */


using System;


namespace IFix.Test
{
    public class ItemInfo
    {
        public int itemId = 0;
        public string name = "";
        public string desc = "";
        public ItemInfo(int itemId, string name, string desc)
        {
            this.itemId = itemId;
            this.name = name;
            this.desc = desc;
        }
    }


    //HelloworldCfg.cs里配置了这个类型
    public class Calculator
    {
        ItemInfo itemInfo = null;

        int loopNum = 1000000;

        //修改成正确的逻辑后，打开如下注释，生成的补丁将修正该函数
        // [Patch]
        public int Add(int a, int b)
        {
            // return a + b;
            long beginTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;

            for(int i = 0; i < this.loopNum; ++i)
            {
                this.Mult(2, 2);
            }

            long now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;
            UnityEngine.Debug.Log($"add cost Time 33 ===>{now - beginTime}");

            return a * b;
        }

        public int Sub(int a, int b)
        {
            long beginTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;

            for(int i = 0; i < this.loopNum; ++i)
            {
                
            }
            long now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/ 10000;
            UnityEngine.Debug.Log($"sub cost Time===> {now - beginTime}");
            
            return a / b;
        }

        public int Mult(int a, int b)
        {
            return a * b;
        }

        public int Div(int a, int b)
        {
            return a / b;
        }

        // [Patch]
        public void RonTestFun()
        {
            this.itemInfo = new ItemInfo(1001, "黑珍珠项链", "攻击力+10000的神器!");
            UnityEngine.Debug.Log("创建itemInfo成功[1]");
        }

        public void RonLogItemInfo()
        {
            if(this.itemInfo != null)
            {
                UnityEngine.Debug.Log($"{this.itemInfo.itemId}, ===>  {this.itemInfo.name + "  " + this.itemInfo.desc}" );
            }
            else
            {
                UnityEngine.Debug.Log("Ron log item info Error!");
            }
        }


        public async void asyncLoad()
        {
            // await load();
        }
        
        public void load()
        {

        }
    }
}
