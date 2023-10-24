using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Map
    {
        
        private Room? currentRoom;
        
        
        public void CreateMap()
        {
        Room? cityCentre = new("City Centre","The city center of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town");
        Room? townHall = new("Town Hall","");
        Room? trainStation = new("Train Station","");
        Room? park1 = new("Park 1 ","");
        Room? market = new("Market","");
        Room? residentialHouses = new("Residentail Houses","");
        Room? residentialArea1 = new("Residential Area 1","");
        Room? residentialBlocks = new("Residentail Blocks","");
        Room? university = new("University",""); 
        Room? park2 = new("Park 2","");
        Room? ghetto = new("Ghetto",""); 
        Room? IndustrialZone = new("Industrial Zone","");
        Room? residentialArea2 = new("Residential Area 2","");
        Room? mayorsOffice = new("Mayors Office","");


        cityCentre.SetExits(townHall, market, trainStation, park1);

        market.SetExits(residentialHouses, residentialArea1, null, cityCentre );

        residentialHouses.SetExit("south", market);
        
        residentialArea1.SetExit("west", market);

        townHall.SetExits(mayorsOffice, null, cityCentre, null);

        mayorsOffice.SetExit("south", townHall);
        
        park1.SetExits(null, cityCentre, university, residentialBlocks);

        residentialBlocks.SetExits(IndustrialZone, park1, null, null);

        IndustrialZone.SetExits(residentialArea2, null, residentialBlocks, null);

        residentialArea2.SetExit("south", IndustrialZone);

        trainStation.SetExits(cityCentre, ghetto, park2, null);

        ghetto.SetExit("west", trainStation);

        park2.SetExits(trainStation, null, null, university);

        university.SetExits(park1, park2, null, null);

        currentRoom = cityCentre;



        
        }

        public void MiniMap()
        {
          if(currentRoom?.north != null) Console.WriteLine($"North - {currentRoom.north}");
          if(currentRoom?.east != null) Console.WriteLine($"East - {currentRoom.east}");
          if(currentRoom?.south != null) Console.WriteLine($"South - {currentRoom.south}");
          if(currentRoom?.west != null) Console.WriteLine($"West - {currentRoom.west}");
          
          
        }   

    


    }
}
