using System;
using System.Collections.Generic;
using System.Linq;
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

            public GameCommand(string name, string helpText, CommandCategory category) {
                this.Name = name;
                this.HelpText = helpText;
                this.Category = category;
            }
        }
        public List<GameCommand> commandList { get; } = new List<GameCommand> { 
            new("north", "Move North", CommandCategory.Movement), 
            new("east", "Move East", CommandCategory.Movement),
            new("south", "Move South", CommandCategory.Movement),
            new("west", "Move West", CommandCategory.Movement),
            new("look", "Investigate your sorroundings", CommandCategory.Actions),
            new("help", "Print Help", CommandCategory.Actions),
            new("back", "Head to the previous room", CommandCategory.Movement),
            new("quit", "Exit the game", CommandCategory.Miscellaneous),
            new("togglein", "Toggle between command input modes", CommandCategory.Miscellaneous)
            };

        public bool IsValidCommand(string command)
        {
            foreach(GameCommand _ in commandList) {
                if(_.Name == command) return true;
            }
            return false;
        }

        public string GenerateCommandString(GameCommand activeCommand) {
            string commandScreenString = "--- Commands ---\n\n";
            string[] categoryNames = Enum.GetNames(typeof(CommandWords.CommandCategory));

            // List Containing x arrays filled with all commands from that category
            List<List<string>> commandStringList = new List<List<string>>();

            // For every category, create new list in commandStringList
            for(int i = 0; i < categoryNames.Length; ++i) {
                commandStringList.Add(new List<string>());
                commandStringList[i].Add(i.ToString());

                // For every command in commandList, check if the command is of the current i loop iteration's categroy, and if so, add it
                for(int j = 0; j < commandList.Count(); ++j) {
                    if((int) commandList[j].Category == int.Parse(commandStringList[i][0])) commandStringList[i].Add(commandList[j].Name);
                }
            }

            // Construct the string that's being returned.
            // First, a new line printing all the categories
            for(int i = 0; i < categoryNames.Length; ++i) {
                commandScreenString += $"{categoryNames[Int32.Parse(commandStringList[i][0])]}";
                for(int j = 0; j < 3 - (categoryNames[Int32.Parse(commandStringList[i][0])].Length) / 4; ++j) commandScreenString += "\t";
            }
            // Remove last three characters (unnecessary tabs), add newline
            if(3 - (categoryNames[Int32.Parse(commandStringList[commandStringList.Count()-1][0])].Length) / 4 != 0) commandScreenString = commandScreenString.Remove(commandScreenString.Length - 3 - (categoryNames[Int32.Parse(commandStringList[commandStringList.Count()-1][0])].Length) / 4, 3 - (categoryNames[Int32.Parse(commandStringList[commandStringList.Count()-1][0])].Length) / 4);
            commandScreenString += "\n";

            // Determine the largest sublist in terms of amount of commands to see how many lines we need
            int totalCommandColumns = 0;
            foreach(List<string> _ in commandStringList) if(_.Count > totalCommandColumns) totalCommandColumns = _.Count;
            --totalCommandColumns;

            for(int i = 1; i <= totalCommandColumns; ++i) {
                for(int j = 0; j < categoryNames.Length; ++j) {    
                    if(commandStringList[j].ElementAtOrDefault(i) != null) {
                    if(commandStringList[j][i] == activeCommand.Name) commandScreenString += $" > {commandStringList[j][i]}";
                    else commandScreenString += $"  {commandStringList[j][i]}";

                    // Figure out how many tabs to use to align commandName, assuming a tab is 4 spaces
                    for(int k = 0; k < 3 - (commandStringList[j][i].Length + 3) / 4; ++k) commandScreenString += "\t";
                    } else commandScreenString += "\t\t";
                }
                commandScreenString += "\n";
            }

            return commandScreenString;
        }

        public GameCommand MenuNavigator(GameCommand activeCommand) {

            // To make this work: Put commandStringList generating code of GenerateCommandString into a new function so that this 
            // function can also access it. 
            
            switch(Console.ReadKey().Key) {
                case ConsoleKey.UpArrow:
                    break;
                
                case ConsoleKey.DownArrow:
                    break;

                case ConsoleKey.LeftArrow:
                    break;
                
                case ConsoleKey.RightArrow:
                    break;
                
                case ConsoleKey.Enter:
                    return activeCommand;
            }

            return activeCommand;
        }

        public GameCommand GetCommand(string commandName) {
            foreach(GameCommand _ in commandList) if(_.Name == commandName) return _;
            return commandList[0];
        }
    }

}

