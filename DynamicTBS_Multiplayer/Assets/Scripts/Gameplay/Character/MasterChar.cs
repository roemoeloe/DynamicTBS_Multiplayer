using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterChar : Character
{
    public MasterChar(Player side) : base(side)
    {
        this.maxHitPoints = 3;
        this.moveSpeed = 1;
        this.attackRange = 0;

        this.activeAbility = new TakeControlAA();
        this.passiveAbility = new InfluenceAuraPA();

        this.characterSprite = side.GetPlayerType() == PlayerType.blue ? SpriteManager.BLUE_MASTER_SPRITE : SpriteManager.PINK_MASTER_SPRITE;

        Init();
    }
}