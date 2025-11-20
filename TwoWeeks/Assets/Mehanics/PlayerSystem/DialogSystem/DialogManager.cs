using Cysharp.Threading.Tasks;
using EasyTextEffects;
using EasyTextEffects.Effects;
using InteractionSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static InteractionSystem.DialogObject;

namespace PlayerSystem.DialogSystem
{
    public class DialogManager
    {
        private static class DialogManagerSupportMethods
        {
            public static TextMeshProUGUI TextStartPersonByTag(List<CanvasForPerson> canvases, string tag)
            {
                foreach (CanvasForPerson canvas in canvases)
                {
                    if (canvas.person_tag == tag)
                        return canvas.textStartPerson;
                }

                return null;
            }

            public static TextMeshProUGUI TextEndPersonByTag(List<CanvasForPerson> canvases, string tag)
            {
                foreach (CanvasForPerson canvas in canvases)
                {
                    if (canvas.person_tag == tag)
                        return canvas.textEndPerson;
                }

                return null;
            }

            public static void MovePersonCanvasToCameraView(Transform relativeObj, Transform cameraTransform, CanvasGroup relativeCanvas)
            {
                Vector3 direction = relativeObj.forward;

                Vector3 directionToCamera = (cameraTransform.position - relativeObj.position).normalized;
                Vector3 targetPos = relativeObj.position + directionToCamera * 1f;

                Vector3 toTarget = targetPos - relativeObj.position;
                if (toTarget.magnitude > 1f)
                {
                    toTarget = toTarget.normalized * 1f;
                    targetPos = relativeObj.position + toTarget;
                }

                relativeCanvas.transform.position = targetPos;

                relativeCanvas.transform.LookAt(relativeCanvas.transform.position + cameraTransform.rotation * Vector3.forward,
                                               cameraTransform.rotation * Vector3.up);
            }
        }

