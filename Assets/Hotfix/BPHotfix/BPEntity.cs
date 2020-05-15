using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;


namespace ETHotfix
{
	public class BPEntity : Entity
	{
        /// <summary>
        /// 由于ET的框架没有自带的go.我在这里先简单继承Entity.然后增加一个gameObject
        /// </summary>
        public GameObject go;
    }

}
