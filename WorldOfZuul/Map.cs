using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Map
    {
        
        public Room CurrentRoom;

          //Creates a Dictionary of Rooms. Each room has a name a short description a long description and a boolean that shows the state of the room (null defaults to Built).

          public Dictionary<string, Room> Rooms  = new() {
            {"cityCentre", new("City Centre","The city centre of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town", null)},
            {"townHall", new("Town Hall","The town hall, a place where you go daily to set up Futubs for a brighter future.", null)},
            {"trainStation", new("Train Station","The train station of Futurbs where hundreds of people arrive back home, or leave to seek a better future.", null)},
            {"park1", new("Park 1 ","The main park of Futurbs, althoug an old one it still has it's beuty in all seasons of the year.", null)},
            {"market", new("Market","The market is as lively as ever with the comotion of pop-up stalls or passers through.", null)},
            {"residentialHouses", new("Residentail Houses","The residentail houses of Futurbs are a range between new and modern architecture and old rustic houses that have been there for decades.", null)},
            {"recreationalArea1", new("Recreational Area 1","A residentail area near the houses with greenery and playgrounds for the local kids to enjoy.", null)},
            {"residentialBlocks", new("Residentail Blocks","The old school comi-blocks are a sore sight to see. Although they fill the purpose of chap housing something must be done to better the quality of living.", null)},
            {"park2", new("Park 2","The newer park in Futurbs is quite nice although many people ar debating between the charm and history of the onld park and the nwely built modern Park.", null)},
            {"ghetto", new("Ghetto","The Ghetto is the last place you would want to end up in, the buildng are a ramshackle of materials put together just to have a roof, and the living conditions are as bad as it can come.", null)},
            {"IndustrialZone", new("Industrial Zone","The industrial zone of Futurbs is filled with wearhouses, factorys, and manufactoring buildings that don't bode that well to the enviroment and wildlife of the area and beyond.", null)},
            {"recreationalArea2", new("Recreational Area 2","A hub of all sports. Skateboarding, rollerblading, parkour and much more to be found at the newly installed recreational area.", null)},
            {"mayorsOffice", new("Mayors Office","The Mayors Office, a place that you can whole heartedly call you second home, tis is where you make all the decidionds that impact the area and the denisens of Futurbs.", null)},
        };
        // Instead of a function CreateMap(), use the class's initialisor to set the exits.
        public Map() {
          Rooms["cityCentre"].SetExits(Rooms["townHall"], Rooms["market"], Rooms["trainStation"], Rooms["park1"]);
          Rooms["market"].SetExits(Rooms["residentialHouses"], Rooms["recreationalArea1"], null, Rooms["cityCentre"] );
          Rooms["residentialHouses"].SetExit("south", Rooms["market"]);
          Rooms["recreationalArea1"].SetExit("west", Rooms["market"]);
          Rooms["townHall"].SetExits(Rooms["mayorsOffice"], null, Rooms["cityCentre"], null);
          Rooms["mayorsOffice"].SetExit("south", Rooms["townHall"]);
          Rooms["park1"].SetExits(null, Rooms["cityCentre"], null, Rooms["residentialBlocks"]);
          Rooms["residentialBlocks"].SetExits(Rooms["IndustrialZone"], Rooms["park1"], null, null);
          Rooms["IndustrialZone"].SetExits(Rooms["recreationalArea2"], null, Rooms["residentialBlocks"], null);
          Rooms["recreationalArea2"].SetExit("south", Rooms["IndustrialZone"]);
          Rooms["trainStation"].SetExits(Rooms["cityCentre"], Rooms["ghetto"], Rooms["park2"], null);
          Rooms["ghetto"].SetExit("west", Rooms["trainStation"]);
          Rooms["park2"].SetExits(Rooms["trainStation"], null, null, null);

          CurrentRoom = Rooms["cityCentre"];
        
        }

        // added currentRoom to function requirements, as it is uncommented above. Added ? so it stops bitching about null reference.
        public string MiniMap()
        {
          string returnString = ""; 
          returnString += $" -= {CurrentRoom.ShortDescription} =-\n";
          if(CurrentRoom.Exits.ContainsKey("north")) returnString += $"North - {CurrentRoom.Exits["north"].ShortDescription}\n"; else returnString +=$"North - \n";
          if(CurrentRoom.Exits.ContainsKey("east")) returnString += $"East  - {CurrentRoom.Exits["east"].ShortDescription}\n";else returnString += $"East  - \n" ;
          if(CurrentRoom.Exits.ContainsKey("south")) returnString += $"South - {CurrentRoom.Exits["south"].ShortDescription}\n"; else returnString += $"South - \n";
          if(CurrentRoom.Exits.ContainsKey("west")) returnString += $"West  - {CurrentRoom.Exits["west"].ShortDescription}\n"; else returnString += $"West  - \n";
          returnString += "\n";
          return returnString;          
        }   

      // Function to create a new building. Also creates a room.
      public string CreateBuilding(string name, string description, (Room?, Room?, Room?, Room?) exits) {
        // Several checks: checks that the the exit is not null, the room that is set at the exit contains an exit to the opposite direction and if that is null.
        // If so, return a string stating that a building cannot be build there because another building is already in that spot.
        if(exits.Item1 != null && exits.Item1.Exits.ContainsKey("south") && exits.Item1.Exits["south"] != null) return $"You can't build here. {exits.Item1.Exits["south"].ShortDescription} is already in this location.";
        else if(exits.Item2 != null && exits.Item2.Exits.ContainsKey("west") && exits.Item2.Exits["west"] != null) return $"You can't build here. {exits.Item2.Exits["west"].ShortDescription} is already in this location.";
        else if(exits.Item3 != null && exits.Item3.Exits.ContainsKey("north") && exits.Item3.Exits["north"] != null) return $"You can't build here. {exits.Item3.Exits["north"].ShortDescription} is already in this location.";
        else if(exits.Item4 != null && exits.Item4.Exits.ContainsKey("east") && exits.Item4.Exits["east"] != null) return $"You can't build here. {exits.Item4.Exits["east"].ShortDescription} is already in this location.";
        else if(exits.Item1 == null && exits.Item2 == null && exits.Item3 == null && exits.Item4 == null) return "You need to connect your new building to at least one existing one.";
        else {
          // checks if the name given to the building exists, if so, return a string stating that.
          if(Rooms.ContainsKey(name)) return "That room already exists. Select a different name.";

          // Create a new room object in the Rooms dict, and set the exits
          Rooms.Add(name, new(name, description, Building.State.ToBeBuilt));
          Rooms[name].SetExits(exits.Item1, exits.Item2, exits.Item3, exits.Item4);
          if(exits.Item1 != null) exits.Item1.SetExit("south", Rooms[name]);
          if(exits.Item2 != null) exits.Item2.SetExit("west", Rooms[name]);
          if(exits.Item3 != null) exits.Item3.SetExit("north", Rooms[name]);
          if(exits.Item4 != null) exits.Item4.SetExit("east", Rooms[name]);

          return $"{name} is scheduled to be built the next day!";

        }

      }


    }
}

