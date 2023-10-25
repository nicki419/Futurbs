# Screen.cs
This is the documentation for `WorldOfZuul/Screen.cs`.

---

### Screen
Screen is a public class in `Screen.cs`.

---


### Room.SetExit() 
```csharp
public void SetExit(string direction, Room? neighbor)
```
Defines a single exit for a room. 

**Example:**
```csharp
theatre.SetExit("west", outside);
```

---

### Screen.PrintScreen()
```csharp
public void PrintScreen(string lastOutputString, bool textInput)
```
Prints the screen from `lastOutputString`, depending on the input mode of the game, set in `textInput`.

**Example:**
```csharp
public class Game {
    private readonly Screen screen = new();
    private bool textInput = true;
    private string? lastOutputString;

    public void Play() {
        Parser parser = new();

        bool continuePlaying = true;
        lastOutputString = $"Welcome to the World of Zuul!\nWorld of Zuul is a new, incredibly boring adventure game.\n\n{currentRoom?.LongDescription}\n";

        screen.PrintScreen(lastOutputString, textInput);
    }
}
```

### Screen.GetNewCommand()
```csharp
public string? GetNewCommand()
```
Acquires a new string to pass to Parser.cs. Used only with input mode Menu Navigation. Returns an empty string if no command has been selected with the `Enter` key yet. 

**Example:**
```csharp
string? input = screen.GetNewCommand();

if (string.IsNullOrEmpty(input))
{
    continue;
}

Command? command = parser.GetCommand(input);
continuePlaying = CommandHandler(command);
```
