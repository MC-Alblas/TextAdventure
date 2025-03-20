class Inventory
{
    // fields
    public int MaxSpace { get; }
    public int SpaceLeft { get; set; }
    private Dictionary<string, Item> Items;

    // constructor
    public Inventory(int maxSpace)
    {
        this.MaxSpace = maxSpace;
        this.SpaceLeft = maxSpace;
        Items = new Dictionary<string, Item>();
    }

    // methods
    public bool Put(string itemName, Item item)
    {
        // TODO implement:
        // Check the Weight of the Item and check
        // for enough space in the Inventory
        // Does the Item fit?
        // Put Item in the items Dictionary
        // Return true/false for success/failure

        if (SpaceLeft < item.Size)
        {
            return false;
        }
        else
        {
            Items.Add(itemName, item);
            SpaceLeft -= item.Size;
            return true;
        }
    }

    public Item Get(string itemName)
    {
        // TODO implement:
        // Find Item in items Dictionary
        // remove Item from items Dictionary if found
        // return Item or null

        if (Items.ContainsKey(itemName))
        {
            Item item = Items[itemName];
            Items.Remove(itemName);
            return item;
        }
        else
        {
            return null;
        }
    }

    public void ListInventory()
    {
        foreach (var key in Items.Keys)
        {
            Console.WriteLine(key);
        }

    }

    public bool IsEmpty()
    {
        if (Items.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsMultiple()
    {
        if (Items.Count >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}