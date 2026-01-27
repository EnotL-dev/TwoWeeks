using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScreen;
    [Space]
    [SerializeField] private UnityEvent completeFadeTransitEvents;
    [SerializeField] private UnityEvent completeShowTransitEvents;

    private void Start()
    {
        StartFadeTransition();
    }

    public void StartFadeTransition()
    {
        StopAllCoroutines();
        StartCoroutine(FadeSCreen());
    }

    public void StartShowTransition()
    {
        StopAllCoroutines();
        StartCoroutine(ShowScreen());
    }

    private IEnumerator FadeSCreen()
    {
        fadeScreen.alpha = 1;
        while (fadeScreen.alpha > 0)
        {
            fadeScreen.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        completeFadeTransitEvents?.Invoke();

        yield return null;
    }

    private IEnumerator ShowScreen()
    {
        while (fadeScreen.alpha < 1)
        {
            fadeScreen.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        completeShowTransitEvents?.Invoke();

        yield return null;
    }
}
