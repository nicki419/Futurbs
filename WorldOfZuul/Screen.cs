using System.Runtime.CompilerServices;
using Figgle;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters;
using System.Xml;

namespace WorldOfZuul {
    public class Screen {

        private readonly CommandWords commandWords = new();

        public CommandWords.GameCommand activeCommand;
        
        public (int, int) NamecardDimensions = (98, 7);
        public (int, int, int) TopBoxDimensions = (73, 25, 7);
        public (int, int, int) BottomBoxDimensions = (73, 25, 8);
        public static List<string> CommandOutputString = new() {""};

        // https://en.wikipedia.org/wiki/Box-drawing_character
        public List<char> BoxCharacters = new() {
                /* ═ */ '\u2550', // 0
                /* ║ */ '\u2551', // 1
                /* ╔ */ '\u2554', // 2
                /* ╗ */ '\u2557', // 3
                /* ╚ */ '\u255a', // 4
                /* ╝ */ '\u255d', // 5

                /* ╠ */ '\u2560', // 6
                /* ╣ */ '\u2563', // 7
                /* ╦ */ '\u2566', // 8
                /* ╩ */ '\u2569', // 9
                /* ╬ */ '\u256c'  // 10
            };
        

        public void DrawOutlines(){
            Console.Clear();
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, 0);

            // Writing the BoxCharacters of the Namecard
            // Top Row
            Console.Write(BoxCharacters[2]);
            for(int i = 0; i <= NamecardDimensions.Item1; ++i) Console.Write(BoxCharacters[0]);
            Console.Write(BoxCharacters[3]);

            // Side rows
            for(int i = 0; i <= NamecardDimensions.Item2; ++i) {
                Console.Write("\n");
                Console.Write(BoxCharacters[1]);
                Console.Write(new string(' ', NamecardDimensions.Item1 + 1));
                Console.Write(BoxCharacters[1]);
            }
            Console.Write("\n");

            // Bottom row
            Console.Write(BoxCharacters[6]);
            for(int i = 0; i <= NamecardDimensions.Item1; ++i) Console.Write(BoxCharacters[0]);
            Console.Write(BoxCharacters[7]);
            Console.SetCursorPosition(BottomBoxDimensions.Item1 + 2, Console.GetCursorPosition().Top);
            Console.Write(BoxCharacters[8]);

            // Side Rows
            for(int i = 0; i <= TopBoxDimensions.Item3; ++i) {
                Console.Write("\n");
                Console.Write(BoxCharacters[1]);
                Console.Write(new string(' ', TopBoxDimensions.Item1 + 1));
                Console.Write(BoxCharacters[1]);
                Console.Write(new string(' ', TopBoxDimensions.Item2 - 1));
                Console.Write(BoxCharacters[1]);
            }
            Console.Write("\n");
            // Bottom row
            Console.Write(BoxCharacters[6]);
            for(int i = 0; i <= TopBoxDimensions.Item1 + TopBoxDimensions.Item2; ++i) Console.Write(BoxCharacters[0]);
            Console.SetCursorPosition(BottomBoxDimensions.Item1 + 2, Console.GetCursorPosition().Top);
            for(int i = 0; i < TopBoxDimensions.Item2; ++i) Console.Write(' ');
            Console.Write(BoxCharacters[1]);
            Console.SetCursorPosition(BottomBoxDimensions.Item1 + 2, Console.GetCursorPosition().Top);
            Console.Write(BoxCharacters[7]);

            //Side Rows
            for(int i = 0; i <= BottomBoxDimensions.Item3; ++i) {
                Console.Write("\n");
                Console.Write(BoxCharacters[1]);
                Console.Write(new string(' ', BottomBoxDimensions.Item1 + 1));
                Console.Write(BoxCharacters[1]);
                Console.Write(new string(' ', BottomBoxDimensions.Item2 - 1));
                Console.Write(BoxCharacters[1]);
            }

            Console.Write("\n");
            
            //Bottom row
            Console.Write(BoxCharacters[4]);
            for(int i = 0; i <= BottomBoxDimensions.Item1; ++i) Console.Write(BoxCharacters[0]);
            Console.Write(BoxCharacters[9]);
            for(int i = 0; i < BottomBoxDimensions.Item2 - 1; ++i) Console.Write(BoxCharacters[0]);
            Console.Write(BoxCharacters[5]);

            
        }

        public void DrawNamecard() {
            //Console.Clear();
            string fontString = FiggleFonts.Chunky.Render("       -=    Futurbs    =-");
            string[] stringArray = Regex.Split(fontString, "\r\n|\r|\n");
            
            for(int i = 0; i < stringArray.Count(); ++i) {
                Console.SetCursorPosition(1, i+1);
                Console.Write(stringArray[i]);
            }
            Console.SetCursorPosition(1, Console.GetCursorPosition().Top + 1);
            Console.Write("       Group 12:  Arhan Erdogan, Mónica Lopez, Niklas Braun, Petar Dreznjak, Rokas Norbutas");
        }

