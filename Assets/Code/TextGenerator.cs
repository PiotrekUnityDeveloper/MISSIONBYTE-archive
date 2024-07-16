using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using Figgle;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextGenerator : MonoBehaviour
{
    [TextArea(15, 20)]
    public string textToGenerate = "";
    public List<string> textLines = new List<string>();

    public GameObject textTile;
    public GameObject textTileHolder;

    [TextArea(15, 20)]
    public string defaultText = "\r\n _______   __              __                          __        __    __   ______                                              \r\n/       \\ /  |            /  |                        /  |      /  |  /  | /      \\                                             \r\n$$$$$$$  |$$/   ______   _$$ |_     ______    ______  $$ |   __ $$ |  $$ |/$$$$$$  |  ______   _____  ____    ______    _______ \r\n$$ |__$$ |/  | /      \\ / $$   |   /      \\  /      \\ $$ |  /  |$$ |__$$ |$$ | _$$/  /      \\ /     \\/    \\  /      \\  /       |\r\n$$    $$/ $$ |/$$$$$$  |$$$$$$/   /$$$$$$  |/$$$$$$  |$$ |_/$$/ $$    $$ |$$ |/    | $$$$$$  |$$$$$$ $$$$  |/$$$$$$  |/$$$$$$$/ \r\n$$$$$$$/  $$ |$$ |  $$ |  $$ | __ $$ |  $$/ $$    $$ |$$   $$<  $$$$$$$$ |$$ |$$$$ | /    $$ |$$ | $$ | $$ |$$    $$ |$$      \\ \r\n$$ |      $$ |$$ \\__$$ |  $$ |/  |$$ |      $$$$$$$$/ $$$$$$  \\       $$ |$$ \\__$$ |/$$$$$$$ |$$ | $$ | $$ |$$$$$$$$/  $$$$$$  |\r\n$$ |      $$ |$$    $$/   $$  $$/ $$ |      $$       |$$ | $$  |      $$ |$$    $$/ $$    $$ |$$ | $$ | $$ |$$       |/     $$/ \r\n$$/       $$/  $$$$$$/     $$$$/  $$/        $$$$$$$/ $$/   $$/       $$/  $$$$$$/   $$$$$$$/ $$/  $$/  $$/  $$$$$$$/ $$$$$$$/  \r\n                                                                                                                                \r\n                                                                                                                                \r\n                                                                                                                                \r\n";

    private void Awake()
    {
        if(PlayerPrefs.GetString("asciitext", "null") == "null")
        {
            PlayerPrefs.SetString("asciitext", defaultText);
        }

        

    }

    // Start is called before the first frame update
    void Start()
    {
        textToGenerate = PlayerPrefs.GetString("asciitext", defaultText);
        fontType.value = PlayerPrefs.GetInt("dropdown", 0);
        ConvertToStringList();
        GenerateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dropdown fontType;

    public void SaveDropdownState()
    {
        PlayerPrefs.SetInt("dropdown", fontType.value);
    }

    public string GenerateASCIIText(string input)
    {
        if(fontType.value == 0) //Default
        {
            return FiggleFonts.Standard.Render(input);
        }
        else if (fontType.value == 1) //3D
        {
            return FiggleFonts.ThreeD.Render(input);
        }
        else if (fontType.value == 2) //Banner
        {
            return FiggleFonts.Banner.Render(input);
        }
        else if (fontType.value == 3) //Blocks
        {
            return FiggleFonts.Blocks.Render(input);
        }
        else if (fontType.value == 4) //Caligraphy
        {
            return FiggleFonts.Caligraphy2.Render(input);
        }
        else if (fontType.value == 5) //Chunky
        {
            return FiggleFonts.Chunky.Render(input);
        }
        else if (fontType.value == 6) //Contessa
        {
            return FiggleFonts.Contessa.Render(input);
        }
        else if (fontType.value == 7) //Cyberlarge
        {
            return FiggleFonts.CyberLarge.Render(input);
        }
        else if (fontType.value == 8) //Lean
        {
            return FiggleFonts.Lean.Render(input);
        }
        else if (fontType.value == 9) //Pepper
        {
            return FiggleFonts.Pepper.Render(input);
        }
        else if (fontType.value == 10) //Larry3d
        {
            return FiggleFonts.Larry3d.Render(input);
        }
        else if (fontType.value == 11) //Henry3d
        {
            return FiggleFonts.Henry3d.Render(input);
        }

        return FiggleFonts.Standard.Render(input);
    }

    public InputField textToConvert;

    public void GenerateOwnText()
    {
        if (string.IsNullOrEmpty(textToConvert.text))
        {
            PlayerPrefs.SetString("asciitext", defaultText);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            PlayerPrefs.SetString("asciitext", GenerateASCIIText(textToConvert.text));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ConvertToStringList()
    {
        if(textToGenerate != null)
        {
            textLines.Clear();
            textLines = ToStringArray(textToGenerate).ToList();
        }
    }

    public static string[] ToStringArray(string text)
    {
        if (String.IsNullOrEmpty(text) == false)
            return text.Split(new string[] { Environment.NewLine },
                StringSplitOptions.None);
        else
        {
            return new string[0];
        }
    }

    public List<GameObject> textTiles = new List<GameObject>();

    public void GenerateText()
    {
        if(!textTile)
            return;

        int currentItemIndex = 0;
        //initial
        int startX = 0;
        int startY = 0;
        //used
        int currentX = startX;
        int currentY = startY;
        float currentXPosition = 0; //dynamic

        //print("number of lines: " + GetNumberOfLines(textToGenerate));
        //print("length of line: " + GetLine(textToGenerate, 4).Length);

        //print(textLines.Count.ToString());

        textTiles.Clear();

        for (int i = 0; i < /*GetNumberOfLines(textToGenerate)*/textLines.Count + 1; i++)
        {
            try
            {
                for (int j = 0; j < /*GetLine(textToGenerate, 4).Length*/ textLines[i].Length - 1; j++)
                {
                    GameObject tile = Instantiate(textTile, new Vector2(currentXPosition, (currentY * this.textTile.GetComponent<BoardTile>().Background.transform.localScale.y)), Quaternion.identity);
                    tile.name = /*X-x-Y*/ j + "x" + i;
                    tile.transform.parent = textTileHolder.transform;
                    tile.GetComponent<BoardTile>().Background.GetComponent<SpriteRenderer>().color = Color.black;

                    

                    try
                    {
                        //print(textLines[i].Length);

                        tile.GetComponent<BoardTile>().ChangeChar(GetCharAtIndex(textLines[i], j));
                    }
                    catch { }

                    tile.GetComponent<BoardTile>().ChangeTextColor(/*GameManager.settingsManager.defMatrixColor*/Color.black);
                    tile.GetComponent<BoardTile>().myXPos = j;
                    tile.GetComponent<BoardTile>().myYPos = i;
                    tile.gameObject.SetActive(true);

                    textTiles.Add(tile);

                    currentItemIndex++; //independent

                    currentX += 1;
                    currentXPosition = (currentX * (tile.GetComponent<BoardTile>().Background.transform.localScale.x));



                }

                currentX = 0;
                currentXPosition = 0;

                currentY -= 1;
            }
            catch { }
            
        }

        Camera.main.gameObject.transform.position = GetPositionBetweenObjects(textTiles[0].transform, textTiles[textTiles.Count - 1].transform);
        Camera.main.gameObject.transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, -10);
    }

    public Vector2 GetPositionBetweenObjects(Transform object1, Transform object2)
    {
        // Calculate the midpoint between the two objects
        Vector2 position = (object1.position + object2.position) / 2f;

        return position;
    }

    private string GetLine(string text, int lineNo)
    {
        string[] lines = text.Replace("\r", "").Split('\n');
        return lines.Length >= lineNo ? lines[lineNo - 1] : null;
    }

    private int GetNumberOfLines(string text)
    {
        return text.Split('\n').Length;        
    }

    private char GetCharAtIndex(string text, int index)
    {
        return text[index];
    }

}
