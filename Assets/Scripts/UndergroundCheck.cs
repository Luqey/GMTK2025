using System.Collections;
using UnityEngine;

public class UndergroundCheck : MonoBehaviour
{
    [SerializeField] MainCameraScript mainCameraScript;
    [SerializeField] float undergroundOffset;

    [SerializeField] float transitionSpeed;
    private float upperOffset;
    //private bool isTransitioning = false;

    private Coroutine currentCoroutine;

    private void Start()
    {
        upperOffset = mainCameraScript.yOffset;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartOffsetTransition(undergroundOffset);
            AudioManager.instance.isUnderground = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartOffsetTransition(upperOffset);
            AudioManager.instance.isUnderground = false;
        }
    }

    void OnDisable()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    void StartOffsetTransition(float targetOffset)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ChangeYOffset(mainCameraScript.yOffset, targetOffset));
    }

    IEnumerator ChangeYOffset(float startOffset, float targetOffset)
    {
        float elapsedTime = 0f;
        float duration = Mathf.Abs(targetOffset - startOffset) / transitionSpeed;

        while (elapsedTime < duration)
        {
            if (mainCameraScript == null) yield break;

            elapsedTime += Time.deltaTime;
            float newOffset = Mathf.Lerp(startOffset, targetOffset, elapsedTime / duration);
            mainCameraScript.yOffset = newOffset;
            yield return null;
        }

        if (mainCameraScript != null)
        {
            mainCameraScript.yOffset = targetOffset;
        }
    }
}
