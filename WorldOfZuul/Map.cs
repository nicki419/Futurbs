using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Map
    {
        
        // currentRoom and previous room should be used in game.cs, as they are required by the game object. They are not part of the map.
        // if it's private, only the map class can see it, it is not meant to be interacted with externally. 
        // private Room? currentRoom;
        
        // instead of creating rooms in a funciton, make them public objects of the class.

        public Room? cityCentre = new("City Centre","The city centre of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town");
        public Room? townHall = new("Town Hall","Your are currently in the Town Hall");
        public Room? trainStation = new("Train Station","Your are currently in the Train Station ");
        public Room? park1 = new("Park 1 ","You are currently in Park1");
        public Room? market = new("Market","You are currently in the Market");
        public Room? residentialHouses = new("Residentail Houses","You are currently near the Residentail Houses");
        public Room? residentialArea1 = new("Residential Area 1","You are currently near Residentail Area 1");
        public Room? residentialBlocks = new("Residentail Blocks","You are currently neat the Residential Blocks");
        public Room? park2 = new("Park 2","You are currently in Park2");
        public Room? ghetto = new("Ghetto","You are currently in the Ghetto"); 
        public Room? IndustrialZone = new("Industrial Zone","You are currently in the Industrial Zone");
        public Room? residentialArea2 = new("Residential Area 2","You are currently near Residential Area2");
        public Room? mayorsOffice = new("Mayors Office","You are currently in the Mayors Office");

        // Intead of a function CreateMap(), use the class's initialisor to set the exits.
        public Map() {
          cityCentre?.SetExits(townHall, market, trainStation, park1);

          market?.SetExits(residentialHouses, residentialArea1, null, cityCentre );

          residentialHouses?.SetExit("south", market);
          
          residentialArea1?.SetExit("west", market);

          townHall?.SetExits(mayorsOffice, null, cityCentre, null);

          mayorsOffice?.SetExit("south", townHall);
          
          park1?.SetExits(null, cityCentre, null, residentialBlocks);

          residentialBlocks?.SetExits(IndustrialZone, park1, null, null);

          IndustrialZone?.SetExits(residentialArea2, null, residentialBlocks, null);

          residentialArea2?.SetExit("south", IndustrialZone);

          trainStation?.SetExits(cityCentre, ghetto, park2, null);

          ghetto?.SetExit("west", trainStation);

          park2?.SetExits(trainStation, null, null, null);
        
        }

        // added currentRoom to function requirements, as it is uncommented above. Added ? so it stops bitching about null reference.
        public string MiniMap(Room? currentRoom)
        {
          string returnString = "";
          if(currentRoom?.north != null) returnString += $"North - {currentRoom.north}\n"; else returnString +=$"North - Nothing\n";
          if(currentRoom?.east != null) returnString += $"East - {currentRoom.east}\n";else returnString += $"East - Nothing\n" ;
          if(currentRoom?.south != null) returnString += $"South - {currentRoom.south}\n"; else returnString += $"South - Nothing\n";
          if(currentRoom?.west != null) returnString += $"West - {currentRoom.west}\n"; else returnString += $"West - Nothing\n";
          returnString += "\n";
          return returnString;          
        }   

    


    }
}

