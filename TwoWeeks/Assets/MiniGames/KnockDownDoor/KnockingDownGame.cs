using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KnockingDownGame : MonoBehaviour
{
    [System.Serializable]
    public class TipMessage
    {
        public LanguageIndex languageIndex;
        public string mes;
    }

    [SerializeField] private List<TipMessage> tipMessages;
    [SerializeField] private TextMeshProUGUI textTip;
    [SerializeField] private CanvasGroup canvasGroup;
    [Space]
    [SerializeField] private int countCnock;
    [SerializeField] private float rollback = 2f;
    [SerializeField] private UnityEvent completeEvents;

    private float timer = 0;
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(ShowTip());
            }
        }

        if(Input.GetKeyDown(KeyCode.E) && timer <= 0)
        {
            timer = rollback;
            countCnock--;

            StopAllCoroutines();
            StartCoroutine(FadeTip());

            if (countCnock <= 0)
                EndGame();
        }
    }

    private void Start()
    {
        var tipMes = tipMessages.FirstOrDefault(index => index.languageIndex == Main.MainManagers.settingsManager.languageIndex);
        textTip.text = tipMes.mes;
    }

    private void EndGame()
    {
        completeEvents?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator FadeTip()
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    private IEnumerator ShowTip()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }
}
