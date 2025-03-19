class Item
{
    // fields
    public int Size { get; }
    public string Description { get; }
    public Room FittingLock { get; }

    // constructor
    public Item(int size, string description)
    {
        Size = size;
        Description = description;
        FittingLock = null;
    }

    public Item(int size, string description, Room fittingLock) : this(size, description)
    {
        FittingLock = fittingLock;
    }
}