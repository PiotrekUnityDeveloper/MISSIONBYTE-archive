using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> localMatrixAnimations = new List<GameObject>();
    [SerializeField] private static List<GameObject> matrixAnimations = new List<GameObject>();

    private void Awake()
    {
        matrixAnimations = localMatrixAnimations;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AnimateMatrixEffect(bool random)
    {
        //animate the matrix effect

        if(random == true)
        {
            int randomAnimation = UnityEngine.Random.Range(0, matrixAnimations.Count);
            matrixAnimations[randomAnimation].gameObject.SetActive(true);
            matrixAnimations[randomAnimation].GetComponent<Animator>().SetTrigger("play");
        }else if(random == false)
        {
            matrixAnimations[0].gameObject.SetActive(true);
            matrixAnimations[0].GetComponent<Animator>().SetTrigger("play");
        }
    }

    public IEnumerator MoveToTargetPosition(Vector3 initialPosition, Vector3 targetPosition, float moveDuration, Transform targetObject)
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exact
        targetObject.transform.position = targetPosition;
    }

    public IEnumerator MoveToTargetPositionThenReset(Vector3 initialPosition, Vector3 targetPosition, float moveDuration, Transform targetObject)
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            targetObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Instantly reset the position after the animation is done
        targetObject.transform.position = initialPosition;
    }
}
