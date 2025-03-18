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
        if(health > 100)
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

    public string UseItem(string itemName)
    {
        Item item = backPack.Get(itemName);
        string desc = item.Description;
        string result = "";

        switch(desc)
        {
            case "a medkit":
                UseMedkit();
                result = "you used the medkit and healed 20 health.";
                break;
            case "the pristine blade":
                UseBlade();
                result = "you used the blade and ended up hurting yourself for 10 health. You decide to drop the blade.";
                break;
            case "an abandoned and untouched snack":
                UseSnack();
                result = "you ate the snack and healed back 5 health.";
                break;
        }

        backPack.spaceLeft += item.Size;
        return result;
    }

    private void UseMedkit()
    {
        Heal(20);
    }

    private void UseBlade()
    {
        Damage(10);
    }

    private void UseSnack()
    {
        Heal(5);
    }
}