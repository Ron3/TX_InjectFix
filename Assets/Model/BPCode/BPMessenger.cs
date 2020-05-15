/*  http://wiki.unity3d.com/index.php?title=Advanced_CSharp_Messenger
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * 
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 * 
 * Features:
 	* Prevents a MissingReferenceException because of a reference to a destroyed message handler.
 	* Option to log all messages
 	* Extensive error detection, preventing silent bugs
 * 
 * Usage examples:
 	1. BPMessenger.AddListener<GameObject>("prop collected", PropCollected);
 	   BPMessenger.Broadcast<GameObject>("prop collected", prop);
 	2. BPMessenger.AddListener<float>("speed changed", SpeedChanged);
 	   BPMessenger.Broadcast<float>("speed changed", 0.5f);
 * 
 * BPMessenger cleans up its evenTable automatically upon loading of a new level.
 * 
 * Don't forget that the messages that should survive the cleanup, should be marked with BPMessenger.MarkAsPermanent(string)
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public delegate void BPCallback();
    public delegate void BPCallback<T>(T arg1);
    public delegate void BPCallback<T, U>(T arg1, U arg2);
    public delegate void BPCallback<T, U, V>(T arg1, U arg2, V arg3);

    //static internal class BPMessenger {
    public static class BPMessenger 
    {
        #region Internal variables

        //Disable the unused variable warning
        #pragma warning disable 0414
        //Ensures that the BPMessengerHelper will be created automatically upon start of the game.
        static private BPMessengerHelper messengerHelper = (new GameObject ("BPMessengerHelper")).AddComponent<BPMessengerHelper>();
        #pragma warning restore 0414

        static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

        //Message handlers that should never be removed, regardless of calling Cleanup
        static public List<string> permanentMessages = new List<string> ();
        
        #endregion

        #region ==========Helper methods begin==========
        
        /// <summary>
        /// Marks a certain message as permanent.
        /// </summary>
        /// <param name="eventType"></param>
        static public void MarkAsPermanent (string eventType) 
        {
#if LOG_ALL_MESSAGES
            Debug.Log ("BPMessenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
            permanentMessages.Add (eventType);
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Cleanup() 
        {
#if LOG_ALL_MESSAGES
            Debug.Log ("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif
            List<string> messagesToRemove = new List<string> ();

            foreach (KeyValuePair<string, Delegate> pair in eventTable) 
            {
                bool wasFound = false;
                foreach (string message in permanentMessages) 
                {
                    if (pair.Key == message) 
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add (pair.Key);
            }

            foreach (string message in messagesToRemove) 
            {
                eventTable.Remove (message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static public void PrintEventTable() 
        {
            Debug.Log ("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<string, Delegate> pair in eventTable) 
            {
                Debug.Log ("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log ("\n");
        }

        #endregion 

        #region Message logging and exception throwing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingAdded"></param>
        static public void OnListenerAdding (string eventType, Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
            Debug.Log ("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

            if(!eventTable.ContainsKey (eventType)) 
            {
                eventTable.Add (eventType, null);
            }

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType()) 
            {
                throw new ListenerException (string.Format ("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType ().Name, listenerBeingAdded.GetType ().Name));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingRemoved"></param>
        static public void OnListenerRemoving (string eventType, Delegate listenerBeingRemoved) 
        {
#if LOG_ALL_MESSAGES
            Debug.Log ("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

            if (eventTable.ContainsKey (eventType)) 
            {
                Delegate d = eventTable[eventType];
                if (d == null) 
                {
                    throw new ListenerException (string.Format ("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType () != listenerBeingRemoved.GetType ()) 
                {
                    throw new ListenerException (string.Format ("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType ().Name, listenerBeingRemoved.GetType ().Name));
                }
            } 
            else 
            {
                throw new ListenerException (string.Format ("Attempting to remove listener for type \"{0}\" but BPMessenger doesn't know about this event type.", eventType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        static public void OnListenerRemoved (string eventType) 
        {
            if (eventTable[eventType] == null) 
            {
                eventTable.Remove (eventType);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        static public void OnBroadcasting (string eventType) 
        {
#if REQUIRE_LISTENER
            if (!eventTable.ContainsKey (eventType)) 
            {
                // throw new BroadcastException (string.Format ("Broadcasting message \"{0}\" but no listener found. Try marking the message with BPMessenger.MarkAsPermanent.", eventType));
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        static public BroadcastException CreateBroadcastSignatureException (string eventType) 
        {
            return new BroadcastException (string.Format ("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        /// <summary>
        /// 
        /// </summary>
        public class BroadcastException : Exception 
        {
            public BroadcastException (string msg) : base (msg) 
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ListenerException : Exception 
        {
            public ListenerException (string msg) : base (msg) 
            {

            }
        }
        #endregion


        #region AddListener
        
        /// <summary>
        /// No parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        static public void AddListener(string eventType, BPCallback handler) 
        {
            OnListenerAdding (eventType, handler);
            eventTable[eventType] = (BPCallback) eventTable[eventType] + handler;
        }


        /// <summary>
        /// Single parameter 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        static public void AddListener<T> (string eventType, BPCallback<T> handler) 
        {
            OnListenerAdding (eventType, handler);
            eventTable[eventType] = (BPCallback<T>) eventTable[eventType] + handler;
        }

        
        /// <summary>
        /// Two parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        static public void AddListener<T, U> (string eventType, BPCallback<T, U> handler)
        {
            OnListenerAdding (eventType, handler);
            eventTable[eventType] = (BPCallback<T, U>) eventTable[eventType] + handler;
        }

    
        /// <summary>
        /// Three parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        static public void AddListener<T, U, V>(string eventType, BPCallback<T, U, V> handler)
        {
            OnListenerAdding (eventType, handler);
            eventTable[eventType] = (BPCallback<T, U, V>) eventTable[eventType] + handler;
        }
        #endregion


        #region RemoveListener
        /// <summary>
        /// No parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        static public void RemoveListener (string eventType, BPCallback handler) 
        {
            OnListenerRemoving (eventType, handler);
            eventTable[eventType] = (BPCallback) eventTable[eventType] - handler;
            OnListenerRemoved (eventType);
        }

        /// <summary>
        /// Single parameter
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        static public void RemoveListener<T> (string eventType, BPCallback<T> handler) 
        {
            OnListenerRemoving (eventType, handler);
            eventTable[eventType] = (BPCallback<T>) eventTable[eventType] - handler;
            OnListenerRemoved (eventType);
        }


        /// <summary>
        /// Two parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        static public void RemoveListener<T, U> (string eventType, BPCallback<T, U> handler)
        {
            OnListenerRemoving (eventType, handler);
            eventTable[eventType] = (BPCallback<T, U>) eventTable[eventType] - handler;
            OnListenerRemoved (eventType);
        }

    
        /// <summary>
        /// Three parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        static public void RemoveListener<T, U, V> (string eventType, BPCallback<T, U, V> handler) 
        {
            OnListenerRemoving (eventType, handler);
            eventTable[eventType] = (BPCallback<T, U, V>) eventTable[eventType] - handler;
            OnListenerRemoved (eventType);
        }
        #endregion


        #region Broadcast
        /// <summary>
        /// No parameters
        /// </summary>
        /// <param name="eventType"></param>
        static public void Broadcast (string eventType) 
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log ("MESSENGER\t" + System.DateTime.Now.ToString ("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting (eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d)) 
            {
                BPCallback callback = d as BPCallback;
                if (callback != null) 
                {
                    callback();
                } 
                else 
                {
                    // throw CreateBroadcastSignatureException (eventType);
                }
            }
        }

        //
        /// <summary>
        /// Single parameter
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="arg1"></param>
        /// <typeparam name="T"></typeparam>
        static public void Broadcast<T> (string eventType, T arg1) 
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log ("MESSENGER\t" + System.DateTime.Now.ToString ("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting (eventType);

            Delegate d;
            if (eventTable.TryGetValue (eventType, out d)) 
            {
                BPCallback<T> callback = d as BPCallback<T>;

                if (callback != null) 
                {
                    callback (arg1);
                } 
                else 
                {
                    throw CreateBroadcastSignatureException (eventType);
                }
            }
        }

        /// <summary>
        /// Two parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        static public void Broadcast<T, U> (string eventType, T arg1, U arg2) 
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log ("MESSENGER\t" + System.DateTime.Now.ToString ("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting (eventType);

            Delegate d;
            if (eventTable.TryGetValue (eventType, out d)) 
            {
                BPCallback<T, U> callback = d as BPCallback<T, U>;

                if (callback != null) 
                {
                    callback (arg1, arg2);
                } 
                else 
                {
                    throw CreateBroadcastSignatureException (eventType);
                }
            }
        }


        /// <summary>
        /// Three parameters
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        static public void Broadcast<T, U, V> (string eventType, T arg1, U arg2, V arg3) 
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            Debug.Log ("MESSENGER\t" + System.DateTime.Now.ToString ("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting (eventType);

            Delegate d;
            if (eventTable.TryGetValue (eventType, out d)) 
            {
                BPCallback<T, U, V> callback = d as BPCallback<T, U, V>;

                if (callback != null) 
                {
                    callback (arg1, arg2, arg3);
                }
                else 
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
    }
    #endregion













    //This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
    public sealed class BPMessengerHelper : MonoBehaviour 
    {
        /// <summary>
        /// 
        /// </summary>
        void Awake() 
        {
            DontDestroyOnLoad(gameObject);
        }
        
        
        /// <summary>
        /// Clean up eventTable every time a new level loads.
        /// </summary>
        /// <param name="unused"></param>
        public void OnLevelWasLoaded(int unused) 
        {
            BPMessenger.Cleanup();
        }
    }
}

