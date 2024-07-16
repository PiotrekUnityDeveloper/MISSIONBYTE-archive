using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float playerHealth = 10;

    //status
    public GameObject midlight;
    public GameObject midnolight;

    public GameObject oklight;
    public GameObject oknolight;

    public GameObject badlight;
    public GameObject badnolight;

    public GameObject haha;
    public GameObject hurt;
    public GameObject die;

    public List<GameObject> statusses = new List<GameObject>();


    public bool isDead = false;

    public void ResetPlayer()
    {
        playerHealth = 10f;
        healthSlider.value = 10f;
        isDead = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 10f; //maximum
        isDead = false;

        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnParticleTrigger()
    {
        
    }

    public Slider healthSlider;

    public void DamagePlayer(float damage)
    {
        if(isDead)
            return;

        if((playerHealth - damage) <= 0f)
        {
            //die
            playerHealth = 0;
            healthSlider.value = 0;
            this.Die();
            return;
        }

        GameManager.soundManager.PlaySound("hurt" + Random.Range(1, 2));

        playerHealth -= damage;

        healthSlider.value = this.playerHealth;

        GameManager.playerManager.UpdatePlayerStatus();

        TriggerHurtStatus();

    }

    public GameObject gameOverScreen;

    public GameObject screamsound;

    public void Die()
    {
        if(isDead == true)
            return;

        isDead = true;
        InputManager.canMove = false;
        InputManager.canUseInventory = false;
        InputManager.canShoot = false;
        LevelManager.ModifyPlayer();
        GameManager.soundManager.PlaySound("death");
        gameOverScreen.SetActive(true);
        //StartCoroutine(DelayDie());
        TriggerDie();

        ///DontDestroyOnLoad(screamsound);
        ///KeepPlayingDeathSound();
        ///Destroy(screamsound, 45);
    }

    public void KeepPlayingDeathSound()
    {
        foreach(AudioSource src in screamsound.GetComponents<AudioSource>())
        {
            if(src.loop == true)
            {
                src.loop = false;
            }

            if(src.playOnAwake == true)
            {
                src.playOnAwake = false;
                src.Stop();
            }
        }
    }

    public GameObject loading;

    public void Revive()
    {
        if(isDead == false)
            return;

        isDead = false;
        loading.SetActive(true);
        playerHealth = 10f;
        //ItemManager.ClearPlayerInventory();
        InputManager.canMove = true;
        InputManager.canUseInventory = true;
        InputManager.canShoot = true;
        //GameManager.soundManager.StopSound("death");
        die.SetActive(false);
        gameOverScreen.SetActive(false);
        loading.SetActive(false);
    }

    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(5f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InputManager.canMove = true;
        InputManager.canShoot = true;
        InputManager.canUseInventory = true;
        LevelManager.LoadLevel(LevelManager.currentLevel);
        GameRenderer.gameRenderer.RenderLevel();
    }

    public void TriggerHaha()
    {
        StartCoroutine(HahaDelay());
    }

    private IEnumerator HahaDelay()
    {
        haha.SetActive(true);
        yield return new WaitForSeconds(2.1f);
        haha.SetActive(false);
    }

    public void TriggerDie()
    {
        die.SetActive(true);
        LevelManager.ModifyPlayer();
        LevelManager.ModifyPlayer();
        LevelManager.ModifyPlayer();
    }

    public void TriggerHurtStatus()
    {
        StartCoroutine(HurtDelay());
    }

    private IEnumerator HurtDelay()
    {
        hurt.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hurt.SetActive(false);
    }

    public void UpdatePlayerStatus()
    {
        foreach(GameObject g in statusses)
        {
            g.SetActive(false);
        }

        if(playerHealth >= 7.5f)
        {
            if (GameRenderer.gameRenderer.isRoomDark)
            {
                oknolight.SetActive(true);
            }
            else
            {
                oklight.SetActive(true);
            }
        }
        else if(playerHealth < 7.5f && playerHealth >= 2.5f)
        {
            if (GameRenderer.gameRenderer.isRoomDark)
            {
                midnolight.SetActive(true);
            }
            else
            {
                midlight.SetActive(true);
            }
        }
        else if(playerHealth < 2.5f && playerHealth > 0f)
        {
            if (GameRenderer.gameRenderer.isRoomDark)
            {
                badnolight.SetActive(true);
            }
            else
            {
                badlight.SetActive(true);
            }
        }
    }


}
