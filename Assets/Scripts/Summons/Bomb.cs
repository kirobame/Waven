using UnityEngine;

public class Bomb : Damageable
{
    [SerializeField] private TileableBase tileable;
    
    protected override void OnDeath()
    {
        var cells = new Vector2Int[]
        {
            tileable.Navigator.Current.FlatPosition + Vector2Int.down,
            tileable.Navigator.Current.FlatPosition + Vector2Int.right,
            tileable.Navigator.Current.FlatPosition + Vector2Int.left,
            tileable.Navigator.Current.FlatPosition + Vector2Int.up,
        };
        foreach (var cell in cells)
        {
            if (!cell.TryGetTile(out var tile)) continue;

            foreach (var entity in tile.Entities)
            {
                if (entity.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
            }
        }
        
        base.OnDeath();
    }
}