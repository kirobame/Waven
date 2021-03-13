using System.Linq;
using Flux;
using UnityEngine;

public class SlideTrap : Trap
{
    public Vector2Int SlideDirection { get; set; }
    
    protected override void Apply(ITileable source)
    {
        if (!Navigator.Current.Entities.Any(entity => entity is Tileable))
        {
            SlideDirection = Vector2Int.zero;
            return;
        }

        foreach (var entity in Navigator.Current.Entities)
        {
            if (entity is Golem golem)
            {
                if (golem.HasJustSpawned) return;
            }
        }
        
        if (source is Tileable tileable)
        {
            Buffer.slideDirections.Enqueue(tileable.LastDirection.ComputeOrientation());
            if (tileable.IsMoving) tileable.InterruptMove();
        }
        else
        {
            if (SlideDirection == Vector2Int.zero) return;

            Buffer.slideDirections.Enqueue(SlideDirection);
            SlideDirection = Vector2Int.zero;
        }

        Routines.Start(Routines.DoAfter(() =>
        {
            spell.Prepare();
            spell.CastFrom(Navigator.Current, Spellcaster.EmptyArgs);
            
            navigator.Current.Mark(Mark.None);

        }, new YieldFrame()));
    }
}