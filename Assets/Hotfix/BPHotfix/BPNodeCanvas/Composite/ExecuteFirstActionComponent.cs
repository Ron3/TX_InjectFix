using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class ExecuteFirstActionComponentAwakeSystem : AwakeSystem<ExecuteFirstActionComponent, Entity, BPCompositeShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(ExecuteFirstActionComponent self, Entity parent, BPCompositeShell bpCompositeShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bpCompositeShell, config);
        }
    }

    public class ExecuteFirstActionComponent : Entity
	{
        private Entity parent;
        private BPCompositeShell bpCompositeShell;

        public void Awake(Entity parent, BPCompositeShell bpCompositeShell, BPNodeCanvasTreeConfig config)
        {
            this.parent = parent;
            this.bpCompositeShell = bpCompositeShell;

            if (this.bpCompositeShell != null)
            {
                this.bpCompositeShell.onExecute = this.OnExecute;
                Log.Debug($"this.bpCompositeShell.onExecute ==> {this.bpCompositeShell.GetHashCode()}");
            }
        }


        public Status OnExecute(UnityEngine.Component agent, IBlackboard blackboard)
        {
            // Log.Debug($"Hello HotfixAction ===> {this.bpActionShell.metaData}");
            // 执行完毕后.一定要调用这个
            // this.bpActionShell.EndAction(true);

            if(this.bpCompositeShell.outConnections.Count > 0)
            {
                return this.bpCompositeShell.outConnections[0].Execute(agent, blackboard);
            }

            return Status.Failure;
        }
        

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            if (this.bpCompositeShell != null)
            {
                this.bpCompositeShell.onExecute = null;
            }

            this.bpCompositeShell = null;
        }
    }
}