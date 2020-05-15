using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;


namespace ETHotfix
{
    [ObjectSystem]
    public class BPMainControllerComponentUpdateSystem : UpdateSystem<BPMainControllerComponent>
    {
        public override void Update(BPMainControllerComponent self)
        {
            self.Update();
        }
    }

    public class BPMainControllerComponent : Component
    {
        // private static BPMainControllerComponent theInstance = null;

        // public static int currentFairyGUILayer = BPG_UI.FAIRY_GUI_LAYER_INDEX; 

        // // sprite renderer的sorting layer的offset，用于顶层显示
        // public static int currentSpriteRendererOffset = 0; 

        // private AudioSource audioSource = null;
        // private AudioSource bgmAudioSource = null;

        // private BPLogicControllerComponent currentLogicControllerComponet;
        // private List<BPLogicControllerComponent> currentLogicControllerComponetStack;   // 当前的controller的栈

        // /// <summary>
        // /// 显示的UI挂靠的Enity会存放在这里，以处理输入屏蔽
        // /// </summary>
        // private List<Entity> uiEntityList = new List<Entity>();

        // /// <summary>
        // /// 是否有UI正在显示中
        // /// </summary>
        // /// <returns><c>true</c>, if has UIS howed was ised, <c>false</c> otherwise.</returns>
        // public bool IsHasUIShowing()
        // {
        //     return uiEntityList.Count > 0;
        // }

        // /// <summary>
        // /// 显示一个UI，返回此UI挂靠的Entity（用于后续删除）
        // /// </summary>
        // /// <returns>The user interface.</returns>
        // public K AddComponent<K, P1>(P1 p1) where K : Component, new()
        // {
        //     Entity entity = new Entity();
        //     K component = entity.AddComponent<K, P1>(p1);
        //     //K component = ComponentFactory.CreateWithParent<K, P1>(this, p1);
        //     //entity.AddComponent(uiComponent);

        //     uiEntityList.Add(entity);
        //     return component;
        // }

        // public K AddComponent<K>() where K : Component, new()
        // {
        //     Entity entity = new Entity();
        //     K component = entity.AddComponent<K>();
        //     //K component = ComponentFactory.CreateWithParent<K, P1>(this, p1);
        //     //entity.AddComponent(uiComponent);

        //     uiEntityList.Add(entity);
        //     return component;
        // }

        // public K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : Component, new()
        // {
        //     Entity entity = new Entity();
        //     K component = entity.AddComponent<K, P1, P2>(p1, p2);
        //     //K component = ComponentFactory.CreateWithParent<K, P1>(this, p1);
        //     //entity.AddComponent(uiComponent);

        //     uiEntityList.Add(entity);
        //     return component;
        // }

        // public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : Component, new()
        // {
        //     Entity entity = new Entity();
        //     K component = entity.AddComponent<K, P1, P2, P3>(p1, p2, p3);
        //     //K component = ComponentFactory.CreateWithParent<K, P1>(this, p1);
        //     //entity.AddComponent(uiComponent);

        //     uiEntityList.Add(entity);
        //     return component;
        // }

        // public void RemoveComponent(Component uiComponent)
        // {
        //     if(uiComponent == null)
        //     {
        //         return;
        //     }

        //     foreach(Entity entity in uiEntityList)
        //     {
        //         if(entity.GetComponent(uiComponent.GetType()) != null)
        //         {
        //             entity.RemoveComponent(uiComponent.GetType());
        //             uiEntityList.Remove(entity);
        //             break;
        //         }
        //     }

        //     Debug.Log("MainControll RemoveComponent, current size == " + uiEntityList.Count);
        // }

        // private BPMainControllerComponent()
        // {
        //     this.currentLogicControllerComponetStack = new List<BPLogicControllerComponent>();
        // }

        // public void PushController(BPLogicControllerComponent controller)
        // {
        //     if(controller == null || this.currentLogicControllerComponetStack.Contains(controller)){
        //         return;
        //     }

        //     // if(controller.isFullScreenController)
        //     // {
        //     //     BPMainControllerComponent.currentFairyGUILayer += 1;

        //     //     // 设置FairyGUI的摄像头
        //     //     GameObject stageCameraGO = GameObject.Find("Stage Camera");
        //     //     stageCameraGO.GetComponent<Camera>().cullingMask = (1 << BPMainControllerComponent.currentFairyGUILayer);
        //     // }

        //     this.currentLogicControllerComponetStack.Add(controller);
        //     Game.Scene.AddComponent(controller);
        // }

        // public void PopController(BPLogicControllerComponent controller)
        // {
        //     if(controller == null || this.currentLogicControllerComponetStack.Contains(controller) == false){
        //         return;
        //     }

        //     // if(controller.isFullScreenController)
        //     // {
        //     //     BPMainControllerComponent.currentFairyGUILayer -= 1;

        //     //     // 设置FairyGUI的摄像头
        //     //     GameObject stageCameraGO = GameObject.Find("Stage Camera");
        //     //     stageCameraGO.GetComponent<Camera>().cullingMask = (1 << BPMainControllerComponent.currentFairyGUILayer);
        //     // }

        //     this.currentLogicControllerComponetStack.Remove(controller);
        //     Game.Scene.RemoveComponent(controller.GetType());
        // }

        // public static void ChangeSpriteRendererOffset(int offset)
        // {
        //     currentSpriteRendererOffset = offset;
        // }

        // private static List<Component> uiMaskComponentStack = new List<Component>();

        // /// <summary>
        // /// 显示指定Layer的FairyGUI介面
        // /// </summary>
        // /// <param name="layer"></param>
        // public static void PushFairyGUILayer()
        // {
        //     BPMainControllerComponent.currentFairyGUILayer += 1;

