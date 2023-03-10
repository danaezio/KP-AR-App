using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingText;

    public void SetUI(string userName, int rating)
    {
        nameText.text = userName;
        ratingText.text = rating.ToString();
    }
}
