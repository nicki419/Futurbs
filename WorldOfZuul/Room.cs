namespace WorldOfZuul
{
    public class Room
    {
        public Building RoomBuilding;
        public string ShortDescription { get; set; }
        public string LongDescription { get; set;}

        public string? north {get; private set;}
        public string? east {get; private set;}
        public string? south {get; private set;}
        public string? west {get; private set;}        
        
        public Dictionary<string, Room> Exits { get; private set; } = new();

        public Room(string shortDesc, string longDesc, Building.State? state)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            RoomBuilding = new(shortDesc, longDesc);
            if(state == null) RoomBuilding.BuildingState = Building.State.Built;
            else RoomBuilding.BuildingState = state;
        }

        public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("north", north);
            SetExit("east", east);
            SetExit("south", south);
            SetExit("west", west);
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }
    }
}
