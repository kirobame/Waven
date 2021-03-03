using UnityEngine;

[CreateAssetMenu(fileName = "NewFakeSpell", menuName = "Waven/FakeSpell")]
public class FakeSpell : ScriptableObject, ICastable
{
    public string Title => title;
    [SerializeField] private string title;

    public string Description => description;
    [SerializeField, TextArea] private string description;

    public Sprite Thumbnail => thumbnail;
    [SerializeField] private Sprite thumbnail;
}