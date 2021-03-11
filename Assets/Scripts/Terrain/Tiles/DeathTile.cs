using UnityEngine;

public class DeathTile : Tile
{
    public DeathTile(Vector2Int position, int height, bool isTop) : base(position, height) => IsTop = isTop;

    public bool IsTop { get; private set; }
    
    public override void Register(ITileable entity)
    {
        base.Register(entity);

        if (!(entity is Tileable tileable) || tileable.Team == TeamTag.Immutable || !tileable.TryGetComponent<IDamageable>(out var damageable)) return;
        
        tileable.InterruptMove();
        damageable.Inflict(10, DamageType.Base);
    }
}