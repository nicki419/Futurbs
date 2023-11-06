using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace WorldOfZuul
{
    public class Game
    {
        //private Room? map?.CurrentRoom;
        private Room? previousRoom;

        // Don't forget to create a new object
        public Map map = new();

        public readonly Screen screen = new();
        public CommandWords.GameCommand activeCommand;
        public bool textInput = true;
        public string lastOutputString = "";
        public string compareOutputString = "";
        public int ScrollingTextSleepDuration = 20;
        private string? input;


        public Game()
        {

        }


        public void Play()
        {
            
            Parser parser = new();

            bool continuePlaying = true;
            lastOutputString = $"Welcome to Futurbs!\nFuturbs is a new, incredibly not-boring adventure game.\n\n{map?.CurrentRoom?.LongDescription}\n";
            //PrintHelp(null);

            screen.InitialiseScreen();

            while (continuePlaying) {
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
            if (map?.CurrentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = map.CurrentRoom;
                map.CurrentRoom = map.CurrentRoom.Exits[direction];
                lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
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
                        lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
                        Screen.CommandOutputString.Add($"> {input}");
                        //Screen.CommandOutputString.Add($"{map?.CurrentRoom?.LongDescription}");
                        break;

                    case "back":
                        if (previousRoom == null) {
                            Screen.CommandOutputString.Add($"> {input}");
                            Screen.CommandOutputString.Add("You can't go back from here!");
                        }
                        else {
                            map.CurrentRoom = previousRoom;
                            lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
                            Screen.CommandOutputString.Add($"> {input}");
                            Screen.CommandOutputString.Add($"{map?.CurrentRoom?.LongDescription}");
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
                        //break;

                    case "help":
                        Screen.CommandOutputString.Add($"> {input}");
                        PrintHelp(command.SecondWord);
                        break;
                    case "togglein":
                        Screen.CommandOutputString.Add($"> {input}");
                        textInput = !textInput;
                        break;
                    /* // UNCOMMENT WHEN READY TO BE USED IN FRONTEND - ALSO UNCOMMENT CommandWords.cs -> commandList
                    case "build":
                        lastOutputString = map.CreateBuilding("School", "Here Kids go to have 12 years of neverending fun!", (map.Rooms["recreationalArea1"], null, null, map.Rooms["ghetto"]));
                        //lastOutputString += map.Rooms["School"].Exits["east"].ShortDescription;
                        break;
                    */
                    case "textspeed":
                        Screen.CommandOutputString.Add($"> {input}");
                        ScrollingText(command?.SecondWord);
                        break;

                    default:
                        // Console Write unnecessary because of earlier input evaluation, thus passed a debug note instead.
                        Debug.WriteLine("Unexpected Error: Somehow, the default switch of CommandHandler() was triggered, which should not be possible. Ensure you validate the input passed to CommandHandler beforehand.");
                        break;
                }
                return true;
        }

        private void ScrollingText(string? args) {
            string outputString = "";

            switch(args) {
                case "instant":
                    outputString = "Text scrolling speed changed to 'instant'.";
                    //Screen.CommandOutputString.Add($"> {input}");
                    ScrollingTextSleepDuration = 0;
                    break;

                case "fast":
                    outputString = "Text scrolling speed changed to 'fast'.";
                    //Screen.CommandOutputString.Add($"> {input}");
                    ScrollingTextSleepDuration = 5;
                    break;

                case "medium":
                    outputString = "Text scrolling speed changed to 'medium'.";
                    //Screen.CommandOutputString.Add($"> {input}");
                    ScrollingTextSleepDuration = 20;
                    break;

                case "slow":
                    outputString = "Text scrolling speed changed to 'slow'.";
                    //Screen.CommandOutputString.Add($"> {input}");
                    ScrollingTextSleepDuration = 100;
                    break;
                default:
                    if(args != "") {
                        if(ScrollingTextSleepDuration == 0) {
                            ScrollingTextSleepDuration = 5;
                            outputString = "Text scrolling speed changed to 'fast'.";
                        }
                        else if(ScrollingTextSleepDuration == 5) {
                            ScrollingTextSleepDuration = 20;
                            outputString = "Text scrolling speed changed to 'medium'.";
                        }
                        else if(ScrollingTextSleepDuration == 20) {
                            ScrollingTextSleepDuration = 100;
                            outputString = "Text scrolling speed changed to 'slow'.";
                        }
                        else {
                            ScrollingTextSleepDuration = 0;
                            outputString = "Text scrolling speed changed to 'instant'.";
                        }
                    } 
                    else {
                        outputString = $"Unknown argument '{args}'. Toggle between modes by specifying no argument, or use 'instant', 'fast', 'medium', or 'slow' to specify a scrolling speed.";
                    }
                    break;
                }

                if(Program.game.textInput) Screen.CommandOutputString.Add(outputString);
                else lastOutputString = outputString;
            }

        private static void PrintHelp(string? arg)
        {
            //List<string> helpStr = new();
            if(arg == null && !Program.game.textInput) Program.game.lastOutputString = "Navigate through the menu by using the arrow Keys.\nPress [Enter] to select a command.\nTo get help with specific commands, please use text input mode.";   
            else if(arg == null && Program.game.textInput) {
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
            else {
                foreach(List<CommandWords.GameCommand> i in CommandWords.commandList) {
                    foreach(CommandWords.GameCommand j in i) {
                        if(j.Name == arg) {
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
