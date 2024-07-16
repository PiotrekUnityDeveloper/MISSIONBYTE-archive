using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
/// <summary> ========================================
/// ==================================================
/// WRITTEN BY PIOTREK4 [Piotrek4Games C 2023]
/// - All rights reserved
/// ==================================================
/// </summary> =======================================

public class GameManager : MonoBehaviour
{
    //game variables
    [SerializeField] private TMP_FontAsset GameFont;

    //other stuff
    public static AnimationManager animationManager;
    public static SettingsManager settingsManager;
    public static InputManager inputManager;
    public static ItemManager itemManager;
    public static WeaponManager weaponManager;
    public static SoundManager soundManager;
    public static DialogManager dialogManager;
    public static PlayerManager playerManager;

    private void Awake()
    {
        animationManager = this.GetComponent<AnimationManager>();
        settingsManager = this.GetComponent<SettingsManager>();
        inputManager = this.GetComponent<InputManager>();
        itemManager = this.GetComponent<ItemManager>();
        weaponManager = this.GetComponent<WeaponManager>();
        dialogManager = this.GetComponent<DialogManager>();
        playerManager = this.GetComponent<PlayerManager>();

        try{ soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>(); } catch { }
    }

    // Start is called before the first frame update
    void Start()
    {

        if(SceneManager.GetActiveScene().name == "Piotrek4Games" || SceneManager.GetActiveScene().name == "Headphones")
            return;

        PlayerPrefs.DeleteKey("asciitext");
        PlayerPrefs.DeleteKey("dropdown");

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 40;

        GameManager.soundManager.PlaySound("door-open");

        //GameRenderer.gameRenderer.SetGenerationProperties(22, 14, 0, 0);
        GameRenderer.gameRenderer.LoadLevelProperties(LevelManager.currentLevel);
        GameRenderer.gameRenderer.GenerateLevel();
        if(PlayerPrefs.GetInt("startlevel", 0) != 0)
        {
            LevelManager.currentLevel = PlayerPrefs.GetInt("startlevel", 0);
            LevelManager.LoadLevel(LevelManager.currentLevel);

            //load old inventory
            ItemManager.LoadPlayerInventory();
        }
        else
        {
            LevelManager.currentLevel = 0;
            LevelManager.LoadLevel(LevelManager.currentLevel);
        }
        
        GameRenderer.gameRenderer.ClearEnemies();
        GameRenderer.gameRenderer.GenerateEnemies();
        
        //GameRenderer.gameRenderer.RenderLevel(); this will be animated
        if (GameManager.settingsManager.staticMatrixColor)
        {
            GameRenderer.RecolorAllTiles(GameManager.settingsManager.defMatrixColor);
        }
        else
        {
            GameRenderer.RecolorAllTiles(new Color(UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeRMin, GameManager.settingsManager.matrixColorRangeRMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeGMin, GameManager.settingsManager.matrixColorRangeGMax), UnityEngine.Random.Range(GameManager.settingsManager.matrixColorRangeBMin, GameManager.settingsManager.matrixColorRangeBMax), UnityEngine.Random.Range(GameManager.settingsManager.opacityColorRangeMin, GameManager.settingsManager.opacityColorRangeMax)));
        }
        AnimationManager.AnimateMatrixEffect(settingsManager.randomMatrixEffect);
        inputManager.PauseMovement(7.5f);
        GameRenderer.gameRenderer.RenderLevelWithDelay(7.5f);
        StartCoroutine(PlayDoorSound());
    }

    private IEnumerator PlayDoorSound()
    {
        yield return new WaitForSeconds(7.5f);
        GameManager.soundManager.PlaySound("door-close");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RateGame()
    {
        //open the game link
        System.Diagnostics.Process.Start("https://piotrek4.itch.io/missionbyte");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
