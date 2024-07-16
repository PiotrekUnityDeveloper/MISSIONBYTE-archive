using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(GameManager.settingsManager.bulletLifeTime);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
