using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class CommandWords
    {

        public enum CommandCategory {
            Movement,
            Actions,
            Miscellaneous
        }

        // UPDATE WHENEVER CommandCategory IS CHANGED!
        public const int NumberOfCategories = 3;
        public struct GameCommand {
            public string Name;
            public string HelpText;
            public CommandCategory Category;
            public char? Hotkey;

            public GameCommand(string name, string helpText, CommandCategory category, char? hotkey) {
                this.Name = name;
                this.HelpText = helpText;
                this.Category = category;
                this.Hotkey = hotkey;
            }
        }

        // Define the list of commands two-dimensionally
        public List<List<GameCommand>> commandList { get; } = new List<List<GameCommand>> { 
            /* Movement */ new List<GameCommand>() {
                new("north", "Move North", CommandCategory.Movement, 'w'),
                new("east", "Move East", CommandCategory.Movement, 'd'),
                new("south", "Move South", CommandCategory.Movement, 's'),
                new("west", "Move West", CommandCategory.Movement, 'a'),
                new("back", "Head to the previous room", CommandCategory.Movement, 'b')
            },

            /* Actions */ new List<GameCommand>() {
                new("look", "Investigate your sorroundings", CommandCategory.Actions, 'l'),
                new("help", "Print Help", CommandCategory.Actions, 'h'),
                
            },

            /* Miscellaneous */ new List<GameCommand>() {
                new("quit", "Exit the game", CommandCategory.Miscellaneous, null),
                new("togglein", "Toggle between command input modes", CommandCategory.Miscellaneous, null),
                new("minimap", "Toggles the mini map", CommandCategory.Miscellaneous, null),
            }
        };

        public bool IsValidCommand(string command)
        {
            foreach(List<GameCommand> i in commandList) {
                foreach(GameCommand _ in i) {
                    if(_.Name == command) return true;
                }
            }
            return false;
        }

        public string GenerateCommandString(GameCommand activeCommand) {
            string commandScreenString = "--- Commands ---\n\n";
            string[] categoryNames = Enum.GetNames(typeof(CommandWords.CommandCategory));

            // Construct the string that's being returned.
            // First, a new line printing all the categories
            for(int i = 0; i < categoryNames.Length; ++i) {
                commandScreenString += $"{categoryNames[i]}";
                for(int j = 0; j < 3 - categoryNames[i].Length  / 4; ++j) commandScreenString += "\t";
            }
            // Remove last three characters (unnecessary tabs), add newline
            if(3 - categoryNames[NumberOfCategories-1].Length / 4 != 0) commandScreenString = commandScreenString.Remove(commandScreenString.Length - 3 - (categoryNames[NumberOfCategories-1].Length) / 4, 3 - (categoryNames[NumberOfCategories-1].Length) / 4);
            commandScreenString += "\n";

            // Determine the largest sublist in terms of amount of commands to see how many lines we need
            int totalCommandColumns = 0;
            foreach(List<GameCommand> _ in commandList) if(_.Count > totalCommandColumns) totalCommandColumns = _.Count;
            --totalCommandColumns;

            // Loop through commandWords row by row (not column by coloumn as foreach would do) to print the screen
            for(int i = 1; i <= totalCommandColumns+1; ++i) {
                for(int j = 0; j < categoryNames.Length; ++j) {   
                    // if our current position in the sublist is smaller than the amount of entries in that sublist
                    if(i-1 < commandList[j].Count()) {
                    if(commandList[j][i-1].Name == activeCommand.Name){
                        commandScreenString += $" > {commandList[j][i-1].Name}";

                        // Figure out how many tabs to use to align commandName, assuming a tab is 8 spaces
                        for(int k = 0; k < 2 - ($" > {commandList[j][i-1].Name}".Length) / 8; ++k) commandScreenString += "\t";
                    }
                    else {
                        commandScreenString += $"  {commandList[j][i-1].Name}";

                        // Figure out how many tabs to use to align commandName, assuming a tab is 8 spaces
                        for(int k = 0; k < 3 - ($"  {commandList[j][i-1].Name}".Length + 7) / 8; ++k) commandScreenString += "\t";
                        }

                    } else commandScreenString += "\t\t";
                }
                commandScreenString += "\n";
            }

            return commandScreenString;
        }

        public (GameCommand, GameCommand) MenuNavigator(GameCommand activeCommand) {
            List<GameCommand> currentList = commandList[(int) activeCommand.Category];
            GameCommand newCommand = new();
            GameCommand selectedCommand = new();
            
            switch(Console.ReadKey().Key) {
                case ConsoleKey.UpArrow:
                    if(currentList.IndexOf(activeCommand) == 0) newCommand = activeCommand;
                    else newCommand = commandList[(int) activeCommand.Category][currentList.IndexOf(activeCommand) - 1];
                    break;
                
                case ConsoleKey.DownArrow:
                    if(currentList.IndexOf(activeCommand) == currentList.Count() - 1) newCommand = activeCommand;
                    else newCommand = commandList[(int) activeCommand.Category][currentList.IndexOf(activeCommand) + 1];
                    break;

                case ConsoleKey.LeftArrow:
                    if(commandList.IndexOf(currentList) == 0) newCommand = activeCommand;
                    else {
                        if(currentList.IndexOf(activeCommand) < commandList[(int) activeCommand.Category - 1].Count()) newCommand = commandList[(int) activeCommand.Category - 1][currentList.IndexOf(activeCommand)];
                        else newCommand = commandList[(int) activeCommand.Category - 1][commandList[(int) activeCommand.Category - 1].Count() - 1];
                        
                    }
                    break;
                
                case ConsoleKey.RightArrow:
                    if(commandList.IndexOf(currentList) == commandList.Count() - 1) newCommand = activeCommand;
                    else {
                        if(currentList.IndexOf(activeCommand) < commandList[(int) activeCommand.Category + 1].Count()) newCommand = commandList[(int) activeCommand.Category + 1][currentList.IndexOf(activeCommand)];
                        else newCommand = commandList[(int) activeCommand.Category + 1][commandList[(int) activeCommand.Category + 1].Count() - 1];
                        
                    }
                    break;
                
                case ConsoleKey.Enter:
                    newCommand = activeCommand;
                    selectedCommand = activeCommand;
                    break;
            }
            return (newCommand, selectedCommand);
        }

        public GameCommand GetCommand(string commandName) {
            foreach(List<GameCommand> i in commandList) foreach(GameCommand _ in i) if(_.Name == commandName) return _;
            return commandList[0][0];
        }
    }

}
