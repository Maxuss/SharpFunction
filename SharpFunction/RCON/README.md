# What is RCON?

[RCON](https://wiki.vg/RCON) is a protocol that allows execution of server commands by sending packets to server.
You can enable rcon by adding this to your server settings:

```properties
rcon.port=25575
rcon.password=<SECURE PASSWORD HERE>
enable-rcon=true
```

# What can I do with RCON in SharpFunction?

Well, you can execute commands straight from application!

It means, that to test something, you will no longer need to make command blocks, 
or function datapacks, just to execute several commands, and instead, run them straight here!

Do note, however, that RCON requires you to have a server working at a time!

# How can I use RCON with SharpFunction?

You can create a new RCON Client like that:

```c#
// port, password, ip
RconClient client = new RconClient(25575, "<SECURE PASSWORD HERE>", "127.0.0.1");
```

After that you can execute commands, or send packets with commands straight to server by using it's PacketManager field:

```c#
var manager = client.PacketManager;

// Sending packet yourself
RconPacket response;
string data = "say Seals are very cool!";
bool success = manager.SendPacket(new RconPacket(data.Length, new Random().Next(), PacketType.Command, data), out response);
if(!success) // Could not receive proper packet return data! Handle this exception
Console.WriteLine(response.Contents); // [@] Seals are very cool!

// Executing single command
string output = manager.ExecuteCommand(data);
Console.WriteLine(output); // [@] Seals are very cool!

// Executing whole command module
CommandModule commands = // Get it as you like
List<string> outputs = manager.ExecuteCommands(commands).ToList(); 
```

Do note, that this RCON implementation is synchronised!