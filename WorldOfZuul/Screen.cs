namespace WorldOfZuul {
    public class Screen {

        private readonly CommandWords commandWords = new();

        public CommandWords.GameCommand activeCommand;

        public void PrintScreen(string lastOutputString, bool textInput) {
            Console.Clear();
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