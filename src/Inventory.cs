class Inventory
{
    // fields
    private int maxSpace;
    private int spaceLeft;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxSpace)
    {
        this.maxSpace = maxSpace;
        this.spaceLeft = maxSpace;
        items = new Dictionary<string, Item>();
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

        if (spaceLeft < item.Size)
        {
            return false;
        }
        else
        {
            items.Add(itemName, item);
            spaceLeft = -item.Size;
            return true;
        }
    }

    public Item Get(string itemName, Item item)
    {
        // TODO implement:
        // Find Item in items Dictionary
        // remove Item from items Dictionary if found
        // return Item or null

        if (items.ContainsKey(itemName))
        {
            items.Remove(itemName);
            return item;
        }
        else
        {
            return null;
        }
    }
}