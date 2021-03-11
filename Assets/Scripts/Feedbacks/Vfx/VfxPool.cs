using System;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class VfxPool : Pool<Animator, PoolableVfx>
{
    #region Nested Types

    [Serializable]
    public class VfxProvider : Provider<Animator, PoolableVfx> { }

    #endregion

    protected override IList<Provider<Animator, PoolableVfx>> Providers => providers;
    [SerializeField] private VfxProvider[] providers;
}