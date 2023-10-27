﻿using System.Data;
using System.Diagnostics;

namespace WorldOfZuul
{
    public class Game
    {
        //private Room? map?.CurrentRoom;
        private Room? previousRoom;

        // Don't forget to create a new object
        private Map map = new();

        private readonly Screen screen = new();
        public CommandWords.GameCommand activeCommand;
        private bool textInput = true;
        private bool mapToggle = false;
        private string? lastOutputString;
        private string? mapString;


        public Game()
        {
            
        }


        public void Play()
        {
            
            Parser parser = new();

            //map.CurrentRoom = map.cityCentre;
            

            bool continuePlaying = true;
            lastOutputString = $"Welcome to the World of Zuul!\nWorld of Zuul is a new, incredibly boring adventure game.\n\n{map?.CurrentRoom?.LongDescription}\n";

            while (continuePlaying)
            {
                if(mapToggle) mapString = map?.MiniMap();
                else mapString = null;
                
                // If input type is text input
                if(textInput == true) {
                    screen.PrintScreen(lastOutputString, mapString, textInput, mapToggle);

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
                    
                    screen.PrintScreen(lastOutputString, mapString, textInput, mapToggle);
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
            if (map?.CurrentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = map.CurrentRoom;
                map.CurrentRoom = map.CurrentRoom.Exits[direction];
                lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
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
                        lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
                        break;

                    case "back":
                        if (previousRoom == null)
                            lastOutputString = "You can't go back from here!";
                        else
                            map.CurrentRoom = previousRoom;
                            lastOutputString = $"{map?.CurrentRoom?.LongDescription}\n";
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
                        mapToggle = !mapToggle;
                        break;
                    /*  UNCOMMENT WHEN READY TO BE USED IN FRONTEND - ALSO UNCOMMENT CommandWords.cs -> commandList
                    case "build":
                        lastOutputString = map.CreateBuilding("School", "Here Kids go to have 12 years of neverending fun!", (map.Rooms["recreationalArea1"], null, null, map.Rooms["ghetto"]));
                        //lastOutputString += map.Rooms["School"].Exits["east"].ShortDescription;
                        break;
                    */

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
