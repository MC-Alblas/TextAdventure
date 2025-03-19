class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    private int health;
    public Inventory backPack;

    // constructor
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backPack = new Inventory(50);
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

    public string UseItem(Command command)
    {
        string itemName = command.SecondWord;
        string target = command.ThirdWord;
        Item item = backPack.Get(itemName);
        string desc = item.Description;
        string result = "";

        if (item == null)
        {
            return $"you don't have {itemName}.";
        }

        switch (itemName)
        {
            case "medkit":
                result = UseMedkit();
                break;
            case "pristine blade":
                result = UseBlade();
                break;
            case "snack":
                result = UseSnack();
                break;
            case "key":
                result = UseKey(target, item);
                break;
        }

        backPack.spaceLeft += item.Size;
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
        return "you used the blade and ended up hurting yourself for 10 health. You decide to drop the blade.";
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
        return "you used to key on the door. It's now unlocked.";
    }
}