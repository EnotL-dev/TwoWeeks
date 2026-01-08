using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerSystem.QuestSystem
{
    [CreateAssetMenu(fileName = "new quest", menuName = "Quest")]
    public class Quest : ScriptableObject
    {
        public string description = "";
    }
}
