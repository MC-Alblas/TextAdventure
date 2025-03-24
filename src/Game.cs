using System.Security.Cryptography.X509Certificates;

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

		office.SetLocked(true);

		// Create Items
		Item medkit = new Item(10, "a medkit", true);
		Item blade = new Item(20, "the pristine blade", true);
		Item snack = new Item(5, "an abandoned and untouched snack", true);
		Item officeKey = new Item(10, "the office key", true, office);
		Item telephone = new Item(0, "a telephone", false);

		// And add them to the Rooms
		secondFloor.AddItem("medkit", medkit);
		theatre.AddItem("blade", blade);
		office.AddItem("snack", snack);
		pub.AddItem("key", officeKey);
		office.AddItem("telephone", telephone);

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

			if (player.Won == true)
			{
				PrintWon();
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

	private void PrintWon()
	{
		Console.WriteLine();
		Console.WriteLine("The ambulance you called is waiting for you.");
		Console.WriteLine("You are helped inside and driven to the hospital where your wounds are treated.");
		Console.WriteLine();
		Console.WriteLine("You beat the game!");
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
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
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
		Console.WriteLine($"you have {player.backPack.SpaceLeft} out of {player.backPack.MaxSpace} space left in your backpack");
		Console.WriteLine("");
		Console.WriteLine("Your inventory:");
		Console.WriteLine(player.backPack.ListInventory());
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
		Room nextRoom = player.CurrentRoom.GetExit(direction);

		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to the " + direction + "!");
			return;
		}

		if (nextRoom.IsLocked())
		{
			Console.WriteLine("This room appears to be locked.");
			return;
		}

		if (player.UsedBlade())
		{
			player.Damage(10);
		}
		else
		{
			player.Damage(5);
		}

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());

		if (player.CalledAmbulance() && nextRoom.GetShortDescription() == "outside the main entrance of the university")
		{
			player.Won = true;
		}
	}

	private void PrintLook()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine("");
		Inventory chest = player.CurrentRoom.chest;
		if (chest.IsEmpty())
		{
			Console.WriteLine("There are no items in this room.");
		}
		else
		{
			if (chest.IsMultiple())
			{
				Console.WriteLine("You see multiple items laying around this room:");
				chest.ListInventory();
			}
			else
			{
				Console.WriteLine("You see an item in this room:");
				chest.ListInventory();
			}
		}
	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;
		Item item = player.CurrentRoom.chest.GetItem(itemName);

		if (!item.CanBePickedUp)
		{
			Console.WriteLine("That cannot be picked up");
			return;
		}

		player.CurrentRoom.chest.Remove(itemName);
		player.backPack.Put(itemName, item);
		Console.WriteLine($"you take {item.Description}");
		Console.WriteLine($"you have {player.backPack.SpaceLeft} out of {player.backPack.MaxSpace} space left in your backpack");
	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord;
		Item item = player.backPack.GetItem(itemName);
		player.backPack.Remove(itemName);

		player.backPack.SpaceLeft += item.Size;
		player.CurrentRoom.chest.Put(itemName, item);
		Console.WriteLine($"You put {item.Description} down");
		Console.WriteLine($"you have {player.backPack.SpaceLeft} out of {player.backPack.MaxSpace} space left in your backpack");
	}

	private void Use(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}

		Console.WriteLine(player.UseItem(command));
	}
}

