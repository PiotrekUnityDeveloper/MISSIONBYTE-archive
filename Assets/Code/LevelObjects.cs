using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjects
{
    //null
}

public class LevelProperties
{
    public int levelwidth { get; set; }
    public int levelheight { get; set; }

    //exit entrance rules
    public bool boxPuzzle { get; set; } = false;
    public int requiredBoxed { get; set; } = 1;
    public bool killPuzzle { get; set; } = false;
    public int requiredKills { get; set; } = 1;

}

public class LaserProjection
{
    public List<GameObject> topObj;
    public List<GameObject> toprightObj;
    public List<GameObject> topleftObj;
    public List<GameObject> bottomObj;
    public List<GameObject> bottomrightObj;
    public List<GameObject> bottomleftObj;
    public List<GameObject> rightObj;
    public List<GameObject> leftObj;
}

public enum RotationType //used by laser projection
{
    Top,
    TopRight,
    TopLeft,
    Bottom,
    BottomRight,
    BottomLeft,
    Right,
    Left
}

/// <summary>
/// LEVELOBJECTS
/// </summary>

public class LevelObject
{
    //main
    public GameObject parentgameobject { get; set; }

    public List<Dialog> objectDialogs { get; set; } = null;
    public bool walkable { get; set; } = false; // used for everything with dialog
    public bool walkableDialog { get; set; } = false; // used for Finish LevelObject (yea i know confusing as fuck)

    //shared
    public int positionindex { get; set; }
    public string displaycharacter { get; set; }
    public Color defaultcolor { get; set; } = Color.white;

    //properties
    public int xpos { get; set; }
    public int ypos { get; set; }
}

public class Player : LevelObject
{
    //credentials
    public string username { get; set; }
    public int id { get; set; }
}

public class PlayerItem : LevelObject { public Item item { get; set; } }

public class Wall : LevelObject { }
public class Crate : LevelObject { }
public class Finish : LevelObject
{
    public bool isOpen { get; set; }
    public int levelGate { get; set; }
    public bool nextLevel { get; set; } = false;

    //finish ID
    public int doorID { get; set; } = 0;
}

public class HoldButton : LevelObject
{
    public bool isActive { get; set; } = false;
}

public class RedEnemy : LevelObject
{
    public float health { get; set; }
    public float moveSpeed { get; set; }
    public float attackDamage { get; set; } = 3f;
    public int attackSpeed { get; set; } = 20; //the lower, the faster
}

public class LaserShooter : LevelObject
{
    public int laserCount { get; set; } = 4;
    public float rotationSpeed { get; set; } = 1.5f;
    public float laserReach { get; set; } = 999f;
    public Color laserColor { get; set; } = Color.red;
    public bool isStatic { get; set; } = false;
    public float laserDamage { get; set; } = 2.0f;
}