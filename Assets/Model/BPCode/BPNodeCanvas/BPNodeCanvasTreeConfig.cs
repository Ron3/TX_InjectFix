
namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class BPNodeCanvasTreeConfigCategory : ACategory<BPNodeCanvasTreeConfig>
	{
	}

	public class BPNodeCanvasTreeConfig: IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Description;
		public string ComponentName;
		// public string ParameterList;
		public string ExampleMetaData;	
	}
}