        private float timer_before_next_dialog = 0;
        public bool InDialog = false;
        private async UniTask WaitForKey(KeyCode key)
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(key));
        }

        public List<CanvasForPerson> canvases;
        private void ShowPersonCanvas(DialogObject dialogObj)
        {
            foreach(CanvasForPerson canvas in canvases)
            {
                if (canvas.canvasGroup) FadeIn(canvas.canvasGroup).Forget();
            }

            FadeIn(Main.MainControllers.playerController.dialogController.canvasGroup_MainUI).Forget();
        }

        private void HidePersonCanvas()
        {
            foreach (CanvasForPerson canvas in canvases)
            {
                if (canvas.canvasGroup) FadeOut(canvas.canvasGroup).Forget();
            }
            
            FadeOut(Main.MainControllers.playerController.dialogController.canvasGroup_MainUI).Forget();
        }

        private void PlayerControllersLocks(bool locking)
        {
            Main.lockedPlayer = locking;
            Main.MainControllers.playerController.moving.enabled = !locking;
            Main.MainControllers.playerController.footSteps.enabled = !locking;
            Main.MainControllers.playerController.jumping.enabled = !locking;
            Main.MainControllers.playerController.cinemachineInputAxisController.enabled = !locking;
        }

        private UnityEvent DialogCompletedEvents;
        public void StartDialog(DialogObject dialogObj) //Speed dermined by text animation, end also in controller
        {
            if (InDialog || timer_before_next_dialog > 0) return;
            InDialog = true;


            PlayerControllersLocks(dialogObj.dialog.lockPlayer); //Включает и выключает контроллер

            canvases = dialogObj.canvases;
            DialogCompletedEvents = dialogObj.DialogCompletedEvents; 
            dialogObj.DialogStartedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject

            ResetEndTextsAfterEndAnimation(); //Повторно вызову, чтобы очистить не окончившийся текст

            ShowPersonCanvas(dialogObj);
            MoveHeadCanvas();
            ProcessBlock(dialogObj.dialog).Forget();
        }

        private void EndDialog()
        {
            InDialog = false;
            DialogCompletedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject
            PlayerControllersLocks(false);
            HidePersonCanvas();
            ResetMainTextsAfterEndDialog(); //Очищает главный оставляя конечную анимацию

            TimerBeforeNextDialog().Forget();
        }

        private async UniTask TimerBeforeNextDialog()
        {
            timer_before_next_dialog = 0.05f;
            while (timer_before_next_dialog > 0)
            {
                timer_before_next_dialog -= Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private event Action PlayEndAnimationTextHandler; //Подписка на установку анимации окончания
        private int msDelay = 1000;
        private bool next_message_flag = false;
        private async UniTask End_Animation_Text()
        {
            Debug.Log("Next message");
            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.RemoveAllListeners(); //чтобы не было обработки дповторной

            await UniTask.Delay(msDelay);

            next_message_flag = true;
        }

        private async UniTask ProcessBlock(Dialog dialog) 
        {
            int numBlock = 0;
            while (numBlock < dialog.CountBlocks())
            {
                NextMessage(dialog, numBlock);

                next_message_flag = false;

                if (dialog.GetMessageBlock(numBlock).canSkip)
                {
                    if (dialog.GetMessageBlock(numBlock).autoskip)
                    {
                        await UniTask.WhenAny(
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Interaction_Key),
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Skip_Message_Key),
                            UniTask.WaitUntil(() => next_message_flag == true)
                        );
                    }
                    else
                    {
                        await UniTask.WhenAny(
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Interaction_Key),
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Skip_Message_Key)
                        );
                    }
                }
                else
                {
                    if (dialog.GetMessageBlock(numBlock).autoskip)
                    {
                        await UniTask.WhenAny(
                        UniTask.WaitUntil(() => next_message_flag == true)
                        );
                    }
                    else
                    {
                        await UniTask.WhenAny(
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Interaction_Key),
                            WaitForKey(Main.MainManagers.settingsManager.InputConfig().Skip_Message_Key)
                        );
                    }
                }

                MoveHeadCanvas();
                PlayEndAnimationTextHandler?.Invoke();
                PlayEndAnimationTextHandler = null;
                numBlock++;
            }

            EndDialog();
        }

        private void MoveHeadCanvas()
        {
            if (Main.MainControllers.playerController.dialogController.TryMoveHeadCanvasToCameraView()) //Смещение канвас игрока для разговора
            {
                Main.MainControllers.playerController.dialogController.canvasGroup_MainUI.alpha = 0;
            }
            else
            {
                Main.MainControllers.playerController.dialogController.canvasGroup_MainUI.alpha = 1;
            }

            foreach(CanvasForPerson canvas in canvases)
            {
                DialogManagerSupportMethods.MovePersonCanvasToCameraView(canvas.person_transform, Main.MainControllers.playerController.interactionController.transform, canvas.canvasGroup);
            }
        }

        private void NextMessage(Dialog dialog, int numBlock)
        {
            msDelay = dialog.GetMessageBlock(numBlock).msDelay_before_next_message;

            List<GlobalTextEffectEntry> endEffects = dialog.GetMessageBlock(numBlock).endEffects;
            if (endEffects != null && endEffects.Count > 0) PlayEndAnimationTextHandler += () => AnimateTextEffectEnd(endEffects, dialog.GetMessage(numBlock)); //Подписка на окончание анимации

            AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>());
            AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>());
            if(DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag))
                AnimateTextEffectStart(dialog, numBlock, DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag).GetComponent<TextEffect>());

            Main.MainControllers.playerController.dialogController.textStart_MainUI.text = "";
            PlaceNewMessageInText(Main.MainControllers.playerController.dialogController.textStart_MainUI, dialog, numBlock);
            Main.MainControllers.playerController.dialogController.textStart_HeadUI.text = "";
            PlaceNewMessageInText(Main.MainControllers.playerController.dialogController.textStart_HeadUI, dialog, numBlock);
            if (DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag))
            {
                DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag).text = "";
                PlaceNewMessageInText(DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag), dialog, numBlock);
            }

            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
            Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
            if(DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag))
                DialogManagerSupportMethods.TextStartPersonByTag(canvases, dialog.GetMessageBlock(numBlock).person_tag).GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
        }

        private void AnimateTextEffectStart(Dialog dialog, int numBlock, TextEffect animatingTextUI)
        {
            animatingTextUI.StopAllEffects();
            animatingTextUI.globalEffects = dialog.GetMessageBlock(numBlock).startEffects;
            animatingTextUI.globalEffects[0].onEffectCompleted.AddListener(() => End_Animation_Text().Forget());
        }

        private void AnimateTextEffectEnd(List<GlobalTextEffectEntry> endEffects, string fillingtext) //Сюда подписка, поэтому анимируем конец для всего
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = fillingtext;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().globalEffects = endEffects;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => ResetEndTextsAfterEndAnimation()); //Только на главном UI
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();

            Main.MainControllers.playerController.dialogController.textEnd_HeadUI.text = fillingtext;
            Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().globalEffects = endEffects;
            Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().Refresh();

            foreach (CanvasForPerson canvas in canvases)
            {
                canvas.textEndPerson.text = fillingtext;
                canvas.textEndPerson.GetComponent<TextEffect>().globalEffects = endEffects;
                canvas.textEndPerson.GetComponent<TextEffect>().Refresh();
            }
        }

        private void ResetEndTextsAfterEndAnimation()
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = " ";
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();

            Main.MainControllers.playerController.dialogController.textEnd_HeadUI.text = " ";
            Main.MainControllers.playerController.dialogController.textEnd_HeadUI.GetComponent<TextEffect>().Refresh();

            foreach (CanvasForPerson canvas in canvases)
            {
                canvas.textEndPerson.text = "";
                canvas.textEndPerson.GetComponent<TextEffect>().Refresh();
            }
        }

        private void ResetMainTextsAfterEndDialog()
        {
            Main.MainControllers.playerController.dialogController.textStart_MainUI.text = " ";
            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().Refresh();

            Main.MainControllers.playerController.dialogController.textStart_HeadUI.text = " ";
            Main.MainControllers.playerController.dialogController.textStart_HeadUI.GetComponent<TextEffect>().Refresh();

            foreach (CanvasForPerson canvas in canvases)
            {
                canvas.textStartPerson.text = "";
                canvas.textStartPerson.GetComponent<TextEffect>().Refresh();
            }
        }

        private void PlaceNewMessageInText(TextMeshProUGUI textMesh, Dialog dialog, int numBlock)
        {
            string composite_message = "";
            if (dialog.GetNamePerson(numBlock) != null || dialog.GetNamePerson(numBlock) != "")
                composite_message += $"{dialog.GetNamePerson(numBlock)}: ";

            if (dialog.GetMessage(numBlock) != null || dialog.GetMessage(numBlock) != "")
                composite_message += dialog.GetMessage(numBlock);

            textMesh.text = composite_message;
        }

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

            if(InDialog)
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

            if(!InDialog)
                canvasGroup.alpha = 0f;
        }
    }
}
