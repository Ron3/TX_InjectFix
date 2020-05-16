using System;

namespace ETModel
{
	public static class Log
	{
		public static string logMsg = "";

		public static void Trace(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.Log(msg);
		}
		
		public static void Debug(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.Log(msg);
		}
		
		public static void Info(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.Log(msg);
		}

		public static void Warning(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.LogWarning(msg);
		}

		public static void Error(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.LogError(msg);
		}
		
		public static void Error(Exception e)
		{
			HandleLogMsg(e.ToString(), true);
			UnityEngine.Debug.LogException(e);
		}

		public static void Fatal(string msg)
		{
			HandleLogMsg(msg);
			UnityEngine.Debug.LogAssertion(msg);
		}

		public static void Trace(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}

		public static void Warning(string message, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(message, args);
		}

		public static void Info(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}

		public static void Debug(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}

		public static void Error(string message, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(message, args);
		}

		public static void Fatal(string message, params object[] args)
		{
			UnityEngine.Debug.LogAssertionFormat(message, args);
		}

		public static void Msg(object msg)
		{
			Debug(Dumper.DumpAsString(msg));
		}

		/// <summary>
		/// 处理Log
		/// </summary>
		/// <param name="msg"></param>
		public static void HandleLogMsg(string msg, bool isError=false)
		{
			if(msg == null)
				return;
			
			logMsg = "\n"	 + logMsg;
			if(isError == false)
				logMsg = msg + "\n"	 + logMsg;
			else
				logMsg = $"[color=#FF4040]{msg}[/color]" + "\n"	 + logMsg;
		}
	}
}