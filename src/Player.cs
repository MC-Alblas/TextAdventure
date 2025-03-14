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

    public int GetHealth() {
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
        return health;
    }

    public bool IsAlive() {
        if(!(health <= 0)) {
            return true;
        } else {
            return false;
        }
    }
}