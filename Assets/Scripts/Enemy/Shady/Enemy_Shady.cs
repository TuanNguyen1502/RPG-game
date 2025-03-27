﻿using JetBrains.Annotations;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Enemy_Shady : Enemy
{

    [Header("Shady spesifics")]
    public float battleStateMoveSpeed;

    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;


    #region States

    // Truyền các trạng thái của nhân vật thông qua EnemyStateMachine
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyBattleState battleState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);

        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);

        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned",this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
    }

    protected override void Start()
    {
        base.Start();

        // State khởi tạo của quái vật khi bắt đầu
        stateMachine.Initialize(idleState);
    }

    // Gây choáng cho quái vật
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

    }
    protected override void Update()
    {
        base.Update();
    }

    // Kích hoạt tấn công explosive của shady
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, attackCheck.position, Quaternion.identity);
        newExplosive.GetComponent<Explosive_Controller>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    // Phá hủy object khi explosive
    public void SelfDestroy() => Destroy(gameObject);

}
