namespace WorldOfZuul {
    public class Building {
        public string Name;
        public enum State {
            Built,
            Demolished,
            ToBeBuilt,
            ToBeDemolished,
        }
        public State? BuildingState;
        public string Description;

        public Building(string name, string description) {
            Name = name;
            Description = description;
        }

    }
}