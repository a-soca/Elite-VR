# Server Client Network
## TCP vs UDP
| Parameter      | TCP                                                        | UDP                               |
| -------------- | ---------------------------------------------------------- | --------------------------------- |
| Service Type   | Connection-oriented                                        | Connectionless                    |
| Reliability    | Guarantees data is received                                | Cannot guarantee data is received |
| Error checking | Robust error checking to prevent incorrect data being read | Basic error checking (Checksums)  |
| Sequencing     | Packets arrive in order                                    | No sequencing                     |
| Retransmission | Can retransmit lost or incorrect packets                   | No retransmission                 |
| Speed          | Lower due to high processing overhead                      | Fast and efficient                |
| Header size    | Variable, 20-60 bytes                                      | Fixed, 8 byte                     |
| Broadcasting   | Not supported                                              | Supported                         |
| Overhead       | Higher                                                     | Lower                             |

### Conclusion
- The server and client will be implemented with `UDP`
- Due to the high frequency, real time nature of the data transmission required, `UDP` would be an excellent choice
- Packet loss or corruption is not a major issue as the next update will occur quickly and replace any incorrect data
- Ensuring the protocol is performant enough to keep up with the rate of transmission is important to ensure smooth movement
- Efficiency, to keep the emulator running at full speed, is another aspect `UDP` excels at
- `UDP` also opens the possibility of multiple connections via broadcast, allowing possible future implementations of multiplayer gameplay

## Approaches for transmission
### Sending the raw position data
- The original game simply updates positions frequently to give the impression of movement
- This is a simple technique making it easier to implement, however it may lead to motion sickness in VR as it is possible ships may stutter around
- Packet loss would result in positions freezing, increasing the stuttering effect
- No sanity checks would be present if a corrupted position is read, possibly making ships momentarily teleport which would be a jarring and disorienting error
- This method would require minimal postprocessing, reducing overhead

### Sending the vector movement and position data
- Sending the magnitude and direction of movement alongside an origin would allow interpolation to occur, tying the movement rate to the frame rate of the client rather than the emulator
- This would be a more complex and computationally intensive method but would be smoother
- It would also allow for detection of blatantly incorrect data reads and provide a fallback until the next valid packet is recieved
- This method is widely used in the game industry in multiplayer online games to correct for the varying connection stability of clients

### Conclusion
- Initially, the simple position implementation will be completed and play tested to see if the transmission rate and errors are issues
- This will provide at a minimum some playability and may be of a high enough standard to not require further work
- If determined to require optimisation, the interpolation method will be implemented as an extension to the project

## Server
### Requirements
- The server should be integrated into BeebEm
- Written in `C++` to allow easy linkage to the other BeebEm source code
- Uses the `Enet` API to create a `UDP` connection to the client as this framework is well supported and lightweight
- Transmits the position information stored in the 6502 memory locations identified [here](/Documentation/Research/Source-Code-Analysis.md/#example-local-bubble-data-block)
- Raw memory data should be preprocessed to be instantly readable by the client, handling most of the compute on server side
- Use a custom messaging protocol built on top of `UDP` to ensure cross language compatibility as the client will be written in `C#`

## Client
### Requirements
- The client should be integrated into the Unity project
- Written in `C#` to avoid unnecessary linkage issues in Unity
- Uses the built in Unity `UDPClient` methods
- Uses the `Enet` API to match the Server implementation
- Should read a constant stream of positions and flags provided by the server and pass these positions to objects in the project (or vectors if updated to use the interpolation method)


## ENET Library for C# / C++
- [Repository Link (C#)](https://github.com/nxrighthere/ENet-CSharp)
- [Enet Native Link (C++)](http://enet.bespin.org/Downloads.html)

### IMPORTANT NOTES
You must link the following libraries to get ENet source to compile: 
- ws2_32.h (WinSock2)
- winmm.lib (Windows multimedia via pragma comment)