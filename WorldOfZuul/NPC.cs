using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class NPC
    {
        public string Name;
        public string Description;
        public Dictionary<int, string> Dialogue;
        public string DefaultDialogue;
        public Room Room;

        public NPC(string name, string description, Dictionary<int, string> dialogue, string defaultDialogue, Room room) {
            Name = name;
            Description = description;
            Dialogue = dialogue;
            DefaultDialogue = defaultDialogue;
            Room = room;
        }
        public void Talk(int dialogueID) {
            if(Dialogue.ContainsKey(dialogueID)) Program.game.lastOutputString = Dialogue[dialogueID];
            else Program.game.lastOutputString = DefaultDialogue; 
        }
    }
}