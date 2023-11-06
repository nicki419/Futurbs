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
    public class CommandWords {
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
        public static List<List<GameCommand>> commandList { get; } = new List<List<GameCommand>> { 
            /* Movement */ new List<GameCommand>() {
                new("north", "Move North", CommandCategory.Movement, 'w'),
                new("east", "Move East", CommandCategory.Movement, 'd'),
                new("south", "Move South", CommandCategory.Movement, 's'),
                new("west", "Move West", CommandCategory.Movement, 'a'),
                new("back", "Head to the previous room", CommandCategory.Movement, 'b')
            },

            /* Actions */ new List<GameCommand>() {
                new("look", "Investigate your surroundings", CommandCategory.Actions, 'l'),
                new("help", "Print Help", CommandCategory.Actions, 'h'),
                // UNCOMMENT WHEN READY TO BE USED IN FRONTEND - ALSO UNCOMMENT IN Game.cs -> CommandHandler
                //new("build", "Creates a new Building.", CommandCategory.Actions, 'b'),
                
            },

            /* Miscellaneous */ new List<GameCommand>() {
                new("quit", "Exit the game", CommandCategory.Miscellaneous, null),
                new("togglein", "Toggle between command input modes", CommandCategory.Miscellaneous, null),
                new("textspeed", "Sets/toggles text scrolling speed: 'instant', 'fast', 'medium', 'slow'.", CommandCategory.Miscellaneous, null),
                
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
