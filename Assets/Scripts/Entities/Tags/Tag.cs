using UnityEngine;

public class Tag : MonoBehaviour, ITag
{
    public TeamTag Team
    {
        get => tag;
        set
        {
            tag = value;
            Refresh();
        }
    }

    [SerializeField] private new TeamTag tag;
    
    [Space, SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Sprite blueTeam;
    [SerializeField] private Sprite redTeam;

    void Start() => Refresh();
    
    private void Refresh()
    {
        if (tag == TeamTag.Player1)
        {
            renderer.enabled = true;
            renderer.sprite = blueTeam;
        }
        else if (tag == TeamTag.Player2)
        {
            renderer.enabled = true;
            renderer.sprite = redTeam;
        }
        else renderer.enabled = false;
    }
}