using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONCastle : CONEntity
{
    private int _hp;
    public int Hp
    {
        get 
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(GameSceneClass.gUiRootGame != null)
            {
                GameSceneClass.gUiRootGame.SetHpBar((float)_hp / MAXHP,_hp.ToString());
            }
            if(_hp <= 0)
            {
                Hp = MAXHP;
                GameSceneClass.gMGGame.GameOver();
                GameSceneClass.gUiRootGame.XBtnInit();
            }
        }
    }
    const int MAXHP = 850;

    public override void Awake()
    {
        
        base.Awake();
    }

    public override void Start()
    {
        //Hp = MAXHP;
    }

    public override void OnEnable()
    {
        Hp = MAXHP;
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void cleanUpOnDisable()
    {

    }

    protected override void firstUpdate()
    {
        base.firstUpdate();
    }

    
}