        public void DrawMiniMap() {
            string mapString = Game.map.MiniMap();
            string[] stringArray = Regex.Split(mapString, "\r\n|\r|\n");

            // Clean the screen:
            for(int i = NamecardDimensions.Item2 + 3; i <= NamecardDimensions.Item2 + 3 + TopBoxDimensions.Item3 + BottomBoxDimensions.Item3 + 2; ++i) {
                Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, i);
                Console.Write(new string(' ', TopBoxDimensions.Item2 -2));
            }
            

            Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, NamecardDimensions.Item2 + 3);
            Console.Write("You Are Here:");
            Console.SetCursorPosition(TopBoxDimensions.Item1 + 6, Console.GetCursorPosition().Top + 1);
            Console.Write(stringArray[0]);
            Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, Console.GetCursorPosition().Top + 1);

            for(int i = 1; i < stringArray.Count(); ++i){
                Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, Console.GetCursorPosition().Top + 1);
                Console.Write(stringArray[i]);
            }

            Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, Console.GetCursorPosition().Top + 1);
            Console.Write("Tracked Quests:");

            // Running loop twice so that completed quests are printed always at the bottom
            foreach(Quests.Quest _ in Game.TrackedQuests) {
                if(Console.GetCursorPosition().Top <= 25 && !_.Completed) {
                    Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, Console.GetCursorPosition().Top + 1);
                    Console.Write($"  {_.Name}");
                }
            }
            foreach(Quests.Quest _ in Game.TrackedQuests) {
                if(Console.GetCursorPosition().Top <= 25 && _.Completed) {
                    Console.SetCursorPosition(TopBoxDimensions.Item1 + 4, Console.GetCursorPosition().Top + 1);
                    Console.Write($"X {_.Name}");
                }
            }
        }

        public void DrawInfoText(){
            Program.game.compareOutputString = Program.game.lastOutputString;
            string[] stringArray = Regex.Split(Program.game.lastOutputString, "\r\n|\r|\n");

            // Clean the screen
            for(int i = NamecardDimensions.Item2 + 3; i <= NamecardDimensions.Item2 + 3 + TopBoxDimensions.Item3; ++i) {
                Console.SetCursorPosition(2, i);
                Console.Write(new string(' ', TopBoxDimensions.Item1));
            }
            
            Console.SetCursorPosition(2, NamecardDimensions.Item2 + 3);
            // Loop through every string in stringArray
            foreach(string i in stringArray) {
                // Split string into words
                string[] wordArray = i.Split(" ");
                // Loop through every word
                foreach(string j in wordArray) {
                    // Check if the next word + space would be too large for the current line, if so, new line
                    if(Console.GetCursorPosition().Left + j.Count() + 1 >= 75) {
                        Console.SetCursorPosition(4, Console.GetCursorPosition().Top + 1);
                    }
                    // Loop through and print every character.
                    foreach(char _ in j){
                        Console.Write(_);
                        if(Program.game.ScrollingTextSleepDuration > 0) Thread.Sleep(Program.game.ScrollingTextSleepDuration);
                    }
                    Console.Write(' ');
                }
                
                
                
                Console.SetCursorPosition(2, Console.GetCursorPosition().Top + 1);
            }
        }

        public void DrawInputText() {
            //Clear the screen
            for(int i = NamecardDimensions.Item2 + TopBoxDimensions.Item3+ 5; i <= NamecardDimensions.Item2 + 5 + TopBoxDimensions.Item3 + BottomBoxDimensions.Item3; ++i) {
                Console.SetCursorPosition(2, i);
                Console.Write(new string(' ', TopBoxDimensions.Item1));
            }
            if(Program.game.textInput) {
                Console.CursorVisible = true;
                Console.SetCursorPosition(2, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + BottomBoxDimensions.Item3 + 5);
                Console.Write("> ");
                Console.SetCursorPosition(2, Console.GetCursorPosition().Top - 2);

                // The ToList() is necessary, even though this actually is a list, so there's no error... :')
                // https://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
                if(CommandOutputString.Count == 0) return;

                for(int i = CommandOutputString.Count() - 1; i > 0; --i) {
                    Console.SetCursorPosition(2, Console.GetCursorPosition().Top - CommandOutputString[i].Length / 73);                    
                    if(Console.GetCursorPosition().Top>= NamecardDimensions.Item2 + TopBoxDimensions.Item3 +5) {
                        // Split string into words
                        string[] wordArray = CommandOutputString[i].Split(" ");
                        // Loop through every word
                        foreach(string j in wordArray) {
                            // Check if the next word + space would be too large for the current line, if so, new line
                            if(Console.GetCursorPosition().Left + j.Count() + 1 >= 75) {
                                Console.SetCursorPosition(4, Console.GetCursorPosition().Top + 1);
                            }
                            // Loop through and print every character.
                            foreach(char _ in j){
                                Console.Write(_);
                                //if(Program.game.ScrollingTextSleepDuration > 0) Thread.Sleep(Program.game.ScrollingTextSleepDuration);
                            }
                            Console.Write(' ');
                        }
                        Console.SetCursorPosition(2, Console.GetCursorPosition().Top - CommandOutputString[i].Length / 73 - 1);
                    }
                    else CommandOutputString.RemoveAt(i);
                }
                Console.SetCursorPosition(4, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + BottomBoxDimensions.Item3 + 5);
                Console.CursorVisible = true;
            } 
            else {
                Console.CursorVisible = false;
                if(activeCommand.Name == null) {
                    activeCommand = CommandWords.commandList[0][0];
                }

                Console.SetCursorPosition(2, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + 5);
                Console.Write("--- Commands ---");
                Console.SetCursorPosition(2, Console.GetCursorPosition().Top + 2);
                string[] categoryNames = Enum.GetNames(typeof(CommandWords.CommandCategory));

                // First category (Movement)
                Console.Write(Enum.GetName(typeof(CommandWords.CommandCategory), 0));

                foreach(CommandWords.GameCommand _ in CommandWords.commandList[0]) {
                    Console.SetCursorPosition(2, Console.GetCursorPosition().Top + 1);
                    if(_.Name == activeCommand.Name) Console.Write($" > {_.Name}");
                    else Console.Write($" {_.Name}");
                }

                // Second Category (Actions)
                Console.SetCursorPosition(19, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + 7);
                Console.Write(Enum.GetName(typeof(CommandWords.CommandCategory), 1));

                foreach(CommandWords.GameCommand _ in CommandWords.commandList[1]) {
                    Console.SetCursorPosition(19, Console.GetCursorPosition().Top + 1);
                    if(_.Name == activeCommand.Name) Console.Write($" > {_.Name}");
                    else Console.Write($" {_.Name}");
                }

                // Third Category (Miscellaneous)
                Console.SetCursorPosition(36, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + 7);
                Console.Write(Enum.GetName(typeof(CommandWords.CommandCategory), 2));

                foreach(CommandWords.GameCommand _ in CommandWords.commandList[2]) {
                    Console.SetCursorPosition(36, Console.GetCursorPosition().Top + 1);
                    if(_.Name == activeCommand.Name) Console.Write($" > {_.Name}");
                    else Console.Write($" {_.Name}");
                }
            }
        }

        public void ClearForMap() {
            // Clean the screen
            for(int i = NamecardDimensions.Item2 + 3; i <= NamecardDimensions.Item2 + TopBoxDimensions.Item3 + BottomBoxDimensions.Item3 + 5; ++i) {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', TopBoxDimensions.Item1 + 1));
            }
            Console.SetCursorPosition(0, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + 4);
            Console.Write(BoxCharacters[1]);
            Console.SetCursorPosition(TopBoxDimensions.Item1 + 2, NamecardDimensions.Item2 + TopBoxDimensions.Item3 + 4);
            Console.Write(BoxCharacters[1]);
        }

        public void InitialiseScreen() {
            DrawOutlines();
            DrawNamecard();
            DrawMiniMap();
            DrawInfoText();
            DrawInputText();
        }

        public string? ReadLine() {
            if(!Program.game.textInput) {
                Program.game.textInput = true;
                DrawInputText();
            }
            string? output = "";
            while(true) {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);
                //string inputString = $"{inputKey.KeyChar}";

                if(inputKey.Key == ConsoleKey.Backspace) {
                    // nested if needed to trigger above condition on all backspace presses. Otherwise will add "Backspace" 
                    // to string and continue to remove characters afterwards, going off screen.
                    if(output.Length > 0) {
                        output = output.Remove(output.Length - 1, 1); 
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                    }
                }
                else if(inputKey.Key == ConsoleKey.Enter) {
                    if(output.Length == 0) return null;
                    else return output;
                }
                else if(inputKey.Key == ConsoleKey.Spacebar) {
                    output += " ";
                    Console.Write(' ');
                }
                else if(new Regex("^[A-Za-z0-9_.]+$").IsMatch($"{inputKey.KeyChar}") && Console.GetCursorPosition().Left < 74) {
                    output += $"{inputKey.KeyChar}";
                    Console.Write($"{inputKey.KeyChar}".ToLower());
                    //Console.Write(inputKey.ToString());
                }

                else if(Console.GetCursorPosition().Left == 74) {
                    Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                    Console.Write(' ');
                    Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                }
            }
        }
        public string? GetNewCommand(){
            CommandWords.GameCommand selectedCommand;
            (activeCommand, selectedCommand) = commandWords.MenuNavigator(activeCommand);
            return selectedCommand.Name;
        }
    }
}