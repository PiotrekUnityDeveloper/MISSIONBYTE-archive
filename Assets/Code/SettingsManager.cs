using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    /*
    [MenuItem("Piotrek4Games/Credits")]
    static void Init()
    {
        SettingsManager window = (SettingsManager)GetWindow(typeof(SettingsManager));
        window.Show();
    }*/

    [InspectorLabel("Piotrek4Games")]

    //visual
    [Header("Visual")]
    [SerializeField] public bool smoothMovement;
    [SerializeField] public float smoothTime;
    [SerializeField] public float preRenderTime;
    [SerializeField] public float smoothPushTime;
    [SerializeField] public float errorSmoothTime;

    [SerializeField] public bool randomMatrixEffect;
    [SerializeField] public float matrixFlashTime;
    [SerializeField] public float matrixMinDelay;
    [SerializeField] public float matrixMaxDelay;
    [SerializeField] public Color defMatrixColor;
    [SerializeField] public bool staticMatrixColor;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeRMin;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeRMax;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeGMin;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeGMax;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeBMin;
    [Range(0, 255)] [SerializeField] public float matrixColorRangeBMax;
    [Range(0, 255)] [SerializeField] public float opacityColorRangeMin;
    [Range(0, 255)] [SerializeField] public float opacityColorRangeMax;

    [SerializeField] public float errorMaxOffset;
    [SerializeField] public bool flashMovementError;
    [SerializeField] public float flashMovementErrorTime;

    [SerializeField] public float itemAnimationPickupTime;
    [SerializeField] public float itemAnimationTime;

    [SerializeField] public float pistolBulletTrailTime;
    [SerializeField] public float bulletLifeTime;
    [SerializeField] public float pistolBulletSpeed;

    [SerializeField] public int lightRenderQuality;

    [SerializeField] public Color redenemyColor;
    [SerializeField] public int redenemyraycastcount;
    [SerializeField] public float redenemycheckdelay;
    [SerializeField] public float redenemysightdistance;

    //gameplay
    [Header("Gameplay")]
    [SerializeField] public bool renderEveryMove;

    //behaviour
    [Header("Behaviour")]
    [SerializeField] public bool canPushMultipleCrates;
}
