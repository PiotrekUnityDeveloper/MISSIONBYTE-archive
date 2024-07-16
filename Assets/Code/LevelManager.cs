using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static List<LevelObject> localLevelObjects = new List<LevelObject>();
    public static List<Player> localPlayers = new List<Player>();
    public static Player mainplayer;

    public static PlayerItem playerHeldItem;

    //other level elements
    public static List<Wall> localWalls = new List<Wall>();
    

    
    // ====================================================================
    //                G A M E    L E V E L S
    // ====================================================================
    public static List<List<LevelObject>> gameLevels = new List<List<LevelObject>>();

    public static int currentLevel = 0;

    //level 1
    public static List<LevelObject> level0 = new List<LevelObject>();
    //level 2
    public static List<LevelObject> level1 = new List<LevelObject>();
    //level 3
    public static List<LevelObject> level2 = new List<LevelObject>();
    //level 4
    public static List<LevelObject> level3 = new List<LevelObject>();
    //level 5
    public static List<LevelObject> level4 = new List<LevelObject>();
    //level 6
    public static List<LevelObject> level5 = new List<LevelObject>();
    //level 7
    public static List<LevelObject> level6 = new List<LevelObject>();
    //level 8
    public static List<LevelObject> level7 = new List<LevelObject>();
    //level 9
    public static List<LevelObject> level8 = new List<LevelObject>();
    //level 10
    public static List<LevelObject> level9 = new List<LevelObject>();
    //level 11
    public static List<LevelObject> level10 = new List<LevelObject>();
    //level 12
    public static List<LevelObject> level11 = new List<LevelObject>();
    //level 13
    public static List<LevelObject> level12 = new List<LevelObject>();
    //level 14
    public static List<LevelObject> level13 = new List<LevelObject>();
    //level 15
    public static List<LevelObject> level14 = new List<LevelObject>();
    //level 16
    public static List<LevelObject> level15 = new List<LevelObject>();


    private void Awake()
    {
        playerHeldItem = new PlayerItem { displaycharacter = " ", defaultcolor = Color.white, xpos = 0, ypos = 0 };

        level0 = new List<LevelObject>() {
            new Player{ xpos = 2, ypos = 2, displaycharacter = "@", defaultcolor = Color.yellow },
            //dialogs
            new Wall{ xpos = 19, ypos = 2, displaycharacter = "!", defaultcolor = Color.white, objectDialogs = new List<Dialog>() { new Dialog{ dialogText = "Welcome to <color=green><b>MISSIONBYTE</b></color>." } , 
                new Dialog{ dialogText = "You are a full-time hacker, and you have been choosen to work on a very important task."},
                new Dialog{ dialogText = "Many professionals tried to crack this system already. No one succeed. This is your chance to make some good money."},
                new Dialog{ dialogText = "This software, you developed like a year ago makes it a lotta easier to DDOS and HACK other devices."},
                new Dialog{ dialogText = "SUCH a good thing you didnt make it public, huh? :)"},
                new Dialog{ dialogText = "Okay, please proceed to to the door (namely, a yellow circle) to send traffic and try to crash the target machine."},
                new Dialog{ dialogText = "This group is considered the best in the world, so i guess they are pretty secure...  Just try to bypass their security systems, ok?"},
                new Dialog{ dialogText = "Good luck then."},
                } 
            },
            //walls
            new Wall{ xpos = 1, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 2, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 3, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            //new Wall{ xpos = 8, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 15, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 16, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 19, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 20, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            //new Wall{ xpos = 4, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 8, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 9, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 10, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 11, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 12, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 13, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 15, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 16, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            //new Wall{ xpos = 2, ypos = 2, displaycharacter = "#", defaultcolor = Color.white },
            /*new HoldButton { xpos = 7, ypos = 7, displaycharacter = "%", defaultcolor = new Color(0.1f, 0.1f, 0.1f) },
            new HoldButton { xpos = 8, ypos = 7, displaycharacter = "%", defaultcolor = new Color(0.1f, 0.1f, 0.1f) },
            new HoldButton { xpos = 9, ypos = 7, displaycharacter = "%", defaultcolor = new Color(0.1f, 0.1f, 0.1f) },
            new HoldButton { xpos = 10, ypos = 7, displaycharacter = "%", defaultcolor = new Color(0.1f, 0.1f, 0.1f) },
            new Crate { xpos = 3, ypos = 3, displaycharacter = "%", defaultcolor = Color.gray },
            new Crate { xpos = 4, ypos = 3, displaycharacter = "%", defaultcolor = Color.gray },
            new Crate { xpos = 5, ypos = 6, displaycharacter = "%", defaultcolor = Color.gray },
            new Crate { xpos = 6, ypos = 6, displaycharacter = "%", defaultcolor = Color.gray },*/
            /*new LaserShooter { xpos = 8, ypos = 8, laserCount = 4, laserDamage = 2f, laserColor = Color.red, laserReach = 999, isStatic = false, rotationSpeed = 1.2f, displaycharacter = "X", defaultcolor = Color.red, walkable = false },*/
            new Finish { xpos = 11, ypos = 7, displaycharacter = "O", defaultcolor = Color.yellow, isOpen = true, nextLevel = true, walkable = true, objectDialogs = new List<Dialog>() { new Dialog { dialogText = "this door is locked" } }, walkableDialog = false },
            new Finish { xpos = 10, ypos = 7, displaycharacter = "O", defaultcolor = Color.yellow, isOpen = true, nextLevel = true, walkable = true, objectDialogs = new List<Dialog>() { new Dialog { dialogText = "this door is locked" } }, walkableDialog = false },
            };

        level1 = new List<LevelObject>() {
            new Player{ xpos = 11, ypos = 7, displaycharacter = "@", defaultcolor = Color.yellow },
            //walls
            //opening
            new Wall{ xpos = 9, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 10, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 11, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 9, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 12, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 13, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 15, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            //new Wall{ xpos = 19, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 20, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 21, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 12, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 8, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 3, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 2, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 1, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Finish{ xpos = 1, ypos = 8, displaycharacter = "O", defaultcolor = Color.yellow, isOpen = true, nextLevel = true, walkable = true, objectDialogs = new List<Dialog>() { new Dialog { dialogText = "this door is locked" } }, walkableDialog = false  },
            //room1 walls
            new Wall{ xpos = 7, ypos = 1, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 2, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Finish{ xpos = 6, ypos = 2, displaycharacter = "O", defaultcolor = Color.grey, isOpen = false, nextLevel = false, doorID = 0, walkable = true, objectDialogs = new List<Dialog>() { new Dialog { dialogText = "this door is locked" } }, walkableDialog = false },
            new Wall{ xpos = 7, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 2, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 1, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            //room2 walls
            new Wall{ xpos = 14, ypos = 1, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 15, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 16, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 19, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 20, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 2, displaycharacter = "{}", defaultcolor = Color.blue, objectDialogs = new List<Dialog>(){ new Dialog{ dialogText = "This is some sort of a machine.\n \nTake a closer look?", isYesNo = true, ifYes = new List<Dialog>{ new Dialog{ dialogText = "- You take a closer look at the machine \nIt looks like a printer with input and output. There's a piece of paper right on it."}, new Dialog{ dialogText = "It says: \"dns_get_record_attack; isathreat:yes; filter:yes; type:dns_abuse;\"" } }, ifNo = new List<Dialog>(){ new Dialog{ dialogText = "Let's not look at it." } } } } },
            ///new Crate { xpos = 3, ypos = 3, displaycharacter = "%", defaultcolor = Color.gray },
            ///new Crate { xpos = 4, ypos = 3, displaycharacter = "%", defaultcolor = Color.gray },
            ///new Crate { xpos = 5, ypos = 6, displaycharacter = "%", defaultcolor = Color.gray },
            ///new Crate { xpos = 6, ypos = 6, displaycharacter = "%", defaultcolor = Color.gray },
            //new RedEnemy { xpos = 9, ypos = 6, moveSpeed = 1.5f, health = 10f, attackDamage = 3, attackSpeed = 55 },
            //new PlayerItem { xpos = 10, ypos = 7, objectDialogs = new List<Dialog>{ new Dialog{ dialogText = "you found a gun!" }}, displaycharacter = ">", item = new Gun { name = "Remington XP-100", attackDamage = 3.0f, icon = "->", iconcolor = Color.white, shootDelay = 0.8f, magazineSize = 7, description = "Small Sized Handgun \n\r.308 Winchester\n\rMFR 289m (approx. 245m)\n\rType: single slide shot (man-op)\n\r \n\r Broken sight" }, defaultcolor = Color.gray }
            };

        level2 = new List<LevelObject>() {
            new Player{ xpos = 2, ypos = 9, displaycharacter = "@", defaultcolor = Color.yellow },
            new Wall{ xpos = 2, ypos = 7, displaycharacter = "!", defaultcolor = Color.white, objectDialogs = new List<Dialog>(){ new Dialog{ dialogText = "One of the anti-ddos security systems is called <color=green><b>\"Traffic Filtering\"</b></color>." }, new Dialog{ dialogText = "<color=green><b>Traffic Filters</b></color> are used to detect any rubbish data sent to the machine. It detects duplicate request, corrupted data, rate limits and such." }, new Dialog{ dialogText = "To bypass this, most hackers tend to use <color=green><b>IP Spoofers</b></color>. It makes the filters think, the rubbish data is coming from multiple, non-related connections." }, new Dialog{ dialogText = "Your task is to move <b><i>the ip spoofers (%)</i></b> in the <color=#575757><b><i>correct place (*)</i></b></color>, to ensure they can work." }, new Dialog{ dialogText = "Since you dont want anyone to reveal your identity, you will need to use a <color=#388dc2><b>VPN</b></color>. You can toggle your VPN on and off by using the <b>\"F\"</b> key." }, new Dialog{ dialogText = "You can only modify the spoofers when in vpn mode. Please, note that your visibility can be limited when using the vpn." }, new Dialog{ dialogText = " \"Why?\", you might ask. Well, think of it like of a robbery..."}, new Dialog{ dialogText = "Whenever you do a robbery, you would likely use a mask or a balaclava, right? \nYeah. While it would mask your identity, it could also prevent you from using your eye's full potential ;)))"} } },
            new Wall{ xpos = 15, ypos = 8, displaycharacter = "?", defaultcolor = Color.white, objectDialogs = new List<Dialog>(){ new Dialog{ dialogText = "PRO TIP: if you mess something up, press B to reset your router (thank me later)"} } },
            //walls
            //opening
            new Wall{ xpos = 1, ypos = 1, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 1, ypos = 2, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 2, ypos = 2, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 8, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            //new Wall{ xpos = 9, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 8, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new HoldButton{ xpos = 7, ypos = 4, displaycharacter = "*", defaultcolor = Color.gray },
            new HoldButton{ xpos = 6, ypos = 4, displaycharacter = "*", defaultcolor = Color.gray },
            new HoldButton{ xpos = 5, ypos = 4, displaycharacter = "*", defaultcolor = Color.gray },
            new Wall{ xpos = 12, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 13, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 14, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 15, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 16, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 19, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 20, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 21, ypos = 3, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 4, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 8, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 9, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 4, ypos = 10, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 5, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 6, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 7, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 8, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 9, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 10, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 11, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 12, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Crate{ xpos = 12, ypos = 8, displaycharacter = "%", defaultcolor = Color.white },
            new Crate{ xpos = 12, ypos = 2, displaycharacter = "%", defaultcolor = Color.white },
            new Crate{ xpos = 13, ypos = 2, displaycharacter = "%", defaultcolor = Color.white },
            new Wall{ xpos = 21, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 20, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 19, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 7, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 6, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 17, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 18, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Wall{ xpos = 19, ypos = 5, displaycharacter = "#", defaultcolor = Color.gray },
            new Finish{ xpos = 5, ypos = 9, displaycharacter = "O", defaultcolor = Color.yellow, isOpen = false, nextLevel = true, walkable = true, objectDialogs = new List<Dialog>() { new Dialog { dialogText = "this door is locked" } }, walkableDialog = false },
            };

        level3 = new List<LevelObject>(){
new Wall{ xpos=1, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Crate{ xpos=8, ypos=5, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=13, ypos=5, displaycharacter="%", defaultcolor=Color.white},
new HoldButton{ xpos=4, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=19, ypos=5, displaycharacter="@", defaultcolor=Color.yellow},
new Finish{ xpos=2, ypos=5, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog = false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=1, displaycharacter="#", defaultcolor=Color.gray} };

        level4 = new List<LevelObject>(){
new Wall{ xpos=1, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=8, displaycharacter="#", defaultcolor=Color.gray}, new PlayerItem { xpos = 8, ypos = 8, objectDialogs = new List<Dialog>{ new Dialog{ dialogText = "You found a gun! \nPress \"E\" to manage your inventory.\n\n PRO TIP: if you ever lose your gun, press 'K'" } }, displaycharacter = ">", item = new Gun { name = "EZ DDOSER 3000", attackDamage = 3.0f, icon = "->", iconcolor = Color.white, shootDelay = 0.8f, magazineSize = 7, description = "This will DDOS their ass!\nLeft click to active while holding. \nAim it at red stuff" }, defaultcolor = Color.gray, },
new Player{ xpos=1, ypos=1, displaycharacter="@", defaultcolor=Color.yellow},
new Finish{ xpos=20, ypos=3, displaycharacter="O", defaultcolor=Color.yellow, isOpen=true, nextLevel = true, walkable = true, walkableDialog = false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },  };

        level5 = new List<LevelObject>(){
new RedEnemy{ xpos = 13, ypos = 5, displaycharacter = "&", defaultcolor = Color.red, attackSpeed = 1, attackDamage = 3f, health = 10f, moveSpeed = 1.1f },
new RedEnemy{ xpos = 13, ypos = 3, displaycharacter = "&", defaultcolor = Color.red, attackSpeed = 1, attackDamage = 3f, health = 10f, moveSpeed = 1.1f },
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=2, displaycharacter="!", defaultcolor=Color.white, objectDialogs = new List<Dialog>(){ new Dialog(){ dialogText = "I have a feeling there's danger nearby..." }, new Dialog(){ dialogText = "They are the network protectors. They will see you in the dark, but only in the dark..." }, new Dialog{ dialogText = "Just keep your gun near you and you'll be fine, okay?"} } },
new Wall{ xpos=1, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Finish{ xpos=20, ypos=9, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkableDialog=false, walkable = true, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Player{ xpos=2, ypos=2, displaycharacter="@", defaultcolor=Color.yellow}, new RedEnemy{ xpos=9, ypos=2, displaycharacter="E", defaultcolor=Color.red}, new RedEnemy{ xpos=13, ypos=2, displaycharacter="E", defaultcolor=Color.red},  };

        level6 = new List<LevelObject>(){
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
//new Wall{ xpos=18, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray}, new LaserShooter{ xpos=10, ypos=5, displaycharacter="x", defaultcolor=Color.red},
new Wall{ xpos=10, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=5, ypos=5, displaycharacter="@", defaultcolor=Color.yellow},
new Finish{ xpos=4, ypos=1, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, walkableDialog=false, walkable = true, nextLevel = true, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Crate{ xpos=16, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=16, ypos=3, displaycharacter="%", defaultcolor=Color.white},
new Wall{xpos = 4, ypos = 9, displaycharacter = "!", defaultcolor = Color.white, objectDialogs = new List<Dialog>(){ new Dialog(){ dialogText = "Oh, it looks like they have one of the vpn detectors."}, new Dialog{ dialogText = "It identifies any vpn and proxies, by anazyling their packets, connection patterns and network traffic."}, new Dialog(){ dialogText = "You know what it means?"}, new Dialog(){ dialogText = "Yeah, it means you cant fucking let it detect you." } } },
new HoldButton{ xpos=16, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=5, displaycharacter="*", defaultcolor=Color.gray},  };

        level7 = new List<LevelObject>(){
new HoldButton{ xpos=20, ypos=1, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=19, ypos=1, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=19, ypos=2, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=20, ypos=2, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=20, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=19, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=20, ypos=6, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=19, ypos=6, displaycharacter="*", defaultcolor=Color.gray},
new Crate{ xpos=18, ypos=5, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=18, ypos=6, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=17, ypos=6, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=17, ypos=5, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=18, ypos=2, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=17, ypos=2, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=17, ypos=1, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=18, ypos=1, displaycharacter="%", defaultcolor=Color.white},
new HoldButton{ xpos=18, ypos=3, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=18, ypos=4, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=4, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=3, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=16, ypos=3, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=16, ypos=4, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=15, ypos=4, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=15, ypos=3, displaycharacter="*", defaultcolor=Color.gray},
new Crate{ xpos=14, ypos=3, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=14, ypos=4, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=11, ypos=4, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=11, ypos=3, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=2, ypos=8, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=3, ypos=8, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=2, ypos=2, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=3, ypos=2, displaycharacter="%", defaultcolor=Color.white},
new Finish{ xpos=1, ypos=9, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkableDialog=false, walkable = true, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Player{ xpos=1, ypos=1, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},  };

        level8 = new List<LevelObject>(){
new Wall{ xpos=13, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=10, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
//new Wall{ xpos=18, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Crate{ xpos=12, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new Player{ xpos=14, ypos=9, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=10, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=1, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=17, ypos=7, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Finish{ xpos=2, ypos=3, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkableDialog=false, walkable = true, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Wall{ xpos=10, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
//new Wall{ xpos=13, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Crate{ xpos=11, ypos=4, displaycharacter="%", defaultcolor=Color.white},
new Wall{ xpos=20, ypos=1, displaycharacter="#", defaultcolor=Color.gray},  };

        level9 = new List<LevelObject>(){
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new HoldButton{ xpos=9, ypos=6, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=2, ypos=2, displaycharacter="@", defaultcolor=Color.yellow},
new Finish{ xpos=15, ypos=2, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Wall{ xpos=20, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Crate{ xpos=10, ypos=4, displaycharacter="%", defaultcolor=Color.white},
new Wall{ xpos=7, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=1, displaycharacter="#", defaultcolor=Color.gray},  };

        level10 = new List<LevelObject>(){
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new HoldButton{ xpos=4, ypos=1, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=4, ypos=3, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=4, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new Crate{ xpos=7, ypos=2, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=4, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=5, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new Player{ xpos=1, ypos=3, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=8, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Crate{ xpos=11, ypos=8, displaycharacter="%", defaultcolor=Color.white},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new HoldButton{ xpos=15, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Finish{ xpos=18, ypos=8, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Wall{ xpos=20, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=8, displaycharacter="#", defaultcolor=Color.gray},  };

        level11 = new List<LevelObject>(){
new Wall{ xpos=12, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
//new Wall{ xpos=12, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
//new Wall{ xpos=13, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=14, ypos=9, displaycharacter="@", defaultcolor=Color.yellow},
new Crate{ xpos=13, ypos=4, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=13, ypos=5, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=13, ypos=6, displaycharacter="%", defaultcolor=Color.white},
new HoldButton{ xpos=14, ypos=4, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=14, ypos=5, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=14, ypos=6, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Finish{ xpos=16, ypos=3, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } }, new RedEnemy{ xpos=2, ypos=3, displaycharacter="E", defaultcolor=Color.red},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=7, displaycharacter="#", defaultcolor=Color.gray},  };

        level12 = new List<LevelObject>(){
new Wall{ xpos=17, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=19, ypos=5, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=10, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=3, displaycharacter="#", defaultcolor=Color.gray}, new LaserShooter{ xpos=7, ypos=5, displaycharacter="x", defaultcolor=Color.gray, laserCount = 2, laserDamage = 2.5f, laserColor = Color.red, isStatic = false, rotationSpeed = 0.6f }, new LaserShooter{ xpos=14, ypos=5, displaycharacter="x", defaultcolor=Color.gray, laserCount = 2, laserDamage = 2.5f, laserColor = Color.red, isStatic = false, rotationSpeed = 0.6f },
new Finish{ xpos=2, ypos=5, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Crate{ xpos=10, ypos=3, displaycharacter="%", defaultcolor=Color.white},
new Crate{ xpos=11, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new HoldButton{ xpos=8, ypos=2, displaycharacter="*", defaultcolor=Color.gray},
new HoldButton{ xpos=13, ypos=8, displaycharacter="*", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=9, displaycharacter="#", defaultcolor=Color.gray},  };

        level13 = new List<LevelObject>(){
new Wall{ xpos=14, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=16, ypos=2, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=15, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=8, displaycharacter="#", defaultcolor=Color.gray}, new RedEnemy{ xpos=18, ypos=6, displaycharacter="E", defaultcolor=Color.red, health = 10f, attackSpeed = 2, attackDamage = 2.5f, moveSpeed = 1.1f},
new Wall{ xpos=10, ypos=9, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=4, displaycharacter="#", defaultcolor=Color.gray}, new RedEnemy{ xpos=13, ypos=7, displaycharacter="E", defaultcolor=Color.red, health = 10f, attackSpeed = 2, attackDamage = 2.5f, moveSpeed = 1.1f},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=7, displaycharacter="#", defaultcolor=Color.gray}, new RedEnemy{ xpos=8, ypos=8, displaycharacter="E", defaultcolor=Color.red, health = 10f, attackSpeed = 2, attackDamage = 2.5f, moveSpeed = 1.1f}, new RedEnemy{ xpos=1, ypos=6, displaycharacter="E", defaultcolor=Color.red, health = 10f, attackSpeed = 2, attackDamage = 2.5f, moveSpeed = 1.1f}, new RedEnemy{ xpos=3, ypos=9, displaycharacter="E", defaultcolor=Color.red, health = 10f, attackSpeed = 2, attackDamage = 2.5f, moveSpeed = 1.1f},
new Finish{ xpos=2, ypos=2, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, levelGate=0, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },  };
        
        level14 = new List<LevelObject>(){
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=6, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=7, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=8, displaycharacter="#", defaultcolor=Color.gray}, new LaserShooter{ xpos=10, ypos=5, displaycharacter="x", defaultcolor=Color.gray, laserCount = 8, laserDamage = 10f, isStatic = false},
new Wall{ xpos=15, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="#", defaultcolor=Color.gray},
new Player{ xpos=2, ypos=3, displaycharacter="@", defaultcolor=Color.yellow},
new Finish{ xpos=2, ypos=8, displaycharacter="O", defaultcolor=Color.yellow, isOpen=false, nextLevel = true, walkable = true, walkableDialog=false, objectDialogs = new List<Dialog>(){ new Dialog { dialogText = "This door is locked"} } },
new Crate{ xpos=19, ypos=7, displaycharacter="%", defaultcolor=Color.white},
new HoldButton{ xpos=19, ypos=9, displaycharacter="*", defaultcolor=Color.gray},  };

        level15 = new List<LevelObject>(){
new Wall{ xpos=2, ypos=8, displaycharacter="T", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=8, displaycharacter="H", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=8, displaycharacter="A", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=8, displaycharacter="N", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=8, displaycharacter="K", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=8, displaycharacter="S", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=8, displaycharacter="F", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=8, displaycharacter="O", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=8, displaycharacter="R", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=8, displaycharacter="P", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=8, displaycharacter="L", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=8, displaycharacter="A", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=8, displaycharacter="Y", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=8, displaycharacter="I", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=8, displaycharacter="N", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=8, displaycharacter="G", defaultcolor=Color.gray},
new Player{ xpos=3, ypos=3, displaycharacter="@", defaultcolor=Color.yellow},
new Wall{ xpos=1, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=1, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=2, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=3, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=4, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=7, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=6, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=1, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=3, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=20, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=19, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=18, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=17, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=16, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=15, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=5, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=14, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=4, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=13, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=12, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=11, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=10, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=9, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=8, ypos=2, displaycharacter="#", defaultcolor=Color.gray},
new Wall{ xpos=5, ypos=3, displaycharacter="!", defaultcolor=Color.gray, objectDialogs = new List<Dialog>(){ new Dialog{ dialogText = "You got em! You are the new worlds best hacker! :D\n\nPayPal Balance: 75 000 000 $   (worth it)"} },  } };

        //save all levels to list
        gameLevels = new List<List<LevelObject>>() { level0, level1, level2, level3, level4, level5, level6, level7, level8, level9, level10, level11, 
            level12, level13, level14, level15 };
    }

    //SHUT THE FUCK UP BITCH

    //public GameObject loading;

    public static bool isRestartted = false;

    public static void RestartLevel()
    {
        GameManager.playerManager.loading.SetActive(true);
        isRestartted = true;
        //ItemManager.SavePlayerInventory();
        GameManager.itemManager.SaveInv();
        PlayerPrefs.SetInt("startlevel", currentLevel);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        //LevelManager.localPlayers.Clear();
        //LoadLevel(currentLevel);

        //LevelManager.mainplayer.xpos = lastXSpawn;
        //LevelManager.mainplayer.ypos = lastYSpawn;
        GameManager.playerManager.gameOverScreen.SetActive(false);
        //LevelManager.mainplayer.xpos = lastXSpawn;
        //LevelManager.mainplayer.ypos = lastYSpawn;
        /*
        foreach(LevelObject obj in LevelManager.localPlayers)
        {
            ((Player)obj).xpos = lastXSpawn;
            ((Player)obj).ypos = lastYSpawn;
        }*/

        //print(mainplayer.xpos + ":" + mainplayer.ypos);

        //GameRenderer.gameRenderer.RenderLevel();
    }


    public static void LoadCustomLevel(Level level)
    {
        
    }

    public static void GoToFirstLevel()
    {
        PlayerPrefs.DeleteKey("startlevel");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void NextLevel()
    {
        currentLevel++;
        //ItemManager.SavePlayerInventory();
        LoadLevel(currentLevel, true);
    }

    public static int lastXSpawn = 0;
    public static int lastYSpawn = 0;

    public static void LoadLevel(int levelID, bool renderAfter = false)
    {
        currentLevel = levelID;
        GameRenderer.gameRenderer.killedEnemies = 0;
        GameRenderer.gameRenderer.levelfinished = false;
        List<LevelObject> levelToLoad = new List<LevelObject>();
        levelToLoad = MergeLevelObjectLists(gameLevels[levelID], GetLevelBorders());
        InputManager.canMove = true;
        InputManager.canShoot = true;
        InputManager.canUseInventory = true;
        GameManager.playerManager.isDead = false;
        GameRenderer.gameRenderer.ResetLevelTiles();
        LevelManager.InitializeLevel(levelToLoad);
        GameRenderer.gameRenderer.ClearEnemies();
        GameRenderer.gameRenderer.GenerateEnemies();

        if (renderAfter)
        {
            GameRenderer.gameRenderer.RenderLevel();
            GameManager.soundManager.PlaySound("door-openclose");
        }
    }

    public static void ModifyPlayer()
    {
        /*
        foreach(LevelObject obj in localLevelObjects)
        {
            if(obj.GetType() == typeof(Player))
            {
                ((Player)obj).displaycharacter = "X";
                ((Player)obj).defaultcolor = Color.red;
            }
        }*/

        //compatibility with revive feature
        GameObject playerTile = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        playerTile.GetComponent<BoardTile>().ChangeText("X");
        playerTile.GetComponent<BoardTile>().ChangeTextColor(Color.red);

        GameRenderer.gameRenderer.RenderLevel();
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ItemRestorer());
    }

    private IEnumerator ItemRestorer()
    {
        yield return new WaitForSeconds(2f);

        if (isRestartted == true)
        {
            //ItemManager.LoadPlayerInventory();
            GameManager.itemManager.LoadInv();
            print("restarted level!");
            isRestartted = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<LevelObject> GetLevelBorders()
    {
        int levelWidth = GameRenderer.gameRenderer.boardWidth;
        int levelHeight = GameRenderer.gameRenderer.boardHeight;

        List<LevelObject> desiredObjects = new List<LevelObject>();

        for (int i = 0; i < levelHeight; i++) //height
        {
            for (int j = 0; j < levelWidth; j++) //width
            {
                if(i == 0 || i == levelHeight - 1 || j == 0 || j == levelWidth - 1)
                {
                    desiredObjects.Add(new Wall{ xpos = j, ypos = i, displaycharacter = "#", defaultcolor = Color.gray });
                }
            }
        }

        return desiredObjects;

    }

    public static List<LevelObject> MergeLevelObjectLists(List<LevelObject> list1, List<LevelObject> list2)
    {
        List<LevelObject> desiredObjets = new List<LevelObject>();

        foreach(LevelObject l in list1)
        {
            desiredObjets.Add(l);
        }

        foreach(LevelObject l in list2)
        {
            desiredObjets.Add(l);
        }

        return desiredObjets;
    }

    public static void InitializeLevel(List<LevelObject> levelObjects)
    {
        localLevelObjects.Clear();
        //==================================
        localPlayers.Clear();
        mainplayer = null;
        localWalls.Clear();
        GameRenderer.gameRenderer.activeEnemies.Clear();

        localLevelObjects = levelObjects;

        foreach(LevelObject lo in levelObjects)
        {
            if(lo.GetType() == typeof(Player))
            {
                lo.parentgameobject = GameRenderer.gameRenderer.GetGameObjectByTilePosition(lo.xpos, lo.ypos);

                localPlayers.Add((Player)lo);

                lastXSpawn = lo.xpos;
                lastYSpawn = lo.ypos;
                //print("LAST SPAWN: " + lo.xpos + ":" + lo.ypos);

                if (mainplayer == null)
                {
                    mainplayer = (Player)lo;
                    //mainplayer.xpos = lastXSpawn;
                    //mainplayer.ypos = lastYSpawn;
                }
            }
            else if(lo.GetType() == typeof(Wall))
            {
                lo.parentgameobject = GameRenderer.gameRenderer.GetGameObjectByTilePosition(lo.xpos, lo.ypos);

                localWalls.Add((Wall)lo);
            }
            
        }

    }

    public static LevelObject GetObjectInPosition(int x, int y, LevelObject objectType)
    {
        if(objectType.GetType() == typeof(LevelObject))
        {
            foreach (LevelObject obj in localLevelObjects)
            {
                if (obj.xpos == x && obj.ypos == y)
                {
                    return obj;
                }
            }

            return null;
        }
        else
        {
            foreach (LevelObject obj in localLevelObjects)
            {
                if (obj.xpos == x && obj.ypos == y)
                {
                    if (obj.GetType() == objectType.GetType())
                    {
                        return obj;
                    }
                }
            }

            return null;
        }

        
    }

    public static string GetObjectInPositionDisplayCharacter(int x, int y, LevelObject objectType)
    {
        if (objectType.GetType() == typeof(LevelObject))
        {
            foreach (LevelObject obj in localLevelObjects)
            {
                if (obj.xpos == x && obj.ypos == y)
                {
                    return obj.displaycharacter;
                }
            }

            return "";
        }
        else
        {
            foreach (LevelObject obj in localLevelObjects)
            {
                if (obj.xpos == x && obj.ypos == y)
                {
                    if (obj.GetType() == objectType.GetType())
                    {
                        return obj.displaycharacter;
                    }
                }
            }

            return "";
        }


    }


}

public class Level
{
    public List<LevelObject> levels;
    public LevelProperties properties;
    public bool borders;
}