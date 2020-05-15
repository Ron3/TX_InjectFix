using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class CompositeHotfixComponentAwakeSystem : AwakeSystem<CompositeHotfixComponent, Entity, BPCompositeShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(CompositeHotfixComponent self, Entity parent, BPCompositeShell bPCompositeShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bPCompositeShell, config);
        }
    }

    public class CompositeHotfixComponent : Entity
	{
        private BPCompositeShell bPCompositeShell;

        public void Awake(Entity parent, BPCompositeShell bPCompositeShell, BPNodeCanvasTreeConfig config)
        {
            this.bPCompositeShell = bPCompositeShell;

            if (this.bPCompositeShell != null)
            {
                this.bPCompositeShell.onExecute = this.OnExecute;
            }
        }
        

        private NodeCanvas.Framework.Status OnExecute(UnityEngine.Component agent,  IBlackboard blackboard)
        {
            Log.Debug("BP Composite hotfix execute!");
            return Status.Success;
        }

        
        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            
            base.Dispose();

            if (this.bPCompositeShell != null)
            {
                this.bPCompositeShell.onExecute = null;
            }

            this.bPCompositeShell = null;
        }
    }
}