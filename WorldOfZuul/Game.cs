using System.Data;
using System.Diagnostics;

namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
        private readonly Screen screen = new();
        public CommandWords.GameCommand activeCommand;
        private bool textInput = true;
        private string? lastOutputString;

        private Map? map;

        public Game()
        {
            map?.CreateMap();
        }


        public void Play()
        {
            Parser parser = new();

            bool continuePlaying = true;
            lastOutputString = $"Welcome to the World of Zuul!\nWorld of Zuul is a new, incredibly boring adventure game.\n\n{currentRoom?.LongDescription}\n";

            while (continuePlaying)
            {
                // If input type is text input
                if(textInput == true) {
                    screen.PrintScreen(lastOutputString, textInput);

                    string? input = Console.ReadLine()?.ToLower();
                    //string? input = "north";

                    if (string.IsNullOrEmpty(input))
                    {
                        lastOutputString = $"Please enter a command.";
                        continue;
                    }

                    Command? command = parser.GetCommand(input);

                    if (command == null)
                    {
                        lastOutputString = $"I don't know that command.";
                        continue;
                    }

                    continuePlaying = CommandHandler(command);
                } 
                // If input type is the menu navigation
                else {
                    
                    screen.PrintScreen(lastOutputString, textInput);
                    string? input = screen.GetNewCommand();

                    if (string.IsNullOrEmpty(input))
                    {
                        continue;
                    }

                    Command? command = parser.GetCommand(input);
                    continuePlaying = CommandHandler(command);
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private void Move(string direction)
        {
            if (currentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = currentRoom;
                currentRoom = currentRoom?.Exits[direction];
                lastOutputString = $"{currentRoom?.LongDescription}\n";
            }
            else
            {
                lastOutputString = $"You can't go {direction}!";
            }
        }

        public bool CommandHandler(Command? command) {

            switch(command?.Name)
                {
                    case "look":
                        lastOutputString = $"{currentRoom?.LongDescription}\n";
                        break;

                    case "back":
                        if (previousRoom == null)
                            lastOutputString = "You can't go back from here!";
                        else
                            currentRoom = previousRoom;
                            lastOutputString = $"{currentRoom?.LongDescription}\n";
                        break;

                    case "north":
                    case "south":
                    case "east":
                    case "west":
                        Move(command.Name);
                        break;

                    case "quit":
                        return false;
                        //break;

                    case "help":
                        lastOutputString = PrintHelp();
                        break;
                    case "togglein":
                        textInput = !textInput;
                        break;
                    case "minimap":
                        map?.MiniMap();
                        break;

                    default:
                        // Console Write unnecessary because of earlier input evaluation, thus passed a debug note instead.
                        Debug.WriteLine("Unexpected Error: Somehow, the default switch of CommandHandler() was triggered, which should not be possible. Ensure you validate the input passed to CommandHandler beforehand.");
                        break;
                }
                return true;
        }
        private static string PrintHelp()
        {
            return @"Navigate by typing 'north', 'south', 'east', or 'west'.
'look'      - print more details about the current room.
'back'      - go to the previous room.
'help'      - print this message again.
'toggleIn'  - toggle input mode between text and menu navigation.
'quit'      - exit the game.
            ";
        }
    }
}
