using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Map
    {
        
        public Room CurrentRoom;

        public Room cityCentre = new("City Centre","The city centre of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town");
        public Room townHall = new("Town Hall","Your are currently in the Town Hall");
        public Room trainStation = new("Train Station","Your are currently in the Train Station ");
        public Room park1 = new("Park 1 ","You are currently in Park1");
        public Room market = new("Market","You are currently in the Market");
        public Room residentialHouses = new("Residentail Houses","You are currently near the Residentail Houses");
        public Room residentialArea1 = new("Residential Area 1","You are currently near Residentail Area 1");
        public Room residentialBlocks = new("Residentail Blocks","You are currently neat the Residential Blocks");
        public Room park2 = new("Park 2","You are currently in Park2");
        public Room ghetto = new("Ghetto","You are currently in the Ghetto"); 
        public Room IndustrialZone = new("Industrial Zone","You are currently in the Industrial Zone");
        public Room residentialArea2 = new("Residential Area 2","You are currently near Residential Area2");
        public Room mayorsOffice = new("Mayors Office","You are currently in the Mayors Office");

        // Intead of a function CreateMap(), use the class's initialisor to set the exits.
        public Map() {
          cityCentre.SetExits(townHall, market, trainStation, park1);

          market.SetExits(residentialHouses, residentialArea1, null, cityCentre );

          residentialHouses.SetExit("south", market);
          
          residentialArea1.SetExit("west", market);

          townHall.SetExits(mayorsOffice, null, cityCentre, null);

          mayorsOffice.SetExit("south", townHall);
          
          park1.SetExits(null, cityCentre, null, residentialBlocks);

          residentialBlocks.SetExits(IndustrialZone, park1, null, null);

          IndustrialZone.SetExits(residentialArea2, null, residentialBlocks, null);

          residentialArea2.SetExit("south", IndustrialZone);

          trainStation.SetExits(cityCentre, ghetto, park2, null);

          ghetto.SetExit("west", trainStation);

          park2.SetExits(trainStation, null, null, null);
          CurrentRoom = cityCentre;
        
        }

        // added currentRoom to function requirements, as it is uncommented above. Added ? so it stops bitching about null reference.
        public string MiniMap()
        {
          string returnString = ""; 
          if(CurrentRoom.Exits.ContainsKey("north")) returnString += $"North - {CurrentRoom.Exits["north"].ShortDescription}\n"; else returnString +=$"North - \n";
          if(CurrentRoom.Exits.ContainsKey("east")) returnString += $"East  - {CurrentRoom.Exits["east"].ShortDescription}\n";else returnString += $"East  - \n" ;
          if(CurrentRoom.Exits.ContainsKey("south")) returnString += $"South - {CurrentRoom.Exits["south"].ShortDescription}\n"; else returnString += $"South - \n";
          if(CurrentRoom.Exits.ContainsKey("west")) returnString += $"West  - {CurrentRoom.Exits["west"].ShortDescription}\n"; else returnString += $"West  - \n";
          returnString += "\n";
          return returnString;          
        }   

    


    }
}

