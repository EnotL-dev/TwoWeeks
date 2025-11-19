using Cysharp.Threading.Tasks;
using EasyTextEffects;
using InteractionSystem;
using TMPro;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    public class DialogManager
    {
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

        public void StartDialog(DialogObject dialogObj) //Speed dermined by text animation, end also in controller
        {
            if (InDialog) return;

            InDialog = true;
            ShowPersonCanvas(dialogObj);
            ProcessBlock(dialogObj.dialog).Forget();
        }

        private void EndDialog()
        {
            InDialog = false;
            HidePersonCanvas();
        }

        private int msDelay = 1000;
        private bool next_message_flag = false;
        public async UniTask End_Animation_Text()
        {
            Debug.Log("Next message");
            Main.MainControllers.playerController.dialogController.textMesh_MainUI.GetComponent<TextEffect>().globalEffects[0].onEffectCompleted.RemoveAllListeners();

            await UniTask.Delay(msDelay);

            next_message_flag = true;
        }

        private async UniTask ProcessBlock(Dialog dialog) 
        {
            int numBlock = 0;
            while (numBlock < dialog.CountBlocks())
            {
                NextMessage(dialog, numBlock); //∆дет пока заполница текст
                next_message_flag = false;

                if (dialog.GetMessageBlock(numBlock).canSkip)
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
                        UniTask.WaitUntil(() => next_message_flag == true)
                    );
                }

                numBlock++;
            }

            EndDialog();
        }

        private void NextMessage(Dialog dialog, int numBlock)
        {
            msDelay = dialog.GetMessageBlock(numBlock).msDelay_before_next_message;

            AnimateTextEffect(dialog, numBlock, Main.MainControllers.playerController.dialogController.textMesh_MainUI.GetComponent<TextEffect>(), null);
            Main.MainControllers.playerController.dialogController.textMesh_MainUI.text = "";
            PlaceNewMessageInText(dialog, numBlock);

            Main.MainControllers.playerController.dialogController.textMesh_MainUI.GetComponent<TextEffect>().Refresh(); //Ќе обновишь - не будет анимации. и € рот ебал как это плагин работает
        }

        private void AnimateTextEffect(Dialog dialog, int numBlock, TextEffect mainUI_text, TextEffect person_text)
        {
            mainUI_text.StopAllEffects();
            mainUI_text.globalEffects = dialog.GetMessageBlock(numBlock).globalEffects;
            mainUI_text.globalEffects[0].onEffectCompleted.AddListener(() => End_Animation_Text().Forget());
        }

        private void PlaceNewMessageInText(Dialog dialog, int numBlock)
        {
            string composite_message = "";
            if (dialog.GetNamePerson(numBlock) != null || dialog.GetNamePerson(numBlock) != "")
                composite_message += $"{dialog.GetNamePerson(numBlock)}: ";

            if (dialog.GetMessage(numBlock) != null || dialog.GetMessage(numBlock) != "")
                composite_message += dialog.GetMessage(numBlock);

            Main.MainControllers.playerController.dialogController.textMesh_MainUI.text = composite_message;
        }

        public async UniTask FadeIn(CanvasGroup canvasGroup, float duration = 1f)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            canvasGroup.alpha = 1f;
        }

        public async UniTask FadeOut(CanvasGroup canvasGroup, float duration = 1f)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
