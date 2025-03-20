class Item
{
    // fields
    public int Size { get; }
    public string Description { get; }
    public Room FittingLock { get; }
    public bool CanBePickedUp;

    // constructor
    public Item(int size, string description, bool canBePickedUp)
    {
        Size = size;
        Description = description;
        FittingLock = null;
        CanBePickedUp = canBePickedUp;
    }

    public Item(int size, string description, bool canBePickedUp, Room fittingLock) 
    : this(size, description, canBePickedUp)
    {
        FittingLock = fittingLock;
    }
}