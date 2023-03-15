using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nsDB;
using UnityEngine;

public class Rating : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject userPanelPrefab;
    [SerializeField] private List<Color> placeColors;

    public List<AdditiveCriterion> additiveCriteria;

    private void Start()
    {
        Recomendations.onGetAdditiveCriteria += GetAdditiveCriteria;
    }

    public void ViewRating()
    {
        if(content.childCount > 0) return;
        
        additiveCriteria = additiveCriteria.OrderByDescending(criterion => criterion.additiveCriteria).ToList();

        List<User> users = Nucleus.instance.GetUsers();

        for (int i = 0; i < additiveCriteria.Count; i++)
        {
            UserPanel newUserPanel = Instantiate(userPanelPrefab.gameObject, content).GetComponent<UserPanel>();

            newUserPanel.SetUI(users.Single(e => e.usr_id == additiveCriteria[i].userId).usr_name, i + 1);

            if (placeColors.Count > i)
            {
                newUserPanel.SetColor(placeColors[i]);
            }
        }
    }

    private void GetAdditiveCriteria(IEnumerable<AdditiveCriterion> additiveCriterion)
    {
        additiveCriteria = additiveCriterion.ToList();
        ViewRating();
    }
}
