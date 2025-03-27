﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(8,null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(8);
    }

    public override void Update()
    {
        base.Update();

        // Set tốc độ di chuyển của nhân vật theo (x, y)
        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);


        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
