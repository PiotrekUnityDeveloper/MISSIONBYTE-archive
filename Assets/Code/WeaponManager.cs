using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Transactions;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        canShootPistol = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject pistolBullet;

    private bool canShootPistol = true;

    private float currentShootDelay = 0.5f;

    private IEnumerator DelayNextShot(float delay, int gunID)
    {
        if(gunID == 0) //pistol
        {
            canShootPistol = false;
            yield return new WaitForSeconds(delay);
            canShootPistol = true;
        }
    }

    public void Shoot()
    {
        if (GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item != null)
        {
            // PISTOL (preferably desert eagle but nvm)

            if (GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.GetType() == typeof(Gun)
            && GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.name == "EZ DDOSER 3000")
            {
                if(canShootPistol == false)
                    return;

                GameObject bullet = Instantiate(pistolBullet, GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).transform.position, Quaternion.identity);
                //rotate towards mouse
                Camera mainCamera = Camera.main;
                Vector3 mousePosition = Input.mousePosition;
                Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, bullet.transform.position.z - mainCamera.transform.position.z));
                Vector3 direction = mousePositionWorld - bullet.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);
                bullet.transform.rotation = rotation;
                //apply force
                //bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.forward * 4 * Time.deltaTime);
                Vector3 direction1 = mousePositionWorld - bullet.transform.position;
                direction1.Normalize();
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * GameManager.settingsManager.pistolBulletSpeed);

                //delay next shot
                this.currentShootDelay = GetGun(GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item).shootDelay;
                StartCoroutine(DelayNextShot(currentShootDelay, 0));

                PlayPistolShootSound();

                StartCoroutine(RerenderPlayer());
            }

            // SACK OF LEAVES

            if (GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.GetType() == typeof(Gun)
            && GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.name == "Sack of Leaves")
            {
                GameObject bullet = Instantiate(pistolBullet, GameRenderer.gameRenderer.GetGameObjectByTilePosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos).transform.position, Quaternion.identity);
                //rotate towards mouse
                Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(bullet.transform.position);
                Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
                float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                //apply force
                bullet.GetComponent<Rigidbody2D>().velocity = mouseOnScreen;
            }
        }
    }

    public Gun GetGun(Item item)
    {
        return (Gun)GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item;
    }

    private void PlayPistolShootSound()
    {
        GameManager.soundManager.PlaySound("pistolshoot1");
        StartCoroutine(PlayPistolSlideSound());
        StartCoroutine(PlayPistolBulletWhizSound());
    }

    private IEnumerator PlayPistolSlideSound()
    {
        yield return new WaitForSeconds(currentShootDelay / 2);
        GameManager.soundManager.PlaySound("pistolslide" + Random.Range(1, 7));
    }

    private IEnumerator PlayPistolBulletWhizSound()
    {
        float time = Random.Range(0f, currentShootDelay / 2);
        yield return new WaitForSeconds(time);
        GameManager.soundManager.PlaySound("bulletwhiz" + Random.Range(1, 3));
    }

    private IEnumerator RerenderPlayer()
    {
        ///yield return new WaitForSeconds(0.2f);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///yield return new WaitForSeconds(0.2f);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        yield return new WaitForSeconds(0.5f);
        GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        GameRenderer.gameRenderer.RenderLevel();
        //GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        //GameRenderer.gameRenderer.RenderLightAtPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        //GameRenderer.gameRenderer.RenderLightAtPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void UpdateHeldItemPosition(string lastMove)
    {
        if(GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item != null)
        {
            if (GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.GetType() == typeof(Gun)
            && GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item.name == "Desert Eagle")
            {
                switch (lastMove)
                {
                    case "up":
                        LevelManager.playerHeldItem.xpos = LevelManager.mainplayer.xpos;
                        LevelManager.playerHeldItem.ypos = LevelManager.mainplayer.ypos - 2;
                        LevelManager.playerHeldItem.displaycharacter = "^";
                        break;
                    case "down":
                        LevelManager.playerHeldItem.xpos = LevelManager.mainplayer.xpos;
                        LevelManager.playerHeldItem.ypos = LevelManager.mainplayer.ypos + 2;
                        LevelManager.playerHeldItem.displaycharacter = "v";
                        break;
                    case "right":
                        LevelManager.playerHeldItem.xpos = LevelManager.mainplayer.xpos + 2;
                        LevelManager.playerHeldItem.ypos = LevelManager.mainplayer.ypos;
                        LevelManager.playerHeldItem.displaycharacter = ">";
                        break;
                    case "left":
                        LevelManager.playerHeldItem.xpos = LevelManager.mainplayer.xpos - 2;
                        LevelManager.playerHeldItem.ypos = LevelManager.mainplayer.ypos;
                        LevelManager.playerHeldItem.displaycharacter = "<";
                        break;
                }

            }
        }
        else
        {
            LevelManager.playerHeldItem.xpos = 0;
            LevelManager.playerHeldItem.ypos = 0;
            LevelManager.playerHeldItem.displaycharacter = " ";
        }   

    }

}
