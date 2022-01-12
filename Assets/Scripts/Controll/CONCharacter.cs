using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONCharacter : CONEntity
{
    // 캐릭터가 가지고 있는 고유 스탯 선언
    // FSM, Detect 기능 등
    // 고유 캐릭터 스탯 데이터
    // 애니메이션 정보

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
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
