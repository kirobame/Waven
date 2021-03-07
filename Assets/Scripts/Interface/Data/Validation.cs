using UnityEngine;
using UnityEngine.UI;

public class Validation : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    
    [Space, SerializeField] private Image checkmark;

    public void Set(bool state)
    {
        if (state)
        {
            image.color = activeColor;
            checkmark.gameObject.SetActive(true);
        }
        else
        {
            image.color = inactiveColor;
            checkmark.gameObject.SetActive(false);
        }
    }
}