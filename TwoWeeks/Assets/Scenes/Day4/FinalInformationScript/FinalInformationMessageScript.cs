using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FinalInformationMessageScript : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [TextArea]
    [SerializeField] private string finalMessage;
    [SerializeField] private UnityEvent completeMessageEvent;
    [Space]
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Start()
    {
        DateTime now = DateTime.Now;
        finalMessage += "\n" + now.ToString("d");

        StopAllCoroutines();
        StartCoroutine(AnimateMessage());
    }

    private IEnumerator AnimateMessage()
    {
        textMesh.text = "";
        int str = 0;
        while(textMesh.text != finalMessage)
        {
            textMesh.text += finalMessage[str];
            str++;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(delay);

        completeMessageEvent?.Invoke();

        yield return null;
    }
}
