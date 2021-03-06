using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerChar : Character
{
    public RunnerChar(Player side) : base(side)
    {
        this.maxHitPoints = 1;
        this.moveSpeed = 2;
        this.attackRange = 1;

        this.activeAbility = new JumpAA();
        this.passiveAbility = new HighPerformancePA();

        this.characterSprite = side.GetPlayerType() == PlayerType.blue ? SpriteManager.BLUE_RUNNER_SPRITE : SpriteManager.PINK_RUNNER_SPRITE;

        Init();
    }
}