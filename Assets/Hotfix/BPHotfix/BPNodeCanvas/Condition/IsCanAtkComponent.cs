using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class IsCanAtkComponentAwakeSystem : AwakeSystem<IsCanAtkComponent, Entity, BPConditionShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(IsCanAtkComponent self, Entity parent, BPConditionShell bpConditionShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bpConditionShell, config);
        }
    }

    public class IsCanAtkComponent : Entity
	{
        private BPConditionShell bpConditionShell;

        public void Awake(Entity parent, BPConditionShell bpConditionShell, BPNodeCanvasTreeConfig config)
        {
            this.bpConditionShell = bpConditionShell;

            if (this.bpConditionShell != null)
            {
                this.bpConditionShell.onCheck = this.OnCheck;
            }
        }


        private bool OnCheck()
        {
            Log.Debug("Is can atk Check!");
            return true;
        }


        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            
            base.Dispose();

            if (this.bpConditionShell != null)
            {
                this.bpConditionShell.onCheck = null;
            }

            this.bpConditionShell = null;
        }
    }
}