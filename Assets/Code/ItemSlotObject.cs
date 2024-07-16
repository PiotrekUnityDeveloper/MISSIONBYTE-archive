using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlotObject : MonoBehaviour
{
    public TMP_Text itemTextIcon;
    public Item item;
    public ItemSlot itemSlot;

    public GameObject itemOverview;
    //public GameObject itemOverviewPrefab;

    private void Awake()
    {
        this.item = new Item();
        this.itemSlot = new ItemSlot();

        this.item.name = "null";

        this.LoadItem(null);
    }

    private void Start()
    {

    }

    public float overviewOffset;

    private void Update()
    {

    }

    public void ShowOverview()
    {
        if(this.item == null)
            return;

        if(this.item.name == "null")
            return;

        itemOverview.SetActive(true);

        //sync info
        itemOverview.transform.GetComponent<ItemOverlay>().itemName.text = this.item.name;
        itemOverview.transform.GetComponent<ItemOverlay>().itemIcon.text = this.item.icon;
        itemOverview.transform.GetComponent<ItemOverlay>().itemIcon.color = this.item.iconcolor;
        itemOverview.transform.GetComponent<ItemOverlay>().itemDescription.text = this.item.description;

    }

    public void HideOverview()
    {
        if (itemOverview == null)
            return;

        itemOverview.gameObject.SetActive(false);
    }

    public void InitItemSlot(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        itemSlot.itemObject = this.gameObject;

        
        if(this.item != null)
        {
            this.itemSlot.item = this.item;
        }
    }

    public void LoadItem(Item item)
    {
        if(item == null || item.name == "null")
        {

            this.item = null;
            this.itemTextIcon.text = null;
            this.itemTextIcon.color = Color.white;

            if (this.itemSlot == null)
                return;

            //item slot
            this.itemSlot.item = null;
        }
        else
        {
            this.item = item;
            this.itemTextIcon.text = item.icon;
            this.itemTextIcon.color = item.iconcolor;

            //item slot
            this.itemSlot.item = item;
        }
    }

    public void SendItem() // pick/move item up
    {
        /*if(this.itemSlot == null || this.item == null)
            return;*/

        //GameManager.itemManager.MoveItem(this.itemSlot, this.item);
        GameManager.itemManager.MoveItem(this.itemSlot, this.item);
    }

    public void RecieveItem() // place item down
    {
        /*if (this.itemSlot == null || this.item == null)
            return;*/

        //GameManager.itemManager.UnMoveItem(this.itemSlot, GameManager.itemManager.pickedItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("dragitem") && Input.GetMouseButtonUp(0))
        {
            GameManager.itemManager.UnMoveItem(this.itemSlot, GameManager.itemManager.pickedItem);
        }
    }

}
