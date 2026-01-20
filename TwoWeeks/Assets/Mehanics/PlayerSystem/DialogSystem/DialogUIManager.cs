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
        DialogObject dialogObject;
        public void SetDialogObject(DialogObject dialogObject) => this.dialogObject = dialogObject;

        public void ProcessNextMessage(Dialog dialog, int numBlock)
        {
            PlaceText(dialog, numBlock);

            MessageBlock mesBlock = dialog.GetMessageBlock(numBlock);
            foreach(EmoteEvents emoteEvent in dialogObject.emoteEvents)
            {
                if(emoteEvent.tagEvent == mesBlock.tagEventEmote)
                {
                    emoteEvent.eventEmote.Invoke();
                    break;
                }
            }
        }

        private void PlaceText(Dialog dialog, int numBlock)
        {
            DialogController dialogController = Main.MainControllers.playerController.dialogController;
            MessageBlock mesBlock = dialog.GetMessageBlock(numBlock);
            if (mesBlock.person_tag != "player")
            {
                foreach (CanvasForPerson canvasForPerson in dialogObject.canvases)
                {
                    if (canvasForPerson.person_tag == mesBlock.person_tag)
                    {
                        MovePersonCanvasToCameraView(canvasForPerson.person_transform, dialogController.GetCamera().transform, canvasForPerson.canvasGroup);
                        canvasForPerson.textStart_PersonUI.text = dialog.GetMessage(numBlock);
                        PlayAnimationStartText(canvasForPerson, mesBlock.startEffects, mesBlock.msDelay_before_next_message);
                        break;
                    }
                }
            }
            else
            {
                MoveHeadCanvasToCameraView();
                dialogController.playerCanvas.textStart_PersonUI.text = dialog.GetMessage(numBlock);
                PlayAnimationStartText(dialogController.playerCanvas, mesBlock.startEffects, mesBlock.msDelay_before_next_message);
            }
        }

        private async UniTask AcceptNextMessageInManager(TextEffect textEffect, int delay)
        {
            await UniTask.Delay(delay);
            textEffect.globalEffects[0].onEffectCompleted.AddListener(() => Main.MainManagers.dialogManager.Set_next_message_flag(true));
        }

        private void PlayAnimationStartText(CanvasForPerson canvasForPerson, List<GlobalTextEffectEntry> effects, int delayMes)
        {
            canvasForPerson.textEnd_PersonUI.text = " ";
            canvasForPerson.textEnd_PersonUI.GetComponent<TextEffect>().Refresh(); //♿♿

            canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>().StopAllEffects();
            canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>().globalEffects = effects;
            canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => AcceptNextMessageInManager(canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>(), delayMes).Forget());
            canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>().Refresh(); //♿♿
        }

        public void PlayAnimationEndText(CanvasForPerson canvasForPerson, List<GlobalTextEffectEntry> effects, string finalText) //Invoke from Manager
        {
            canvasForPerson.textStart_PersonUI.text = " ";
            canvasForPerson.textStart_PersonUI.GetComponent<TextEffect>().Refresh(); //♿♿

            canvasForPerson.textEnd_PersonUI.text = finalText;
            canvasForPerson.textEnd_PersonUI.GetComponent<TextEffect>().StopAllEffects();
            canvasForPerson.textEnd_PersonUI.GetComponent<TextEffect>().globalEffects = effects;
            canvasForPerson.textEnd_PersonUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.AddListener(() => ClearThisTextAction(canvasForPerson.textEnd_PersonUI)); //В конце вызовется очистка
            canvasForPerson.textEnd_PersonUI.GetComponent<TextEffect>().Refresh(); //♿♿
        }

        private void ClearThisTextAction(TextMeshProUGUI textMeshToClear)
        {
            textMeshToClear.text = " ";
            textMeshToClear.GetComponent<TextEffect>().StopAllEffects();
            textMeshToClear.GetComponent<TextEffect>().globalEffects = null;
            textMeshToClear.GetComponent<TextEffect>().Refresh(); //♿♿
        }

        Camera _camera => Main.MainControllers.playerController.dialogController.GetCamera();

        LayerMask interactableLayer => Main.MainControllers.playerController.dialogController.GetInteractableLayer();
        float rayDistance => Main.MainControllers.playerController.dialogController.GetRayDistance();

        private float moveDistance = 1f; //отодвигает текст на такую дистанцию от игрока
        private float minDistanceToWall = 0.5f; //сдвигает от стены текст на такое расстояние

        private TextMeshProUGUI textStart_HeadUI => Main.MainControllers.playerController.dialogController.playerCanvas.textStart_PersonUI;
        private TextMeshProUGUI textEnd_HeadUI => Main.MainControllers.playerController.dialogController.playerCanvas.textEnd_PersonUI;

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
            Vector3 offset = new Vector3(0, -0.2f, 0);
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
