class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;
    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        items = new Dictionary<string, Item>();
    }
    // methods
    public void Put(string itemName, Item item)
    {
        // TODO implement:
        // Check the Weight of the Item and check
        // for enough space in the Inventory
        // Does the Item fit?
        // Put Item in the items Dictionary
        // Return true/false for success/failure

        items.Add(itemName, item);
        // return false;
    }
    public Item Get(string itemName)
    {
        // TODO implement:
        // Find Item in items Dictionary
        // remove Item from items Dictionary if found
        // return Item or null

        return null;
    }
}