﻿using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Futurbs
{
    public class Game
    {
        private Room? previousRoom;
        public static string? playerName;
        private GameLogic gameLogic;
        private GameLogic.StageTut stageTut;
        private GameLogic.Stage0 stage0;
        private GameLogic.Stage1 stage1;
        private GameLogic.Stage2 stage2;
        public GameLogic.Stage3? stage3;

        public static Map map = GameLogic.StageTut.StageMap;
        public static Dictionary<int, Quests.Quest> currentQuests = new();

        public readonly Screen screen = new();
        public CommandWords.GameCommand activeCommand;
        public bool textInput = true;
        public bool mapMode = false;
        public string lastOutputString = "";
        public string compareOutputString = "";
        public int ScrollingTextSleepDuration = 5;
        private string? input;
        public static List<Quests.Quest> TrackedQuests = new();

        public enum MayorDecisionKeys {
            TravelByCar,
            RenovatedGhetto,
            RebuiltHousing,
            BuiltSchool,
            BuiltBikeInfrastructure,
            BuiltCarInfrastructure
        }
        public Dictionary<MayorDecisionKeys, bool?> MayorDecisions = new();


        public Game()
        {
            gameLogic = new();
            stageTut = new();
            stage0 = new();
            stage1 = new();
            stage2 = new();

            MayorDecisions.Add(MayorDecisionKeys.RenovatedGhetto, null);
            MayorDecisions.Add(MayorDecisionKeys.RebuiltHousing, null);
            MayorDecisions.Add(MayorDecisionKeys.TravelByCar, null);
            MayorDecisions.Add(MayorDecisionKeys.BuiltSchool, null);
            MayorDecisions.Add(MayorDecisionKeys.BuiltBikeInfrastructure, null);
            MayorDecisions.Add(MayorDecisionKeys.BuiltCarInfrastructure, null);
        }

        public void Play()
        {
            
            Parser parser = new();

            bool continuePlaying = true;
            lastOutputString = $"{map.CurrentRoom?.LongDescription}\n";

            screen.InitialiseScreen();

            while (continuePlaying) {
                if(mapMode) {
                    MapDrawer mapDrawer = new();
                }

                GameLogic.UpdateGameState();

                if(compareOutputString != lastOutputString) screen.DrawInfoText();
                screen.DrawMiniMap();
                screen.DrawInputText();
                // If input type is text input
                if(textInput == true) {

                    input = screen.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(input))
                    {
                        Screen.CommandOutputString.Add($"Please enter a command.");
                        Screen.CommandOutputString.Add($"> {input}");
                        continue;
                    }

                    Command? command = parser.GetCommand(input);

                    if (command == null)
                    {
                        Screen.CommandOutputString.Add($"Invalid command.");
                        Screen.CommandOutputString.Add($"> {input}");
                        continue;
                    }

                    continuePlaying = CommandHandler(command);
                } 
                // If input type is the menu navigation
                else {
                    input = screen.GetNewCommand();

                    if (string.IsNullOrEmpty(input))
                    {
                        continue;
                    }

                    Command? command = parser.GetCommand(input);
                    continuePlaying = CommandHandler(command);
                }
            }

            Console.Clear();
            Console.SetCursorPosition(1,1);
            Console.CursorVisible = true;
            Console.WriteLine("Thank you for playing Futurbs!");
        }

        private void Move(string direction)
        {
            if (map.CurrentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = map.CurrentRoom;
                map.CurrentRoom = map.CurrentRoom.Exits[direction];
                lastOutputString = $"{map.CurrentRoom?.LongDescription}\n";
            }
            else
            {
                Screen.CommandOutputString.Add($"You can't go {direction}!");
            }
        }
        public bool CommandHandler(Command? command) {
            switch(command?.Name)
                {
                    case "look":
                        lastOutputString = $"{map.CurrentRoom?.LongDescription}\n";
                        Screen.CommandOutputString.Add($"> {input}");
                        break;

                    case "back":
                        if (previousRoom == null) {
                            Screen.CommandOutputString.Add($"> {input}");
                            Screen.CommandOutputString.Add("You can't go back from here!");
                        }
                        else {
                            map.CurrentRoom = previousRoom;
                            lastOutputString = $"{map.CurrentRoom?.LongDescription}\n";
                            Screen.CommandOutputString.Add($"> {input}");
                            Screen.CommandOutputString.Add($"{map.CurrentRoom?.LongDescription}");
                        }
                        break;

                    case "north":
                    case "south":
                    case "east":
                    case "west":
                        Screen.CommandOutputString.Add($"> {input}");
                        Move(command.Name);
                        break;

                    case "quit":
                        return false;

                    case "help":
                        Screen.CommandOutputString.Add($"> {input}");
                        PrintHelp(command.SecondWord);
                        break;
                    case "togglein":
                        Screen.CommandOutputString.Add($"> {input}");
                        textInput = !textInput;
                        break;                    
                    case "textspeed":
                        Screen.CommandOutputString.Add($"> {input}");
                        ScrollingText(command.SecondWord);
                        break;

                    case "talk":
                        Screen.CommandOutputString.Add($"> {input}");
                        GameLogic.TalkToNPC(); 
                        break;

                    case "quests":
                        Screen.CommandOutputString.Add($"> {input}");
                        QuestsCommand(command.SecondWord);
                        break;
                    
                    case "map":
                        Screen.CommandOutputString.Add($"> {input}");
                        mapMode = true;
                        break;

                    default:
                        Debug.WriteLine("Unexpected Error: Somehow, the default switch of CommandHandler() was triggered, which should not be possible. Ensure you validate the input passed to CommandHandler beforehand.");
                        break;
                }
                return true;
        }

        private void QuestsCommand(string[]? args) {
            if(args?.Count() == 0) {
                string questString = "Quests: \n";
                foreach(Quests.Quest _ in currentQuests.Values) {
                    if(!_.Completed) {
                        questString += $"  {currentQuests.FirstOrDefault(x => x.Value == _).Key}: [ ] {_.Name}";
                        if(_.SubQuests != null) {
                            questString += ": ";
                            foreach(Quests.Quest sub in _.SubQuests.Values) if(!sub.Completed) questString += $"{sub.Name}, ";
                            questString = questString.Remove(questString.Length - 2, 2);
                        }
                        questString += "\n";
                    }
                }
                foreach(Quests.Quest _ in currentQuests.Values) if(_.Completed) questString += $"  {currentQuests.FirstOrDefault(x => x.Value == _).Key}: [X] {_.Name}\n";
                lastOutputString = questString.Remove(questString.Length - 1, 1);
            }
            else if(args?.Count() == 1 || args?.Count() > 2) Screen.CommandOutputString.Add("Invalid Syntax. Quests takes 2 arguments. Use [help quests] to find out more.");
            else {
                if(!Int32.TryParse(args?[1], out int id) || !currentQuests.ContainsKey(id)) {
                    Screen.CommandOutputString.Add($"Invalid Syntax. {args?[1]} is not a valid ID.");
                    return;
                }

                switch(args[0]) {
                    case "help":
                        lastOutputString = $"{currentQuests[id].Name}: {currentQuests[id].HelpText}";
                        break;

                    case "track":
                        if(TrackedQuests.Contains(currentQuests[id])) Screen.CommandOutputString.Add("This quest is already tracked.");
                        else {
                            TrackedQuests.Add(currentQuests[id]);
                            Screen.CommandOutputString.Add($"Tracked Quest \"{currentQuests[id].Name}\".");
                        }
                        break;

                    case "untrack":
                        if(!TrackedQuests.Contains(currentQuests[id])) Screen.CommandOutputString.Add("This quest is not tracked.");
                        else {
                            TrackedQuests.Remove(currentQuests[id]);
                            Screen.CommandOutputString.Add($"Untracked Quest \"{currentQuests[id].Name}\".");
                        }
                        break;
                    
                    default:
                        Screen.CommandOutputString.Add($"{args[0]} is not a valid argument. Use [help quests] for help.");
                        break;
                }
            }
        }
        private void ScrollingText(string[]? args) {
            if(args?.Count() > 1) {
                Screen.CommandOutputString.Add("Invalid Syntax. textspeed takes at most 1 argument. Use [help textspeed] for help.");
                return;
            }
            string outputString = "";
            if(args?.Count() == 0) {
                if(ScrollingTextSleepDuration == 0) {
                    ScrollingTextSleepDuration = 5;
                    outputString = "Text scrolling speed changed to 'fast'.";
                }
                else if(ScrollingTextSleepDuration == 5) {
                    ScrollingTextSleepDuration = 20;
                    outputString = "Text scrolling speed changed to 'medium'.";
                }
                else if(ScrollingTextSleepDuration == 20) {
                    ScrollingTextSleepDuration = 50;
                    outputString = "Text scrolling speed changed to 'slow'.";
                }
                else {
                    ScrollingTextSleepDuration = 0;
                    outputString = "Text scrolling speed changed to 'instant'.";
                }
            }
            else if(args?.Count() == 1) {
                switch(args[0]) {
                    case "instant":
                        outputString = "Text scrolling speed changed to 'instant'.";
                        ScrollingTextSleepDuration = 0;
                        break;

                    case "fast":
                        outputString = "Text scrolling speed changed to 'fast'.";
                        ScrollingTextSleepDuration = 5;
                        break;

                    case "medium":
                        outputString = "Text scrolling speed changed to 'medium'.";
                        ScrollingTextSleepDuration = 20;
                        break;

                    case "slow":
                        outputString = "Text scrolling speed changed to 'slow'.";
                        ScrollingTextSleepDuration = 100;
                        break;
                    default:
                        break;
                }
            }
                if(Program.game.textInput) Screen.CommandOutputString.Add(outputString);
                else lastOutputString = outputString;
            }

        private static void PrintHelp(string[]? arg)
        {
            if(arg?.Count() == 0 && !Program.game.textInput) Program.game.lastOutputString = "Navigate through the menu by using the arrow Keys.\nPress [Enter] to select a command.\nTo get help with specific commands, please use text input mode.";   
            else if(arg?.Count() == 0 && Program.game.textInput) {
                string helpString = "";
                for(int i = 0; i < 3; ++i) {
                    switch(i) {
                        case 0:
                            helpString += "Movement: ";
                            break;
                        case 1:
                            helpString += "Actions: ";
                            break;
                        case 2:
                            helpString += "Miscellaneous: ";
                            break;
                    }
                    foreach(CommandWords.GameCommand j in CommandWords.commandList[i]) helpString += $"{j.Name}, ";
                    helpString = helpString.Remove(helpString.Length - 2, 2);
                    helpString += "\n";
                }
                helpString = helpString.Remove(helpString.Length - 1, 1);
                Program.game.lastOutputString = helpString;
                Screen.CommandOutputString.Add("To get help with a specific command, type help + [command name].");
            }   
            else if(arg?.Count() > 1) Screen.CommandOutputString.Add("Invalid Syntax. Help takes 1 argument.");
            else {
                foreach(List<CommandWords.GameCommand> i in CommandWords.commandList) {
                    foreach(CommandWords.GameCommand j in i) {
                        if(j.Name == arg?[0]) {
                            Screen.CommandOutputString.Add(j.HelpText);
                            return;
                        }
                    }
                }
                Screen.CommandOutputString.Add($"{arg} is not a valid command. Cannot print help");
            }
        }
    }
}
