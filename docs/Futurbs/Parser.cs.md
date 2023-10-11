# Parser.cs
This is the documentation for `WorldOfZuul/Parser.cs`.

---

### Parser
Parser is a Public class in `Parser.cs`.
It parses user input into `Command` objects.

Parser is an object:
```csharp
Parser parser = new()
```

---

### GetCommand()

```csharp
public Command? GetCommand(string inputLine)
```
Parses an input string and returns a `Command` object, if applicable. Returns `null` otherwise.

**Example:**
```csharp
Command? command = parser.GetCommand("take apple");
```
