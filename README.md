# Unity Client-ServerApp
 
**Reef Networking** - An open source test project for the Unity Engine that contains ready-made mechanisms for:
- Connections to a remote server (TCP).
- Sending and receiving messages from the server.
- Failure handling (loss of connection to the server).

**Server** - a project in Visual Studio, contains mechanisms for:
- Accepting connections.
- Sending messages (to all, one, all except one client).
- Receiving messages from clients.
- Handling failures (loss of connection with the client).

The code uses TcpListener and TcpClient as network communication components.

### How Use
To pass data structures other than those built into the language:
- Use the Packets project (located in Unity-Client-Server-App -> Server -> Packets). 
- In this project, you must declare all custom types that will be passed between client and server. 
- Next, you need to build it into a DLL and place it in the Server and Client project.
