using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGGame : MonoBehaviour
{
    // public MGTeam _gTeamManager;
    // public MGStage _gStageManager;
    // public MGMinion _gMinionManager;
    // public MGHero.MGHero _gHeroManager;

    List<CONEntity> heroConList = new List<CONEntity>();

    void Awake()
    {
        GameSceneClass.gMGGame = this;

        // GameSceneClass._gColManager = new MGUCCollider2D();

        // _gTeamManager = new MGTeam();
        // _gStageManager = new MGStage();
        // _gMinionManager = new MGMinion();
        // _gHeroManager = new MGHero.MGHero();

        // Global._gameStat = eGameStatus.Playing;

        GameObject.Instantiate(Global.prefabsDic[ePrefabs.MainCamera]);

        heroConList.Clear();
    }

    void OnEnable()
    {
        // GamePlayData.init();
        // MGGameStatistics.instance.initData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CONEntity heroCon = GameSceneClass.gMGPool.CreateObj(ePrefabs.HeroGirl, Random.insideUnitCircle);
            heroConList.Add(heroCon);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (heroConList.Count > 0)
            {
                heroConList[heroConList.Count - 1].SetActive(false);
                heroConList.RemoveAt(heroConList.Count - 1);
            }

        }



        // if (Global._gameStat == eGameStatus.Playing)
        // {
        //     if (Global._gameMode == eGameMode.Collect)
        //     {
        //         _gStageManager.UpdateCollect();
        //         _gMinionManager.UpdateCollect();
        //     }
        //     else if(Global._gameMode == eGameMode.Adventure)
        //     {
        //         _gStageManager.UpdateAdventure();
        //         _gMinionManager.UpdateAdventure();
        //         _gHeroManager.UpdateAdventure();
        //     }
        // }
    }

    void LateUpdate()
    {
        // GameSceneClass._gColManager.LateUpdate();
    }
}
