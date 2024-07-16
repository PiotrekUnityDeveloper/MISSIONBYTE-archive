using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardTile : MonoBehaviour
{
    [HideInInspector] public Vector3 originalPosition;

    //Local variables
    [HideInInspector] public string myText = "";
    [HideInInspector] public int tileIndex = 0;

    [HideInInspector] public int myXPos = 0;
    [HideInInspector] public int myYPos = 0;

    //Non
    [SerializeField] public GameObject Background;
    [SerializeField] public GameObject Text;

    public bool pauseRender = false;

    //Initialize children gameobjects
    private void Awake()
    {
        if(Background == null)
        {
            this.Background = this.GetComponentInChildren<SpriteRenderer>().gameObject;
        }

        if(Text == null)
        {
            this.Text = this.GetComponentInChildren<TMP_Text>().gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Piotrek4Games")
        {
            StartCoroutine(ResetPosition());
        }
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(6f);

        if(this.transform.position != this.originalPosition)
        {
            this.transform.position = originalPosition;
        }

        StartCoroutine(ResetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(string _string)
    {
        this.GetComponentInChildren<TMP_Text>().text = _string;
    }

    public void ChangeChar(char _char)
    {
        this.GetComponentInChildren<TMP_Text>().text = _char.ToString();
    }

    public void ChangeCharArray(char[] _chararray)
    {
        this.GetComponentInChildren<TMP_Text>().text = _chararray.ToString();
    }

    public void ChangeTextColor(Color _color)
    {
        this.GetComponentInChildren<TMP_Text>().color = _color;
    }

    public void ChangeFont(TMP_FontAsset _font)
    {
        this.GetComponentInChildren<TMP_Text>().font = _font;
    }

    public void SmoothlyMove(Vector3 destination, float duration/*!in seconds!*/, bool resetAfter = false)
    {
        if (resetAfter)
        {
            GameManager.animationManager.StartCoroutine(GameManager.animationManager.MoveToTargetPositionThenReset(this.transform.position, destination, duration, this.transform));
        }
        else
        {
            GameManager.animationManager.StartCoroutine(GameManager.animationManager.MoveToTargetPosition(this.transform.position, destination, duration, this.transform));
        }
    }

    public void SmoothlyMoveThenSmoothlyReturn(Vector3 destination, float duration)
    {
        //cal the ienumerator
        StartCoroutine(SmoothlyMoveThenSmoothlyReturnIEnum(destination, duration));
    }

    private IEnumerator SmoothlyMoveThenSmoothlyReturnIEnum(Vector3 destination, float duration)
    {
        // duration is divided by 2 here to manage the time limit

        Vector3 initialPosition = this.transform.position;

        GameManager.animationManager.StartCoroutine(GameManager.animationManager.MoveToTargetPosition(this.transform.position, destination, (duration / 2), this.transform));

        //wait
        yield return new WaitForSeconds(duration / 2);

        //return to the old position

        GameManager.animationManager.StartCoroutine(GameManager.animationManager.MoveToTargetPosition(this.transform.position, initialPosition, (duration / 2), this.transform));
    }

    private bool canFlashColor = true;

    public void FlashText(Color endColor, float duration)
    {
        if(canFlashColor == false)
            return;

        StartCoroutine(TransitionFontColor(this.Text.GetComponent<TMP_Text>().color, endColor, duration));
        canFlashColor = false;
    }

    public void FlashText(Color startColor, Color endColor, float duration)
    {
        StartCoroutine(TransitionFontColor(startColor, endColor, duration));
    }

    public void ReplaceTextThenRevert(string text, float duration)
    {
        StartCoroutine(ReplaceTextThenRevertIEnum(text, duration));
    }

    public void ReplaceTextThenSetTo(string text, string endText, float duration)
    {
        StartCoroutine(ReplaceTextThenSetToIEnum(text, endText, duration));
    }

    public IEnumerator ReplaceTextThenSetToIEnum(string text, string endText, float duration)
    {
        this.Text.GetComponent<TMP_Text>().text = text;
        yield return new WaitForSeconds(duration);
        this.Text.GetComponent<TMP_Text>().text = endText;
    }

    public IEnumerator ReplaceTextThenRevertIEnum(string text, float duration)
    {
        string oldText = this.Text.GetComponent<TMP_Text>().text;
        this.Text.GetComponent<TMP_Text>().text = text;
        yield return new WaitForSeconds(duration);
        this.Text.GetComponent<TMP_Text>().text = oldText;
    }

    public IEnumerator ReplaceTextThenEraseIEnum(string text, float duration)
    {
        string oldText = "";
        this.Text.GetComponent<TMP_Text>().text = text;
        yield return new WaitForSeconds(duration);
        this.Text.GetComponent<TMP_Text>().text = oldText;
    }

    // START COLOR SPECIFIED

    public void FlashTextThenRevert(Color startColor, Color endColor, float duration)
    {
        StartCoroutine(FlashTextThenRevertIEnum(startColor, endColor, duration));
    }

    public IEnumerator FlashTextThenRevertIEnum(Color startColor, Color endColor, float duration)
    {
        StartCoroutine(TransitionFontColor(startColor, endColor, (duration / 2)));

        yield return new WaitForSeconds((duration / 2));

        StartCoroutine(TransitionFontColor(endColor, startColor, (duration / 2)));
    }

    // START COLOR UNSPECIFIED ==

    public void FlashTextThenRevert(Color endColor, float duration)
    {
        StartCoroutine(FlashTextThenRevertIEnum(this.Text.GetComponent<TMP_Text>().color, endColor, duration));
    }
    
    // ==========================

    private IEnumerator TransitionFontColor(Color startColor, Color endColor, float duration, TMP_Text textMeshPro = null)
    {
        float elapsedTime = 0f;
        Color currentColor = startColor;

        if(textMeshPro == null)
        {
            textMeshPro = this.Text.GetComponent<TMP_Text>();
        }

        while (elapsedTime < duration)
        {
            // Calculate the normalized time (0 to 1) based on the elapsed time and duration
            float t = elapsedTime / duration;

            // Lerp the color between startColor and endColor
            Color lerpedColor = Color.Lerp(startColor, endColor, t);

            // Apply the lerped color to the TextMeshPro component
            textMeshPro.color = lerpedColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set correctly
        textMeshPro.color = endColor;

        canFlashColor = true;
    }

    public bool MoveObjectLeft()
    {
        if (GameRenderer.gameRenderer.IsPositionOutOfBounds(this.myXPos - 1, this.myYPos))
        {
            //the position is out of bounds
            //do not move XXX
            return false;
        }

        if (!GameRenderer.gameRenderer.IsPositionWalkable(this.myXPos - 1, this.myYPos))
        {
            if (GameManager.settingsManager.smoothMovement)
            {
                this.SmoothlyMoveThenSmoothlyReturn(new Vector3(this.gameObject.transform.position.x - GameManager.settingsManager.errorMaxOffset, this.gameObject.transform.position.y, this.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                StartCoroutine(InputManager.DelayMovement(GameManager.settingsManager.errorSmoothTime));
            }

            if (GameManager.settingsManager.flashMovementError)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(this.myXPos - 1, this.myYPos).GetComponent<BoardTile>();
                bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);

                if (GameRenderer.gameRenderer.isRoomDark)
                {
                    bt.ChangeText("?");
                }
            }
            return false;
        }

        if (GameRenderer.gameRenderer.IsObjectAtPosition(this.myXPos - 1, this.myYPos, new Crate()))
        {
            if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
            {
                if (GameManager.settingsManager.canPushMultipleCrates)
                {
                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectLeft();

                    if(movementPossible != true) { return false; }

                }

                
            }
        }

        GameManager.soundManager.PlaySound("box-push");

        if (GameManager.settingsManager.smoothMovement == true)
        {
            //play animation
            this.SmoothlyMove(new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), GameManager.settingsManager.smoothPushTime, true);
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).xpos -= 1;
            StartCoroutine(DelayLevelRender(GameManager.settingsManager.smoothPushTime));
            return true;
        }
        else
        {
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).xpos -= 1;
            this.RenderLevel();
            return true;
        }
    }

    public bool MoveObjectRight()
    {
        if (GameRenderer.gameRenderer.IsPositionOutOfBounds(this.myXPos + 1, this.myYPos))
        {
            //the position is out of bounds
            //do not move XXX
            return false;
        }

        if (!GameRenderer.gameRenderer.IsPositionWalkable(this.myXPos + 1, this.myYPos))
        {
            if (GameManager.settingsManager.smoothMovement)
            {
                this.SmoothlyMoveThenSmoothlyReturn(new Vector3(this.gameObject.transform.position.x + GameManager.settingsManager.errorMaxOffset, this.gameObject.transform.position.y, this.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                StartCoroutine(InputManager.DelayMovement(GameManager.settingsManager.errorSmoothTime));
            }

            if (GameManager.settingsManager.flashMovementError)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(this.myXPos + 1, this.myYPos).GetComponent<BoardTile>();
                bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);

                if (GameRenderer.gameRenderer.isRoomDark)
                {
                    bt.ChangeText("?");
                }
            }
            return false;
        }

        if (GameRenderer.gameRenderer.IsObjectAtPosition(this.myXPos + 1, this.myYPos, new Crate()))
        {
            if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
            {
                if (GameManager.settingsManager.canPushMultipleCrates)
                {
                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectRight();
                    if (movementPossible != true) { return false; }
                }

                
            }
        }

        GameManager.soundManager.PlaySound("box-push");

        if (GameManager.settingsManager.smoothMovement == true)
        {
            //play animation
            this.SmoothlyMove(new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), GameManager.settingsManager.smoothPushTime, true);
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).xpos += 1;
            StartCoroutine(DelayLevelRender(GameManager.settingsManager.smoothPushTime));
            return true;
        }
        else
        {
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).xpos += 1;
            this.RenderLevel();
            return true;
        }
    }

    public bool MoveObjectUp()
    {
        if (GameRenderer.gameRenderer.IsPositionOutOfBounds(this.myXPos, this.myYPos - 1))
        {
            //the position is out of bounds
            //do not move XXX
            return false;
        }

        if (!GameRenderer.gameRenderer.IsPositionWalkable(this.myXPos, this.myYPos - 1))
        {
            if (GameManager.settingsManager.smoothMovement)
            {
                this.SmoothlyMoveThenSmoothlyReturn(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + GameManager.settingsManager.errorMaxOffset, this.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                StartCoroutine(InputManager.DelayMovement(GameManager.settingsManager.errorSmoothTime));
            }

            if (GameManager.settingsManager.flashMovementError)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(this.myXPos, this.myYPos - 1).GetComponent<BoardTile>();
                bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);

                if (GameRenderer.gameRenderer.isRoomDark)
                {
                    bt.ChangeText("?");
                }
            }
            return false;
        }

        if (GameRenderer.gameRenderer.IsObjectAtPosition(this.myXPos, this.myYPos - 1, new Crate()))
        {
            if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
            {
                if (GameManager.settingsManager.canPushMultipleCrates)
                {
                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectUp();
                    if (movementPossible != true) { return false; }
                }

                
            }
        }

        GameManager.soundManager.PlaySound("box-push");

        if (GameManager.settingsManager.smoothMovement == true)
        {
            //play animation
            this.SmoothlyMove(new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), GameManager.settingsManager.smoothPushTime, true);
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).ypos -= 1;
            StartCoroutine(DelayLevelRender(GameManager.settingsManager.smoothPushTime));
            return true;
        }
        else
        {
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).ypos -= 1;
            this.RenderLevel();
            return true;
        }
    }

    public bool MoveObjectDown()
    {
        if (GameRenderer.gameRenderer.IsPositionOutOfBounds(this.myXPos, this.myYPos + 1))
        {
            //the position is out of bounds
            //do not move XXX
            return false;
        }

        if (!GameRenderer.gameRenderer.IsPositionWalkable(this.myXPos, this.myYPos + 1))
        {
            if (GameManager.settingsManager.smoothMovement)
            {
                this.SmoothlyMoveThenSmoothlyReturn(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - GameManager.settingsManager.errorMaxOffset, this.gameObject.transform.position.z), GameManager.settingsManager.errorSmoothTime);
                StartCoroutine(InputManager.DelayMovement(GameManager.settingsManager.errorSmoothTime));
            }

            if (GameManager.settingsManager.flashMovementError)
            {
                BoardTile bt = GameRenderer.gameRenderer.GetGameObjectByTilePosition(this.myXPos, this.myYPos + 1).GetComponent<BoardTile>();
                bt.FlashTextThenRevert(LevelManager.GetObjectInPosition(bt.myXPos, bt.myYPos, new LevelObject()).defaultcolor, Color.red, GameManager.settingsManager.flashMovementErrorTime);

                if (GameRenderer.gameRenderer.isRoomDark)
                {
                    bt.ChangeText("?");
                }
            }
            return false;
        }

        if (GameRenderer.gameRenderer.IsObjectAtPosition(this.myXPos, this.myYPos + 1, new Crate()))
        {
            if (GameRenderer.gameRenderer.objectAtPosition/*this is the target crate*/ != null)
            {
                if (GameManager.settingsManager.canPushMultipleCrates)
                {
                    bool movementPossible = GameRenderer.gameRenderer.GetGameObjectByTilePosition(GameRenderer.gameRenderer.objectAtPosition.xpos, GameRenderer.gameRenderer.objectAtPosition.ypos).GetComponent<BoardTile>().MoveObjectDown();
                    if (movementPossible != true) { return false; }
                }

                
            }
        }

        GameManager.soundManager.PlaySound("box-push");

        if (GameManager.settingsManager.smoothMovement == true)
        {
            //play animation
            this.SmoothlyMove(new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), GameManager.settingsManager.smoothPushTime, true);
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).ypos += 1;
            StartCoroutine(DelayLevelRender(GameManager.settingsManager.smoothPushTime));
            return true;
        }
        else
        {
            LevelManager.GetObjectInPosition(myXPos, myYPos, new Crate()).ypos += 1;
            this.RenderLevel();
            return true;
        }
    }

    private IEnumerator DelayLevelRender(float time)
    {
        yield return new WaitForSeconds(time - GameManager.settingsManager.preRenderTime);
        this.RenderLevel();
    }

    private void RenderLevel()
    {
        GameRenderer.gameRenderer.RenderLevel();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GameManager.settingsManager != null)
        {
            if (collision.CompareTag("RedEnemy"))
            {
                LevelObject lvlobj = LevelManager.GetObjectInPosition(this.myXPos, this.myYPos, new LevelObject());

                if (lvlobj == null)
                {
                    this.pauseRender = true;
                    this.Text.GetComponent<TMP_Text>().color = GameManager.settingsManager.defMatrixColor;
                    this.FlashText(Color.red, Color.black, 1.1f);
                    this.ReplaceTextThenSetTo(/*bullet icon?*/"&", LevelManager.GetObjectInPositionDisplayCharacter(this.myXPos, this.myYPos, new LevelObject()), GameManager.settingsManager.pistolBulletTrailTime - 0.1f);
                    StartCoroutine(DelayRenderPauseState(1.1f));
                }

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.settingsManager != null)
        {
            if (collision.CompareTag("RedEnemy"))
            {
                //this.ChangeTextColor(GameManager.settingsManager.redenemyColor);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("trigger collision!");

        if(GameManager.settingsManager != null) //DEFAULT SETTINGS MANAGER
        {
            if (collision.CompareTag("FadeFlash"))
            {
                float randomTime = UnityEngine.Random.Range(GameManager.settingsManager.matrixMinDelay, GameManager.settingsManager.matrixMaxDelay);

                if (GameManager.settingsManager.staticMatrixColor)
                {
                    this.FlashTextThenRevert(GameManager.settingsManager.defMatrixColor, Color.black, randomTime);
                }
                else
                {
                    //new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax));
                    this.FlashTextThenRevert(new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax), UnityEngine.Random.Range(GameManager.settingsManager.opacityColorRangeMin, GameManager.settingsManager.opacityColorRangeMax)), Color.black, randomTime);
                }
            }
            else if (collision.CompareTag("FadeFlashSingleRender"))
            {
                float randomTime = UnityEngine.Random.Range(GameManager.settingsManager.matrixMinDelay, GameManager.settingsManager.matrixMaxDelay);

                if (GameManager.settingsManager.staticMatrixColor)
                {
                    this.FlashTextThenRevert(GameManager.settingsManager.defMatrixColor, Color.black, randomTime);
                }
                else
                {
                    //new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax));
                    this.FlashTextThenRevert(new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax), UnityEngine.Random.Range(GameManager.settingsManager.opacityColorRangeMin, GameManager.settingsManager.opacityColorRangeMax)), Color.black, randomTime);
                }
                this.StartCoroutine(DelaySingleRender(this.myXPos, this.myYPos, randomTime));
            }
            else if (collision.CompareTag("Flash"))
            {
                //reset color
                if (GameManager.settingsManager.staticMatrixColor)
                {
                    this.Text.GetComponent<TMP_Text>().color = GameManager.settingsManager.defMatrixColor;
                }
                else
                {
                    this.Text.GetComponent<TMP_Text>().color = new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.opacityColorRangeMin, GameManager.settingsManager.opacityColorRangeMax));
                }

                //color transition

                float flashduration = UnityEngine.Random.Range(GameManager.settingsManager.matrixMinDelay, GameManager.settingsManager.matrixMaxDelay);
                this.FlashText(this.Text.GetComponent<TMP_Text>().color, Color.black, flashduration);

            }
            else if (collision.CompareTag("FlashSingleRender"))
            {
                //reset color
                if (GameManager.settingsManager.staticMatrixColor)
                {
                    this.Text.GetComponent<TMP_Text>().color = GameManager.settingsManager.defMatrixColor;
                }
                else
                {
                    this.Text.GetComponent<TMP_Text>().color = new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax),
                                                                         UnityEngine.Random.Range(GameManager.settingsManager.opacityColorRangeMin, GameManager.settingsManager.opacityColorRangeMax));
                }

                //color transition

                float flashduration = UnityEngine.Random.Range(GameManager.settingsManager.matrixMinDelay, GameManager.settingsManager.matrixMaxDelay);
                this.FlashText(this.Text.GetComponent<TMP_Text>().color, Color.black, flashduration);
                this.StartCoroutine(DelaySingleRender(this.myXPos, this.myYPos, flashduration));
            }
            else if (collision.CompareTag("PistolBullet"))
            {
                LevelObject lvlobj = LevelManager.GetObjectInPosition(this.myXPos, this.myYPos, new LevelObject());

                if (lvlobj == null)
                {
                    this.pauseRender = true;
                    this.Text.GetComponent<TMP_Text>().color = GameManager.settingsManager.defMatrixColor;
                    this.FlashText(this.Text.GetComponent<TMP_Text>().color, Color.black, GameManager.settingsManager.pistolBulletTrailTime);
                    this.ReplaceTextThenSetTo(/*bullet icon?*/"*", LevelManager.GetObjectInPositionDisplayCharacter(this.myXPos, this.myYPos, new LevelObject()), GameManager.settingsManager.pistolBulletTrailTime - 0.1f);
                    StartCoroutine(DelayRenderPauseState(GameManager.settingsManager.pistolBulletTrailTime));
                }
                else if(lvlobj.GetType() == typeof(Player) || lvlobj.GetType() == typeof(PlayerItem))
                {
                    //nor play the effect nor destroy the bullet ;P
                }
                else if(lvlobj.GetType() != typeof(RedEnemy))
                {

                    PlayBulletImpactSound();
                    Destroy(collision.gameObject); //bullet hit a non-empty
                }
            }
            /*
            else if (collision.CompareTag("RedEnemy"))
            {
                LevelObject lvlobj = LevelManager.GetObjectInPosition(this.myXPos, this.myYPos, new LevelObject());

                if (lvlobj == null)
                {
                    this.pauseRender = true;
                    this.Text.GetComponent<TMP_Text>().color = GameManager.settingsManager.defMatrixColor;
                    this.FlashText(Color.red, Color.black, 1.1f);
                    this.ReplaceTextThenSetTo("&", LevelManager.GetObjectInPositionDisplayCharacter(this.myXPos, this.myYPos, new LevelObject()), GameManager.settingsManager.pistolBulletTrailTime - 0.1f);
                    StartCoroutine(DelayRenderPauseState(1.1f));
                }

            }*/
        }
        else
        {
            if (collision.CompareTag("FadeFlash"))
            {
                float randomTime = UnityEngine.Random.Range(SettingsManager1.matrixMinDelay, SettingsManager1.matrixMaxDelay);

                if (SettingsManager1.staticMatrixColor)
                {
                    this.FlashTextThenRevert(Color.black, SettingsManager1.defMatrixColor, randomTime);
                }
                else
                {
                    //new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax));
                    this.FlashTextThenRevert(new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax), UnityEngine.Random.Range(SettingsManager1.opacityColorRangeMin, SettingsManager1.opacityColorRangeMax)), Color.black, randomTime);
                }
            }
            else if (collision.CompareTag("FadeFlashSingleRender"))
            {
                float randomTime = UnityEngine.Random.Range(SettingsManager1.matrixMinDelay, SettingsManager1.matrixMaxDelay);

                if (SettingsManager1.staticMatrixColor)
                {
                    this.FlashTextThenRevert(SettingsManager1.defMatrixColor, Color.black, randomTime);
                }
                else
                {
                    //new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax));
                    this.FlashTextThenRevert(new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax), UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax), UnityEngine.Random.Range(SettingsManager1.opacityColorRangeMin, SettingsManager1.opacityColorRangeMax)), Color.black, randomTime);
                }
                this.StartCoroutine(DelaySingleRender(this.myXPos, this.myYPos, randomTime));
            }
            else if (collision.CompareTag("Flash"))
            {
                //reset color
                if (SettingsManager1.staticMatrixColor)
                {
                    this.Text.GetComponent<TMP_Text>().color = SettingsManager1.defMatrixColor;
                }
                else
                {
                    this.Text.GetComponent<TMP_Text>().color = new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.opacityColorRangeMin, SettingsManager1.opacityColorRangeMax));
                }

                //color transition

                float flashduration = UnityEngine.Random.Range(SettingsManager1.matrixMinDelay, SettingsManager1.matrixMaxDelay);
                this.FlashText(this.Text.GetComponent<TMP_Text>().color, Color.black, flashduration);

                try
                {
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlaySound("click" + UnityEngine.Random.Range(1, 2));
                }
                catch { }

            }
            else if (collision.CompareTag("FlashSingleRender"))
            {
                //reset color
                if (SettingsManager1.staticMatrixColor)
                {
                    this.Text.GetComponent<TMP_Text>().color = SettingsManager1.defMatrixColor;
                }
                else
                {
                    this.Text.GetComponent<TMP_Text>().color = new Color(UnityEngine.Random.Range(SettingsManager1.matrixColorRangeRMin, SettingsManager1.matrixColorRangeRMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.matrixColorRangeGMin, SettingsManager1.matrixColorRangeGMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.matrixColorRangeBMin, SettingsManager1.matrixColorRangeBMax),
                                                                         UnityEngine.Random.Range(SettingsManager1.opacityColorRangeMin, SettingsManager1.opacityColorRangeMax));
                }

                //color transition

                float flashduration = UnityEngine.Random.Range(SettingsManager1.matrixMinDelay, SettingsManager1.matrixMaxDelay);
                this.FlashText(this.Text.GetComponent<TMP_Text>().color, Color.black, flashduration);
                this.StartCoroutine(DelaySingleRender(this.myXPos, this.myYPos, flashduration));
            }
        }

        

    }

    private IEnumerator DelaySingleRender(int x, int y, float duration)
    {
        yield return new WaitForSeconds(duration);
        GameRenderer.gameRenderer.RenderSpecificPosition(x, y);
    }

    private IEnumerator DelayRenderPauseState(float duration)
    {
        yield return new WaitForSeconds(duration);
        this.pauseRender = false;
    }

    public void PlayBulletImpactSound()
    {
        GameManager.soundManager.PlaySound("bulletimpact1");
    }

}
