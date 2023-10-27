# Map.cs
This is the documentation of `World of Zuul/Map.cs`.

---

### Map
Map is a public class in `Map.cs`.

---

### Rooms
Room objects are declared in `Map.cs` that are in the map.

**Example:**
```csharp
public Room cityCentre = new("City Centre","The city centre of Futurbs, the hustle and bustle of the town is loud and cheerful as if its the first day that you are visiting the town");
```
---

### Map.cs initializer
The initializer sets the exits for each room with the `SetExit` method that is located in `Room.cs`.

**Example:**
```csharp
cityCentre.SetExits(townHall, market, trainStation, park1);
```

---

### Map.MiniMap()

Checks if a room has an exit to any of the directions using `CurrentRoom.SetExits` and prints the direction and the name of the Room in that direction

**Example:**

```csharp
string returnString = ""; 
          if(CurrentRoom.Exits.ContainsKey("north")) returnString += $"North - {CurrentRoom.Exits["north"].ShortDescription}\n"; else returnString +=$"North - \n";
```

