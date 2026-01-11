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

            DialogCompletedEvents = dialogObj.DialogCompletedEvents; 
            dialogObj.DialogStartedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject

            ProcessDialog(dialogObj).Forget();
        }

        private void EndDialog()
        {
            InDialog = false;
            DialogCompletedEvents?.Invoke(); //Вызов ивентов назначенных в DialogObject
            PlayerControllersLocks(false);

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

        private bool next_message_flag = false;

        public void Set_next_message_flag(bool newflag) => next_message_flag = newflag;

        private async UniTask ProcessDialog(DialogObject dialogObject) 
        {
            Main.MainManagers.dialogUIManager.SetDialogObject(dialogObject);
            Dialog dialog = dialogObject.dialog;

            int numBlock = 0;
            while (numBlock < dialog.CountBlocks())
            {
                next_message_flag = false;
                Main.MainManagers.dialogUIManager.ProcessNextMessage(dialog, numBlock);
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

                MessageBlock messageBlock = dialog.GetMessageBlock(numBlock);
                if (messageBlock.person_tag != "player")
                {
                    foreach (CanvasForPerson canvasForPerson in dialogObject.canvases)
                    {
                        if (canvasForPerson.person_tag == messageBlock.person_tag)
                        {
                            Main.MainManagers.dialogUIManager.PlayAnimationEndText(canvasForPerson, messageBlock.endEffects, dialog.GetMessage(numBlock));
                            break;
                        }
                    }
                }
                else
                    Main.MainManagers.dialogUIManager.PlayAnimationEndText(Main.MainControllers.playerController.dialogController.playerCanvas, messageBlock.endEffects, dialog.GetMessage(numBlock));

                numBlock++;
            }

            EndDialog();
        }
    }
}
