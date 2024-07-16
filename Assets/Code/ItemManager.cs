using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public Canvas Canvas;
    public Transform inventoryWindow;

    public List<ItemSlotObject> itemSlotObjects = new List<ItemSlotObject>();
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public List<Item> items = new List<Item>();

    public Item pickedItem;
    public ItemSlot lastItemSlot;

    public GameObject selectionArrow;
    public List<ItemSlotObject> mainSlots = new List<ItemSlotObject>();
    public int selectedItemID = 0;

    public void SwitchSelectedItem(bool next)
    {
        if (next)
        {
            if(selectedItemID >= mainSlots.Count - 1)
            {
                selectedItemID = 0;
            }
            else
            {
                selectedItemID++;
            }
        }
        else
        {
            if (selectedItemID <= 0)
            {
                selectedItemID = mainSlots.Count - 1;
            }
            else
            {
                selectedItemID--;
            }
        }

        GameManager.soundManager.PlaySound("selectclick");

        selectionArrow.gameObject.transform.position = new Vector2(mainSlots[selectedItemID].gameObject.transform.position.x, selectionArrow.transform.position.y);
        //GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);

        StartCoroutine(RerenderPlayer());
    }

    private IEnumerator RerenderPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///yield return new WaitForSeconds(0.2f);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///yield return new WaitForSeconds(0.2f);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.playerHeldItem.xpos, LevelManager.playerHeldItem.ypos);
        ///GameRenderer.gameRenderer.RenderSpecificPosition(LevelManager.mainplayer.xpos, LevelManager.mainplayer.ypos);
    }

    public void ToggleInventory(bool show)
    {
        inventoryWindow.gameObject.SetActive(show);
        isInventoryOpened = inventoryWindow.gameObject.activeInHierarchy;

        if (isInventoryOpened)
        {
            GameManager.soundManager.PlaySound("invopen");
        }
        else
        {
            GameManager.soundManager.PlaySound("invclose");
        }
    }

    public void AutoToggleInventory()
    {
        inventoryWindow.gameObject.SetActive(inventoryWindow.gameObject.activeInHierarchy ? false : true);
        isInventoryOpened = inventoryWindow.gameObject.activeInHierarchy;

        if (isInventoryOpened)
        {
            GameManager.soundManager.PlaySound("invopen");
        }
        else
        {
            GameManager.soundManager.PlaySound("invclose");
        }
    }

    public bool isInventoryOpened;

    private void PlaceItemOnStart()
    {
        //GiveItemToPlayer(new Gun { name = "Remington XP-100", attackDamage = 3.0f, icon = "->", iconcolor = Color.white, shootDelay = 0.8f, magazineSize = 7, description = "Small Sized Handgun \n\r.308 Winchester\n\rMFR 289m (approx. 245m)\n\rType: single slide shot (man-op)\n\r \n\r Broken sight" });
    }

    public void GiveItemToPlayer(Item item)
    {
        foreach (ItemSlot itemslot in itemSlots)
        {
            if (itemslot.item == null || itemslot.item.name == "null")
            {
                itemslot.itemObject.GetComponent<ItemSlotObject>().LoadItem(item);
                items.Add(item);
                break;
            }
        }

        SaveInv();
    }

    public static List<Item> itemsOwned = new List<Item>();

    public static void SavePlayerInventory()
    {
        itemsOwned.Clear();

        foreach(ItemSlotObject iso in GameManager.itemManager.itemSlotObjects)
        {
            if(iso == null)
                continue;

            if(iso.item != null && (iso.item.name == null || iso.item.name == "null"))
                return;

            itemsOwned.Add(iso.item);
        }

        print(itemsOwned.Count);
    }

    public static void LoadPlayerInventory()
    {
        ClearPlayerInventory();

        foreach(Item i in itemsOwned)
        {
            GameManager.itemManager.GiveItemToPlayer(i);
        }

        print(itemsOwned.Count);
    }

    public static List<ItemSlotObject> isos = new List<ItemSlotObject>();
    public void SaveInv()
    {
        isos.Clear();

        foreach(ItemSlotObject iso in itemSlotObjects)
        {
            isos.Add(new ItemSlotObject { item = iso.item, itemSlot = iso.itemSlot, itemTextIcon = iso.itemTextIcon, itemOverview = iso.itemOverview, overviewOffset = iso.overviewOffset });
        }

    }

    public void LoadInv()
    {
        int itemIndex = 0;

        foreach(ItemSlotObject iso in itemSlotObjects)
        {
            iso.LoadItem(itemSlotObjects[itemIndex].item);
            itemIndex++;
        }

        /*
        itemSlotObjects.Clear();
        itemSlots.Clear();

        foreach(ItemSlotObject iso in isos)
        {
            ItemSlot itmslot = new ItemSlot { itemObject = iso.gameObject, isLocked = false, itemSlotState = ItemSlotState.Default, item = null };
            itemSlotObjects.Add(iso);
            itemSlots.Add(itmslot);
            iso.LoadItem(iso.item);
        }*/
    }

    public static void ClearPlayerInventory()
    {
        foreach (ItemSlot itemslot in GameManager.itemManager.itemSlots)
        {
            itemslot.item = null;
        }

        foreach(ItemSlotObject iso in GameManager.itemManager.itemSlotObjects)
        {
            iso.LoadItem(null);
        }

        GameManager.itemManager.items.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryWindow.gameObject.SetActive(true);

        foreach (ItemSlotObject itemSlotObject in itemSlotObjects)
        {
            ItemSlot itmslot = new ItemSlot { itemObject = itemSlotObject.gameObject, isLocked = false, itemSlotState = ItemSlotState.Default, item = null };
            itemSlotObject.InitItemSlot(itmslot);
            this.itemSlots.Add(itmslot);
            itemSlotObject.LoadItem(null);
        }

        PlaceItemOnStart();

        inventoryWindow.gameObject.SetActive(false);
    }

    private Vector3 mousePos = new Vector3(); 

    // Update is called once per frame
    void Update()
    {
        if(inventoryWindow.gameObject.activeInHierarchy == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                SwitchSelectedItem(false);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                SwitchSelectedItem(true);
            }
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z += Camera.main.nearClipPlane;
        mousePos = mousePosition;

        if (MovingItem != null && MovableItemAnimating == false)
        {
            MovingItem.transform.position = mousePosition;
        }

        if(Input.GetMouseButtonUp(0) && MovingItem != null && MovableItemAnimating == false)
        {
            //StartCoroutine(CheckForItem());
        }
                
    }

    private int itemCount;

    private IEnumerator CheckForItem()
    {
        //itemCount = items.Count;

        yield return new WaitForSeconds(GameManager.settingsManager.itemAnimationTime + 0.1f);

        if(MovingItem != null && itemCount == items.Count)
        {
            this.UnMoveItem(lastItemSlot, pickedItem);
        }

    }

    public GameObject MovableItem; // prefab
    private static GameObject MovingItem; // keeps track so only one item can be moved at a time

    private bool MovableItemAnimating = false;

    public void MoveItem(ItemSlot itemSlot, Item item) // used when player takes an item from itemslot
    {
        if(MovingItem != null)
            return;

        if(itemSlot.isLocked == true)
            return;

        if(itemSlot.itemSlotState != ItemSlotState.Default)
            return;

        if (itemSlot.itemSlotState == ItemSlotState.Input)
            return;

        if (item == null)
            return;

        StopCoroutine(CheckForItem());

        GameManager.soundManager.PlaySound("pickupinvitem");

        MovingItem = Instantiate(MovableItem, itemSlot.itemObject.transform.position, Quaternion.identity);
        MovingItem.GetComponent<TMP_Text>().text = item.icon;
        MovingItem.GetComponent<TMP_Text>().color = item.iconcolor;
        MovingItem.transform.localScale = new Vector3(1, 1, 100);
        MovingItem.transform.parent = inventoryWindow.transform;
        MovingItem.transform.localScale = new Vector3(1, 1, 100);
        pickedItem = item;
        lastItemSlot = itemSlot;
        //take item from the itemslotobject
        itemSlot.itemObject.GetComponent<ItemSlotObject>().LoadItem(null);
        MovableItemAnimating = true;
        Vector3 pos = mousePos;
        //StopCoroutine(CheckForItem());
        itemCount = items.Count;
        items.Remove(item);
        StartCoroutine(MoveToTarget(MovingItem.transform.position, pos, GameManager.settingsManager.itemAnimationPickupTime, MovingItem));
    }

    public void UnMoveItem(ItemSlot itemSlot, Item item) // used when player places an item taken from another slot
    {
        if(MovingItem == null)
            return;

        if (itemSlot.isLocked == true)
            return;

        if (itemSlot.itemSlotState != ItemSlotState.Default)
            return;

        if (itemSlot.itemSlotState == ItemSlotState.Output)
            return;

        if (itemSlot.itemObject.GetComponent<ItemSlotObject>().item != null)
            return;

        GameManager.soundManager.PlaySound("dropinvitem");
        MovableItemAnimating = true;
        //StopCoroutine(CheckForItem());
        Vector3 pos = itemSlot.itemObject.transform.position;
        StartCoroutine(MoveToTarget(MovingItem.transform.position, pos, GameManager.settingsManager.itemAnimationTime, MovingItem));
        //StopCoroutine(CheckForItem());
        StartCoroutine(PlaceItemInSlot(itemSlot, item));
        items.Add(item);
    }

    public void UnMoveItemWithoutAnimation(ItemSlot itemSlot, Item item) // used when player places an item taken from another slot
    {
        if (MovingItem == null)
            return;

        if (itemSlot.isLocked == true)
            return;

        if (itemSlot.itemSlotState != ItemSlotState.Default)
            return;

        if (itemSlot.itemSlotState == ItemSlotState.Output)
            return;

        if (itemSlot.itemObject.GetComponent<ItemSlotObject>().item != null)
            return;

        //MovableItemAnimating = true;
        //StopCoroutine(CheckForItem());
        Vector3 pos = itemSlot.itemObject.transform.position;
        //StartCoroutine(MoveToTarget(MovingItem.transform.position, pos, GameManager.settingsManager.itemAnimationTime, MovingItem));
        //StopCoroutine(CheckForItem());
        PlaceItem(itemSlot, item);
    }

    private IEnumerator MoveToTarget(Vector3 initialPosition, Vector3 targetPosition, float moveDuration, GameObject g)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            if(g != null)
            {
                g.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }

        //g.transform.position = targetPosition;

        MovableItemAnimating = false;
    }

    private IEnumerator PlaceItemInSlot(ItemSlot itemSlot, Item item)
    {
        yield return new WaitForSeconds(GameManager.settingsManager.itemAnimationTime);
        PlaceItem(itemSlot, item);
    }

    public void PlaceItem(ItemSlot itemSlot, Item item)
    {
        itemSlot.itemObject.gameObject.GetComponent<ItemSlotObject>().LoadItem(item);
        Destroy(MovingItem);
    }


}

//INVENTORY

public enum ItemSlotState
{
    Input, //can place item, not take
    Output, //can take item, not place
    Default, //can take and store
}

public class ItemSlot
{
    public GameObject itemObject { get; set; }
    public Item item { get; set; }
    public int slotID { get; set; }

    //behaviour
    public bool isLocked { get; set; } = false;
    public ItemSlotState itemSlotState { get; set; } = ItemSlotState.Default;

    public bool ResetItem()
    {
        item = null;
        return (item == null) ? false : true;
    }
}

// ITEMS

public class Item
{
    public string name { get; set; }
    public string icon { get; set; }
    public Color iconcolor { get; set; } = Color.white;
    public string description { get; set; }
    public int rarity { get; set; } //optional
}

public class MeeleWeapon : Item
{
    public float attackDamage { get; set; }
    public float useDelay { get; set; }
}

public class Gun : Item
{
    public float attackDamage { get; set; }
    public float magazineSize { get; set; }
    public float shootDelay { get; set; }
}
