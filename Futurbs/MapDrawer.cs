namespace Futurbs
{
    public class MapDrawer
    {
        private Room selectedRoom = Game.map.CurrentRoom;
        private (int, int) selectedRoomPosition;
        private (int, int) currentRoomPosition;
        private List<Room> processedRooms = new();
        private bool ExitMap = false;

        public MapDrawer() {
            InitialiseScreen();
            DrawMap();
            MapLogic();
        }
        private void InitialiseScreen() {
            Console.CursorVisible = false;
            Program.game.screen.ClearForMap();
        }

        private void MapLogic() {
            while(true) {
                // Refresh information every loop
                Console.SetCursorPosition(2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write(new string(' ', Program.game.screen.TopBoxDimensions.Item1));
                Console.SetCursorPosition(2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write($"Currently Selected: {selectedRoom.ShortDescription} \t");
                if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] != null) {
                    Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1 - 27, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                    Console.Write("Press [Enter] to fast travel.");
                }
                Console.SetCursorPosition(selectedRoomPosition.Item1, selectedRoomPosition.Item2);
                Room? fastTravelRoom = NavigateMap();
                if(fastTravelRoom != null) FastTravel(fastTravelRoom);

                if(ExitMap) {
                    ExitMapMode();
                    break;
                }
            }
        }

        private void DrawMap() {
            Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1/2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + 5);
            if(selectedRoom == Game.map.Rooms["cityCentre"] || (Game.map.Rooms.ContainsKey("tutorialRoom") && selectedRoom == Game.map.Rooms["tutorialRoom"])) {
                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                currentRoomPosition = selectedRoomPosition;
                Console.Write("[X]");
            } else Console.Write("[ ]");
            
            
            if(GameLogic.GameStage >= 0) {
                processedRooms.Add(Game.map.Rooms["cityCentre"]);

                Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1/2 + 1, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + 5);
                Room currentRoom = Game.map.Rooms["cityCentre"];
                DrawRoom(currentRoom, null);
            }
        }

        private void DrawRoom(Room currentRoom, string? previousRoom) {
            foreach(KeyValuePair<string, Room> _ in currentRoom.Exits) {
                if(_.Value != null && _.Key != previousRoom) {
                    switch(_.Key) {
                        case "north":
                            if(!processedRooms.Contains(_.Value)) {       
                                Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 1);
                                Console.Write('\u2502');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top - 1);
                                if(_.Value == selectedRoom) {
                                    selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                    currentRoomPosition = selectedRoomPosition;
                                    Console.Write("[X]");
                                } else Console.Write("[ ]");
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                processedRooms.Add(_.Value);
                                DrawRoom(_.Value, "south");
                            } else {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 1);
                                Console.Write('\u2502');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 1);
                            }

                            break;
                        
                        case "east":
                            if(!processedRooms.Contains(_.Value)) {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left + 2, Console.GetCursorPosition().Top);
                                Console.Write('\u2500');
                                if(_.Value == selectedRoom) {
                                    selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                    currentRoomPosition = selectedRoomPosition;
                                    Console.Write("[X]");
                                } else Console.Write("[ ]");
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                processedRooms.Add(_.Value);
                                DrawRoom(_.Value, "west");
                            } else {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left + 2, Console.GetCursorPosition().Top);
                                Console.Write('\u2500');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 3, Console.GetCursorPosition().Top);
                            }

                            break;

                        case "south":
                            if(!processedRooms.Contains(_.Value)) {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top + 1);
                                Console.Write('\u2502');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top + 1);
                                if(_.Value == selectedRoom) {
                                    selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                    currentRoomPosition = selectedRoomPosition;
                                    Console.Write("[X]");
                                } else Console.Write("[ ]");
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                processedRooms.Add(_.Value);
                                DrawRoom(_.Value, "north");
                            } else {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top + 1);
                                Console.Write('\u2502');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top - 1);
                            }
                            break;

                        case "west":
                            if(!processedRooms.Contains(_.Value)) {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                Console.Write('\u2500');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 4, Console.GetCursorPosition().Top);
                                if(_.Value == selectedRoom) {
                                    selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                    currentRoomPosition = selectedRoomPosition;
                                    Console.Write("[X]");
                                } else Console.Write("[ ]");
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                processedRooms.Add(_.Value);
                                DrawRoom(_.Value, "east");
                            } else {
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                                Console.Write('\u2500');
                                Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            }

                            break;
                    }
                }
            }
            if(previousRoom != null) {
                switch(previousRoom) {
                    case "north":
                        Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 2);
                        previousRoom = null;
                        break;

                    case "east":
                        Console.SetCursorPosition(Console.GetCursorPosition().Left + 4, Console.GetCursorPosition().Top);
                        break;
                    
                    case "south":
                        Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top + 2);
                        previousRoom = null;
                        break;
                    
                    case "west":
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 4, Console.GetCursorPosition().Top);
                        break;
                }
            }
        }

        private Room? NavigateMap() {
            switch(Console.ReadKey(true).Key) {
                case ConsoleKey.Escape:
                    ExitMap = true;
                    break;
                
                case ConsoleKey.Enter:
                    if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] != null && selectedRoom != Game.map.CurrentRoom) {
                        return selectedRoom;
                    }
                    break;
                
                case ConsoleKey.UpArrow:
                    if(selectedRoom.Exits.ContainsKey("north") && selectedRoom.Exits["north"] != null) {
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top - 2);
                        Console.Write('X');
                        selectedRoomPosition = (Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                        selectedRoom = selectedRoom.Exits["north"];
                    }
                    break;
                
                case ConsoleKey.RightArrow:
                    if(selectedRoom.Exits.ContainsKey("east") && selectedRoom.Exits["east"] != null) {
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.GetCursorPosition().Left + 3, Console.GetCursorPosition().Top);
                        Console.Write('X');
                        selectedRoomPosition = (Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                        selectedRoom = selectedRoom.Exits["east"];
                    }
                    break;
                
                case ConsoleKey.DownArrow:
                    if(selectedRoom.Exits.ContainsKey("south") && selectedRoom.Exits["south"] != null) {
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 2);
                        Console.Write('X');
                        selectedRoomPosition = (Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                        selectedRoom = selectedRoom.Exits["south"];
                    }
                    break;
                
                case ConsoleKey.LeftArrow:
                    if(selectedRoom.Exits.ContainsKey("west") && selectedRoom.Exits["west"] != null) {
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.GetCursorPosition().Left - 5, Console.GetCursorPosition().Top);
                        Console.Write('X');
                        selectedRoomPosition = (Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                        selectedRoom = selectedRoom.Exits["west"];
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }

        private void ExitMapMode() {
            Program.game.mapMode = false;
            Program.game.screen.DrawOutlines();
            Program.game.screen.DrawNamecard();
            Program.game.screen.DrawMiniMap();
            Program.game.screen.DrawInfoText();
            Program.game.screen.DrawInputText();
            if(Program.game.textInput) Console.CursorVisible = true;
        }

        private void FastTravel(Room destinationRoom) {
            Dictionary<int, List<char>> possiblePaths = new();
            int dictIndex = 0;
            List<char> path = new();
            processedRooms = new();
            possiblePaths.Add(0, new());
            FindPath(Game.map.CurrentRoom, null);
            path = new(possiblePaths[0]);
            for(int i = 1; i < possiblePaths.Count; ++i) {
                if(possiblePaths[i].Count > 0 && possiblePaths[i].Count < path.Count) path = new(possiblePaths[i]);
            }
            DrawPath();
            int travelSpeed;
            if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == true) {
                if(Program.game.MayorDecisions[Game.MayorDecisionKeys.BuiltCarInfrastructure] == true) travelSpeed = 100; else travelSpeed = 150;
            } else {
                if(Program.game.MayorDecisions[Game.MayorDecisionKeys.BuiltBikeInfrastructure] == true) travelSpeed = 500; else travelSpeed = 1000;
            }
            GoToPath();

            void FindPath(Room currentRoom, string? previousRoom) {
                foreach(KeyValuePair<string, Room> _ in currentRoom.Exits) {
                    if(!processedRooms.Contains(_.Value) && _.Key != previousRoom) {
                        processedRooms.Add(_.Value);
                        switch(_.Key){
                            case "north":
                                possiblePaths[dictIndex].Add('n');
                                if(_.Value == selectedRoom) {
                                    ++dictIndex;
                                    possiblePaths.Add(dictIndex, new(possiblePaths[dictIndex - 1]));
                                }
                                FindPath(_.Value, "south");
                                break;

                            case "east":
                                possiblePaths[dictIndex].Add('e');
                                if(_.Value == selectedRoom) {
                                    ++dictIndex;
                                    possiblePaths.Add(dictIndex, new(possiblePaths[dictIndex - 1]));
                                }
                                FindPath(_.Value, "west");
                                break;

                            case "south":
                                possiblePaths[dictIndex].Add('s');
                                if(_.Value == selectedRoom) {
                                    ++dictIndex;
                                    possiblePaths.Add(dictIndex, new(possiblePaths[dictIndex - 1]));
                                }
                                FindPath(_.Value, "north");
                                break;

                            case "west":
                                possiblePaths[dictIndex].Add('w');
                                if(_.Value == selectedRoom) {
                                    ++dictIndex;
                                    possiblePaths.Add(dictIndex, new(possiblePaths[dictIndex - 1]));
                                }
                                FindPath(_.Value, "east");
                                break;
                        }
                    }
                }
                if(possiblePaths[dictIndex].Count > 0) possiblePaths[dictIndex].RemoveAt(possiblePaths[dictIndex].Count - 1);
                processedRooms.Remove(currentRoom);
            }
            void DrawPath() {
                Console.Write(' ');
                Console.SetCursorPosition(1, Program.game.screen.NamecardDimensions.Item2 + 3);
                Console.Write(new string(' ', Program.game.screen.TopBoxDimensions.Item1));
                Console.SetCursorPosition(1, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write(new string(' ', Program.game.screen.TopBoxDimensions.Item1));
                Console.SetCursorPosition(2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write($"{(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == true ? "Driving" : "Cycling")} to {selectedRoom.ShortDescription}...");

                Console.SetCursorPosition(currentRoomPosition.Item1, currentRoomPosition.Item2);
                Console.Write('X');
                Console.SetCursorPosition(currentRoomPosition.Item1, currentRoomPosition.Item2);
                foreach(char _ in path) {
                    switch(_) {
                        case 'n':
                            Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 1);
                            Console.Write('\u2551');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top - 1);
                            Thread.Sleep(150);
                            break;

                        case 'e':
                            Console.SetCursorPosition(Console.GetCursorPosition().Left + 2, Console.GetCursorPosition().Top);
                            Console.Write('\u2550');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                            Thread.Sleep(150);
                            break;

                        case 's':
                            Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top + 1);
                            Console.Write('\u2551');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 1);
                            Thread.Sleep(150);
                            break;

                        case 'w':
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            Console.Write('\u2550');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 3, Console.GetCursorPosition().Top);
                            Thread.Sleep(150);
                            break;

                    }
                }
            }
            void GoToPath() {
                Console.SetCursorPosition(currentRoomPosition.Item1, currentRoomPosition.Item2);

                foreach(char _ in path) {
                    Thread.Sleep(travelSpeed);
                    switch(_) {
                        case 'n':
                            Console.Write(' ');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top - 1);
                            Console.Write('X');
                            Thread.Sleep(travelSpeed);
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            Console.Write('\u2551');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top - 1);
                            Console.Write('X');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            break;

                        case 'e':
                            Console.Write(' ');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                            Console.Write('X');
                            Thread.Sleep(travelSpeed);
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            Console.Write('\u2550');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                            Console.Write('X');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            break;

                        case 's':
                            Console.Write(' ');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 1);
                            Console.Write('X');
                            Thread.Sleep(travelSpeed);
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            Console.Write('\u2551');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top + 1);
                            Console.Write('X');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            break;

                        case 'w':
                            Console.Write(' ');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 3, Console.GetCursorPosition().Top);
                            Console.Write('X');
                            Thread.Sleep(travelSpeed);
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            Console.Write('\u2550');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 3, Console.GetCursorPosition().Top);
                            Console.Write('X');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                            break;

                    }
                }
                Thread.Sleep(travelSpeed);
                Game.map.CurrentRoom = destinationRoom;
                Program.game.lastOutputString = Game.map.CurrentRoom.LongDescription;
                ExitMap = true;
            }
        }
    }
}