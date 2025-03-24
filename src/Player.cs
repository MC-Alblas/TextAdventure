class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    private int health;
    public Inventory backPack;
    private bool UsedTheBlade;
    private bool CalledAnAmbulance;
    public bool Won { get; set; }

    // constructor
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backPack = new Inventory(50);
        UsedTheBlade = false;
        CalledAnAmbulance = false;
        Won = false;
    }

    public int GetHealth()
    {
        return health;
    }

    public int Damage(int amount)
    {
        health -= amount;
        return health;
    }

    public int Heal(int amount)
    {
        health += amount;
        if (health > 100)
        {
            health = 100;
        }
        return health;
    }

    public bool IsAlive()
    {
        if (!(health <= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UsedBlade()
    {
        return UsedTheBlade;
    }

    public bool CalledAmbulance()
    {
        return CalledAnAmbulance;
    }

    public string UseItem(Command command)
    {
        string itemName = command.SecondWord;
        string target = command.ThirdWord;
        Item item = backPack.GetItem(itemName);
        string result = "";

        if (item == null && itemName != "telephone")
        {
            return $"you don't have a {itemName}.";
        }

        switch (itemName)
        {
            case "medkit":
                result = UseMedkit();
                break;
            case "blade":
                result = UseBlade();
                break;
            case "snack":
                result = UseSnack();
                break;
            case "key":
                result = UseKey(target, item);
                break;
            case "telephone":
                result = UseTelephone();
                break;
        }

        if (itemName != "telephone")
        {
        backPack.Remove(itemName);
        backPack.SpaceLeft += item.Size;
        }
        return result;
    }

    private string UseMedkit()
    {
        Heal(20);
        return "you used the medkit and healed 20 health.";
    }

    private string UseBlade()
    {
        Damage(10);
        UsedTheBlade = true;
        return "you used the pristine blade and ended up hurting yourself for 10 health. Since you clearly don't know how to use it, you decide to drop the blade.";
    }

    private string UseSnack()
    {
        Heal(5);
        return "you ate the snack and healed back 5 health.";
    }

    private string UseKey(string target, Item key)
    {
        if (target == null)
        {
            return "What direction?";
        }

        Room targetRoom = CurrentRoom.GetExit(target);

        if(targetRoom.IsLocked() == false)
        {
            return "This room is already unlocked";
        }

        if(targetRoom != key.FittingLock)
        {
            return "This key doesn't fit this lock";
        }

        targetRoom.SetLocked(false);
        return "you used the key on the door. It's now unlocked.";
    }

    private string UseTelephone()
    {
        Item telephone = CurrentRoom.chest.GetItem("telephone");
        if(telephone == null)
        {
            return "There is no telephone in this room.";
        }
        else
        {
            CalledAnAmbulance = true;
            return "You dial 112 to call an ambulance. You should make your way outside to be taken in.";
        }
    }
}