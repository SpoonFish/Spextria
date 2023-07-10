using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spextria.Entities.EntityParts
{
    class Dialog
    {
        public string Message;
        public string Condition;
        public string ConditionValue;
        public string Action;
        public string ActionValue;
        public bool ConditionalContinue;
        public int LineAmount;
        public Dialog(int length, string message, string condition = "", string conditionValue = "", bool conditionalContinue = false, string action = "", string actionValue = "")
        {
            LineAmount = length;
            Message = message;
            Condition = condition;
            ConditionValue = conditionValue;
            ConditionalContinue = conditionalContinue;
            Action = action;
            ActionValue = actionValue;

        }
    }
}
