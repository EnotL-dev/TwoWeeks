using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem.QuestSystem
{
    public class QuestController : MonoBehaviour
    {
        protected class QuestPair
        {
            public GameObject objQ;
            public Quest quest;

            public QuestPair(GameObject objQ, Quest quest)
            {
                this.objQ = objQ;
                this.quest = quest;
            }
        }

        [SerializeField] private VerticalLayoutGroup panelQuests;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI textPrefab;
        [Header("pull")]
        [SerializeField] private List<QuestPair> questPull = new List<QuestPair>();

        public void AddQuest(Quest quest)
        {
            GameObject questObj = Instantiate(textPrefab, panelQuests.transform).gameObject;
            questObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.description;
            questPull.Add(new QuestPair(questObj, quest));

            CheckMaxPull();
        }

        public void CompleteQuest(Quest quest)
        {
            foreach(QuestPair questPair in questPull)
            {
                if(questPair.quest == quest)
                {
                    Destroy(questPair.objQ);
                    questPull.Remove(questPair);
                    break;
                }
            }
        }

        private void CheckMaxPull()
        {
            while(questPull.Count > 3)
            {
                Destroy(questPull[0].objQ);
                questPull.RemoveAt(0);
            }
        }
    }
}
