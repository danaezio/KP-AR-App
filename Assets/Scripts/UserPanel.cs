using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Image ratingImage;

    public void SetUI(string userName, int rating)
    {
        nameText.text = userName;
        ratingText.text = rating.ToString();
    }

    public void SetColor(Color color)
    {
        ratingImage.color = color;
        ratingText.color = color;
    }
}
