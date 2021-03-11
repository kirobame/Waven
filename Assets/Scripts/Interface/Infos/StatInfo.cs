using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInfo : MonoBehaviour, IComparable<StatInfo>
{
    public RectTransform RectTransform => (RectTransform)transform;
    public TMP_Text TextMesh => textMesh;
    
    [SerializeField] private RectTransform background;
    [SerializeField] private float spacing;
    
    [Space, SerializeField] private Image image;
    [SerializeField] private TMP_Text textMesh;

    private bool hasSource;
    private Stats.Info current;
    
    public void Assign(Stats.Info source, string text)
    {
        hasSource = true;
        current = source;

        float statSize = 0f;
        statSize += transform.GetComponentInChildren<RectTransform>().sizeDelta.x;

        var bgSize = background.sizeDelta;
        bgSize.x += (statSize + spacing);
        
        gameObject.SetActive(true);
        
        image.sprite = source.Icon;
        image.color = source.Color;
        textMesh.text = text;
        
        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.flexibleWidth + textMesh.preferredWidth + spacing);
    }

    public void Clear()
    {
        hasSource = false;
        gameObject.SetActive(false);
    }

    public int CompareTo(StatInfo other)
    {
        if (gameObject.activeInHierarchy) return -1;
        else
        {
            if (!other.hasSource) return 1;
            else return current.Type.CompareTo(other.current.Type);
        }
    }
}