namespace ETModel
{
	public class GlobalProto
	{
		public string AssetBundleServerUrl;
		public string Address;

		public string GetUrl()
		{
			string url = this.AssetBundleServerUrl;
			
#if UNITY_ANDROID
			url += "Android/";
#elif UNITY_IOS
			url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#elif UNITY_STANDALONE_OSX
			url += "MacOS/";
#else
			url += "PC/";
#endif
			Log.Debug(url);
			return url;
		}


		public static string StaticGetUrl()
		{
			// string url = this.AssetBundleServerUrl;
			// TODO: Ron
			// 这里先直接写死url
			string url = "http://106.75.57.197/";

#if UNITY_ANDROID
			url += "Android/";
#elif UNITY_IOS
			url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#elif UNITY_STANDALONE_OSX
			url += "MacOS/";
#else
			url += "PC/";
#endif
			Log.Debug(url);
			return url;
		}
	}
}
