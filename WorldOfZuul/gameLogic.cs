using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class gameLogic
    {
        public static int GameStage;

        public gameLogic() {
            GameStage = -1;
        }


        public class StageTut {
            public static Map StageMap = new(new string[] {});

            public static Dictionary<string, Quests.Quest> Quests = new() {
                {"tutorial", new(
                    0.01f,
                    "Intro to Quests",
                    "Use 'help' to get an overview of commands to use. Use 'help' followed by the command name to learn how to use that command, e.g. 'help quests'. To finish this quest, track it.",
                    "notCompleted",
                    null,
                    WorldOfZuul.Quests.QuestType.stageProgress
                )}
            };

            public StageTut() {
                StageMap.Rooms.Add("tutorialRoom", new("Tutorial", "Welcome, Mayor, to Futurbs! This short tutorial will help guide you the right way to manage the city. To start, use the 'help' command to get an overview of the commands you can use. To view this message again, use the 'look' command. Make sure to read everything carefully, as you cannot access this tutorial again later on.", null));
                StageMap.CurrentRoom = StageMap.Rooms["tutorialRoom"];
                Game.currentQuests.Add(1, Quests["tutorial"]);
            }

            public static void UpdateState() {
                if(Game.TrackedQuests.Contains(Quests["tutorial"])) {
                    Quests["tutorial"].CompletionCondition = "completed";
                }
                foreach(KeyValuePair<int, Quests.Quest> _ in Game.currentQuests) _.Value.updateQuest(_.Key);
                if(Quests["tutorial"].Completed) {
                    Screen.CommandOutputString.Add("Well done. You are now ready to enter the city.");
                    GameStage = 0;
                    Stage0.InitialiseState();
                }
            }
        }


        public class Stage0 {
            public static Map StageMap = new(new string[] {"townHall", "mayorsOffice"});

            public static Dictionary<string, Quests.Quest> Quests = new(){
                {"headToOffice", new(
                0.1f,
                "Head to your Office",
                "Use cardinal direction commands to navigate to your office.",
                "Mayor's Office",
                null,
                WorldOfZuul.Quests.QuestType.visitRoom
            )}
            };

            public static Dictionary<string, bool> StageProgression = new() {
                {"questsDone", false},
            };

            public static NPC[] npcs = {
                new(
                    "Advisor",
                    "Your Advisor in all matters around the city",
                    new() {
                        {0, "Welcome to the city, Sir! Care to share your name?"},
                        {1, "Pleasure meeting you, [playerName]! To start out, you should familiarise yourself with the city. Have a little walk, and return to me once you have visited every area. The map command can help you get an overview of the city. One last thing: You can keep track of your progress with the quests command. Use [help quests] to find out how to track your quests."},
                    },
                    $"What a lovely day, isn't it {Game.playerName}?",
                    StageMap.Rooms["mayorsOffice"]
                )
            };

            public Stage0() {
                StageMap.Rooms["cityCentre"].SetExits(StageMap.Rooms["townHall"], null, null, null);
                //StageMap.CurrentRoom = StageMap.Rooms["cityCentre"];
            }

            public static void InitialiseState() {
                Game.currentQuests = new();
                //Game.TrackedQuests = new();
                //foreach(int _ in Game.currentQuests.Keys) Game.currentQuests.Remove(_);

                int questCounter = 1;
                foreach(Quests.Quest _ in Quests.Values) {
                    Game.currentQuests.Add(questCounter, _);
                    ++questCounter;
                }
                Game.map = StageMap;
                StageMap.CurrentRoom = StageMap.Rooms["cityCentre"];
                UpdateState();
                Program.game.lastOutputString = StageMap.Rooms["cityCentre"].LongDescription;
                //Program.game.screen.DrawInfoText();
            }
            public static void UpdateState() {
                foreach(KeyValuePair<int, Quests.Quest> _ in Game.currentQuests) _.Value.updateQuest(_.Key);
                if(Quests["headToOffice"].Completed && !Quests.ContainsKey("talkToAdvisor")) {
                    Quests.Add(
                        "talkToAdvisor", new(
                        0.2f,
                        "Talk to your Advisor",
                        "After navigating to your office, use the talk command to talk to your advisor.",
                        "uncompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress
                    ));
                    Game.currentQuests.Add(2, Quests["talkToAdvisor"]);
                    Quests["talkToAdvisor"].updateQuest(2);
                }
                if(Quests["headToOffice"].Completed && Quests.ContainsKey("talkToAdvisor") && Quests["talkToAdvisor"].Completed) StageProgression["questsDone"] = true;
                if(StageProgression["questsDone"]) {
                    GameStage = 1;
                    Stage1.InitialiseState();
                }
                //Game.map = StageMap;
            }

            public static void Advisor() {
                if(!Quests["talkToAdvisor"].Completed) {
                    Program.game.lastOutputString = $"Advisor: {npcs[0].Dialogue[0]}";
                    do {
                        Program.game.screen.DrawInfoText();
                        Program.game.screen.DrawInputText();
                        Game.playerName = Program.game.screen.ReadLine()?.ToUpper();
                        if(Game.playerName != null) Game.playerName = new String(Game.playerName.Select((ch, index) => (index == 0) ? ch : Char.ToLower(ch)).ToArray());
                        if(Game.playerName == null) Program.game.lastOutputString = $"Advisor: You can't be without a name, mayor, can you? Please tell me your name.";
                    } while(Game.playerName == null);
                    Program.game.lastOutputString = $"Advisor: {npcs[0].Dialogue[1].Replace("[playerName]", Game.playerName)}";
                    Quests["talkToAdvisor"].CompletionCondition = "completed";
                
                }
            }
        }

        public class Stage1 {
            public static Map StageMap =  new(new string[] {"cityCentre", "townHall", "trainStation", "park1", "market", "residentialHouses", "recreationalArea1", "residentialBlocks", "park2", "ghetto", "IndustrialZone", "recreationalArea2", "mayorsOffice"});
            public Dictionary<string, bool> StageProgression;

            public static Dictionary<string, Quests.Quest> Quests = new(){
                {"exploreCity", new(
                1.1f,
                "Explore the City",
                "Use cardinal direction commands to navigate throughout the city. Use the map command to get an overview of the city.",
                null,
                new() {
                    {1, new(
                        1.11f,
                        "Visit City Centre",
                        "",
                        "City Centre",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {2, new(
                        1.12f,
                        "Visit Train Station",
                        "",
                        "Train Station",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {3, new(
                        1.13f,
                        "Visit Ghetto",
                        "",
                        "Ghetto",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {4, new(
                        1.14f,
                        "Visit Park 2",
                        "",
                        "Park 2",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {5, new(
                        1.15f,
                        "Visit Park 1",
                        "",
                        "Park 1",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {6, new(
                        1.16f,
                        "Visit Residential Blocks",
                        "",
                        "Residential Blocks",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {7, new(
                        1.17f,
                        "Visit Industrial Zone",
                        "",
                        "Industrial Zone",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {8, new(
                        1.18f,
                        "Visit Recreational Area 2",
                        "",
                        "Recreational Area 2",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {9, new(
                        1.19f,
                        "Visit Market",
                        "",
                        "Market",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {10, new(
                        1.110f,
                        "Visit Residential Houses",
                        "",
                        "Residential Houses",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                    {11, new(
                        1.111f,
                        "Visit Recreational Area 1",
                        "",
                        "Recreational Area 1",
                        null,
                        WorldOfZuul.Quests.QuestType.visitRoom,
                        false
                    )},
                },
                WorldOfZuul.Quests.QuestType.subQuests
            )},
            };

            public static NPC[] npcs = {
                new(
                    "Advisor",
                    "Your Advisor in all matters around the city",
                    new() {
                        {0, "Are you sure you've visited the entire city yet?"},
                        {1, "Well done, [playerName]! Now that you know the city a little better, you can start setting it en route for a better tomorrow."},
                    },
                    $"What a lovely day, isn't it {Game.playerName}?",
                    StageMap.Rooms["mayorsOffice"]
                )
            };

            public Stage1() {
                StageMap.CurrentRoom = StageMap.Rooms["mayorsOffice"];
                StageProgression = new() {
                    {"visitTown", false},
                    {"talkToAdvisor", false},
                };
            }

            public static void InitialiseState() {
                Game.currentQuests = new();
                Game.TrackedQuests = new();
                //foreach(int _ in Game.currentQuests.Keys) Game.currentQuests.Remove(_);

                int questCounter = 1;
                foreach(Quests.Quest _ in Quests.Values) {
                    Game.currentQuests.Add(questCounter, _);
                    ++questCounter;
                }
                Game.map = StageMap;
                StageMap.CurrentRoom = StageMap.Rooms["townHall"];
                UpdateState();
            }
            public static void UpdateState() {
                foreach(KeyValuePair<int, Quests.Quest> _ in Game.currentQuests) _.Value.updateQuest(_.Key);
                if(Quests["exploreCity"].Completed && !Quests.ContainsKey("talkToAdvisor")) {
                    Quests.Add(
                        "talkToAdvisor",
                        new(
                        1.2f,
                        "Talk to Advisor",
                        "Return to your office to tell your advisor that you have visited the city.",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress
                    ));
                    Game.currentQuests.Add(2, Quests["talkToAdvisor"]);
                }
            }

            public static void Advisor() {
                if(!Quests["exploreCity"].Completed) Program.game.lastOutputString = $"Advisor: {npcs[0].Dialogue[0]}";
                else {
                    Program.game.lastOutputString = $"Advisor: {npcs[0].Dialogue[1].Replace("[playerName]", Game.playerName)}";
                    Quests["talkToAdvisor"].Completed = true;
                }
            }
        }

        public class Stage2{

            public static Map StageMap =  new(new string[] {"cityCentre", "townHall", "trainStation", "park1", "market", "residentialHouses", "recreationalArea1", "residentialBlocks", "park2", "ghetto", "IndustrialZone", "recreationalArea2", "mayorsOffice"});

            public Dictionary<string, bool> StageProgression;
            public static Dictionary<string, Quests.Quest> Quests = new(){
                {"mayorsDuty", new(
                    2.1f,
                    "Mayor's Duty",
                    "As the new Mayor, you have a few decisions to make, talk to your Advisor to for help.",
                    null,
                    new(){
                    {1, new(
                        2.11f,
                        "Choose Transport",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.StageProgression,
                        false
                    )},
                    {2, new(
                        2.12f,
                        "Read the News",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.StageProgression,
                        false
                    )},
                    {3, new(
                        2.13f,
                        "Inspect the Ghetto",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.StageProgression,
                        false
                    )}, },
                    WorldOfZuul.Quests.QuestType.SubQuests
                )

                },                       
                }
                
            public static void NPC[] npcs = {
                new(
                    "Advisor",
                    "Your Advisor in all matters around the city",
                    new() {
                        {0, "You need to choose a mode of transportation, you can go to the City center and proceed to the car dealership, alternativly you can go to the Market and get a bike, which might be better but slower."}
                        {1, "Congratulations on getting a car, now you can go where ever you want instantly, albeit at the cost of polluting the city."}
                        {2, "Congratulations on getting a bike, now you can get around town faster without in a climate-friendly manner."}
                    },
                    $"What a lovely day, isn't it {Game.playerName}?",
                    StageMap.Rooms["mayorsOffice"]
                )
            };

            public Stage2(){
                StageMap.CurrentRoom = StageMap.Rooms["townHall"];
                StageProgression = new(){
                    {"mayorsDuty", false},
                    {"theGhetoQuestion", false}
                }
            };
            
            public static void InitialiseState(){
                Game.currentQuests = new();
                Game.TrackedQuests = new();
                
                int questCounter = 1;
                foreach(Quests.Quest _ in Quests.Values) {
                    Game.currentQuests.Add(questCounter, _);
                    ++questCounter;
                }
                Game.map = StageMap;
                StageMap.CurrentRoom = StageMap.Rooms["townHall"];
                UpdateState();
            }

            public static void UpdateState(){
                foreach(KeyValuePair<int, Quests.Quest> _ in Game.currentQuests) _.Value.updateQuest(_.Key);
                if(Quests["mayorsDuty"].Completed && !Quests.ContainsKey("theGhettoQuestion")){
                    Quests.Add("theGhettoQuestion", new(
                        2.2f,
                        "The Ghetto Question",
                        "Having inspected the situation in the Ghettos, you must now make a decision that impacts the future of the area. Talk to your advisor to make the decision.",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgression
                    ))
                }

            }

            public void Advisor(){
                if(!Quests["mayorsDuty"].Completed){
                    
                }
            }


        }

        public static void UpdateGameState() {
            switch(GameStage) {
                case -1:
                    StageTut.UpdateState();
                    break;
                case 0:
                    Stage0.UpdateState();
                    break;

                case 1:
                    Stage1.UpdateState();
                    break;
                case 2:
                    Stage2.UpdateGameState();
                    break;
            }
        }
    }
}