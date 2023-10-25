# CommandWords.cs
This is the documentation for `WorldOfZuul/CommandWords.cs`.

---

### ValidCommands
ValidCommands is a list of all valid commands:
```csharp
{ "north", "east", "south", "west", "look", "back", "quit" }
```

---

### IsValidCommand()

```csharp
public bool IsValidCommand(string command)
```
Function checking on whether a command is valid. Returns bool.

**Example:**
```csharp

if (words.Length == 0 || !commandWords.IsValidCommand(words[0])) {
        return null;
    }
else {
return new Command(words[0]);
}
```
---

### New Additions from the InteractiveCommandMenu branch


### CommandCategory
```csharp
public enum CommandCategory
```

CommandCategory is an enumeration storing different command categories that command objects can have. Used only when printing commands categorised in Menu Input mode.

---

### GameCommand 
```csharp
public struct GameCommand
```
A structure defining a GameCommand object. Stores name, helptext, a category, and a hotkey (to be implemented yet).

**Example:**
See commandList.

---

### commandList
```csharp
public List<List<GameCommand>> commandList { get; }
```
is a list of lists of GameObject types storing all commands in a two-dimensional way in order to easily categorise them and print them in Menu Input mode. 

**Current commandList:**
```csharp
public List<List<GameCommand>> commandList { get; } = new List<List<GameCommand>> { 
    /* Movement */ new List<GameCommand>() {
        new("north", "Move North", CommandCategory.Movement, 'w'),
        new("east", "Move East", CommandCategory.Movement, 'd'),
        new("south", "Move South", CommandCategory.Movement, 's'),
        new("west", "Move West", CommandCategory.Movement, 'a'),
        new("back", "Head to the previous room", CommandCategory.Movement, 'b')
    },

    /* Actions */ new List<GameCommand>() {
        new("look", "Investigate your sorroundings", CommandCategory.Actions, 'l'),
        new("help", "Print Help", CommandCategory.Actions, 'h'),
    },

    /* Miscellaneous */ new List<GameCommand>() {
        new("quit", "Exit the game", CommandCategory.Miscellaneous, null),
        new("togglein", "Toggle between command input modes", CommandCategory.Miscellaneous, null),
    }
};
```

---

### GenerateCommandString
```csharp
public string GenerateCommandString(GameCommand activeCommand)
```
Generates and returns a string of the commandmenu to be used in Screen.PrintScreen().

**Example:**
```csharp
Console.WriteLine(commandWords.GenerateCommandString(activeCommand));
```

---

### MenuNavigator
```csharp
public (GameCommand, GameCommand) MenuNavigator(GameCommand activeCommand)
```
Function reading input in Menu Input mode and returning two GameCommand objects - one for the newly/currently highlighted object and one for the selected object. If input is not `Enter`, the selected object will be empty.

**Example:**
```csharp
public string? GetNewCommand(){
    CommandWords.GameCommand selectedCommand;
    (activeCommand, selectedCommand) = commandWords.MenuNavigator(activeCommand);
    return selectedCommand.Name;
}
```