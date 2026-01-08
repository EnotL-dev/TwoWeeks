using Cysharp.Threading.Tasks;
using EasyTextEffects;
using EasyTextEffects.Effects;
using InteractionSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem.DialogSystem
{
    /*
     * Данный объект ответственен за логику отображения и контроля текстов,
     * НО НЕ ЗА РАБОТОЙ САМИХ ДИАЛОГОВ
     */
    public class DialogUIManager
    {
        List<CanvasForPerson> personCanvases = null;
        public void StartUpdateCanvases(List<CanvasForPerson> personCanvases)
        {
            updating = true;
            this.personCanvases = personCanvases;
            Update().Forget();
        }

        public void StopUpdateCanvases()
        {
            updating = false;
        }

        bool updating = false;
        private async UniTask Update()
        {
            while (updating)
            {
                await UniTask.Delay(1000);
            }
        }

        private async UniTask End_Animation_Text(int msDelay, TextEffect animatingTextUI) //запускается только с одного ТЕКСТА по сути | animatingTextUI - ЭТО СТАРТОВЫЙ ТЕКСТ |
        {
            Debug.Log("Next message");
            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.RemoveAllListeners(); //чтобы не было обработки повторной

            foreach(CanvasForPerson canvasFromPool in pool_personCanvases)
            {
                try
                {
                    if (canvasFromPool.textStart_PersonUI.GetComponent<TextEffect>() == animatingTextUI) //animatingTextUI - ЭТО СТАРТОВЫЙ ТЕКСТ 
                    {
                        canvasFromPool.textStart_PersonUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.RemoveAllListeners(); //на всякий
                        canvasFromPool.textEnd_PersonUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.RemoveAllListeners(); //на всякий
                        canvasFromPool.textEnd_PersonUI.GetComponent<TextEffect>().StopAllEffects();

                        canvasFromPool.textEnd_PersonUI.text = canvasFromPool.textStart_PersonUI.text;
                        canvasFromPool.textStart_PersonUI.text = "";

                        canvasFromPool.textEnd_PersonUI.GetComponent<TextEffect>().Refresh();
                        break;
                    }
                } catch { }
            }

            await UniTask.Delay(msDelay);

            Main.MainManagers.dialogManager.Set_next_message_flag(true);
        }

        Dictionary<string, Action> PlayEndAnimationTextHandler = new Dictionary<string, Action>(); //Подписка на установку анимации окончания
        public void PlayEndAnimationInBlock(MessageBlock messageBlock)
        {
            PlayEndAnimationTextHandler[messageBlock.person_tag]?.Invoke();
            PlayEndAnimationTextHandler[messageBlock.person_tag] = null;
        }

        List<CanvasForPerson> pool_personCanvases = new List<CanvasForPerson>();
        private void ProcessWithPersonPool(Dialog dialog, int numBlock, string person_tag)
        {
            List<GlobalTextEffectEntry> endEffects = dialog.GetMessageBlock(numBlock).endEffects;
            if (endEffects != null && endEffects.Count > 0) PlayEndAnimationTextHandler[person_tag] = () => AnimateTextEffectEnd(endEffects, dialog.GetMessage(numBlock), person_tag); //Подписка на окончание анимации

            PlacementNewMessageInCanvase(Main.MainControllers.playerController.dialogController.textStart_MainUI, dialog, numBlock, true);
            AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>());
            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает

            bool pool_empty_flag = true; //Если в пуле не было нужного канваса
            foreach (CanvasForPerson canvasFromPool in pool_personCanvases)
            {
                if (canvasFromPool.person_tag == person_tag)
                {
                    if (person_tag != "player")
                    {
                        PlacementNewMessageInCanvase(canvasFromPool.textStart_PersonUI, dialog, numBlock, false);
                        AnimateTextEffectStart(dialog, numBlock, canvasFromPool.textStart_PersonUI.GetComponent<TextEffect>());
                        canvasFromPool.textStart_PersonUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
                    }
                    else
                    {
                        PlacementNewMessageInCanvase(Main.MainControllers.playerController.dialogController.textStart_HeadUI, dialog, numBlock, false);
                        AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>());
                        Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
                    }

                    pool_personCanvases.Remove(canvasFromPool);
                    pool_empty_flag = false;
                    break;
                }
            }

            if (pool_empty_flag)
            {
                if (person_tag == "player") //Канвас игркоа обрабатываем И ДОБАВЛЯЕМ отдельно
                {
                    PlacementNewMessageInCanvase(Main.MainControllers.playerController.dialogController.textStart_HeadUI, dialog, numBlock, false);
                    AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>());
                    Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
                    MoveHeadCanvasToCameraView();

                    CanvasForPerson playerCanvas = new CanvasForPerson();
                    playerCanvas.person_tag = "player"; //костыль ну да ладно
                    pool_personCanvases.Add(playerCanvas);
                }
                else
                {
                    foreach (CanvasForPerson personCanvas in personCanvases)
                    {
                        if (personCanvas.person_tag == person_tag)
                        {
                            PlacementNewMessageInCanvase(personCanvas.textStart_PersonUI, dialog, numBlock, false);
                            AnimateTextEffectStart(dialog, numBlock, personCanvas.textStart_PersonUI.GetComponent<TextEffect>());
                            personCanvas.textStart_PersonUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
                            MovePersonCanvasToCameraView(personCanvas.person_transform, Main.MainControllers.playerController.dialogController.GetCamera().transform, personCanvas.canvasGroup);

                            pool_personCanvases.Add(personCanvas);
                            break;
                        }
                    }
                }
            }
        }

        //########################## START ########################################
        public void ProcessNextMessage(Dialog dialog, int numBlock) 
        {
            ProcessWithPersonPool(dialog, numBlock, dialog.GetMessageBlock(numBlock).person_tag);
        }
        //########################## START ########################################

        private void PlacementNewMessageInCanvase(TextMeshProUGUI textMesh, Dialog dialog, int numBlock, bool withName)
        {
            string message = "";
            string composite_message = ""; // with name
            if (dialog.GetNamePerson(numBlock) != null || dialog.GetNamePerson(numBlock) != "")
                composite_message += $"{dialog.GetNamePerson(numBlock)}: ";

            if (dialog.GetMessage(numBlock) != null || dialog.GetMessage(numBlock) != "")
                message += dialog.GetMessage(numBlock);

            if(withName) textMesh.text = composite_message;
            else textMesh.text = message;
        }

        private void AnimateTextEffectStart(Dialog dialog, int numBlock, TextEffect animatingTextUI)
        {
            animatingTextUI.StopAllEffects();
            animatingTextUI.globalEffects = dialog.GetMessageBlock(numBlock).startEffects;
            animatingTextUI.globalEffects[0].onEffectCompleted.AddListener(() => End_Animation_Text(dialog.GetMessageBlock(numBlock).msDelay_before_next_message, animatingTextUI).Forget());
        }

        private void AnimateTextEffectEnd(List<GlobalTextEffectEntry> endEffects, string fillingtext, string person_tag) //Сюда подписка, поэтому анимируем конец для всего
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = fillingtext;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().globalEffects = endEffects;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();

            if (person_tag != "player")
            {
                foreach (CanvasForPerson canvas in personCanvases)
                {
                    if (canvas.person_tag == person_tag)
                    {
                        canvas.textEnd_PersonUI.text = fillingtext;
                        canvas.textEnd_PersonUI.GetComponent<TextEffect>().globalEffects = endEffects;
                        canvas.textEnd_PersonUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => ResetEndTextAfterEndAnimation(person_tag));
                        canvas.textEnd_PersonUI.GetComponent<TextEffect>().Refresh();

                        break;
                    }
                }
            }
            else
            {
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.text = fillingtext;
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().globalEffects = endEffects;
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => ResetEndTextAfterEndAnimation(person_tag));
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().Refresh();
            }
        }

        private void ResetEndTextAfterEndAnimation(string person_tag)
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = " ";
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();

            if (person_tag != "player")
            {
                foreach (CanvasForPerson canvas in personCanvases)
                {
                    if (canvas.person_tag == person_tag)
                    {
                        canvas.textEnd_PersonUI.text = "";
                        canvas.textEnd_PersonUI.GetComponent<TextEffect>().Refresh();

                        break;
                    }
                }
            }
            else
            {
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.text = " ";
                Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().Refresh();
            }
        }

        #region Fade
        bool InDialog => Main.MainManagers.dialogManager.InDialog;
        private async UniTask FadeIn(CanvasGroup canvasGroup, float duration = 0.6f)
        {
            canvasGroup.alpha = 0f;

            float elapsed = 0f;
            while (elapsed < duration && InDialog)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            if (InDialog)
                canvasGroup.alpha = 1f;
        }

        private async UniTask FadeOut(CanvasGroup canvasGroup, float duration = 0.6f)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration && !InDialog)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            if (!InDialog)
                canvasGroup.alpha = 0f;
        }
        #endregion Fade

        Camera _camera => Main.MainControllers.playerController.dialogController.GetCamera();

        LayerMask interactableLayer => Main.MainControllers.playerController.dialogController.GetInteractableLayer();
        float rayDistance => Main.MainControllers.playerController.dialogController.GetRayDistance();

        private float moveDistance = 1f; //отодвигает текст на такую дистанцию от игрока
        private float minDistanceToWall = 0.5f; //сдвигает от стены текст на такое расстояние

        private CanvasGroup canvasGroup_MainUI => Main.MainControllers.playerController.dialogController.canvasGroup_MainUI;
        private TextMeshProUGUI textStart_MainUI => Main.MainControllers.playerController.dialogController.textStart_MainUI;
        private TextMeshProUGUI textEnd_MainUI => Main.MainControllers.playerController.dialogController.textEnd_MainUI;

        private TextMeshProUGUI textStart_HeadUI => Main.MainControllers.playerController.dialogController.textStart_HeadUI;
        private TextMeshProUGUI textEnd_HeadUI => Main.MainControllers.playerController.dialogController.textEnd_HeadUI;

        Transform headTransform => _camera.transform;

        #region personUI
        private void MovePersonCanvasToCameraView(Transform relativeObj, Transform cameraTransform, CanvasGroup relativeCanvas)
        {
            Vector3 directionToCamera = (cameraTransform.position - relativeObj.position).normalized;
            Vector3 targetPos = relativeObj.position + directionToCamera * 1f;

            Vector3 toTarget = targetPos - relativeObj.position;
            if (toTarget.magnitude > 1f)
            {
                toTarget = toTarget.normalized * 1f;
                targetPos = relativeObj.position + toTarget;
            }

            Vector3 offset = new Vector3(0, 0.6f, 0);
            relativeCanvas.transform.position = targetPos + offset;

            relativeCanvas.transform.LookAt(relativeCanvas.transform.position + cameraTransform.rotation * Vector3.forward,
                                           cameraTransform.rotation * Vector3.up);
        }
        #endregion personUI

        #region headUI
        private void MoveHeadCanvasToCameraView()
        {
            Vector3 offset = new Vector3(0, -0.6f, 0);
            Vector3 cameraPos = headTransform.position + offset;
            Vector3 direction = headTransform.forward;

            if (Physics.Raycast(cameraPos, direction, out RaycastHit hit, moveDistance))
            {
                textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = hit.point - direction * minDistanceToWall;
                textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = hit.point - direction * minDistanceToWall;
            }
            else
            {
                textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = cameraPos + direction * moveDistance;
                textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position = cameraPos + direction * moveDistance;
            }

            textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.LookAt(textStart_HeadUI.GetComponentInParent<CanvasGroup>().transform.position + headTransform.rotation * Vector3.forward,
                                   headTransform.rotation * Vector3.up);
            textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.LookAt(textEnd_HeadUI.GetComponentInParent<CanvasGroup>().transform.position + headTransform.rotation * Vector3.forward,
                                   headTransform.rotation * Vector3.up);
        }
        #endregion headUI
    }
}
