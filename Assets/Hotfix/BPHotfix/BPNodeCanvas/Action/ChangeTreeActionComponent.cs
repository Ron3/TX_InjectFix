using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class ChangeTreeActionComponentAwakeSystem : AwakeSystem<ChangeTreeActionComponent, Entity, BPActionShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(ChangeTreeActionComponent self, Entity parent, BPActionShell bpActionShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bpActionShell, config);
        }
    }

    public class ChangeTreeActionComponent : Entity
	{
        private BPActionShell bpActionShell;
        private Entity parent;

        public void Awake(Entity parent, BPActionShell bpActionShell, BPNodeCanvasTreeConfig config)
        {
            this.bpActionShell = bpActionShell;

            if (this.bpActionShell != null)
            {
                this.bpActionShell.onExecute = this.OnExecute;
            }
        }


        private void OnExecute()
        {
            this.bpActionShell.EndAction(true);

            Log.Debug($"Hello ChangeTreeAction ===> {this.bpActionShell.metaData}");

            // this.parent.GameObject.GetComponent<BehaviourTreeOwner>();
            GameObject npcGO = GameObject.Find("NPC");
            BehaviourTreeOwner oldBtOwner = npcGO.GetComponent<BehaviourTreeOwner>();
            BPEntity npcEntity = Game.Scene.Ensure<BPEntity>();

            Log.Debug($"oldBtOwner ==> {oldBtOwner.GetHashCode()}");

            ResourcesComponent resComp = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resComp.LoadBundle("bttree.unity3d");

            // assets/res/nodecanvasassets/ronaaa.txt
            if(this.bpActionShell.metaData == "npcattack2.asset")
            {
                UnityEngine.Object btTreeAsset = resComp.GetAsset("bttree.unity3d", "assets/res/nodecanvasassets/npcattack2.asset");
                // Log.Debug($"btTree11 ==> {btTreeAsset.GetHashCode()}");
                NodeCanvas.BehaviourTrees.BehaviourTree btTree = btTreeAsset as NodeCanvas.BehaviourTrees.BehaviourTree;
                // if(btTree != null)
                //     Log.Debug($"btTree22 ==> {btTree.GetHashCode()}");
                
                
                Log.Debug($"oldBtOwner.behaviour ==> {oldBtOwner.behaviour.GetHashCode()},  btTree ===> {btTree.GetHashCode()}");
                // oldBtOwner.behaviour = btTree;

                // oldBtOwner.StopBehaviour();
                oldBtOwner.SwitchBehaviour(btTree);
                NodeCanvasHelper.ReInit(npcEntity, npcGO);
                oldBtOwner.RestartBehaviour();

                // oldBtOwner.StartBehaviour(btTree);
                // NodeCanvasHelper.ReInit(npcEntity, npcGO);
                // oldBtOwner.RestartBehaviour();
                
                
                // BehaviourTreeOwner btOwner = npcGO.AddComponent<BehaviourTreeOwner>();
                // btOwner.StartBehaviour(btTree);
                // btOwner.StopBehaviour();

                // // Game.Scene.RemoveComponent<NPCEntity>();
                
                // NodeCanvasHelper.Init(entity, npcGO);
                // btOwner.StartBehaviour();    
            }
            else
            {
                UnityEngine.Object btTreeAsset = resComp.GetAsset("bttree.unity3d", "assets/res/nodecanvasassets/npcattack.asset");
                Log.Debug($"btTree333 ==> {btTreeAsset.GetHashCode()}");
                NodeCanvas.BehaviourTrees.BehaviourTree btTree = btTreeAsset as NodeCanvas.BehaviourTrees.BehaviourTree;
                if(btTree != null)
                    Log.Debug($"btTree666 ==> {btTree.GetHashCode()}");

                // oldBtOwner.StopBehaviour();
                oldBtOwner.SwitchBehaviour(btTree);
                // NodeCanvasHelper.ReInit(npcEntity, npcGO);
                // oldBtOwner.RestartBehaviour();

                
                // oldBtOwner.StartBehaviour(btTree);
                // NodeCanvasHelper.ReInit(npcEntity, npcGO);
                // oldBtOwner.RestartBehaviour();
                
                // oldBtOwner.StartBehaviour(btTree);
                // NodeCanvasHelper.ReInit(npcEntity, npcGO);
                // oldBtOwner.RestartBehaviour();
                

                // GameObject.Destroy(oldBtOwner);
                // BehaviourTreeOwner btOwner = npcGO.AddComponent<BehaviourTreeOwner>();
                // btOwner.StartBehaviour(btTree);
                // btOwner.StopBehaviour();
                // NodeCanvasHelper.Init(entity, npcGO);
                // btOwner.StartBehaviour();

                // NPCEntity entity = Game.Scene.Ensure<NPCEntity>();
                // NodeCanvasHelper.Init(entity, npcGO);

                // // btOwner.behaviour = btTree;
                // btOwner.RestartBehaviour();    
            }
            


            // btOwner.behaviour = this.bpActionShell.metaData;

            
            
            // 执行完毕后.一定要调用这个
            // this.bpActionShell.EndAction(true);
        }

        

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            if (this.bpActionShell != null)
            {
                this.bpActionShell.onExecute = null;
            }

            this.bpActionShell = null;
        }
    }
}