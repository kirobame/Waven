using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable 
{
    string Title { get; }
    string Description { get; }
    Sprite Thumbnail { get; }
}