        //     // 设置FairyGUI的摄像头
        //     GameObject stageCameraGO = GameObject.Find("Stage Camera");
        //     stageCameraGO.GetComponent<Camera>().cullingMask = (1 << BPMainControllerComponent.currentFairyGUILayer);

        //     // 加一个全屏透明的底UI
        //     var mask = BPMainControllerComponent.Instance().AddComponent<BPUIMaskComponent>();
        //     uiMaskComponentStack.Add(mask);
        //     Debug.Log("PushFairyGUILayer -> " + uiMaskComponentStack.Count);
        // }

        // /// <summary>
        // /// 显示指定Layer的FairyGUI介面
        // /// </summary>
        // /// <param name="layer"></param>
        // public static void PopFairyGUILayer()
        // {
        //     BPMainControllerComponent.currentFairyGUILayer -= 1;

        //     // 设置FairyGUI的摄像头
        //     GameObject stageCameraGO = GameObject.Find("Stage Camera");
        //     stageCameraGO.GetComponent<Camera>().cullingMask = (1 << BPMainControllerComponent.currentFairyGUILayer);

        //     // 加一个全屏透明的底UI
        //     var mask = uiMaskComponentStack[uiMaskComponentStack.Count - 1];
        //     BPMainControllerComponent.Instance().RemoveComponent(mask);
        //     uiMaskComponentStack.RemoveAt(uiMaskComponentStack.Count - 1);
        //     Debug.Log("PopFairyGUILayer -> " + uiMaskComponentStack.Count);
        // }

        // /// <summary>
        // /// 添加一个Controller的Component，理论上同一时间只会有一个Controller Component
        // /// </summary>
        // /// <param name="controllerComponent">Controller component.</param>
        // public void SwitchController(BPLogicControllerComponent controllerComponent)
        // {
        //     // 如果当前的LogicController不为空，则先删除
        //     if(this.currentLogicControllerComponet != null)
        //     {
        //         Game.Scene.RemoveComponent(this.currentLogicControllerComponet.GetType());
        //         this.currentLogicControllerComponet = null;
        //     }

        //     // 设置controller
        //     if(controllerComponent != null)
        //     {
        //         this.currentLogicControllerComponet = controllerComponent;
        //         Game.Scene.AddComponent(controllerComponent);
        //     }
        // }


        // // Update is called once per frame
        // public static BPMainControllerComponent Instance()
        // { 
        //     if(theInstance == null)
        //     {
        //         theInstance = new BPMainControllerComponent();
        //     }

        //     GameObject globalObject = GameObject.Find("AudioSource");
        //     theInstance.audioSource = globalObject.GetComponent<AudioSource>();

        //     globalObject = GameObject.Find("BGM");
        //     theInstance.bgmAudioSource = globalObject.GetComponent<AudioSource>();

        //     return theInstance;
        // }

        // /// <summary>
        // /// 用Global里的BPShell开启一个协程
        // /// </summary>
        // /// <param name="routine"></param>
        // /// <returns></returns>
        // public static Coroutine StartCoroutine(IEnumerator routine)
        // {
        //     BPShell shell = GameObject.Find("Global").GetComponent<BPShell>();
        //     return shell.StartCoroutine(routine);
        // }

        // /// <summary>
        // /// 播放BGM
        // /// </summary>
        // /// <param name="bgmName">Bgm name.</param>
        // public void PlayBGM(string bgmName)
        // {
        //     this.bgmAudioSource.Stop();

        //     //string musicPath = "Music/" + bgmName;
        //     //AudioClip clip = Resources.Load<AudioClip>(musicPath);//调用Resources方法加载AudioClip资源

        //     string musicPath = "Assets/Res/Music/" + bgmName + ".mp3";
        //     AudioClip clip = BPUtility.LoadObjectFromAB<AudioClip>(BPModel.AB_NAME_MUSIC, musicPath);

        //     Debug.Log("Play bgm -> " + musicPath + ", clip=" + clip);

        //     bgmAudioSource.clip = clip;
        //     bgmAudioSource.loop = true;
        //     bgmAudioSource.Play();
        // }

        // /// <summary>
        // /// 播放音效，一次
        // /// </summary>
        // /// <param name="effectName">Effect name.</param>
        // public void PlayEffect(string effectName, int loopNum=1)
        // {
        //     if(loopNum == 1)
        //     {
        //         PlayerOnce(effectName);
        //     }else
        //     {
        //         //string musicPath = "" + effectName;
        //         //AudioClip clip = Resources.Load<AudioClip>(musicPath);//调用Resources方法加载AudioClip资源

        //         string musicPath = "Assets/Res/Music/" + effectName + ".mp3";
        //         AudioClip clip = BPUtility.LoadObjectFromAB<AudioClip>(BPModel.AB_NAME_MUSIC, musicPath);

        //         audioSource.clip = clip;
        //         double time = clip.length * loopNum;
        //         audioSource.PlayScheduled(time);
        //     }
        // }

        public void Update()
        {

        }

        // /// <summary>
        // /// 播放一次音效
        // /// </summary>
        // /// <param name="name">Name.</param>
        // public static void PlayerOnce(string name)
        // {
        //     //string musicPath = "" + name;
        //     //AudioClip clip = Resources.Load<AudioClip>(musicPath);//调用Resources方法加载AudioClip资源


        //     string musicPath = "Assets/Res/Music/" + name + ".mp3";
        //     AudioClip clip = BPUtility.LoadObjectFromAB<AudioClip>(BPModel.AB_NAME_MUSIC, musicPath);

        //     AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        // }
    }
}
