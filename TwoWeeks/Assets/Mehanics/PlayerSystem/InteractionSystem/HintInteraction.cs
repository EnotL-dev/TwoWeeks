using TMPro;
using UnityEngine;
using System.Collections;

namespace InteractionSystem
{
    public class HintInteraction : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private float activationDistance = 4f;
        [SerializeField] private float fadeSpeed = 2f;

        private Transform playerTransform;
        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;

        private void Start()
        {
            playerTransform = Main.MainControllers.playerController.moving.transform;

            string value = GetComponent<ItemObject>().item.GetName();
            if (value != null)
                textMesh.text = $"E - {value}"; 
        }

        private void Update()
        {
            if (playerTransform == null || textMesh == null) return;

            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= activationDistance)
            {
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine);
                    fadeOutCoroutine = null;
                }

                if (fadeInCoroutine == null)
                {
                    fadeInCoroutine = StartCoroutine(FadeIn());
                }

                Vector3 direction = transform.position - playerTransform.position;
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    textMesh.transform.parent.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                if (fadeInCoroutine != null)
                {
                    StopCoroutine(fadeInCoroutine);
                    fadeInCoroutine = null;
                }

                if (fadeOutCoroutine == null)
                {
                    fadeOutCoroutine = StartCoroutine(FadeOut());
                }
            }
        }

        private IEnumerator FadeIn()
        {
            Color color = textMesh.color;

            while (color.a < 0.9f)
            {
                color.a += fadeSpeed * Time.deltaTime;
                textMesh.color = color;
                yield return null;
            }

            color.a = 1f;
            textMesh.color = color;
            fadeInCoroutine = null;
        }

        private IEnumerator FadeOut()
        {
            Color color = textMesh.color;

            while (color.a > 0f)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                textMesh.color = color;
                yield return null;
            }

            color.a = 0f;
            textMesh.color = color;
            fadeOutCoroutine = null;
        }
    }
}