using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static bool canMove = true;
    public static bool canShoot = true;
    public static bool canUseInventory = true;
    public static bool pauseMovement = false;

    public void PauseMovement(float time)
    {
        StartCoroutine(TemporarilyPausePlayerMovement(time));
    }

    private IEnumerator TemporarilyPausePlayerMovement(float time)
    {
        pauseMovement = true;
        yield return new WaitForSeconds(time);
        pauseMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !pauseMovement)
        {
            InputUpdate();
        }
    }

    private void InputUpdate()
    {
        ///how it works:
        ///   0         
        ///   |          7x5 square board
        /// 0-#######
        ///   #     #
        ///   #     #
        ///   #     #
        ///   #######-7
        ///         |
        ///         5
        /// i hope its easier to understand now :P
        /// relativeXCoordinates/relativeYCoordinates can be modified to change the "corner" in which x:0 and y:0 is.
        /// 

        if(GameManager.playerManager.isDead)
            return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            //LevelManager.LoadLevel(13, true);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print("item given.");
            GameManager.itemManager.GiveItemToPlayer(new Gun { name = "EZ DDOSER 3000", attackDamage = 3.0f, icon = "->", iconcolor = Color.white, shootDelay = 0.8f, magazineSize = 7, description = "This will DDOS their ass!\nLeft click to active while holding. \nAim it at red stuff" });
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //restart
            LevelManager.RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameRenderer.gameRenderer.ToggleLight();
        }


        if (canShoot && Input.GetMouseButtonDown(0) && GameManager.itemManager.isInventoryOpened == false)
        {
            GameManager.weaponManager.Shoot();
        }

        // INVENTORY CONTROLS

        if (Input.GetKeyDown(KeyCode.E) && canUseInventory)
        {
            GameManager.itemManager.AutoToggleInventory();
        }

        if(GameManager.itemManager.isInventoryOpened == true)
            return;

        if(GameManager.dialogManager.isDialogActive == true)
            return;

        ////   ====================
        ////   > UPWARDS/DOWNWARDS
        ////   ====================

        //move upwards
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameManager.weaponManager.UpdateHeldItemPosition("up");

            if(GameRenderer.gameRenderer.IsPositionADialog(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1))
            {
                if(!GameRenderer.gameRenderer.IsDialogWalkable(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1))
                {
                    return;
                }
            }

            if(GameRenderer.gameRenderer.IsPositionOutOfBounds(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1))
            {
                //the position is out of bounds
                //do not move XXX
                return;
            }

            if (!GameRenderer.gameRenderer.IsPositionWalkable(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1))
            {
                if (GameManager.settingsManager.smoothMovement)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.SmoothlyMoveThenSmoothlyReturn(new Vector3(bt.gameObject.transform.position.x, bt.gameObject.transform.position.y + GameManager.settingsManager.errorMaxOffset, bt.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                    StartCoroutine(DelayMovement(GameManager.settingsManager.errorSmoothTime));
                }

                if (GameManager.settingsManager.flashMovementError)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1).GetComponent<BoardTile>();
                    bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);                  
                    
                }
                return;
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1, new Crate()))
            {
                //there's a crate at specified position, move it with the player!
                if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
                {
                    if(GameRenderer.gameRenderer.canMoveBoxes == false)
                        return;

                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectUp();
                    if (movementPossible == false) { return; }
                }
            }

            if(GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1, new Finish()))
            {
                if(GameRenderer.gameRenderer.objectAtPosition != null)
                {
                    //next level door

                    if(((Finish)GameRenderer.gameRenderer.objectAtPosition).isOpen == true)
                    {
                        if(((Finish)GameRenderer.gameRenderer.objectAtPosition).nextLevel == true)
                        {
                            LevelManager.NextLevel();
                        }
                        else
                        {
                            LevelManager.LoadLevel(((Finish)GameRenderer.gameRenderer.objectAtPosition).levelGate, true);
                        }
                    }

                    if(((Finish)GameRenderer.gameRenderer.objectAtPosition).walkableDialog == false)
                    {
                        return;
                    }
                }
            }

            PlayFootstepSound();
            LevelManager.mainplayer.ypos -= 1;

            if (GameManager.settingsManager.smoothMovement)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1).GetComponent<BoardTile>();
                bt.SmoothlyMove(new Vector3(bt.gameObject.transform.position.x, bt.gameObject.transform.position.y + 1, bt.gameObject.transform.position.z), GameManager.settingsManager.smoothTime, true);
                StartCoroutine(DelayNextRender(GameManager.settingsManager.smoothTime));
                StartCoroutine(DelayMovement(GameManager.settingsManager.smoothTime));
            }
            else
            {
                //re-render the level
                GameRenderer.gameRenderer.RenderLevel();
            }
                        
        }

        //move downwards
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameManager.weaponManager.UpdateHeldItemPosition("down");

            if (GameRenderer.gameRenderer.IsPositionADialog(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1))
            {
                if (!GameRenderer.gameRenderer.IsDialogWalkable(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1))
                {
                    return;
                }
            }

            if (GameRenderer.gameRenderer.IsPositionOutOfBounds(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1))
            {
                //the position is out of bounds
                //do not move XXX
                return;
            }

            if (!GameRenderer.gameRenderer.IsPositionWalkable(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1))
            {
                if (GameManager.settingsManager.smoothMovement)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.SmoothlyMoveThenSmoothlyReturn(new Vector3(bt.gameObject.transform.position.x, bt.gameObject.transform.position.y - GameManager.settingsManager.errorMaxOffset, bt.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                    StartCoroutine(DelayMovement(GameManager.settingsManager.errorSmoothTime));
                }

                if (GameManager.settingsManager.flashMovementError)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1).GetComponent<BoardTile>();
                    bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);
                }
                return;
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1, new Crate()))
            {
                //there's a crate at specified position, move it with the player!
                if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
                {
                    if (GameRenderer.gameRenderer.canMoveBoxes == false)
                        return;

                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectDown();
                    if (movementPossible == false) { return; }
                }
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos + 1, new Finish()))
            {
                if (GameRenderer.gameRenderer.objectAtPosition != null)
                {
                    //next level door

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).isOpen == true)
                    {
                        if (((Finish)GameRenderer.gameRenderer.objectAtPosition).nextLevel == true)
                        {
                            LevelManager.NextLevel();
                        }
                        else
                        {
                            LevelManager.LoadLevel(((Finish)GameRenderer.gameRenderer.objectAtPosition).levelGate, true);
                        }
                    }

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).walkableDialog == false)
                    {
                        return;
                    }
                }
            }

            PlayFootstepSound();
            LevelManager.mainplayer.ypos += 1;

            if (GameManager.settingsManager.smoothMovement)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos - 1).GetComponent<BoardTile>();
                bt.SmoothlyMove(new Vector3(bt.gameObject.transform.position.x, bt.gameObject.transform.position.y - 1, bt.gameObject.transform.position.z), GameManager.settingsManager.smoothTime, true);
                StartCoroutine(DelayNextRender(GameManager.settingsManager.smoothTime));
                StartCoroutine(DelayMovement(GameManager.settingsManager.smoothTime));
            }
            else
            {
                //re-render the level
                GameRenderer.gameRenderer.RenderLevel();
            }
        }

        ////   =============
        ////   > RIGHT/LEFT
        ////   =============

        //move left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameManager.weaponManager.UpdateHeldItemPosition("left");

            if (GameRenderer.gameRenderer.IsPositionADialog(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos))
            {
                if (!GameRenderer.gameRenderer.IsDialogWalkable(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos))
                {
                    return;
                }
            }

            if (GameRenderer.gameRenderer.IsPositionOutOfBounds(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos))
            {
                //the position is out of bounds
                //do not move XXX
                return;
            }

            if (!GameRenderer.gameRenderer.IsPositionWalkable(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos))
            {
                if (GameManager.settingsManager.smoothMovement)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.SmoothlyMoveThenSmoothlyReturn(new Vector3(bt.gameObject.transform.position.x - GameManager.settingsManager.errorMaxOffset, bt.gameObject.transform.position.y, bt.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                    StartCoroutine(DelayMovement(GameManager.settingsManager.errorSmoothTime));
                }

                if (GameManager.settingsManager.flashMovementError)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);
                }
                return;
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos, new Crate()))
            {
                //there's a crate at specified position, move it with the player!
                if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
                {
                    if (GameRenderer.gameRenderer.canMoveBoxes == false)
                        return;

                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectLeft();
                    if (movementPossible == false) { return; }
                }
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos, new Finish()))
            {
                if (GameRenderer.gameRenderer.objectAtPosition != null)
                {
                    //next level door

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).isOpen == true)
                    {
                        if (((Finish)GameRenderer.gameRenderer.objectAtPosition).nextLevel == true)
                        {
                            LevelManager.NextLevel();
                        }
                        else
                        {
                            LevelManager.LoadLevel(((Finish)GameRenderer.gameRenderer.objectAtPosition).levelGate, true);
                        }
                    }

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).walkableDialog == false)
                    {
                        return;
                    }
                }
            }

            PlayFootstepSound();
            LevelManager.mainplayer.xpos -= 1; //modify as normal
            /// right is right, left is left

            if (GameManager.settingsManager.smoothMovement)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                bt.SmoothlyMove(new Vector3(bt.gameObject.transform.position.x - 1, bt.gameObject.transform.position.y, bt.gameObject.transform.position.z), GameManager.settingsManager.smoothTime, true);
                StartCoroutine(DelayNextRender(GameManager.settingsManager.smoothTime));
                StartCoroutine(DelayMovement(GameManager.settingsManager.smoothTime));
            }
            else
            {
                //re-render the level
                GameRenderer.gameRenderer.RenderLevel();
            }
        }

        //move right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameManager.weaponManager.UpdateHeldItemPosition("right");

            if (GameRenderer.gameRenderer.IsPositionADialog(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos))
            {
                if (!GameRenderer.gameRenderer.IsDialogWalkable(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos))
                {
                    return;
                }
            }

            if (GameRenderer.gameRenderer.IsPositionOutOfBounds(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos))
            {
                //the position is out of bounds
                //do not move XXX
                return;
            }

            if (!GameRenderer.gameRenderer.IsPositionWalkable(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos))
            {
                if (GameManager.settingsManager.smoothMovement)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.SmoothlyMoveThenSmoothlyReturn(new Vector3(bt.gameObject.transform.position.x + GameManager.settingsManager.errorMaxOffset, bt.gameObject.transform.position.y, bt.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                    StartCoroutine(DelayMovement(GameManager.settingsManager.errorSmoothTime));
                }

                if (GameManager.settingsManager.flashMovementError)
                {
                    BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                    bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);
                }
                return;
            }

            if(GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos, new Crate()))
            {
                //there's a crate at specified position, move it with the player!
                if(GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
                {
                    if (GameRenderer.gameRenderer.canMoveBoxes == false)
                        return;

                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectRight();
                    if(movementPossible == false) { return; }
                }
            }

            if (GameRenderer.gameRenderer.IsObjectAtPosition(LevelManager.mainplayer.xpos + 1, LevelManager.mainplayer.ypos, new Finish()))
            {
                if (GameRenderer.gameRenderer.objectAtPosition != null)
                {
                    //next level door

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).isOpen == true)
                    {
                        if (((Finish)GameRenderer.gameRenderer.objectAtPosition).nextLevel == true)
                        {
                            LevelManager.NextLevel();
                        }
                        else
                        {
                            LevelManager.LoadLevel(((Finish)GameRenderer.gameRenderer.objectAtPosition).levelGate, true);
                        }
                    }

                    if (((Finish)GameRenderer.gameRenderer.objectAtPosition).walkableDialog == false)
                    {
                        return;
                    }
                }
            }

            PlayFootstepSound();
            LevelManager.mainplayer.xpos += 1; //modify as normal
            /// right is right, left is left

            if (GameManager.settingsManager.smoothMovement)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos - 1, LevelManager.mainplayer.ypos).GetComponent<BoardTile>();
                bt.SmoothlyMove(new Vector3(bt.gameObject.transform.position.x + 1, bt.gameObject.transform.position.y, bt.gameObject.transform.position.z), GameManager.settingsManager.smoothTime, true);
                StartCoroutine(DelayNextRender(GameManager.settingsManager.smoothTime));
                StartCoroutine(DelayMovement(GameManager.settingsManager.smoothTime));
            }
            else
            {
                //re-render the level
                GameRenderer.gameRenderer.RenderLevel();
            }
        }

    }

    public void PlayFootstepSound()
    {
        GameManager.soundManager.PlaySound("footstep" + Random.Range(1, 6));
    }


    private IEnumerator DelayNextRender(float time)
    {
        yield return new WaitForSeconds(time);
        GameRenderer.gameRenderer.RenderLevel();
    }

    public static IEnumerator DelayMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time - GameManager.settingsManager.preRenderTime);
        canMove = true;
    }

}
