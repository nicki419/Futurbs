using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Quests
    {

        public enum QuestType {
            visitRoom,
            talkToNPC,
            buildBuilding,
            demolishBuilding,
            subQuests,
            stageProgress
        };
        public class Quest {
            public float Id; //e.g. 0.1 for stage 0, quest 1
            public string Name;
            public string HelpText;
            public bool Completed;
            Quests.QuestType? Type;
            public string? CompletionCondition;
            public Dictionary<int, Quest>? SubQuests;

            public Quest(float id, string name, string helpText, string? completionCondition, Dictionary<int, Quest>? subQuests, Quests.QuestType? type = Quests.QuestType.stageProgress) {
                Id = id;
                Name = name;
                HelpText = helpText;
                Completed = false;
                Type = type;
                CompletionCondition = completionCondition;
                SubQuests = subQuests;
            }

            public void updateQuest() {
                if(SubQuests != null) foreach(Quest _ in SubQuests.Values) _.updateQuest();
                switch(Type) {
                    case QuestType.visitRoom:
                        if(!Completed && Game.map.CurrentRoom.ShortDescription == CompletionCondition) Completed = true;
                        break;

                    case QuestType.talkToNPC:
                        break;

                    case QuestType.buildBuilding:
                        break;
                    
                    case QuestType.demolishBuilding:
                        break;

                    case QuestType.subQuests:
                        if(SubQuests != null) foreach(Quest _ in SubQuests.Values) {
                            if(!_.Completed) return;
                            }
                            Completed = true;
                        break;
                    
                    case QuestType.stageProgress:
                        // No idea how to implement this yet.
                        if(CompletionCondition == "completed") Completed = true;
                        break;
                }
            }
        }
    }
}