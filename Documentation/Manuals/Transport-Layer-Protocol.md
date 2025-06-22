# Transport Layer Protocol
## Server Side Incoming Communication
| Channel | Function                          |
| ------- | --------------------------------- |
| 0       | Server Commands                   |
| 1       | User Input - Key Presses          |
| 2       | User Input - Key Holds            |
| 3       | User Input - Key Releases         |
| 4       | User Input - Arrow Key Presses    |
| 5       | User Input - Arrow Key Holds      |
| 6       | User Input - Arrow Key Releases   |
| 7       | User Input - Function Key Presses |
| 8       | User Input - Return Key Presses   |
| 9       | Analogue Stick Input - X Axis     |
| 10      | Analogue Stick Input - Y Axis     |

### Server Commands
| Command            | Function                                                   |
| ------------------ | ---------------------------------------------------------- |
| `Stream Ship Data` | Starts sending the translated data for ship data blocks    |
| `Load Elite`       | Loads Elite in the emulator                                |
| `Shut Down`        | Closes the server gracefully and ends the emulator process |

## Server Side Outgoing Communication
| Channel | Function                  |
| ------- | ------------------------- |
| 0       | Miscellaneous Logs        |
| 2       | Ship Block Data           |
| 3       | Player Ship Data          |

### Ship Block Data Format
`Format: [A]|[B]|[C]|[D]|[E]|[X],[Y],[Z]|[NX],[NY],[NZ]|[RX],[RY],[RZ]|[SX],[SY],[SZ]`
```
[A] = Ship ID
[B] = Visible on scanner (1/0)
[C] = Exploding (1/0)
[D] = Firing Lasers (1/0)
[E] = Ship Killed (1/0)

[X] = X coordinate
[Y] = Y coordinate
[Z] = Z coordinate

[NX] = Nose X Vector
[NY] = Nose Y Vector
[NZ] = Nose Z Vector

[RX] = Roof X Vector
[RY] = Roof Y Vector
[RZ] = Roof Z Vector

[SX] = Side X Vector
[SY] = Side Y Vector
[SZ] = Side Z Vector
```

- Example: 2|1|0|0|0|-100,20,400|-20,2,6|14,20,-50|90,45,-10` = Ship with ID 2 is visible on scanner, not exploding, firing lasers or killed and at position (-100, 20, 400) with nose vector (-20, 2, 6), roof vector (14, 20, -50) and side vector (90, 45, -10)

- Each ship block is delimited with a \n char, therefore by splitting the string by newline we can retrieve the ship slot by the index in the split array

### Player Ship Movement Data Format
`Format: [S]|[P]|[R]|[FS]|[AS]|[E]|[CT]|[LT]|[AL]|[FV]|[DC]|[DK]|[HS]|[NM]|[LS]|[LT]|[EA]|[MS]`
```
[S] = Speed
[P] = Pitch
[R] = Roll
[FS] = Front Shield
[AS] = Aft Shield
[E] = Energy
[CT] = Cabin Temperature
[LT] = Laser Temperature
[AL] = Altitude
[FV] = Fuel Volume
[DC] = Docking Computer Status
[DK] = Docked Status
[HS] = Hyperspace Timer
[NM] = Number of Missiles Fitted
[LS] = Lock Searching for Target
[LT] = Lock Target
[EA] = ECM Active in Area
[MS] = Menu Screen
```