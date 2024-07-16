using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using Color = UnityEngine.Color;
using Unity.Burst.CompilerServices;

public class GameRenderer : MonoBehaviour
{
    public static GameRenderer gameRenderer;

    //Generation variables
    [SerializeField] public int boardWidth = 0;
    [SerializeField] public int boardHeight = 0;

    [SerializeField] private float itemXSpacing = 0;
    [SerializeField] private float itemYSpacing = 0;

    public GameObject /*prefab type*/ contentTile;
    [SerializeField] private Transform boardTileHolder; //set through inspector

    [HideInInspector] public static List<GameObject> boardTiles = new List<GameObject>();

    [HideInInspector] public List<LevelProperties> levelProperties = new List<LevelProperties>()
    {
        /*LEVEL 0*/new LevelProperties{ levelwidth = 22, levelheight = 11/*, boxPuzzle = true, requiredBoxed = 4*/ },
        /*LEVEL 1*/new LevelProperties{ levelwidth = 22, levelheight = 11 },
        /*LEVEL 2*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 3 },
        /*LEVEL 3*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 2 },
        /*LEVEL 4*/new LevelProperties{ levelwidth = 22, levelheight = 11, killPuzzle = true, requiredKills = 2 },
        /*LEVEL 5*/new LevelProperties{ levelwidth = 22, levelheight = 11, killPuzzle = true, requiredKills = 2 },
        /*LEVEL 6*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 2 },
        /*LEVEL 7*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 16 },
        /*LEVEL 8*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 2 },
        /*LEVEL 9*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 1 },
        /*LEVEL 10*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 4 },
        /*LEVEL 11*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 3 },
        /*LEVEL 12*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 2 },
        /*LEVEL 13*/new LevelProperties{ levelwidth = 22, levelheight = 11, killPuzzle = true, requiredKills = 5 },
        /*LEVEL 14*/new LevelProperties{ levelwidth = 22, levelheight = 11, boxPuzzle = true, requiredBoxed = 1 },
        new LevelProperties{ levelwidth = 22, levelheight = 11 } //the end
    };

    public List<GameObject> activeEnemies = new List<GameObject>();

    public void LoadLevelProperties(int levelID)
    {
        SetGenerationProperties(levelProperties[levelID].levelwidth, levelProperties[levelID].levelheight, 0, 0);
    }

    private void Awake()
    {
        gameRenderer = this.GetComponent<GameRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canMoveBoxes = false;
        canToggle = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGenerationProperties(int width, int height, float xspace, float yspace)
    {
        gameRenderer.boardWidth = width;
        gameRenderer.boardHeight = height;
        gameRenderer.itemXSpacing = xspace;
        gameRenderer.itemYSpacing = yspace;
    }

    [SerializeField] private bool relativeXCoordinates = false;
    [SerializeField] private bool relativeYCoordinates = true;

    public void GenerateLevel()
    {
        if(contentTile == null)
            return;

        foreach(GameObject g in activeEnemies)
        {
            Destroy(g);
        }

        activeEnemies.Clear();

        int currentItemIndex = 0;
        //initial
        int startX = 0;
        int startY = 0;
        //used
        int currentX = startX;
        int currentY = startY;
        float currentXPosition = 0; //dynamic

        if(boardTiles != null) {boardTiles.Clear();}

        for (int i = 0; i < gameRenderer.boardHeight; i++)
        {
            for (int j = 0; j < gameRenderer.boardWidth; j++)
            {
                GameObject tile = Instantiate(contentTile, new Vector2(currentXPosition, (currentY * this.contentTile.GetComponent<BoardTile>().Background.transform.localScale.y)), Quaternion.identity);
                tile.name = /*X-x-Y*/ j + "x" + i;
                tile.transform.parent = boardTileHolder.transform;
                tile.GetComponent<BoardTile>().Background.GetComponent<SpriteRenderer>().color = Color.black;
                tile.GetComponent<BoardTile>().ChangeChar('#');
                tile.GetComponent<BoardTile>().myXPos = j;
                tile.GetComponent<BoardTile>().myYPos = i;
                tile.GetComponent<BoardTile>().originalPosition = tile.transform.position;
                tile.gameObject.layer = LayerMask.NameToLayer("nocastShadow");
                tile.gameObject.SetActive(true);

                boardTiles.Add(tile);

                currentItemIndex++; //independent

                if (relativeXCoordinates)
                {
                    currentX -= 1;
                    currentXPosition = (currentX * (tile.GetComponent<BoardTile>().Background.transform.localScale.x/*width*/ * -1)) + (currentX * itemXSpacing);
                }
                else
                {
                    currentX += 1;
                    currentXPosition = (currentX * (tile.GetComponent<BoardTile>().Background.transform.localScale.x/*width*/)) + (currentX * itemXSpacing);
                }

                
                
            }

            currentX = 0;
            currentXPosition = 0;

            if (relativeYCoordinates)
            {
                currentY -= 1;
            }
            else
            {
                currentY += 1;
            }
        }

        
    }

    public void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("RedEnemy");

        foreach(GameObject g in enemies)
        {
            try{ activeEnemies.Remove(g); } catch { }
            Destroy(g);
        }
    }

