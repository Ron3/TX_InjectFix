using UnityEngine;
using FairyGUI;

namespace ETHotfix
{
    public delegate void Callback();
    public delegate void CallbackWithInt(int arg);


    public static class BPG_COMMON
    {
        public static string BPSavePath
	    {
		    get
		    {
			    return Application.persistentDataPath;
		    }
	    }

        // 水平组合的属性 Horizontal
	    public const int UI_ALIGNMENT_HORIZONTAL_TOP = 0;
	    public const int UI_ALIGNMENT_HORIZONTAL_CENTER = 1;
	    public const int UI_ALIGNMENT_HORIZONTAL_BOTTOM = 2;

        // 垂直组合的属性
	    public const int UI_ALIGNMENT_VERTICAL_LEFT = 3;
	    public const int UI_ALIGNMENT_VERTICAL_CENTER = 4;
	    public const int UI_ALIGNMENT_VERTICAL_RIGHT = 5;

        public const string KEY_INFO_ID = "KEY_INFO_ID";
    }



    public static class BPG_UI
    {
        // 默认fairyGUI的设计分辨率
        public static int _FAIRY_GUI_WIDTH = 1334;
        public static int _FAIRY_GUI_HIGHT = 750;

        public static int FAIRY_GUI_WIDTH
        {
            get {
                return _FAIRY_GUI_WIDTH;
            }
        }

        public static int FAIRY_GUI_HIGHT
        {
            get {
                return _FAIRY_GUI_HIGHT;
            }
        }

        public static int MAKE_FULL_SCREEN_DESIGN_WIDTH
        {
            get
            {
                int screenWidth = UnityEngine.Screen.width;
                int designWith = (int)(screenWidth / BPG_UI.FairyGUIScale);
                return designWith;
            }
        }
        
        public static int MAKE_FULL_SCREEN_DESIGN_HEIGHT
        {
            get
            {
                int screenHeight = UnityEngine.Screen.height;
                int desginHeight = (int)(screenHeight / BPG_UI.FairyGUIScale);
                return desginHeight;
            }
        }


        /// <summary>
        /// fairyGUI的缩放率
        /// </summary>
        /// <value></value>
        public static float FairyGUIScale
        {
            get
            {
                return UIContentScaler.scaleFactor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static float GetFGUIWidthByRate(float rate)
        {
            return FAIRY_GUI_WIDTH * rate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static float GetFGUIHeightByRate(float rate)
        {
            return FAIRY_GUI_HIGHT * rate;
        }

        // 事件定义
        public const string BP_MSG_REFRESH_GEMS = "BP_MSG_REFRESH_GEMS";
        public const string BP_MSG_REFRESH_MONEY = "BP_MSG_REFRESH_MONEY";
        public const string BP_MSG_REFRESH_GEMS_AND_MONEY = "BP_MSG_REFRESH_GEMS_AND_MONEY";
        public const string BP_MSG_REFRESH_ITEM_NUM = "BP_MSG_REFRESH_ITEM_NUM";
        

        // uinty 包名
        public static string AB_NAME_TEXT = "text.unity3d";
        public static string AB_NAME_ITEM = "item.unity3d";
        public static string AB_NAME_SKILL = "skill.unity3d";
        public static string AB_NAME_BATTLE = "battle.unity3d";

        
        // 定义FairyGUI的包名
        public const string PACKET_NAME = "PixelGame";
        public const string PACKET_PATH = "Assets/Res/FairyGUI/PixelGame";

        // fairyGUI的Layers
        public const int UINITY_LAYER_UI = 5;
        public const int FAIRY_GUI_LAYER_INDEX = 12;

        // 所有scene的名字都定义在这里
        public const string SCENE_MINI_MAP = "BattleScene_MiniMap";

        // 所有FairyGUI的Component名字都定义在这里.
        public const string UI_TITLE_PANEl = "UI_Title";
    }
}
