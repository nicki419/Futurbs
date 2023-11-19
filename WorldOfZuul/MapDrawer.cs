using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class MapDrawer
    {
        private Room selectedRoom = Game.map.CurrentRoom;
        private (int, int) selectedRoomPosition;
        private List<Room> processedRooms = new();
        private bool ExitMap = false;

        public MapDrawer() {
            InitialiseScreen();
            DrawMap();
            MapLogic();
        }
        private void InitialiseScreen() {
            //Console.CursorVisible = false;
            Program.game.screen.ClearForMap();
        }

        private void MapLogic() {
            while(true) {
                // Refresh information every loop
                Console.SetCursorPosition(2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write(new string(' ', Program.game.screen.TopBoxDimensions.Item1));
                Console.SetCursorPosition(2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                Console.Write($"Currently Selected: {selectedRoom.ShortDescription} \t");
                if(gameLogic.GameStage >= 3) {
                    Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1 - 27, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + Program.game.screen.BottomBoxDimensions.Item3 + 5);
                    Console.Write("Press [Enter] to fast travel.");
                }
                Console.SetCursorPosition(selectedRoomPosition.Item1, selectedRoomPosition.Item2);
                Room? fasttravelRoom = NavigateMap();
                if(ExitMap) {
                    ExitMapMode();
                    break;
                }
            }
        }

        private void DrawMap() {
            Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1/2, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + 5);
            if(selectedRoom == Game.map.Rooms["cityCentre"] || (Game.map.Rooms.ContainsKey("totorialRoom") && selectedRoom == Game.map.Rooms["tutorialRoom"])) {
                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                Console.Write("[X]");
            } else Console.Write("[ ]");
            
            
            if(gameLogic.GameStage >= 0) {
                processedRooms.Add(Game.map.Rooms["cityCentre"]);

                Console.SetCursorPosition(Program.game.screen.TopBoxDimensions.Item1/2 + 1, Program.game.screen.NamecardDimensions.Item2 + Program.game.screen.TopBoxDimensions.Item3 + 5);
                Room currentRoom = Game.map.Rooms["cityCentre"];
                DrawRoom(currentRoom, null);
            }
        }

        private void DrawRoom(Room currentRoom, string? previousRoom) {
            foreach(KeyValuePair<string, Room> _ in currentRoom.Exits) {
                if(_.Value != null && _.Key != previousRoom && !processedRooms.Contains(_.Value)) {
                    switch(_.Key) {
                        case "north":
                            Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 1);
                            Console.Write('\u2502');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top - 1);
                            if(_.Value == selectedRoom) {
                                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                Console.Write("[X]");
                            } else Console.Write("[ ]");
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            processedRooms.Add(_.Value);
                            DrawRoom(_.Value, "south");

                            break;
                        
                        case "east":
                            Console.SetCursorPosition(Console.GetCursorPosition().Left + 2, Console.GetCursorPosition().Top);
                            Console.Write('\u2500');
                            if(_.Value == selectedRoom) {
                                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                Console.Write("[X]");
                            } else Console.Write("[ ]");
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            processedRooms.Add(_.Value);
                            DrawRoom(_.Value, "west");

                            break;

                        case "south":
                            Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top + 1);
                            Console.Write('\u2502');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top + 1);
                            if(_.Value == selectedRoom) {
                                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                Console.Write("[X]");
                            } else Console.Write("[ ]");
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            processedRooms.Add(_.Value);
                            DrawRoom(_.Value, "north");
                            break;

                        case "west":
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            Console.Write('\u2500');
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 4, Console.GetCursorPosition().Top);
                            if(_.Value == selectedRoom) {
                                selectedRoomPosition = (Console.GetCursorPosition().Left + 1, Console.GetCursorPosition().Top);
                                Console.Write("[X]");
                            } else Console.Write("[ ]");
                            Console.SetCursorPosition(Console.GetCursorPosition().Left - 2, Console.GetCursorPosition().Top);
                            processedRooms.Add(_.Value);
                            DrawRoom(_.Value, "east");

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
        }
    }
}