    public void ResetBoardTilesPositions()
    {
        return;/*
        foreach(BoardTile bt in boardTiles)
        {
            bt.transform.position = bt.GetComponent
        }*/
    }

    public void GenerateEnemies()
    {
        this.activeEnemies.Clear();

        //spawn enemies prehand
        foreach (LevelObject lvlobj in LevelManager.localLevelObjects)
        {
            if (lvlobj.GetType() == typeof(RedEnemy))
            {
                GameObject g = GetGameObjectByTilePosition(lvlobj.xpos, lvlobj.ypos);
                GameObject enemy = Instantiate(redEnemy, g.transform.position, Quaternion.identity);
                enemy.SetActive(true);
                RedEnemy redEnemy1 = (RedEnemy)lvlobj;
                enemy.GetComponent<Enemy>().ChangeMoveSpeed(redEnemy1.moveSpeed);
                enemy.GetComponent<Enemy>().ChangeHP(redEnemy1.health);
                enemy.GetComponent<Enemy>().ChangeMovementStatus(true);
                enemy.GetComponent<Enemy>().atkdmg = ((RedEnemy)lvlobj).attackDamage;
                enemy.GetComponent<Enemy>().atkspeed = ((RedEnemy)lvlobj).attackSpeed;

                if (isRoomDark)
                {
                    enemy.GetComponent<Enemy>().ChangeMovementStatus(true);
                }
                else
                {
                    enemy.GetComponent<Enemy>().ChangeMovementStatus(false);
                }
                activeEnemies.Add(enemy);
                //LevelManager.localLevelObjects.Remove(lvlobj);
            }
        }
    }

    public void SwitchEnemiesState(bool canMove)
    {
        foreach (GameObject g in activeEnemies)
        {
            g.GetComponent<Enemy>().ChangeMovementStatus(canMove);
        }
    }

    public void RenderLevelWithDelay(float time) { StartCoroutine(RenderLevelWithDelayIEnum(time)); }
    private IEnumerator RenderLevelWithDelayIEnum(float time) { yield return new WaitForSeconds(time); RenderLevel(); }

    public bool isRoomDark = false;
    public bool renderLightMode = false;
    public bool isOn = true;

    public GameObject lightingRaycaster;

    public LayerMask castShadow;
    public LayerMask dontCastShadow;

    public float lightDistance;
    //light strenght settings

    public GameObject redEnemy;

    //public Path pathfinder;

    private bool canToggle = true;

    public bool canMoveBoxes = false;

    public void ToggleLight()
    {
        if(!canToggle)
            return;

        if (isOn)
        {
            //turn off
            isRoomDark = true;
            renderLightMode = true;
            isOn = false;
            canMoveBoxes = true;
            SwitchEnemiesState(true);
            RenderLevel();
            GameManager.soundManager.StopSound("ON_LOOP");
            GameManager.soundManager.PlaySound("OFF");
        }
        else if (!isOn)
        {
            //turn on
            isRoomDark = false;
            renderLightMode = false;
            isOn = true;
            canMoveBoxes = false;
            SwitchEnemiesState(false);
            RenderLevel();
            GameManager.soundManager.PlaySound("ON");
            StartCoroutine(DelayONLight());
        }

        GameManager.playerManager.UpdatePlayerStatus();

        StartCoroutine(DelayLightSwitch());

        //rerender level

        //RenderLevel();
    }

    private IEnumerator DelayLightSwitch()
    {
        canToggle = false;
        yield return new WaitForSeconds(2.5f);
        canToggle = true;
    }

    private IEnumerator DelayONLight()
    {
        yield return new WaitForSeconds(2.2f);
        GameManager.soundManager.PlaySound("ON_LOOP");
    }

    public int boxCheckDelay = 0;

    public void ResetLevelTiles()
    {
        foreach (GameObject bt in boardTiles)
        {
            if(bt.GetComponent<Laser>() != null)
            {
                Destroy(bt.GetComponent<Laser>());
            }
        }
    }

