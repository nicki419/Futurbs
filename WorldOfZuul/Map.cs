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
        public Room? townHall = new("Town Hall","");
        public Room? trainStation = new("Train Station","");
        public Room? park1 = new("Park 1 ","");
        public Room? market = new("Market","");
        public Room? residentialHouses = new("Residentail Houses","");
        public Room? residentialArea1 = new("Residential Area 1","");
        public Room? residentialBlocks = new("Residentail Blocks","");
        public Room? university = new("University",""); 
        public Room? park2 = new("Park 2","");
        public Room? ghetto = new("Ghetto",""); 
        public Room? IndustrialZone = new("Industrial Zone","");
        public Room? residentialArea2 = new("Residential Area 2","");
        public Room? mayorsOffice = new("Mayors Office","");

        // Intead of a function CreateMap(), use the class's initialisor to set the exits.
        public Map() {
          cityCentre?.SetExits(townHall, market, trainStation, park1);

          market?.SetExits(residentialHouses, residentialArea1, null, cityCentre );

          residentialHouses?.SetExit("south", market);
          
          residentialArea1?.SetExit("west", market);

          townHall?.SetExits(mayorsOffice, null, cityCentre, null);

          mayorsOffice?.SetExit("south", townHall);
          
          park1?.SetExits(null, cityCentre, university, residentialBlocks);

          residentialBlocks?.SetExits(IndustrialZone, park1, null, null);

          IndustrialZone?.SetExits(residentialArea2, null, residentialBlocks, null);

          residentialArea2?.SetExit("south", IndustrialZone);

          trainStation?.SetExits(cityCentre, ghetto, park2, null);

          ghetto?.SetExit("west", trainStation);

          park2?.SetExits(trainStation, null, null, university);

          university?.SetExits(park1, park2, null, null);
        
        }

        // added currentRoom to function requirements, as it is uncommented above. Added ? so it stops bitching about null reference.
        public void MiniMap(Room? currentRoom)
        {
          if(currentRoom?.north != null) Console.WriteLine($"North - {currentRoom.north}");
          if(currentRoom?.east != null) Console.WriteLine($"East - {currentRoom.east}");
          if(currentRoom?.south != null) Console.WriteLine($"South - {currentRoom.south}");
          if(currentRoom?.west != null) Console.WriteLine($"West - {currentRoom.west}");
          
          
        }   

    


    }
}

