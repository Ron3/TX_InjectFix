using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BPAnimationEventShell : MonoBehaviour
{
    private List<Action<GameObject, int>> doAnimationColisionList;
    public Action<GameObject, int> doAnimationFinish;

    public void ClearAllAction()
    {
        this.doAnimationColisionList?.Clear();
        this.doAnimationFinish = null;
    }

    public void ClearColistionAction()
    {
        this.doAnimationColisionList?.Clear();
    }

    public void AddColisionAction(Action<GameObject, int> action)
    {
        if(action == null){
            return;
        }

        if(this.doAnimationColisionList == null)
        {
            this.doAnimationColisionList = new List<Action<GameObject, int>>();
        }

        this.doAnimationColisionList.Add(action);
    }

    /// <summary>
    /// 
    /// </summary>
    private void DoAnimationColision(int colisionIndex)
    {
        // Debug.Log("DoAnimationColision -> " + frameIndex);
        if(doAnimationColisionList == null || doAnimationColisionList.Count == 0)
        {
            return;
        }

        // 防止action list执行的过程中被清空
        List<Action<GameObject, int>> tempActionList = new List<Action<GameObject, int>>(this.doAnimationColisionList.ToArray());

        foreach(var action in tempActionList)
        {
            // 被清空，中断退出
            if(this.doAnimationColisionList.Count == 0){
                break;
            }
            // this.doAnimationColision?.Invoke(this.gameObject, colisionIndex);
            action.Invoke(this.gameObject, colisionIndex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void DoAnimationFinish(int frameIndex)
    {
        // Debug.Log("DoAnimationFinish -> " + frameIndex);
        this.doAnimationFinish?.Invoke(this.gameObject, frameIndex);
    }
}