    public void RenderLevel()
    {
        boxCheckDelay++;

        foreach(Transform t in boardTileHolder)
        {
            t.gameObject.layer = LayerMask.NameToLayer("nocastShadow");
            t.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        foreach(GameObject bt in boardTiles)
        {
            bt.gameObject.layer = LayerMask.NameToLayer("nocastShadow");
            bt.gameObject.tag = "Untagged";
            /*
            if(bt.GetComponent<Laser>() != null)
            {
                Destroy(bt.GetComponent<Laser>());
            }*/

            if (bt.GetComponent<BoardTile>().pauseRender == true)
            {
                continue;
            }

            bt.GetComponent<BoardTile>().ChangeText("");
            bt.GetComponent<BoardTile>().ChangeTextColor(Color.white);
        }

        foreach(LevelObject lvlobj in LevelManager.localLevelObjects)
        {
            if(lvlobj.GetType() == typeof(Player) || lvlobj.GetType() == typeof(Wall) || lvlobj.GetType() == typeof(Crate) || lvlobj.GetType() == typeof(PlayerItem) || lvlobj.GetType() == typeof(Finish) || lvlobj.GetType() == typeof(HoldButton) || lvlobj.GetType() == typeof(LaserShooter))
            {
                GameObject g = GetGameObjectByTilePosition(lvlobj.xpos, lvlobj.ypos);

                if(lvlobj.GetType() == typeof(Player))
                {
                    lightingRaycaster.transform.position = g.transform.position;
                    g.gameObject.tag = "Player";
                    LevelManager.mainplayer.xpos = lvlobj.xpos;
                    LevelManager.mainplayer.ypos = lvlobj.ypos;
                }

                if(lvlobj.GetType() == typeof(LaserShooter))
                {
                    if(g.GetComponent<Laser>() == null)
                    {
                        Laser laser = g.AddComponent<Laser>();

                        laser.laserColor = ((LaserShooter)lvlobj).laserColor;
                        
                        laser.rotationSpeed = ((LaserShooter)lvlobj).rotationSpeed;
                        laser.rotationCut = ((LaserShooter)lvlobj).laserCount;
                        laser.laserReach = ((LaserShooter)lvlobj).laserReach;
                        laser.isStatic = ((LaserShooter)lvlobj).isStatic;
                        laser.laserDamage = ((LaserShooter)lvlobj).laserDamage;

                        laser.RunLaser();
                    }
                    else
                    {
                        if (g.GetComponent<Laser>().isStatic == true)
                        {
                            //render laser manually to damage player of possible >>:))

                            g.GetComponent<Laser>().UpdateManually();
                        }
                    }
                }

                if(lvlobj.GetType() != typeof(Player) && lvlobj.GetType() != typeof(PlayerItem) && lvlobj.GetType() != typeof(RedEnemy) && lvlobj.GetType() != typeof(HoldButton) && lvlobj.GetType() != typeof(LaserShooter))
                {
                    //make this object cast shadow
                    g.gameObject.layer = LayerMask.NameToLayer("castShadow");
                    g.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                }
                else
                {
                    g.gameObject.layer = LayerMask.NameToLayer("nocastShadow");
                }

                if(g.GetComponent<BoardTile>().pauseRender == true)
                    continue;

                g.GetComponent<BoardTile>().ChangeText(lvlobj.displaycharacter);

                if(isRoomDark == false)
                {
                    if (lvlobj.defaultcolor != null)
                        g.GetComponent<BoardTile>().ChangeTextColor(lvlobj.defaultcolor);
                }
                else //the room is dark
                {
                    g.GetComponent<BoardTile>().ChangeTextColor(Color.black);
                }
            }   //in case of invisible level objects leave it as it is :)
            

        }

        if(LevelManager.playerHeldItem != null)
        {
            GameObject g1 = GetGameObjectByTilePosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
            if (LevelManager.GetObjectInPosition(g1.GetComponent<BoardTile>().myXPos, g1.GetComponent<BoardTile>().myYPos, new LevelObject()) == null)
            {
                //tile empty, can render held item
                //if (g.GetComponent<BoardTile>().pauseRender == true)
                //continue;

                g1.gameObject.layer = LayerMask.NameToLayer("nocastShadow");

                g1.GetComponent<BoardTile>().ChangeText(LevelManager.playerHeldItem.displaycharacter);

                if(isRoomDark == false)
                {
                    if (LevelManager.playerHeldItem.defaultcolor != null)
                        g1.GetComponent<BoardTile>().ChangeTextColor(LevelManager.playerHeldItem.defaultcolor);
                }
                else
                {
                    g1.GetComponent<BoardTile>().ChangeTextColor(Color.black);
                }

            }
        }

        RenderLight();
        //StartCoroutine(DelayLightRender());

        AstarPath.active.Scan();

        if(boxCheckDelay >= 7)
        {
            UpdateBoxPlacement();
            boxCheckDelay = 0;
        }

    }

    public int buttonsactive = 0;
    public bool levelfinished = false;
    public void UpdateBoxPlacement()
    {
        if(levelfinished == true)
            return;

        List<ButtonPosition> buttons = new List<ButtonPosition>();

        buttonsactive = 0;

        foreach(LevelObject button in LevelManager.localLevelObjects)
        {
            if(button.GetType() == typeof(HoldButton))
            {
                buttons.Add(new ButtonPosition { buttonX = button.xpos, buttonY = button.ypos });
            }
        }

        foreach(ButtonPosition btnpos in buttons)
        {
            foreach(LevelObject box in LevelManager.localLevelObjects)
            {
                if(box.GetType() == typeof(Crate))
                {
                    if(box.xpos == btnpos.buttonX && box.ypos == btnpos.buttonY)
                    {
                        //print(box.xpos + ":" + btnpos.buttonX);
                        buttonsactive++;
                        break;
                    }
                }
            }

        }

        

        CheckForLevelCompletion();

    }

    public int killedEnemies = 0;

    public void CheckForLevelCompletion()
    {
        if (levelProperties[LevelManager.currentLevel].boxPuzzle == true)
        {
            if(levelProperties[LevelManager.currentLevel].requiredBoxed == buttonsactive)
            {
                //UNLOCK THE FINISH DOORS
                UnlockFinish();
            }
            else
            {
                //LOCK THE FINISH BACK
                //LockFinish();
            }
        }

        if (levelProperties[LevelManager.currentLevel].killPuzzle == true)
        {
            if (levelProperties[LevelManager.currentLevel].requiredKills == killedEnemies)
            {
                //UNLOCK THE FINISH DOORS
                UnlockFinish();
            }
        }

    }

    public void UnlockFinish()
    {
        foreach(LevelObject lvlobj in LevelManager.localLevelObjects)
        {
            if(lvlobj.GetType() == typeof(Finish))
            {
                if(((Finish)lvlobj).doorID == 0)
                {
                    ((Finish)lvlobj).isOpen = true;
                    //play sound
                    //dialog
                    levelfinished = true;
                    GameManager.dialogManager.RunCustomDialog("you hear a door opening sound...");
                }
            }
        }
    }

    public void LockFinish()
    {
        foreach (LevelObject lvlobj in LevelManager.localLevelObjects)
        {
            if (lvlobj.GetType() == typeof(Finish))
            {
                if (((Finish)lvlobj).doorID == 0)
                {
                    if(((Finish)lvlobj).isOpen == false)
                        return;

                    ((Finish)lvlobj).isOpen = false;
                    //play sound
                    //dialog
                    GameManager.dialogManager.RunCustomDialog("you hear a door closing somewhere...");
                }
            }
        }
    }

    private IEnumerator DelayLightRender()
    {
        yield return new WaitForSeconds(0.01f);
        RenderLight();
    }

    //public Pathfinding.Pathfinder pathfinder;

    public void RenderLight()
    {
        if (isRoomDark)
        {
            //render the lighting

            GameObject[] tilesToLight = GetWallRaycastAroundPlayer();

            foreach (GameObject g in tilesToLight)
            {
                if (g.GetComponent<BoardTile>() != null && LevelManager.GetObjectInPosition(g.GetComponent<BoardTile>().myXPos, g.GetComponent<BoardTile>().myYPos, new LevelObject()) != null)
                {
                    g.GetComponent<BoardTile>().ChangeTextColor(CalculateColor(LevelManager.GetObjectInPosition(g.GetComponent<BoardTile>().myXPos, g.GetComponent<BoardTile>().myYPos, new LevelObject()).defaultcolor, 10 * CalculatePercentage(GetDistanceToPlayer(g.transform), this.lightDistance)));
                    g.GetComponent<BoardTile>().ChangeTextColor(GetColorBasedOnDistance(LevelManager.GetObjectInPosition(g.GetComponent<BoardTile>().myXPos, g.GetComponent<BoardTile>().myYPos, new LevelObject()).defaultcolor, g));
                    //print("distance from player: " + 10 * CalculatePercentage(GetDistanceToPlayer(g.transform), this.lightDistance));
                }
            }
        }

        GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        //GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);

        //AstarPath.active.UpdateGraphs();
    }

    public float fadeDistance;
    public float maxDistance;
    public float minDistance;

    public Color GetColorBasedOnDistance(Color baseColor, GameObject tile)
    {
        GameObject targetObject = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);

        // Calculate the distance between this game object and the target object
        float distance = Vector3.Distance(tile.transform.position, targetObject.transform.position);

        // Calculate the fade factor based on the distance
        float fadeFactor = Mathf.Clamp01((distance - fadeDistance) / (maxDistance - fadeDistance));

        // Apply the faded color
        Color fadedColor = Color.Lerp(baseColor, Color.black, fadeFactor);

        // Check if the distance is beyond the max or min distance
        if (distance > maxDistance)
        {
            fadedColor = Color.black;
        }
        else if (distance < minDistance)
        {
            fadedColor = baseColor;
        }

        return fadedColor;
    }

