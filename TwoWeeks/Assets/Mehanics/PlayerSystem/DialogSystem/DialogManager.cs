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
    public class DialogManager
    {
        private float timer_before_next_dialog = 0;
        public bool InDialog = false;
        private async UniTask WaitForKey(KeyCode key)
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(key));
        }

        public CanvasGroup canvasGroup;
        public TextMeshProUGUI textMesh_Character;

        private void ShowPersonCanvas(DialogObject dialogObj)
        {
            canvasGroup = dialogObj.canvasGroup;
            textMesh_Character = dialogObj.textMesh_Character;

            
            if (canvasGroup) FadeIn(canvasGroup).Forget();
            FadeIn(Main.MainControllers.playerController.dialogController.canvasGroup_MainUI).Forget();
        }

        private void HidePersonCanvas()
        {
            if(canvasGroup) FadeOut(canvasGroup).Forget();
            FadeOut(Main.MainControllers.playerController.dialogController.canvasGroup_MainUI).Forget();
        }

        private void PlayerControllers(bool locking)
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

            PlayerControllers(dialogObj.dialog.lockPlayer); //Включает и выключает контроллер

            DialogCompletedEvents = dialogObj.DialogCompletedEvents; 
            dialogObj.DialogStartedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject

            ClearEndTextAfterEndAnimation(); //Повторно вызову, чтобы очистить не окончившийся текст

            ShowPersonCanvas(dialogObj);
            ProcessBlock(dialogObj.dialog).Forget();
        }

        private void EndDialog()
        {
            InDialog = false;
            DialogCompletedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject
            PlayerControllers(false);
            HidePersonCanvas();
            ClearMainTextsAfterEndDialog(); //Очищает главный оставляя конечную анимацию

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

                PlayEndAnimationTextHandler?.Invoke();
                PlayEndAnimationTextHandler = null;
                numBlock++;
            }

            EndDialog();
        }

        private void NextMessage(Dialog dialog, int numBlock)
        {
            msDelay = dialog.GetMessageBlock(numBlock).msDelay_before_next_message;

            AnimateTextEffectStart(dialog, numBlock, Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>(), null);
            Main.MainControllers.playerController.dialogController.textStart_MainUI.text = "";
            PlaceNewMessageInText(dialog, numBlock);

            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().Refresh(); //Не обновишь - не будет анимации. и я рот ебал как это плагин работает
        }

        private void AnimateTextEffectStart(Dialog dialog, int numBlock, TextEffect mainUI_text, TextEffect person_text)
        {
            List<GlobalTextEffectEntry> endEffects = dialog.GetMessageBlock(numBlock).endEffects;
            if (endEffects != null && endEffects.Count > 0) PlayEndAnimationTextHandler += () => AnimateTextEffectEnd(endEffects, dialog.GetMessage(numBlock)); //Подписка на окончание анимации

            mainUI_text.StopAllEffects();
            mainUI_text.globalEffects = dialog.GetMessageBlock(numBlock).startEffects;
            mainUI_text.globalEffects[0].onEffectCompleted.AddListener(() => End_Animation_Text().Forget());
        }

        private void AnimateTextEffectEnd(List<GlobalTextEffectEntry> endEffects, string fillingtext)
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = fillingtext;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().globalEffects = endEffects;
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => ClearEndTextAfterEndAnimation());

            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();
        }

        private void ClearEndTextAfterEndAnimation()
        {
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.text = " ";
            Main.MainControllers.playerController.dialogController.textEnd_MainUI.GetComponent<TextEffect>().Refresh();
        }

        private void ClearMainTextsAfterEndDialog()
        {
            Main.MainControllers.playerController.dialogController.textStart_MainUI.text = " ";
            Main.MainControllers.playerController.dialogController.textStart_MainUI.GetComponent<TextEffect>().Refresh();
        }

        private void PlaceNewMessageInText(Dialog dialog, int numBlock)
        {
            string composite_message = "";
            if (dialog.GetNamePerson(numBlock) != null || dialog.GetNamePerson(numBlock) != "")
                composite_message += $"{dialog.GetNamePerson(numBlock)}: ";

            if (dialog.GetMessage(numBlock) != null || dialog.GetMessage(numBlock) != "")
                composite_message += dialog.GetMessage(numBlock);

            Main.MainControllers.playerController.dialogController.textStart_MainUI.text = composite_message;
        }

        private async UniTask FadeIn(CanvasGroup canvasGroup, float duration = 0.6f)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);

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

            canvasGroup.gameObject.SetActive(false);
        }
    }
}
