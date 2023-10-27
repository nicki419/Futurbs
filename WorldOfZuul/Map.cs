using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Map
    {
        
        public Room CurrentRoom;

        public Room cityCentre = new("City Centre","The city centre of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town");
        public Room townHall = new("Town Hall","The town hall, a place where you go daily to set up Futubs for a brighter future.");
        public Room trainStation = new("Train Station","The train station of Futurbs where hundreds of people arrive back home, or leave to seek a better future.");
        public Room park1 = new("Park 1 ","The main park of Futurbs, althoug an old one it still has it's beuty in all seasons of the year.");
        public Room market = new("Market","The market is as lively as ever with the comotion of pop-up stalls or passers through.");
        public Room residentialHouses = new("Residentail Houses","The residentail houses of Futurbs are a range between new and modern architecture and old rustic houses that have been there for decades.");
        public Room residentialArea1 = new("Residential Area 1","A residentail area near the houses with greenery and playgrounds for the local kids to enjoy.");
        public Room residentialBlocks = new("Residentail Blocks","The old school comi-blocks are a sore sight to see. Although they fill the purpose of chap housing something must be done to better the quality of living.");
        public Room park2 = new("Park 2","The newer park in Futurbs is quite nice although many people ar debating between the charm and history of the onld park and the nwely built modern Park.");
        public Room ghetto = new("Ghetto","The Ghetto is the last place you would want to end up in, the buildng are a ramshackle of materials put together just to have a roof, and the living conditions are as bad as it can come."); 
        public Room IndustrialZone = new("Industrial Zone","The industrial zone of Futurbs is filled with wearhouses, factorys, and manufactoring buildings that don't bode that well to the enviroment and wildlife of the area and beyond.");
        public Room residentialArea2 = new("Residential Area 2","A older residential, not in the date that it was built or the age of the building, but oldern in the demografic of the people that inhabit this area.");
        public Room mayorsOffice = new("Mayors Office","The Mayors Office, a place that you can whole heartedly call you second home, tis is where you make all the decidionds that impact the area and the denisens of Futurbs.");

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

