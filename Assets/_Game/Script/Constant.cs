using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    // tag
    public const string TAG_PLAYER = "Player";
    public const string TAG_BOT = "Bot";

    // layer
    public const string LAYER_PLAYER = "Player";
    public const string LAYER_ENEMY = "Enemy";
    public const string LAYER_INGORNE = "Ignore Raycast";

    //Axis
    public const string AXIS_HORIZONTAL = "Horizontal";
    public const string AXIS_VETICAL = "Vertical";

    // animation state name
    public const string ANIMSTATE_SHOOT = "Shooting";
    public const string ANIMSTATE_DRAWING = "DrawingWeapon";


    // animation UI
    public const string ANIM_START = "Start";
    public const string ANIM_END = "End";

    // animation character
    public const string ANIM_NONE = "None";
    public const string ANIM_SPEED = "Speed";
    public const string ANIM_IDLE = "Idle";
    public const string ANIM_WIN = "Win";
    public const string ANIM_LOSE = "Lose";
    public const string ANIM_DIE = "Die";
    public const string ANIM_SHOT = "Shot";
    public const string ANIM_DRAWING = "DrawingWeapon";
    public const string ANIM_SKILL = "Skill";

    // enemy state machine
    public static IState<Enemy> ENEMY_STATE_FIND = new FindStateEnemy();
    public static IState<Enemy> ENEMY_STATE_ATTACK = new AttackStateEnemy();

    // enemy name
    public static string[] ENEMY_NAME = new string[] { "Lol", "Linh", "Thanh", "Ayaya", "Asus", "GG", "PhapSuTrungHoa", "***" };

    // material property
    public const string MAT_MAINCOLOR = "_MainColor";

    // Weapon shop title
    public const string TITLE_INVENTORY_SWORD = "SWORD INVENTORY";
    public const string TITLE_INVENTORY_HAMMER = "HAMMER INVENTORY";
    public const string TITLE_INVENTORY_GUN = "GUN INVENTORY";

    // Notification
    public const string NOTE_ENOUGH_MONEY = "Are you sure to buy this item ?";
    public const string NOTE_NOT_ENOUGH_MONEY = "Sorry you don't have enough coin !";
}

[System.Serializable]
public class AnimWeaponData
{
    [field: SerializeField] public AnimationClip CharShootClip { get; private set; }
    [field: SerializeField] public AnimationClip CharPullOutClip { get; private set; }
    [field: SerializeField] public float MultiSpeed { get; private set; }
    [field: SerializeField] public float DelaySpawnBullet { get; private set; }
}

[System.Serializable]
public class VFXPrefab
{
    public VFXSpawn vfxObj;
    public AudioClip soundEffect;
}


public enum ELight
{
    Day = 0,
    Night = 1
}

public enum EGameState
{
    None = 0,
    MainMenu = 1,
    Setting = 2,
    Victory = 3,
    Fail = 4,
    GamePlay = 5,
    Inventory = 6
}
public enum ECharacterColor
{
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
    Black = 5,
    Pink = 6
}

public enum EItemType
{
    Sword = 0,
    Hammer = 1,
    Gun = 2,
    Char = 3,
    Hat = 4,
    Sheild = 5,
    Skill = 6
}

public enum EPooling
{
    None = 0,

    CombatPlayer = 10,
    CombatEnemy = 11,

    BuffSpeed = 20,
    BuffRange = 21,
    BuffSkill1 = 22,
    BuffSkill2 = 23,
    BuffSkill3 = 24,

    MeshSword1 = 60,
    MeshSword2 = 61,
    MeshSword3 = 62,

    MeshHammer1 = 70,
    MeshHammer2 = 71,
    MeshHammer3 = 72,

    MeshGun1 = 80,
    MeshGun2 = 81,
    MeshGun3 = 82,

    CharHoshino = 90,
    CharHaruka = 91,
    CharWakamo = 92,

    IndicatorArrow = 120,

    IconItem = 130,

    VFXLevelUp = 140,

    Hat1 = 160,
    Hat2 = 161,
    Hat3 = 162,
    Hat4 = 163,
    Hat5 = 164,
    Hat6 = 165,
    Hat7 = 166,
    Hat8 = 167,
    Hat9 = 168,
    Hat10 = 169,

    Sheild1 = 180,
    Sheild2 = 181,

    Skill1 = 190,
    Skill2 = 191,
    Skill3 = 192
}
