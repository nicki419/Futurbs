# Room.cs
This is the documentation for WorldOfZuul/Room.cs

---

### Room {#room}
Room is a public class in Room.cs.

---

### Room Object {#room_object}
```csharp
public Room(string shortDesc, string longDesc)
```
The Room object is initialised with a `ShortDescription` (The room name) and a `LongDescription` (used in-game as a description to the player).

**Example:**

```csharp
Room? outside = new("Outside", "You are standing outside the main entrance of the university. To the east is a large building, to the south is a computing lab, and to the west is the campus pub.");
```

---

### Room.SetExit() {#room_setexit}
```csharp
public void SetExit(string direction, Room? neighbor)
```
Defines a single exit for a room. 

**Example:**
```csharp
theatre.SetExit("west", outside);
```

---

### Room.SetExits() {#room_setexits}
```csharp
public void SetExits(Room? north, Room? east, Room? south, Room? west)
```
Defines multiple exits for a room. Non-existent exits must be initialised with `null`. 
Depends on `Room.SetExit()`.

**Example:**

```csharp
outside.SetExits(null, theatre, lab, pub);
```
