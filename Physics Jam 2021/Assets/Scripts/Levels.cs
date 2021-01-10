using PurpleCable;

public class Levels : ResourceDictionary<int, LevelDef>
{
    public Levels()
        : base("Levels")
    { }

    protected override int GetKeyForItem(LevelDef item)
    {
        return int.Parse(item.name.Substring(5));
    }
}
