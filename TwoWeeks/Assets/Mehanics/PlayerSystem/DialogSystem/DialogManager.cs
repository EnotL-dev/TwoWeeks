using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerSystem.DialogSystem
{
    public class DialogManager
    {
        private async UniTask WaitForKey(KeyCode key)
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(key));
        }

        public void StartDialog(Dialog dialog)
        {
            ProcessBlock(dialog).Forget();
        }

        private async UniTask ProcessBlock(Dialog dialog)
        {
            int numBlock = -1;
            while (numBlock < dialog.CountBlocks())
            {
                numBlock++;
                
                await UniTask.WhenAny(
                    WaitForKey(Main.MainManagers.settingsManager.InputConfig().Interaction_Key),
                    WaitForKey(Main.MainManagers.settingsManager.InputConfig().Skip_Message_Key),
                    ProcessMessage(dialog.GetMessageBlock(numBlock))
                    );
            }
        }

        private async UniTask ProcessMessage(MessageBlock messageBlock)
        {


            int delay = (int)messageBlock.speed_char * 1000;
            await UniTask.Delay(delay);
        }
    }
}
