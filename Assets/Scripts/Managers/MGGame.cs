using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGGame : MonoBehaviour
{
    // public MGTeam _gTeamManager;
    // public MGStage _gStageManager;
    // public MGMinion _gMinionManager;
    // public MGHero.MGHero _gHeroManager;

    //sList<CONEntity> heroConList = new List<CONEntity>();

    int waveCount = 1;
    public int WaveCount
    {
        get
        {
            return waveCount;
        }
        set
        {
            waveCount = value;
            GameSceneClass.gUiRootGame.SetWaveText(waveCount.ToString());
        }
    }
    List<int> waveInfo = new List<int>();

    List<CONEnemy> enemyConList = new List<CONEnemy>();
    public CONHeroMan man;
    public CONHeroGirl girl;
    CONCastle castle;

    void Awake()
    {
        GameSceneClass.gMGGame = this;

        WaveCount = 1;

        waveInfo.Add(-1);

        for (int i = 1; i < 15; i++)
        {
            waveInfo.Add(i * 7);
        }

        // GameSceneClass._gColManager = new MGUCCollider2D();

        // _gTeamManager = new MGTeam();
        // _gStageManager = new MGStage();
        // _gMinionManager = new MGMinion();
        // _gHeroManager = new MGHero.MGHero();

        // Global._gameStat = eGameStatus.Playing;

        GameObject.Instantiate(Global.prefabsDic[ePrefabs.MainCamera]);
        castle = Instantiate(Global.prefabsDic[ePrefabs.Castle]).GetComponent<CONCastle>();

        

        //heroConList.Clear();
        enemyConList.Clear();
    }

    private void Start()
    {
        man = GameSceneClass.gMGPool.CreateObj(ePrefabs.HeroMan, Random.insideUnitCircle) as CONHeroMan;
        girl = GameSceneClass.gMGPool.CreateObj(ePrefabs.HeroGirl, Random.insideUnitCircle) as CONHeroGirl;

    }

    void OnEnable()
    {
        // GamePlayData.init();
        // MGGameStatistics.instance.initData();
    }
    public void GameOver()
    {
        GameSceneClass.gUiRootGame.ChangeImg(true);
        enemyConList.ForEach(x => x.gameObject.SetActive(false));
        enemyConList.Clear();
    }
    public void RemoveEnemy(CONEnemy enemy)
    {
        if(enemyConList.Contains(enemy))
        {
            enemyConList.Remove(enemy);
        }
    }

    public void EnemySpawn()
    {
        CONEnemy enemyCon = GameSceneClass.gMGPool.CreateObj(ePrefabs.Enemy1, Random.insideUnitCircle) as CONEnemy;
        enemyCon.DieEvent += () =>
        {
            RemoveEnemy(enemyCon);
            if (enemyConList.Count <= 0)
            {
                //Å¬¸®¾î
                GameSceneClass.gUiRootGame.ChangeImg(true);
                if (!(WaveCount >= waveInfo.Count - 1))
                {
                    WaveCount++;
                }
            }
        };
        enemyConList.Add(enemyCon);
    }

    public IEnumerator WaveStart()
    {
        //for (int i = 0; i < waveInfo[waveCount]; i++)
        //{
        //    EnemySpawn();
        //}
        int idx = waveInfo[waveCount];
        while (idx > 0)
        {
            if(idx < 3)
            {
                for (int i = 0; i < idx; i++)
                {
                    EnemySpawn();
                }
                break;
            }
            else
            {
                int i = Random.Range(0, 4);
                for (int j = 0; j < i; j++)
                {
                    EnemySpawn();
                }
                idx -= i;
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void EnemyBack()
    {
        
        for (int i = 0; i < enemyConList.Count; i++)
        {
            enemyConList[i].myTrm.Translate(3, 0, 0);
        }
    }

    //public void ManSkillButton()
    //{
    //    if (man != null)
    //    {
    //        man.Back();
    //        print("back");
    //    }
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //ManSkillButton();

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
