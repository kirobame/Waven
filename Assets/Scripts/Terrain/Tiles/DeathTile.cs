using UnityEngine;

public class DeathTile : Tile
{
    public DeathTile(Vector2Int position, int height) : base(position, height) { }

    public override void Register(ITileable entity)
    {
        base.Register(entity);
        
        if (!(entity is Player player)) return;
        
        player.InterruptMove();
        var damageable = player.GetComponent<IDamageable>();
        damageable.Inflict(10, DamageType.Base);
    }
}