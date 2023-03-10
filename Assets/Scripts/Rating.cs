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

            newUserPanel.SetUI(users.Single(e => e.usr_id == additiveCriteria[i].userId).usr_name + 1, i);
        }
    }

    private void GetAdditiveCriteria(IEnumerable<AdditiveCriterion> additiveCriterion)
    {
        additiveCriteria = additiveCriterion.ToList();
    }
}
