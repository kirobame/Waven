public struct SpellKey 
{
    public SpellKey(SpellCategory category, int tier)
    {
        Category = category;
        Tier = tier;
    }
    
    public SpellCategory Category { get; private set; }
    public int Tier { get; private set; }

    public void Downgrade() => Tier--;

    public override int GetHashCode() => Category.GetHashCode() * Tier.GetHashCode() / 2;
}