class Key : Item 
{
    Room FittingLock;
    string Desc;
    public Key(Room fittingLock) : base(10, "a key")
    {
        FittingLock = fittingLock;
    }
}