using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DecoratorHotfixComponentAwakeSystem : AwakeSystem<DecoratorHotfixComponent, Entity, BPDecoratorShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(DecoratorHotfixComponent self, Entity parent, BPDecoratorShell bPDecoratorShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bPDecoratorShell, config);
        }
    }

    public class DecoratorHotfixComponent : Entity
	{
        private BPDecoratorShell bPDecoratorShell;

        public void Awake(Entity parent, BPDecoratorShell bPDecoratorShell, BPNodeCanvasTreeConfig config)
        {
            this.bPDecoratorShell = bPDecoratorShell;

            if (this.bPDecoratorShell != null)
            {
                this.bPDecoratorShell.onExecute = this.OnExecute;
            }
        }


        private NodeCanvas.Framework.Status OnExecute(UnityEngine.Component agent,  IBlackboard blackboard)
        {
            Log.Debug("BP DecoratorShell hotfix execute!");
            return Status.Success;
        }


        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            
            base.Dispose();

            if (this.bPDecoratorShell != null)
            {
                this.bPDecoratorShell.onExecute = null;
            }

            this.bPDecoratorShell = null;
        }
    }
}