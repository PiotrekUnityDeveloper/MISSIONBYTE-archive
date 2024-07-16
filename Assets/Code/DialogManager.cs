using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public float characterDelay;
    //objects
    public GameObject dialogWindow;

    public TMP_Text dialogText;
    public GameObject pressEnter;

    private List<Dialog> dialogQueue = new List<Dialog>();
    private List<Item> itemsToGive = new List<Item>();
    private List<LevelObject> objectToRemove = new List<LevelObject>();

    public bool isDialogActive = false;
    public bool skippedDialog = false;

    private string currentDialogText = "";

    // Start is called before the first frame update
    void Start()
    {
        dialogWindow.SetActive(false);
        isTypingDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Return) && isYesNoDialog == true)
        {
            if (dialogWindow.activeInHierarchy == false || isTypingDialog == true)
            {
                StopCoroutine(DeployDialog(currentDialogText));
                StopCoroutine(DeployDialogInQueue());
                dialogText.text = currentDialogText;
                skippedDialog = true;
                isTypingDialog = false;
                dialogText.text = currentDialogText;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && isYesNoDialog == false)
        {
            if(dialogWindow.activeInHierarchy == false || isTypingDialog == true)
            {
                StopCoroutine(DeployDialog(currentDialogText));
                StopCoroutine(DeployDialogInQueue());
                dialogText.text = currentDialogText;
                skippedDialog = true;
                isTypingDialog = false;
                dialogText.text = currentDialogText;
                return;
            }

            dialogWindow.SetActive(false);
            StartCoroutine(DeployDialogInQueue());

            if(this.dialogQueue.Count <= 0)
            {
                //remove pickup objects
                foreach(LevelObject lvl in objectToRemove)
                {
                    LevelManager.localLevelObjects.Remove(lvl);
                }

                //give the items
                foreach(Item i in itemsToGive)
                {
                    GameManager.itemManager.GiveItemToPlayer(i);
                }

                GameRenderer.gameRenderer.SwitchEnemiesState(true);
                //InputManager.canMove = true;
                this.isDialogActive = false;
                InputManager.canUseInventory = true;

                //rerender

                GameRenderer.gameRenderer.RenderLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Y) && isYesNoDialog == true)
        {
            isYesNoDialog = false;

            if (dialogWindow.activeInHierarchy == false || isTypingDialog == true)
            {
                StopCoroutine(DeployDialog(currentDialogText));
                StopCoroutine(DeployDialogInQueue());
                dialogText.text = currentDialogText;
                skippedDialog = true;
                isTypingDialog = false;
                dialogText.text = currentDialogText;
                return;
            }

            if(yesDialogs != null)
            {
                this.AddDialogsToQueue(yesDialogs);
            }

            if(yesMethod != null)
            {
                Invoke(yesMethod, 0f);
            }            

            dialogWindow.SetActive(false);
            StartCoroutine(DeployDialogInQueue());

            if (this.dialogQueue.Count <= 0)
            {
                //remove pickup objects
                foreach (LevelObject lvl in objectToRemove)
                {
                    LevelManager.localLevelObjects.Remove(lvl);
                }

                //give the items
                foreach (Item i in itemsToGive)
                {
                    GameManager.itemManager.GiveItemToPlayer(i);
                }

                GameRenderer.gameRenderer.SwitchEnemiesState(true);
                //InputManager.canMove = true;
                this.isDialogActive = false;
                InputManager.canUseInventory = true;

                //rerender

                GameRenderer.gameRenderer.RenderLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.N) && isYesNoDialog == true)
        {
            isYesNoDialog = false;

            if (dialogWindow.activeInHierarchy == false || isTypingDialog == true)
            {
                StopCoroutine(DeployDialog(currentDialogText));
                StopCoroutine(DeployDialogInQueue());
                dialogText.text = currentDialogText;
                skippedDialog = true;
                isTypingDialog = false;
                dialogText.text = currentDialogText;
                return;
            }

            if (noDialogs != null)
            {
                this.AddDialogsToQueue(noDialogs);
            }

            if (noMethod != null)
            {
                Invoke(noMethod, 0f);
            }

            dialogWindow.SetActive(false);
            StartCoroutine(DeployDialogInQueue());

            if (this.dialogQueue.Count <= 0)
            {
                //remove pickup objects
                foreach (LevelObject lvl in objectToRemove)
                {
                    LevelManager.localLevelObjects.Remove(lvl);
                }

                //give the items
                foreach (Item i in itemsToGive)
                {
                    GameManager.itemManager.GiveItemToPlayer(i);
                }

                GameRenderer.gameRenderer.SwitchEnemiesState(true);
                //InputManager.canMove = true;
                this.isDialogActive = false;
                InputManager.canUseInventory = true;

                //rerender

                GameRenderer.gameRenderer.RenderLevel();
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            print("dialog");
            RunCustomDialog("what is this place?");
        }*/

    }

    public void AddDialogsToQueue(List<Dialog> dialogsToAdd)
    {
        foreach(Dialog d in dialogsToAdd)
        {
            this.dialogQueue.Add(d);
        }
    }

    public void AddDialogsToQueueAndDeploy(List<Dialog> dialogsToAdd)
    {
        foreach (Dialog d in dialogsToAdd)
        {
            this.dialogQueue.Add(d);
        }

        StartCoroutine(DeployDialogInQueue());
    }

    public void RunDialog(Dialog dialog)
    {
        if(isTypingDialog)
            return;

        StartCoroutine(DeployDialog(dialog.dialogText));
    }

    public void RunCustomDialog(string dialogText)
    {
        if (isTypingDialog)
            return;

        StartCoroutine(DeployDialog(dialogText));
    }

    private bool isTypingDialog = false;

    private IEnumerator DeployDialog(string message)
    {
        this.isDialogActive = true;
        this.skippedDialog = false;
        GameRenderer.gameRenderer.SwitchEnemiesState(false);
        currentDialogText = message;
        InputManager.canUseInventory = false;
        isTypingDialog = true;
        //reset old shit
        dialogText.text = "";
        pressEnter.gameObject.SetActive(false);
        dialogWindow.gameObject.SetActive(true);
        foreach(char c in message)
        {
            if (this.skippedDialog == false)
            {
                yield return new WaitForSeconds(characterDelay);
                dialogText.text = dialogText.text + c;
                GameManager.soundManager.PlaySound("typeclick");
            }
            else
            {
                dialogText.text = currentDialogText;
            }
        }
        GameManager.soundManager.PlaySound("typestop");
        pressEnter.gameObject.SetActive(true);

        isTypingDialog = false;
    }

    public bool isYesNoDialog = false;
    public List<Dialog> yesDialogs = new List<Dialog>();
    public List<Dialog> noDialogs = new List<Dialog>();
    public string yesMethod;
    public string noMethod;

    private IEnumerator DeployDialogInQueue()
    {
        if(dialogQueue.Count > 0)
        {
            isYesNoDialog = dialogQueue[0].isYesNo;

            if (dialogQueue[0].isYesNo == false)
            {
                GameRenderer.gameRenderer.SwitchEnemiesState(false);
                this.isDialogActive = true;
                this.skippedDialog = false;
                InputManager.canUseInventory = false;
                isTypingDialog = true;
                //reset old shit
                dialogText.text = "";
                pressEnter.gameObject.SetActive(false);
                dialogWindow.gameObject.SetActive(true);
                currentDialogText = dialogQueue[0].dialogText;
                foreach (char c in dialogQueue[0].dialogText)
                {
                    if (this.skippedDialog == false)
                    {
                        yield return new WaitForSeconds(characterDelay);
                        dialogText.text = dialogText.text + c;
                        GameManager.soundManager.PlaySound("typeclick");
                    }
                    else
                    {
                        dialogText.text = currentDialogText;
                    }

                }
                GameManager.soundManager.PlaySound("typestop");
                pressEnter.gameObject.GetComponent<TMP_Text>().text = "PRESS ENTER TO PROCEED >>>";
                pressEnter.gameObject.SetActive(true);
                dialogQueue.RemoveAt(0);

                isTypingDialog = false;
            }
            else
            {
                GameRenderer.gameRenderer.SwitchEnemiesState(false);
                this.isDialogActive = true;
                this.skippedDialog = false;
                InputManager.canUseInventory = false;
                isTypingDialog = true;
                //reset old shit
                dialogText.text = "";
                pressEnter.gameObject.SetActive(false);
                dialogWindow.gameObject.SetActive(true);
                currentDialogText = dialogQueue[0].dialogText;
                foreach (char c in dialogQueue[0].dialogText)
                {
                    if (this.skippedDialog == false)
                    {
                        yield return new WaitForSeconds(characterDelay);
                        dialogText.text = dialogText.text + c;
                        GameManager.soundManager.PlaySound("typeclick");
                    }
                    else
                    {
                        dialogText.text = currentDialogText;
                    }

                }

                if(dialogQueue[0].yesMethod != null)
                {
                    yesMethod = dialogQueue[0].yesMethod;
                }

                if(dialogQueue[0].noMethod != null)
                {
                    noMethod = dialogQueue[0].noMethod;
                }

                if(dialogQueue[0].ifYes != null)
                {
                    yesDialogs = dialogQueue[0].ifYes;
                }

                if(dialogQueue[0].ifNo != null)
                {
                    noDialogs = dialogQueue[0].ifNo;
                }

                GameManager.soundManager.PlaySound("typestop");
                pressEnter.gameObject.GetComponent<TMP_Text>().text = "PRESS Y FOR YES/N FOR NO >>>";
                pressEnter.gameObject.SetActive(true);
                dialogQueue.RemoveAt(0);

                isTypingDialog = false;
            }
            
        }        
    }

    public void AddItemsToGiveAfterDialog(List<Item> items)
    {
        foreach(Item i in items)
        {
            this.itemsToGive.Add(i);
        }
    }

    public void AddLevelObjectsToRemoveAfterDialog(List<LevelObject> lvlobj)
    {
        foreach(LevelObject lvl in lvlobj)
        {
            this.objectToRemove.Add(lvl);
        }
    }
}

public class Dialog
{
    public string dialogText { get; set; } = "...";
    public bool isYesNo { get; set; } = false;
    public List<Dialog> ifNo { get; set; }
    public List<Dialog> ifYes { get; set; }
    public string yesMethod { get; set; }
    public string noMethod { get; set; }
    //public string invokeMethod { get; set; } = null;
}
