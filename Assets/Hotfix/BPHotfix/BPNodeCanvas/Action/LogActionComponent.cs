using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class LogActionComponentAwakeSystem : AwakeSystem<LogActionComponent, Entity, BPActionShell, BPNodeCanvasTreeConfig>
    {
        public override void Awake(LogActionComponent self, Entity parent, BPActionShell bpActionShell, BPNodeCanvasTreeConfig config)
        {
            self.Awake(parent, bpActionShell, config);
        }
    }

    public class LogActionComponent : Entity
	{
        private Entity parent;
        private BPActionShell bpActionShell;

        public void Awake(Entity parent, BPActionShell bpActionShell, BPNodeCanvasTreeConfig config)
        {
            this.parent = parent;
            this.bpActionShell = bpActionShell;

            if (this.bpActionShell != null)
            {
                this.bpActionShell.onExecute = this.OnExecute;
                this.bpActionShell.onInit = this.OnInit;
                this.bpActionShell.onStop = this.OnStop;
                
                // Log.Debug($"this.bpActionShell.onExecute ==> {this.bpActionShell.GetHashCode()}");
            }
        }


        private void OnExecute()
        {
            Log.Debug($"Hello HotfixAction ===> {this.bpActionShell.metaData}");
            
            // 执行完毕后.一定要调用这个
            this.bpActionShell.EndAction(true);
        }


        /// <summary>
        /// Use for initialization. This is called only once in the lifetime of the task.
		/// Return null if init was successfull. Return an error string otherwise
        /// </summary>
        /// <returns></returns>
        private string OnInit()
        {
            // Log.Debug($"[Hotfix] Log Action init");
            return null;
        }
        

        private void OnStop()
        {
            // Log.Debug($"[Hotfix] Log Action OnStop");
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
                this.bpActionShell.onInit = null;
                this.bpActionShell.onStop = null;
            }

            this.bpActionShell = null;
        }
    }
}