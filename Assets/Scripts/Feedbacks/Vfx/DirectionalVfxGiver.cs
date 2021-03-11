using System;
using System.Linq;
using Flux;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectionalVfxGiver", menuName = "Waven/Vfx/Directional")]
public class DirectionalVfxGiver : VfxGiver
{
    #region Nested Types

    [Serializable]
    private class VfxDirectionPair
    {
        public Vector2Int Direction => direction;
        [SerializeField] private Vector2Int direction;

        public PoolableVfx Value => value;
        [SerializeField] private PoolableVfx value;
    }

    #endregion

    [SerializeField] private VfxDirectionPair[] pairs;

    public override PoolableVfx Get(EventArgs args)
    {
        var direction = ((IWrapper<Vector2Int>)args).Value;
        return pairs.First(pair => pair.Direction == direction).Value;
    }
}