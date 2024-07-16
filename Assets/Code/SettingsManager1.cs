using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager1 : MonoBehaviour
{
    /*
    [MenuItem("Piotrek4Games/Credits")]
    static void Init()
    {
        SettingsManager window = (SettingsManager)GetWindow(typeof(SettingsManager));
        window.Show();
    }*/

    

    //visual
    [Header("Visual")]
    [SerializeField] public static bool smoothMovement = true;
    [SerializeField] public static float smoothTime = 1f;
    [SerializeField] public static float preRenderTime = 1f;
    [SerializeField] public static float smoothPushTime = 1f;
    [SerializeField] public static float errorSmoothTime = 0.25f;

    [SerializeField] public static bool randomMatrixEffect = true;
    [SerializeField] public static float matrixFlashTime = 0.72f;
    [SerializeField] public static float matrixMinDelay = 0.72f;
    [SerializeField] public static float matrixMaxDelay = 1.72f;
    [SerializeField] public static Color defMatrixColor = Color.green;
    [SerializeField] public static bool staticMatrixColor = true;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeRMin = 0;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeRMax = 0;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeGMin = 40;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeGMax = 255;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeBMin = 0;
    [Range(0, 255)] [SerializeField] public static float matrixColorRangeBMax = 0;
    [Range(0, 255)] [SerializeField] public static float opacityColorRangeMin = 0;
    [Range(0, 255)] [SerializeField] public static float opacityColorRangeMax = 0;

    [SerializeField] public static float errorMaxOffset = 0.25f;
    [SerializeField] public static bool flashMovementError = true;
    [SerializeField] public static float flashMovementErrorTime = 0.25f;

    //gameplay
    [Header("Gameplay")]
    [SerializeField] public static bool renderEveryMove = false;

    //behaviour
    [Header("Behaviour")]
    [SerializeField] public static bool canPushMultipleCrates = true;

#if UNITY_EDTIOR
    private void OnGUI()
    {
        //matrix RED
        EditorGUILayout.LabelField("matrix R min:", matrixColorRangeRMin.ToString());
        EditorGUILayout.LabelField("matrix R max:", matrixColorRangeRMax.ToString());
        EditorGUILayout.MinMaxSlider(ref matrixColorRangeRMin, ref matrixColorRangeRMax, 0, 255);
        //matrix GREEN
        EditorGUILayout.LabelField("matrix G min:", matrixColorRangeGMin.ToString());
        EditorGUILayout.LabelField("matrix G max:", matrixColorRangeGMax.ToString());
        EditorGUILayout.MinMaxSlider(ref matrixColorRangeGMin, ref matrixColorRangeGMax, 0, 255);
        //matrix BLUE
        EditorGUILayout.LabelField("matrix B min:", matrixColorRangeBMin.ToString());
        EditorGUILayout.LabelField("matrix B max:", matrixColorRangeBMax.ToString());
        EditorGUILayout.MinMaxSlider(ref matrixColorRangeBMin, ref matrixColorRangeBMax, 0, 255);
    }
#endif
}
