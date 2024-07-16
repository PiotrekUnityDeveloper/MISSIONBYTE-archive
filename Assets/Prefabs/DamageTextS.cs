using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z += Camera.main.nearClipPlane;
        this.transform.position = new Vector3(mousePosition.x + Random.Range(-2, 2), mousePosition.y + Random.Range(-2, 2), this.transform.position.z);
        //transform.position = mousepo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
