using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanel : MonoBehaviour
{
    [SerializeField] private GameObject activatePanel;
    [SerializeField] private GameObject deactivatePanel;

    private void Start()
    {
        if (TryGetComponent(out Button button))
        {
            button.onClick.AddListener(ButtonClick);
        }
        else
        {
            Destroy(this);
        }
    }

    private void ButtonClick()
    {
        activatePanel.SetActive(true);
        deactivatePanel.SetActive(false);
    }
}
