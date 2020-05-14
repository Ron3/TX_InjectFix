using UnityEngine;


/// <summary>
/// 这个类是角色的属性
/// </summary>
public class BPActor : MonoBehaviour 
{
    public int hp = 13;
    public int speed = 1;
    public int atk = 20;
    public int def = 10;
    public string roleName = "狼人";


    /// <summary>
    /// 
    /// </summary>
    void Start() 
    {

    }


    /// <summary>
    /// 返回攻击力
    /// </summary>
    /// <returns></returns>
    public int GetAtk()
    {
        return this.atk;
    }

    public int Atk
    {
        get{
            return this.atk;
        }
    }


    // [IFix.Interpret]
    // public int Hp
    // {
    //     get{
    //         return this.hp;
    //     }
    // }

    [IFix.Interpret]
    public int GetHp2()
    {
        return this.hp;
    }
}

