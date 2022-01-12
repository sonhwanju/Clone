using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global
{
    public static Dictionary<ePrefabs, GameObject> prefabsDic;
    public static Dictionary<string, Sprite> spritesDic;

    public static Vector2 referenceResolution;
    public static Image blackPannel;
}

public enum ePrefabs
{
    None = -1,
    MainCamera,
    HEROS = 1000,
    HeroMan,
    HeroGirl,
    MANAGERS = 2000,
    MGPool,
    MGGame,
    UI = 3000,
    UIRoot,
    UIRootLoading,
    UIRootTitle,
    UIRootGame,
}

public enum eSceneName
{
    None =-1,
    Loading,
    Title,
    Game,     
}

public class GameSceneClass
{
    public static MGGame gMGGame;
    public static MGPool gMGPool;
    public static UIRootGame gUiRootGame;
}
