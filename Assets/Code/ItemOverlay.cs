using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemOverlay : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemIcon;
    public TMP_Text itemDescription;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y += 2f;
        mousePosition.x -= 3f;

        if(mousePosition.y > -2.14f)
        {
            mousePosition.y = -2.14f;
        }

        //print(mousePosition.y + " IS THE MOUSEPOS");

        transform.position = new Vector3(mousePosition.x, mousePosition.y, this.transform.position.z);
    }
}
