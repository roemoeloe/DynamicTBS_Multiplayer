using UnityEngine;

public class SpriteManager
{
    // Characters
    public static Sprite BLUE_MASTER_SPRITE;
    public static Sprite PINK_MASTER_SPRITE;
    public static Sprite BLUE_TANK_SPRITE;
    public static Sprite PINK_TANK_SPRITE;
    public static Sprite BLUE_SHOOTER_SPRITE;
    public static Sprite PINK_SHOOTER_SPRITE;
    public static Sprite BLUE_RUNNER_SPRITE;
    public static Sprite PINK_RUNNER_SPRITE;
    public static Sprite BLUE_MECHANIC_SPRITE;
    public static Sprite PINK_MECHANIC_SPRITE;
    public static Sprite BLUE_MEDIC_SPRITE;
    public static Sprite PINK_MEDIC_SPRITE;
    
    // Tiles
    public static Sprite EMPTY_TILE_SPRITE;
    public static Sprite FLOOR_TILE_SPRITE;
    public static Sprite START_TILE_SPRITE;
    public static Sprite MASTER_START_TILE_SPRITE;
    public static Sprite GOAL_TILE_SPRITE;
    
    // UI
    public static Sprite ABILITY_CIRCLE_SPRITE;
    public static Sprite ATTACK_CIRCLE_SPRITE;
    public static Sprite MOVE_CIRCLE_SPRITE;
    public static Sprite ATTACK_ROW_DOWN_SPRITE;
    public static Sprite ATTACK_ROW_LEFT_SPRITE;
    public static Sprite ATTACK_ROW_RIGHT_SPRITE;
    public static Sprite ATTACK_ROW_UP_SPRITE;
    public static Sprite COOLDOWN_1_SPRITE;
    public static Sprite COOLDOWN_2_SPRITE;
    public static Sprite COOLDOWN_3_SPRITE;
    public static Sprite HP_1_SPRITE;
    public static Sprite HP_2_SPRITE;
    public static Sprite HP_3_SPRITE;
    public static Sprite HP_4_SPRITE;
    public static Sprite TANK_BLOCK_FRAME_SPRITE;

    public static void LoadSprites()
    {
        BLUE_MASTER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Master");
        PINK_MASTER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Master");
        BLUE_TANK_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Tank");
        PINK_TANK_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Tank");
        BLUE_SHOOTER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Shooter");
        PINK_SHOOTER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Shooter");
        BLUE_RUNNER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Runner");
        PINK_RUNNER_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Runner");
        BLUE_MECHANIC_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Mechanic");
        PINK_MECHANIC_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Mechanic");
        BLUE_MEDIC_SPRITE = Resources.Load<Sprite>("CharacterSprites/Blue_Medic");
        PINK_MEDIC_SPRITE = Resources.Load<Sprite>("CharacterSprites/Pink_Medic");

        EMPTY_TILE_SPRITE = Resources.Load<Sprite>("TileSprites/EmptyTile");
        FLOOR_TILE_SPRITE = Resources.Load<Sprite>("TileSprites/FloorTile");
        START_TILE_SPRITE = Resources.Load<Sprite>("TileSprites/StartTile");
        MASTER_START_TILE_SPRITE = Resources.Load<Sprite>("TileSprites/MasterStartTile");
        GOAL_TILE_SPRITE = Resources.Load<Sprite>("TileSprites/GoalTile");
        
        ABILITY_CIRCLE_SPRITE = Resources.Load<Sprite>("UI/AbilityCircle");
        ATTACK_CIRCLE_SPRITE = Resources.Load<Sprite>("UI/AttackCircle");
        MOVE_CIRCLE_SPRITE = Resources.Load<Sprite>("UI/MoveCircle");
        ATTACK_ROW_DOWN_SPRITE = Resources.Load<Sprite>("UI/AttackRowDown");
        ATTACK_ROW_LEFT_SPRITE = Resources.Load<Sprite>("UI/AttackRowLeft");
        ATTACK_ROW_RIGHT_SPRITE = Resources.Load<Sprite>("UI/AttackRowRight");
        ATTACK_ROW_UP_SPRITE = Resources.Load<Sprite>("UI/AttackRowUp");
        COOLDOWN_1_SPRITE = Resources.Load<Sprite>("UI/Cooldown_1");
        COOLDOWN_2_SPRITE = Resources.Load<Sprite>("UI/Cooldown_2");
        COOLDOWN_3_SPRITE = Resources.Load<Sprite>("UI/Cooldown_3");
        HP_1_SPRITE = Resources.Load<Sprite>("UI/HP_1");
        HP_2_SPRITE = Resources.Load<Sprite>("UI/HP_2");
        HP_3_SPRITE = Resources.Load<Sprite>("UI/HP_3");
        HP_4_SPRITE = Resources.Load<Sprite>("UI/HP_4");
        TANK_BLOCK_FRAME_SPRITE = Resources.Load<Sprite>("UI/Tank_BlockFrame");
    }
}