using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONEnemy : CONEntity
{
    public event Action DieEvent;

    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
        }
    }
    private const int MAX_HP = 30;

    private const int MOVE_SPEED = 3;

    public bool isCastleEnter = false;
    private bool isOnce = false;
    private bool isDie = false;

    public Vector3 originPos;

    private CONCastle castle;

    public override void Awake()
    {
        originPos = transform.position;
        base.Awake();
    }

    public override void OnEnable()
    {
        hp = MAX_HP;
        isDie = false;
        isOnce = false;
        isCastleEnter = false;
        DieEvent = null;
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

    public override void Update()
    {
        if(!isCastleEnter)
        {
            myTrm.Translate(Vector3.left * MOVE_SPEED * Time.deltaTime);
        }
        else
        {
            if(!isOnce)
            {
                isOnce = true;
                StartCoroutine(Attack());
            }
            
        }

        base.Update();
    }

    IEnumerator Attack()
    {
        while (!isDie)
        {
            castle.Hp -= 80;
            yield return new WaitForSeconds(1f);
        }
    }

    public void Die()
    {
        isDie = true;
        DieEvent?.Invoke();

        gameObject.SetActive(false);
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.gameObject.CompareTag("Castle"))
    //    {
    //        //col.gameObject.GetComponent<>
    //        isCastleEnter = true;
    //        print("enter");
    //    }
    //}

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Castle"))
        {
            isCastleEnter = true;
            castle = col.GetComponent<CONCastle>();
            //Die();
        }
    }
}
