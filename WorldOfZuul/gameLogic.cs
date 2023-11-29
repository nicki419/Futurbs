using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
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
                    "Advisor ",
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
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[0]}";
                    do {
                        Program.game.screen.DrawInfoText();
                        Program.game.screen.DrawInputText();
                        Game.playerName = Program.game.screen.ReadLine()?.ToUpper();
                        if(Game.playerName != null) Game.playerName = new String(Game.playerName.Select((ch, index) => (index == 0) ? ch : Char.ToLower(ch)).ToArray());
                        if(Game.playerName == null) Program.game.lastOutputString = $"{npcs[0].Name}: You can't be without a name, mayor, can you? Please tell me your name.";
                    } while(Game.playerName == null);
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[1].Replace("[playerName]", Game.playerName)}";
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
                StageMap.CurrentRoom = StageMap.Rooms["mayorsOffice"];
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
                else if(Quests["exploreCity"].Completed && Quests.ContainsKey("talkToAdvisor") && Quests["talkToAdvisor"].Completed) {
                    GameStage = 2;
                    Stage2.InitialiseState();
                }
            }

            public static void Advisor() {
                if(!Quests["exploreCity"].Completed) Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[0]}";
                else {
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[1].Replace("[playerName]", Game.playerName)}";
                    Quests["talkToAdvisor"].Completed = true;
                }
            }
        }

        public class Stage2 {

            public static Map StageMap =  new(new string[] {"cityCentre", "townHall", "trainStation", "park1", "market", "residentialHouses", "recreationalArea1", "residentialBlocks", "park2", "ghetto", "IndustrialZone", "recreationalArea2", "mayorsOffice"});

            public static Dictionary<string, bool> StageProgression = new(){
                {"mayorsDuty", false},
                {"theGhettoQuestion", false},
                {"townNews", false}
            };
            public static Dictionary<string, Quests.Quest> Quests = new(){
                {"mayorsDuty", new(
                    2.1f,
                    "Mayor's Duty",
                    "As the new Mayor, you have a few decisions to make, talk to your Advisor to for help.",
                    null,
                    new(){
                    {1, new(
                        2.11f,
                        "Choose Mode of Transport",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress,
                        false
                    )},
                    
                    {2, new(
                        2.12f,
                        "Talk to the Informant",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress,
                        false
                    )},
                    {3, new(
                        2.13f,
                        "Inspect the Ghetto",
                        "",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress,
                        false
                    )}, },
                    WorldOfZuul.Quests.QuestType.subQuests,
                    false   
                )},             
            };
                
            public static NPC[] npcs = {
                new(
                    "Advisor Nick",
                    "Your Advisor in all matters around the city",
                    new() {
                        {0, "You need to choose a mode of transportation, you can go to the City center and proceed to the car dealership, alternativly you can go to the Market and get a bike, which might be better but slower."},
                        {1, "Congratulations on getting a car, now you can go whereever you want instantly, albeit at the cost of polluting the city."},
                        {2, "Congratulations on getting a bike, now you can get around town faster without in a climate-friendly manner."},
                        {3, "After you have inspected the Ghetto and spoke with the residents you must now make a decision. You can either renovate it and improve the living conditions or demolish it and start from scratch. What dp you wish to do? (renovate/demolish)"},
                        {4, "Good decision, I am sure the citizens will be looking forward to it. We shall place them in steel 18 square metter containers until the renovations are done."},
                        {5, "A brave decision for a better future for the city. We shall place them in steel 18 square metter containers until the teraforming is done."}
                    },
                    $"What a lovely day, isn't it {Game.playerName}?",
                    StageMap.Rooms["mayorsOffice"]
                ),
                new(
                    "Car Vendor Petar",
                    "Your go to man for vehicular travel",
                    new() {
                        {0, "Welcome mayor to my humble garage, which takes care of all your vehicular needs. I heard you are in need of a car and I have specially prepared a 2009 VW Passat 2.0 TDI just for you boss man. Would you like this car? (y/n)"},
                        {1, "This car will provide you great comfort, but at a price... Use the map command, select your destination with the arrow keys, and press [Enter] to travel anywhere in the city."},
                        {2, "Come back anytime if you change your mind!"},
                        {3, "You chose to travel by bike, huh? Oh well, don't expect me to respect you on the road."}
                    },
                    "",
                    StageMap.Rooms["cityCentre"]
                ),
                new(
                    "Bike Vendor Mónica",
                    "The Woman specialising in two-wheeled endeavours.",
                    new() {
                        {0, "Welcome, Mayor! I'm glad you want to travel around by bike. While it might not be the fastest way to get around, it is most certainly the best for the environment! Would you like a bike? (y/n)"},
                        {1, "Thank you for choosing to travel by bike! Use the map command, select your destination with the arrow keys, and press [Enter] to travel anywhere in the city."},
                        {2, "Come back anytime if you change your mind!"},
                        {3, "It looks like you already chose a car... I hope you made the right choice."}
                        },
                    "",
                    StageMap.Rooms["market"]
                ),
                new(
                    "Informant Arhan",
                    "The man that knows all the news of Futurbs",
                    new(){
                        {0, "Hello, Mayor! On todays new's the city is doing moderatly well, except there has been some concerns in The Ghetto you should talk to the citizens there to find out what are their concerns."},
                        {1, "What a lovely day, isn't it [playerName]?"}
                    },
                    "",
                    StageMap.Rooms["townHall"]
                ),
                new(
                    "Ghetto Citizen Rokas",
                    "One of the last citizens of the ghetto",
                    new(){
                        {0, "What a pleasent surprise, welcome Mayor. Life in The Ghetto is tough, especially these times; but it's the only place that people like me can afford to live nowadays. It would be nice to see this place renovated, but that isn't in our hands."},
                        {1, "I once was a mayor like you, but then I got an arrow to the knee"}
                    },
                    "",
                    StageMap.Rooms["ghetto"]
                )
            };

            public Stage2(){
                StageMap.CurrentRoom = StageMap.Rooms["townHall"];
            }
            
            public static void InitialiseState(){
                Game.currentQuests = new();
                Game.TrackedQuests = new();
                
                int questCounter = 1;
                foreach(Quests.Quest _ in Quests.Values) {
                    Game.currentQuests.Add(questCounter, _);
                    ++questCounter;
                }
                Game.map = StageMap;
                StageMap.CurrentRoom = StageMap.Rooms["mayorsOffice"];
                UpdateState();
            }

            public static void UpdateState(){
                foreach(KeyValuePair<int, Quests.Quest> _ in Game.currentQuests) _.Value.updateQuest(_.Key);
                if(Quests["mayorsDuty"].Completed && !Quests.ContainsKey("theGhettoQuestion")){
                    Quests.Add("theGhettoQuestion", new(
                        2.2f,
                        "The Ghetto Question",
                        "Having inspected the situation in the Ghetto, you must now make a decision that impacts the future of the area. Talk to your advisor to make the decision.",
                        "notCompleted",
                        null,
                        WorldOfZuul.Quests.QuestType.stageProgress
                    ));
                    Game.currentQuests.Add(2, Quests["theGhettoQuestion"]);
                    Quests["theGhettoQuestion"].updateQuest(2);
                }
                if(StageProgression["mayorsDuty"] && StageProgression["theGhettoQuestion"]) {
                    GameStage = 3;
                    Program.game.stage3 = new();

                };

            }

            public static void Advisor(){
                Quests.Quest? mayorsDutyQuest;
                Quests.TryGetValue("mayorsDuty", out mayorsDutyQuest);
                
                if(mayorsDutyQuest?.SubQuests != null && !mayorsDutyQuest.SubQuests[1].Completed){
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[0]}";
                }
                else if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == true){
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[1]}";
                }
                else if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == false){
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[2]}";
                };

                if(Quests["mayorsDuty"].Completed && Quests.ContainsKey("theGhettoQuestion") && !Quests["theGhettoQuestion"].Completed){
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[3]}";
                    string? playerAnswer;
                    do{
                        Program.game.screen.DrawInfoText();
                        Program.game.screen.DrawInputText();
                        playerAnswer = Program.game.screen.ReadLine();
                        if(playerAnswer == null || !(playerAnswer == "renovate" || playerAnswer == "demolish")) Program.game.lastOutputString = $"Advisor: I don't understand your answer. Please tell me if you want to 'demolish' or 'renovate.' ";
                    }while(playerAnswer == null || !(playerAnswer == "renovate" || playerAnswer == "demolish"));
                    Quests["theGhettoQuestion"].CompletionCondition = "completed";
                    if(playerAnswer == "renovate"){
                        Program.game.MayorDecisions[Game.MayorDecisionKeys.RenovatedGhetto] = true;
                        Program.game.lastOutputString = $"Advisor : {npcs[0].Dialogue[4]}";
                        Game.map.Rooms["ghetto"].ShortDescription = "New Living Blocks";
                        Game.map.Rooms["ghetto"].LongDescription = "The newly renovated Ghettos can hardly be referred to as such anymore. The living conditions have been improved substantially!   ";
                        
                    }else {
                        Program.game.MayorDecisions[Game.MayorDecisionKeys.RenovatedGhetto] = false;
                        Program.game.lastOutputString = $"Advisor: {npcs[0].Dialogue[5]}";
                        Game.map.Rooms["trainStation"].Exits.Remove("east");
                        
                    }
                    UpdateGameState();
                }
            }

            public static void CarVendor(){
                if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == null){
                    string? playerAnswer;
                    Program.game.lastOutputString = $"{npcs[1].Name}: {npcs[1].Dialogue[0]}";
                    do {
                        Program.game.screen.DrawInfoText();
                        Program.game.screen.DrawInputText();
                        playerAnswer = Program.game.screen.ReadLine();
                        if(playerAnswer != null && !(playerAnswer == "y" || playerAnswer == "n")) Program.game.lastOutputString = $"{npcs[1].Name}: I don't understand your answer. Do you want the car? Answer either 'y' for yes or 'n' for no.";
                    } while(playerAnswer != null && !(playerAnswer == "y" || playerAnswer == "n"));
                        if(playerAnswer == "y"){
                            Program.game.lastOutputString = $"{npcs[1].Name}: {npcs[1].Dialogue[1]}";
                            Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] = true;
                            Quests.Quest? mayorsDutyQuest;
                            if(Quests.TryGetValue("mayorsDuty", out mayorsDutyQuest) && mayorsDutyQuest.SubQuests != null) mayorsDutyQuest.SubQuests[1].CompletionCondition = "completed";
                        } else Program.game.lastOutputString = npcs[1].Dialogue[2];
                } else if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == true) Program.game.lastOutputString = $"{npcs[1].Name}: {npcs[1].Dialogue[1]}";
                else Program.game.lastOutputString = $"{npcs[1].Name}: {npcs[1].Dialogue[3]}";
            }

            public static void BikeVendor(){
                if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] == null) {
                    string? playerAnswer;
                    Program.game.lastOutputString = $"Bike Vendor: {npcs[2].Dialogue[0]}";
                    do {
                        Program.game.screen.DrawInfoText();
                        Program.game.screen.DrawInputText();
                        playerAnswer = Program.game.screen.ReadLine();
                        if(playerAnswer != null && !(playerAnswer == "y" || playerAnswer == "n")) Program.game.lastOutputString = "Bike Vendor: I don't understand your answer. Do you want the bike? Answer either 'y' for yes or 'n' for no.";
                    } while(playerAnswer != null && !(playerAnswer == "y" || playerAnswer == "n"));
                    if(playerAnswer == "y") {
                        Program.game.lastOutputString = $" {npcs[2].Name}: {npcs[2].Dialogue[1]}";
                        Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] = false;
                        Quests.Quest? mayorsDutyQuest;
                        if(Quests.TryGetValue("mayorsDuty", out mayorsDutyQuest) && mayorsDutyQuest.SubQuests != null) mayorsDutyQuest.SubQuests[1].CompletionCondition = "completed";
                    } else Program.game.lastOutputString = $"{npcs[2].Name}: {npcs[2].Dialogue[2]}";
                } else if(Program.game.MayorDecisions[Game.MayorDecisionKeys.TravelByCar] != true) Program.game.lastOutputString = $"Bike Vendor: {npcs[2].Dialogue[1]}";
                else Program.game.lastOutputString = $"Monica: {npcs[2].Dialogue[3]}";
            }

            public static void Informant(){
                Quests.Quest? mayorsDutyQuest;
                Quests.TryGetValue("mayorsDuty", out mayorsDutyQuest);
                if(mayorsDutyQuest?.SubQuests != null && (!mayorsDutyQuest.SubQuests[3].Completed || !mayorsDutyQuest.SubQuests[2].Completed))
                {
                    Program.game.lastOutputString = $"{npcs[3].Name}: {npcs[3].Dialogue[0]}";
                    if(!mayorsDutyQuest.SubQuests[2].Completed) mayorsDutyQuest.SubQuests[2].CompletionCondition = "completed";
                }else{
                    Program.game.lastOutputString = $"{npcs[3].Name}: {npcs[3].Dialogue[1].Replace("[playerName]", Game.playerName)}";
                }
            }

            public static void GhettoCitizen(){
                Quests.Quest? mayorsDutyQuest;
                Quests.TryGetValue("mayorsDuty", out mayorsDutyQuest);
                if(mayorsDutyQuest?.SubQuests != null && !mayorsDutyQuest.SubQuests[3].Completed){
                    Program.game.lastOutputString = $"{npcs[4].Name}: {npcs[4].Dialogue[0]} ";
                    mayorsDutyQuest.SubQuests[3].CompletionCondition = "completed";
                }else{
                    Program.game.lastOutputString = $"{npcs[4].Name}: {npcs[4].Dialogue[1]}";
                }
            }
        }

        public class Stage3 {
            public Dictionary<string, bool> StageProgression;
            public static Dictionary<string, Quests.Quest> Quests = new(){
                {"buildInfrastrucutre", new(
                    3.1f,
                    "Build Infrastrucutre",
                    "The citizens would love some new Infrastructure. Talk to your advisor to make a choice on what to build.",
                    "notCompleted",
                    null,
                    WorldOfZuul.Quests.QuestType.stageProgress
                )}

            };
            public static NPC[] npcs = {
                new(
                    "Advisor Nick",
                    "Your trusted Advisor in all matters.",
                    new() {
                        {1, "The Market is usually a good place to learn about the city. Perhaps you could overhear some chatter there, or even talk to people for more details."},
                        {2, "Building infrastructure, huh? Well, it's either going to be in favour of cars, or bikes. Either option will allow residents to get around faster in the respective mode of transport, but keep in mind what cars mean for the environment. Which infrastructure would you like to build? (car/bike) "},
                        {3, "[vehicle] infrastructure it is. Let's hope you made the right choice."}
                    },
                    "Hello there, Mayor",
                    Game.map.Rooms["mayorsOffice"]
                ),
                new(
                    "Informant Arhan",
                    "The informant knows all about what's going on in the city.",
                    new() {
                        {1, "You should talk to people in the market. Talking to and overhearing the citizens talk is the best way to get firsthand information on what your citizens want."},
                        {2, "To build infrastructure, talk to your advisor."},
                        {3, "Having built [vehicle] infrastructure will allow you to get around faster [vehicle2] your [vehicle]."}
                    },
                    "Hello there, Mayor!",
                    Game.map.Rooms["townHall"]

                ),
                new(
                    "Resident Krzysztof",
                    "A local resident of Futurbs, particularly fond of retro computers.",
                    new() {
                        {1, "Greetings, Mayor! Everyone is talking about how difficult it is to get around town recently. Perhaps you could build some new infrastructure for everyone to traverse better."},
                        {2, "Well done, Mayor. With the new bike paths, everyone should be able to get around more swiftly on their bikes. Very climate-friendly!"},
                        {3, "Getting around faster by car is nice, but sadly, I do not own a car... It is even more difficult and unsafe to get around by bike now."}
                    },
                    "Good day, Mayor!",
                    Game.map.Rooms["market"]),
            };
            public Stage3() {
                StageProgression = new(){};
                InitialiseState();
                UpdateGameState();
            }

            public static void InitialiseState() {
                Game.currentQuests = new();
                Game.TrackedQuests = new();
                
                Game.map.Rooms["market"].LongDescription = "The Market is as lively as ever. People are chattering happily about the town, eager to see what changes the new mayor will bring. Infrastructure seems to be especially on everyone's mind today.";

                int questCounter = 1;
                foreach(Quests.Quest _ in Quests.Values) {
                    Game.currentQuests.Add(questCounter, _);
                    ++questCounter;
                }
                UpdateState();
            }

            public static void UpdateState() {
                
            }
            public static void Advisor() {
                if(!Quests["buildInfrastructure"].Completed) {
                    Program.game.lastOutputString = $"{npcs[0].Name}: {npcs[0].Dialogue[1]}";
                }
            }

            public static void Informant() {
                if(!Quests["buildInfrastructure"].Completed) {
                    Program.game.lastOutputString = $"{npcs[1].Name}: {npcs[1].Dialogue[1]}";
                }
            }

            public static void MarketResident() {

            }
        }
        public static void TalkToNPC() {
            switch(GameStage) {
                case 0:
                    if(Game.map.CurrentRoom.ShortDescription == "Mayor's Office") Stage0.Advisor();
                    else Screen.CommandOutputString.Add("There is no NPC in this area to talk to.");
                    break;
                case 1:
                    if(Game.map.CurrentRoom.ShortDescription == "Mayor's Office") Stage1.Advisor();
                    else Screen.CommandOutputString.Add("There is no NPC in this area to talk to.");
                    break;
                case 2:
                    if(Game.map.CurrentRoom.ShortDescription == "Mayor's Office") Stage2.Advisor();
                    else if(Game.map.CurrentRoom.ShortDescription == "City Centre") Stage2.CarVendor();
                    else if(Game.map.CurrentRoom.ShortDescription == "Market") Stage2.BikeVendor();
                    else if(Game.map.CurrentRoom.ShortDescription == "Ghetto") Stage2.GhettoCitizen();
                    else if(Game.map.CurrentRoom.ShortDescription == "Town Hall") Stage2.Informant();
                    else Screen.CommandOutputString.Add("There is no NPC in this area to talk to.");
                    break;
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
                    Stage2.UpdateState();
                    break;
                case 3:
                    Stage3.UpdateState();
                    break;                
            }
        }
    }
}