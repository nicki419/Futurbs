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