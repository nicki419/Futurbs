# Buildings.cs
This is the documentation for `WorldOfZuul/Buildings.cs`.

---

### Building
Building is an object that is used only and in every `Room` object.
It Contains a name, a description, and a state.
If null is passed to the constructor as an argument for `state`, it will default to `State.Built`.

---

### State
```csharp
public enum State {
    Built,
    Demolished,
    ToBeBuilt,
    ToBeDemolished,
}
```
State is an enumeration type containing various states a building can have.