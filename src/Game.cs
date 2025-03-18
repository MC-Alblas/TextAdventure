using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room mainHall = new Room("in the main hall");
		Room secondFloor = new Room("on the second floor");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");

		// Initialise room exits
		outside.AddExit("south", mainHall);

		mainHall.AddExit("west", pub);
		mainHall.AddExit("east", theatre);
		mainHall.AddExit("south", lab);
		mainHall.AddExit("up", secondFloor);
		mainHall.AddExit("north", outside);

		secondFloor.AddExit("down", mainHall);

		theatre.AddExit("west", mainHall);

		pub.AddExit("east", mainHall);

		lab.AddExit("north", mainHall);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		// Create Items
		Item medkit = new Item(10, "a medkit");
		Item blade = new Item(20, "the pristine blade");
		Item snack = new Item(5, "an abandoned and untouched snack");

		// And add them to the Rooms
		secondFloor.AddItem("medkit", medkit);
		theatre.AddItem("pristine blade", blade);
		office.AddItem("snack", snack);

		// Start game outside
		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);

			if (!player.IsAlive())
			{
				Console.WriteLine();
				Console.WriteLine("You died.");
				Console.WriteLine();
				finished = true;
			}
		}

		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				PrintLook();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				take(command);
				break;
			case "drop":
				drop(command);
				break;
			case "use":
				
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	private void PrintStatus()
	{
		int HP = player.GetHealth();
		Console.WriteLine($"You have {HP}/100 health");
		Console.WriteLine($"you have {player.backPack.spaceLeft} out of {player.backPack.maxSpace} space left in your backpack");
		Console.WriteLine("");
		Console.WriteLine("Your inventory:");
		player.backPack.listInventory();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to the " + direction + "!");
			return;
		}

		player.Damage(5);
		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	private void PrintLook()
	{
		Console.WriteLine(player.CurrentRoom.GetShortDescription());
		// Console.WriteLine($"There is {}");
	}

	private void take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;
		Item item = player.CurrentRoom.chest.Get(itemName);

		player.backPack.Put(itemName, item);
		Console.WriteLine($"you take {item.Description}");
		Console.WriteLine($"you have {player.backPack.spaceLeft} out of {player.backPack.maxSpace} space left in your backpack");
	}

	private void drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord;
		Item item = player.backPack.Get(itemName);

		player.backPack.spaceLeft += item.Size;
		player.CurrentRoom.chest.Put(itemName, item);
		Console.WriteLine($"You put {item.Description} in the chest");
		Console.WriteLine($"you have {player.backPack.spaceLeft} out of {player.backPack.maxSpace} space left in your backpack");
	}

	private void use(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}

		string itemName = command.SecondWord;

		
	}
}

