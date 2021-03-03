using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDammageable
{
    int lifePoints { get; set; }
    int TakeDamage(int damage);
}
