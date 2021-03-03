using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable 
{
    string Name { get; }
    string Description { get; }
    Sprite Thumbnail { get; }
}
