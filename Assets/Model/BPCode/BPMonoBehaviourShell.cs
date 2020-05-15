using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BPMonoBehaviourShell : MonoBehaviour
{
    public Action<GameObject> awake;
    public Action<GameObject> start;
    public Action<GameObject> update;
    public Action<GameObject> fixedUpdate;
    public Action<GameObject> lateUpdate;
    public Action<GameObject> onDisable;
    public Action<GameObject> onEnable;
    public Action<GameObject> onGUI;


    /// <summary>
    /// 
    /// </summary>
    private void Awake() 
    {
        this.awake?.Invoke(this.gameObject);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        this.start?.Invoke(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        this.update?.Invoke(this.gameObject);
    }


    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        this.fixedUpdate?.Invoke(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    void LateUpdate()
    {
        this.lateUpdate?.Invoke(this.gameObject);
    }


    /// <summary>
    /// 
    /// </summary>
    private void OnGUI()
    {
        this.onGUI?.Invoke(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        this.onDisable?.Invoke(this.gameObject);
    }


    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        this.onEnable?.Invoke(this.gameObject);
    }

    // private void OnGUI()
    // {
    //     Debug.Log("bp mono behaviour shell's on gui");

    //     Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y, transform.position.z);
	// 	//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
	// 	Vector2 position = Camera.main.WorldToScreenPoint (worldPosition);
	// 	//得到真实NPC头顶的2D坐标
	// 	position = new Vector2 (position.x, Screen.height - position.y);

    //     // Vector3 position = new Vector3(150, 150, -10);
    //     // Vector2 nameSize = new Vector2(130, 130);
    //     string text = "123456789";
    //     Vector2 nameSize = GUI.skin.label.CalcSize (new GUIContent(text));
    //     GUI.color  = Color.green;
	// 	//绘制NPC名称
	// 	GUI.Label(new Rect(position.x - (nameSize.x/2), position.y - nameSize.y, nameSize.x,nameSize.y), text);
    // }
}
