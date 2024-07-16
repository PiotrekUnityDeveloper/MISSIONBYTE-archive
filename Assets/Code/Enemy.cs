using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject playerRaycastPoint;
    public LayerMask playerLayer;
    //public float sightDistance;

    //Enemies, yayy!

    public float atkdmg;
    public int atkspeed;

    private void Awake()
    {
        //playerRaycastPoint = Instantiate(new GameObject(), this.transform.position, Quaternion.identity);
        playerRaycastPoint = new GameObject();
        playerRaycastPoint.transform.position = this.transform.position;
    }

    void Start()
    {
        StartCoroutine(CheckForPlayerNearby());
    }

    public void ChangeMovementStatus(bool canMove)
    {
        this.GetComponent<AIPath>().canMove = canMove;
    }

    public void ChangeMoveSpeed(float moveSpeed)
    {
        this.GetComponent<AIPath>().maxSpeed = moveSpeed;
    }

    private IEnumerator CheckForPlayerNearby()
    {
        //code
        GameObject g = GetNearbyPlayer();

        /*
        if(GameRenderer.gameRenderer.isRoomDark == false)
        {
            this.ChangeMovementStatus(false);
        }*/

        this.ChangeMovementStatus(GameRenderer.gameRenderer.isRoomDark);

        if (g != null)
        {
            // PLAYER IN SIGHT
            playerRaycastPoint.transform.position = g.transform.position;
            this.GetComponent<AIDestinationSetter>().target = playerRaycastPoint.transform;
        }
        else
        {
            this.GetComponent<AIDestinationSetter>().target = playerRaycastPoint.transform;
        }

        yield return new WaitForSeconds(GameManager.settingsManager.redenemycheckdelay);
        StartCoroutine(CheckForPlayerNearby());
    }

    public GameObject GetNearbyPlayer()
    {
        List<GameObject> objectToLight = new List<GameObject>();

        int rayCount = GameManager.settingsManager.redenemyraycastcount;  // Number of raycasts
        float radius = 5f;  // Radius of the circle

        float angleIncrement = 360f / rayCount;

        outerLoop: for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleIncrement;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            RaycastHit2D[] hit = Physics2D.RaycastAll(this.transform.position, direction, GameManager.settingsManager.redenemysightdistance, playerLayer);

            foreach (RaycastHit2D rchit in hit)
            {
                if (rchit.collider != null)
                {
                     return rchit.collider.gameObject;
                }
                else
                {
                    // didnt hit
                    Debug.DrawRay(this.transform.position, direction * radius, Color.green);
                }
            }

        }

        return null;

    }

    public GameObject GetNearbyPlayerFromFarAway()
    {
        List<GameObject> objectToLight = new List<GameObject>();

        int rayCount = GameManager.settingsManager.redenemyraycastcount + 8;  // Number of raycasts
        float radius = 5f;  // Radius of the circle

        float angleIncrement = 360f / rayCount;

        outerLoop: for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleIncrement;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            RaycastHit2D[] hit = Physics2D.RaycastAll(this.transform.position, direction, GameManager.settingsManager.redenemysightdistance * 2, playerLayer);

            foreach (RaycastHit2D rchit in hit)
            {
                if (rchit.collider != null)
                {
                    return rchit.collider.gameObject;
                }
                else
                {
                    // didnt hit
                    Debug.DrawRay(this.transform.position, direction * radius, Color.green);
                }
            }

        }

        return null;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PistolBullet") && GameRenderer.gameRenderer.isRoomDark == true)
        {
            DamageEnemy(GameManager.weaponManager.GetGun(GameManager.itemManager.mainSlots[GameManager.itemManager.selectedItemID].GetComponent<ItemSlotObject>().item).attackDamage);
            Destroy(collision.gameObject);
            GameManager.soundManager.PlaySound("enemyshoot");
        }
        
    }

    private int countdownDamage = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        countdownDamage++;

        if (collision.CompareTag("Player") && GameManager.playerManager.isDead == false && countdownDamage >= this.atkspeed)
        {
            GameManager.playerManager.DamagePlayer(atkdmg);
            countdownDamage = 0;
            MoveBackwards();
        }
    }

    public float enemyBackwardsAmount = 10f;

    public void MoveBackwards()
    {
        // Calculate the movement vector
        //Vector2 movement = transform.forward * 50 * -1;

        // Apply the movement to the Rigidbody
        //this.GetComponent<Rigidbody2D>().velocity = movement;

        // Calculate the movement vector
        //Vector3 movement = transform.up * 5 * -1;

        // Apply the movement to the Transform position
        //transform.position += movement;

        // Calculate the movement vector
        Vector2 movement = transform.up * enemyBackwardsAmount * -1;

        // Apply the movement to the Transform position
        transform.Translate(movement);
    }

    public void ChangeHP(float HP)
    {
        this.hp = HP;
    }

    private float hp = 10;

    private void DamageEnemy(float damage)
    {
        this.hp -= damage;

        GameObject g = GetNearbyPlayerFromFarAway();

        if (g != null)
        {
            // PLAYER IN SIGHT
            playerRaycastPoint.transform.position = g.transform.position;
            this.GetComponent<AIDestinationSetter>().target = playerRaycastPoint.transform;
        }
        else
        {
            this.GetComponent<AIDestinationSetter>().target = playerRaycastPoint.transform;
        }

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            SpawnCombatText("-" + damage + " hp");
            PlayerDamageSound();
        }
    }

    public GameObject damageTextPrefab;

    private void Die()
    {
        SpawnCombatText(GetRandomDeathText());
        PlayerDeathSound();

        GameRenderer.gameRenderer.killedEnemies +=1 ;
        GameRenderer.gameRenderer.activeEnemies.Remove(this.gameObject);
        GameRenderer.gameRenderer.CheckForLevelCompletion();

        Destroy(this.playerRaycastPoint);
        Destroy(this.gameObject);
    }

    public GameObject damageTextObject;

    private void SpawnCombatText(string text)
    {
        GameObject g = Instantiate(damageTextPrefab, WorldToCanvasPosition(GameObject.Find("Canvas").GetComponent<RectTransform>(), Camera.main, new Vector3(this.transform.position.x + Random.Range(-2.0f, 2.0f), this.transform.position.y + Random.Range(-2.0f, 2.0f), this.transform.position.z)), Quaternion.identity);
        g.transform.parent = GameObject.Find("Canvas").transform;
        g.GetComponent<TMP_Text>().text = GetRandomDamageText();
        Destroy(g, Random.Range(2f, 4f));
        //gameobject
        GameObject g1 = Instantiate(damageTextObject, this.transform.position, Quaternion.identity);
        g1.GetComponent<Canvas>().worldCamera = Camera.main;
        g1.GetComponentInChildren<TMP_Text>().text = text;
        g1.SetActive(true);
        Destroy(g1, Random.Range(2.5f, 5.2f));
    }

    private string GetRandomDamageText()
    {
        int rnd = Random.Range(0, 10);
        switch (rnd)
        {
            case 0:
                return "that hurts >";
            case 1:
                return "UBGYWEH";
            case 2:
                return "remember me";
            case 3:
                return "UH FUCK YOU";
            case 4:
                return "bbbb..bwe.";
            case 5:
                return "*weird noise*";
            case 6:
                return "hEisO nThEW AY";
            case 7:
                return "M O N S T E R - K I L L";
            case 8:
                return "aimbot confirmed";
            case 9:
                return "seeu in hell";
            default:
                return "x";
        }
    }

    private string GetRandomDeathText()
    {
        int rnd = Random.Range(0, 10);
        switch (rnd)
        {
            case 0:
                return "ultimatum!";
                case 1:
                return "nice shot!";
                case 2:
                return " > HEADSHOT <";
                case 3:
                return "*death";
                case 4:
                return "ugh, rg";
                case 5:
                return "EXTREME TOTALITY!";
                case 6:
                return "XoX";
                case 7:
                return "FUCK YOU!";
                case 8:
                return "KILLING SPREE!";
                case 9:
                return "why u mad, bro?";
                default:
                return "i rip";
        }
    }

    private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }

    private void PlayerDamageSound()
    {
        GameManager.soundManager.PlaySound("enemydamage" + Random.Range(1, 3));
    }

    private void PlayerDeathSound()
    {
        GameManager.soundManager.PlaySound("enemydeath" + Random.Range(1, 2));
    }

}