    public void RenderLightAtPosition(int x, int y)
    {
        if (isRoomDark)
        {
            //render the lighting

            GameObject[] tilesToLight = GetWallRaycastAroundPlayer();

            foreach (GameObject g in tilesToLight)
            {
                if (g.GetComponent<BoardTile>() != null && LevelManager.GetObjectInPosition(g.GetComponent<BoardTile>().myXPos, g.GetComponent<BoardTile>().myYPos, new LevelObject()) != null)
                {
                    g.GetComponent<BoardTile>().ChangeTextColor(CalculateColor(LevelManager.GetObjectInPosition(g.GetComponent<BoardTile>().myXPos, g.GetComponent<BoardTile>().myYPos, new LevelObject()).defaultcolor, CalculatePercentage(GetDistanceToPlayer(g.transform), this.lightDistance)));
                }
            }
        }
    }

    public float GetDistanceToPlayer(Transform position)
    {
        return Vector3.Distance(GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).transform.position, position.position);
    }

    public static Color CalculateColor(Color baseColor, float percentage)
    {
        // Convert percentage to a decimal value
        percentage /= 100.0f;

        // Calculate the RGB values for the resulting color
        int red = (int)(baseColor.r * percentage);
        int green = (int)(baseColor.g * percentage);
        int blue = (int)(baseColor.b * percentage);

        //print(red + " " + green + " " + blue);

        // Return the resulting color as a Color struct
        return new Color(red, green, blue);
    }

    public static float CalculatePercentage(float part, float total)
    {
        return (part / total) * 100;
    }

    public LayerMask shadowLayers;

    public GameObject[] GetWallRaycastAroundPlayer()
    {
        List<GameObject> objectToLight = new List<GameObject>();

        int rayCount = GameManager.settingsManager.lightRenderQuality;  // Number of raycasts
        float radius = 5f;  // Radius of the circle

        float angleIncrement = 360f / rayCount;

        outerLoop: for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleIncrement;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            RaycastHit2D[] hit = Physics2D.RaycastAll(lightingRaycaster.transform.position, direction, lightDistance, shadowLayers);

            foreach(RaycastHit2D rchit in hit)
            {
                if (rchit.collider != null)
                {
                    if(rchit.collider.gameObject.layer == LayerMask.NameToLayer("castShadow"))
                    {
                        // hit
                        Debug.DrawLine(lightingRaycaster.transform.position, rchit.point, Color.red, 1);
                        objectToLight.Add(rchit.collider.gameObject);
                        break;
                    }
                    else if(rchit.collider.gameObject.layer == LayerMask.NameToLayer("nocastShadow"))
                    {
                        // hit
                        Debug.DrawLine(lightingRaycaster.transform.position, rchit.point, Color.red, 1);
                        objectToLight.Add(rchit.collider.gameObject);
                    }
                }
                else
                {
                    // didnt hit
                    Debug.DrawRay(lightingRaycaster.transform.position, direction * radius, Color.green);
                }
            }

        }

        return objectToLight.ToArray();

    }

    public void ProjectLasersOnGameObjects(GameObject laserProjector, int laserCount, float laserReach, int rotationIndex, Color laserColor, bool isStatic = false, float laserDamage = 2.0f)
    {
        LaserProjection lasers = GetWallRaycastAroundLaserPointer(laserProjector, laserCount, laserReach, rotationIndex, laserDamage);

        foreach(GameObject g in lasers.topObj)
        {
            g.GetComponent<BoardTile>().ChangeText("|");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("|", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.bottomObj)
        {
            g.GetComponent<BoardTile>().ChangeText("|");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("|", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.toprightObj)
        {
            g.GetComponent<BoardTile>().ChangeText("/");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("/", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.bottomleftObj)
        {
            g.GetComponent<BoardTile>().ChangeText("/");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("/", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.topleftObj)
        {
            g.GetComponent<BoardTile>().ChangeText(@"\");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo(@"\", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.bottomrightObj)
        {
            g.GetComponent<BoardTile>().ChangeText(@"\");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo(@"\", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.rightObj)
        {
            g.GetComponent<BoardTile>().ChangeText("-");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("-", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }

        foreach (GameObject g in lasers.leftObj)
        {
            g.GetComponent<BoardTile>().ChangeText("-");

            if (isStatic)
            {
                g.GetComponent<BoardTile>().ChangeTextColor(laserColor);
            }
            else
            {
                g.GetComponent<BoardTile>().ReplaceTextThenSetTo("-", "", 0.8f);
                g.GetComponent<BoardTile>().FlashText(laserColor, Color.black, 0.8f);
            }
        }
    }

    public LayerMask playerAndShadow;

    public LaserProjection GetWallRaycastAroundLaserPointer(GameObject origin, int _rayCount, float _radius, float startAnglePos, float laserDamage)
    {
        List<GameObject> top = new List<GameObject>();
        List<GameObject> topright = new List<GameObject>();
        List<GameObject> topleft = new List<GameObject>();
        List<GameObject> bottom = new List<GameObject>();
        List<GameObject> bottomright = new List<GameObject>();
        List<GameObject> bottomleft = new List<GameObject>();
        List<GameObject> right = new List<GameObject>();
        List<GameObject> left = new List<GameObject>();

        int rayCount = _rayCount;  // Number of raycasts
        float radius = _radius;  // Radius of the circle

        float angleIncrement = 360f / rayCount;

        int anglepos = (int)startAnglePos;

        outerLoop: for (int i = 0; i < rayCount; i++)
        {
            float angle = (i * angleIncrement) + /*(((360f / rayCount) * (anglepos)) / 2)*//*SHITanglepos*//*(((360f / rayCount) * (anglepos * (360f / rayCount))) / 8)*/0;
            //angle += ((360 / rayCount) * anglepos) / rayCount;
            //angle *= i;
            angle += CalculatePercentage(((360f / rayCount) * anglepos) / (rayCount / 2), 360f);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            RaycastHit2D[] hit = Physics2D.RaycastAll(origin.transform.position, direction, radius, playerAndShadow);

            foreach (RaycastHit2D rchit in hit)
            {
                if (rchit.collider != null)
                {
                    if (rchit.collider.gameObject.layer == LayerMask.NameToLayer("castShadow"))
                    {
                        /*
                        if (rchit.collider.gameObject.CompareTag("Player"))
                        {
                            GameManager.playerManager.DamagePlayer(laserDamage);
                            continue;
                        }*/

                        Debug.DrawLine(origin.transform.position, rchit.point, Color.red, 1);
                        //goto skiploop;
                        break;
                    }
                    else if (rchit.collider.gameObject.layer == LayerMask.NameToLayer("nocastShadow"))
                    {
                        if (rchit.collider.gameObject.CompareTag("Player") || rchit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            GameManager.playerManager.DamagePlayer(laserDamage);
                            continue;
                        }

                        if(rchit.collider.gameObject.GetComponent<BoardTile>().Text.GetComponent<TMP_Text>().text != "")
                        {
                            continue;
                        }

                        //determine type and add to correct list
                        RotationType rotType = GetSimiliarRotation(direction);

                        if(rotType == RotationType.Top)
                        {
                            top.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.TopRight)
                        {
                            topright.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.TopLeft)
                        {
                            topleft.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.Bottom)
                        {
                            bottom.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.BottomRight)
                        {
                            bottomright.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.BottomLeft)
                        {
                            bottomleft.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.Right)
                        {
                            right.Add(rchit.collider.gameObject);
                        }
                        else if (rotType == RotationType.Left)
                        {
                            left.Add(rchit.collider.gameObject);
                        }

                        Debug.DrawLine(origin.transform.position, rchit.point, Color.green, 1);
                    }
                    else if (rchit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        GameManager.playerManager.DamagePlayer(laserDamage);
                    }
                }
                else
                {
                    // didnt hit
                    Debug.DrawRay(origin.transform.position, direction * radius, Color.green);
                }
            }

            skiploop:;

        }

        return new LaserProjection { bottomleftObj = bottomleft, bottomrightObj = bottomright, topleftObj = topleft, toprightObj = topright, topObj = top, bottomObj = bottom, rightObj = right, leftObj = left};

    }

    

    public static RotationType GetSimiliarRotation(Vector2 vector)
    {
        float tolerance = 0.5f; // Adjust the tolerance as needed

        if (Mathf.Abs(vector.y) <= tolerance)
        {
            if (vector.x > 0)
            {
                return RotationType.Right;
            }
            else if (vector.x < 0)
            {
                return RotationType.Left;
            }
            else
            {
                return RotationType.Top; // Treat straight vectors as "Top"
            }
        }
        else if (Mathf.Abs(vector.x) <= tolerance)
        {
            if (vector.y > 0)
            {
                return RotationType.Top;
            }
            else
            {
                return RotationType.Bottom;
            }
        }
        else if (vector.y > 0 && vector.x > 0)
        {
            return RotationType.TopRight;
        }
        else if (vector.y > 0 && vector.x < 0)
        {
            return RotationType.TopLeft;
        }
        else if (vector.y < 0 && vector.x > 0)
        {
            return RotationType.BottomRight;
        }
        else
        {
            return RotationType.BottomLeft;
        }
    }

    public static RotationType GetRotation(Vector2 vector)
    {
        if (vector.y > 0 && vector.x == 0)
        {
            return RotationType.Top;
        }
        else if (vector.y > 0 && vector.x > 0)
        {
            return RotationType.TopRight;
        }
        else if (vector.y > 0 && vector.x < 0)
        {
            return RotationType.TopLeft;
        }
        else if (vector.y < 0 && vector.x == 0)
        {
            return RotationType.Bottom;
        }
        else if (vector.y < 0 && vector.x > 0)
        {
            return RotationType.BottomRight;
        }
        else if (vector.y < 0 && vector.x < 0)
        {
            return RotationType.BottomLeft;
        }
        else if (vector.y == 0 && vector.x > 0)
        {
            return RotationType.Right;
        }
        else
        {
            return RotationType.Left;
        }
    }

    public void RenderSpecificPosition(int x, int y)
    {
        foreach (GameObject bt in boardTiles)
        {
            if(bt.GetComponent<BoardTile>().myXPos == x && bt.GetComponent<BoardTile>().myYPos == y)
            {
                bt.gameObject.layer = LayerMask.NameToLayer("nocastShadow");

                if (bt.GetComponent<BoardTile>().pauseRender == true)
                {
                    //continue;
                }

                bt.GetComponent<BoardTile>().ChangeText("");
                bt.GetComponent<BoardTile>().ChangeTextColor(Color.white);
                break;
            }
        }

        bool isPlayer = false;

        foreach (LevelObject lvlobj in LevelManager.localLevelObjects)
        {
            if(lvlobj.xpos == x && lvlobj.ypos == y)
            {
                GameObject g = GetGameObjectByTilePosition(lvlobj.xpos, lvlobj.ypos);

                if (lvlobj.GetType() != typeof(Player) || lvlobj.GetType() != typeof(PlayerItem))
                {
                    //make this object cast shadow
                    g.gameObject.layer = LayerMask.NameToLayer("castShadow");
                }

                if(lvlobj.GetType() == typeof(Player) || lvlobj.GetType() == typeof(PlayerItem))
                {
                    isPlayer = true;
                }

                if(lvlobj.GetType() == typeof(Player))
                {
                    g.gameObject.layer = LayerMask.NameToLayer("Player");
                }

                g.GetComponent<BoardTile>().ChangeText(lvlobj.displaycharacter);
                if (lvlobj.defaultcolor != null)
                g.GetComponent<BoardTile>().ChangeTextColor(lvlobj.defaultcolor);
                break;
            }
        }

        if(isPlayer == false)
        {
            RenderLightAtPosition(x, y);
        }

        //AstarPath.active.Scan();

    }

    public static void RecolorAllTiles(Color color)
    {
        foreach (GameObject bt in boardTiles)
        {
            bt.GetComponent<BoardTile>().Text.GetComponent<TMP_Text>().color = color;

        }
    }

    //UTIL

    public GameObject GetGameObjectByTilePosition(int xPos, int yPos)
    {
        foreach(GameObject g in boardTiles)
        {
            if(g.gameObject.name == xPos + "x" + yPos)
            {
                return g;
            }
        }

        return null;
    }

    public bool IsPositionOutOfBounds(int x, int y)
    {
        if(x > this.boardWidth - 1)
        {
            //out of bounds
            return true;
        }
        else if(x < 0)
        {
            //out of bounds
            return true;
        }
        /// == Y ==
        else if(y > this.boardHeight - 1)
        {
            //out of bounds
            return true;
        }
        else if(y < 0)
        {
            //out of bounds
            return true;
        }

        return false;
    }

    public bool IsPositionWalkable(int x, int y)
    {
        // OUT OF BORDER TEST
        
        if(IsPositionOutOfBounds(x, y))
        {
            return false;
        }
        else
        {
            // OCCUPATION TEST
            //check for other levelobject in the same position

            foreach(LevelObject lo in LevelManager.localLevelObjects)
            {
                //check for position
                if(lo.xpos == x && lo.ypos == y)
                {
                    //check for element type - player can walk on certain ones
                    if(lo.GetType() == typeof(Player) && lo.walkable == false)
                    {
                        //cant walk lol
                        return false;
                    }
                    else if(lo.GetType() == typeof(Wall) && lo.walkable == false)
                    {
                        //cant walk also
                        return false;
                    }
                    else if (lo.GetType() == typeof(Finish) && ((Finish)lo).isOpen == false)
                    {
                        //cant walk also
                        return false;
                    }
                    else if(lo.GetType() == typeof(LaserShooter) && ((LaserShooter)lo).walkable == false)
                    {
                        return false;
                    }
                    else if(lo.walkable == true) { return true; } 
                }
            }

            return true;
        }



    }


    public bool IsPositionADialog(int x, int y)
    {
        // OUT OF BORDER TEST

        if (IsPositionOutOfBounds(x, y))
        {
            return false;
        }
        else
        {
            // OCCUPATION TEST
            //check for other levelobject in the same position

            foreach (LevelObject lo in LevelManager.localLevelObjects)
            {
                //check for position
                if (lo.xpos == x && lo.ypos == y)
                {
                    if(lo.objectDialogs == null)
                        return false;

                    //check for element type - player can walk on certain ones
                    if (lo.objectDialogs.Count <= 0)
                    {
                        //no dialogs
                        return false;
                    }
                    else
                    {
                        //trigger the dialog
                        if(lo.GetType() != typeof(Finish))
                        {
                            //GameManager.dialogManager.AddDialogsToQueueAndDeploy(lo.objectDialogs);
                        }                        

                        if(lo.GetType() == typeof(PlayerItem))
                        {
                            //GameManager.itemManager.GiveItemToPlayer(((PlayerItem)lo).item);
                            GameManager.dialogManager.AddItemsToGiveAfterDialog(new List<Item> { ((PlayerItem)lo).item } );
                            GameManager.dialogManager.AddLevelObjectsToRemoveAfterDialog(new List<LevelObject> { lo });
                        }
                        else if(lo.GetType() == typeof(Finish))
                        {
                            if(((Finish)lo).isOpen == false)
                            {
                                GameManager.dialogManager.AddDialogsToQueueAndDeploy(lo.objectDialogs);
                            }
                        }

                        //trigger the dialog
                        if (lo.GetType() != typeof(Finish))
                        {
                            GameManager.dialogManager.AddDialogsToQueueAndDeploy(lo.objectDialogs);
                        }

                        return true;
                    }
                }
            }

            return false;
        }



    }

    public bool IsDialogWalkable(int x, int y)
    {
        // OUT OF BORDER TEST

        if (IsPositionOutOfBounds(x, y))
        {
            return false;
        }
        else
        {
            // OCCUPATION TEST
            //check for other levelobject in the same position

            foreach (LevelObject lo in LevelManager.localLevelObjects)
            {
                //check for position
                if (lo.xpos == x && lo.ypos == y)
                {
                    //check for element type - player can walk on certain ones
                    if (lo.objectDialogs.Count <= 0)
                    {
                        //no dialogs
                        return false;
                    }
                    else
                    {
                        if(lo.walkable == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                            
                    }
                }
            }

            return false;
        }



    }




    public LevelObject objectAtPosition;
    public bool IsObjectAtPosition(int x, int y, LevelObject objectType)
    {
        foreach (LevelObject lo in LevelManager.localLevelObjects)
        {
            //check for position
            if (lo.xpos == x && lo.ypos == y)
            {
                //check for element type - player can walk on certain ones
                if (lo.GetType() == objectType.GetType())
                {
                    this.objectAtPosition = lo;
                    return true; //object found!
                }
            }
        }

        return false; //not found
    }

}

public class ButtonPosition
{
    public int buttonX;
    public int buttonY;
    public int boxX;
    public int boxY;
}
