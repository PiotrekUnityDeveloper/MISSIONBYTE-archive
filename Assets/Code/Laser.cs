using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float rotationSpeed; // it will rotate every...
    public int rotationCut; // max rotations possible (value same as rayCount)
    public float laserReach;
    public Color laserColor;
    public float laserDamage;

    public int rotationOffset; // used for rotation

    public bool isStatic;

    public bool runOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if(isStatic == false && runOnStart)
        {
            StartCoroutine(RotateLaser());
        }
    }

    public void RunLaser()
    {
        if(this.isStatic)
            return;

        StartCoroutine(RotateLaser());
    }

    public void ForceRunLaser()
    {
        StartCoroutine(RotateLaser());
    }

    private IEnumerator RotateLaser()
    {
        if(isStatic == false)
        {
            //print("DRAWING LASER...  " + this.rotationOffset);
            GameRenderer.gameRenderer.ProjectLasersOnGameObjects(this.gameObject, rotationCut, laserReach, rotationOffset, laserColor, false, laserDamage);
            yield return new WaitForSeconds(rotationSpeed);
            this.rotationOffset += 1;

            //print("rotation offset: " + rotationOffset);

            if(rotationOffset >= rotationCut * 2)
            {
                rotationOffset = 0;
            }

            StartCoroutine(RotateLaser());
        }
    }

    public void UpdateManually()
    {
        GameRenderer.gameRenderer.ProjectLasersOnGameObjects(this.gameObject, rotationCut, laserReach, rotationOffset, laserColor, false, laserDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
