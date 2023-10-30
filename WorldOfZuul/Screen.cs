namespace WorldOfZuul {
    public class Screen {

        private readonly CommandWords commandWords = new();

        public CommandWords.GameCommand activeCommand;

        //public Map map = new();

        public void PrintScreen(string lastOutputString, string? mapString, bool textInput, bool mapToggle) {
            Console.Clear();

            Console.WriteLine(mapString);
            Console.WriteLine(lastOutputString);

            if(textInput == true) {
                Console.Write("> ");
            }
            else {
                if(activeCommand.Name == null) {
                activeCommand = commandWords.commandList[0][0];
                }

                Console.WriteLine(commandWords.GenerateCommandString(activeCommand));
                
            }
        }
        public string? GetNewCommand(){
            CommandWords.GameCommand selectedCommand;
            (activeCommand, selectedCommand) = commandWords.MenuNavigator(activeCommand);
            return selectedCommand.Name;
        }
    }
}