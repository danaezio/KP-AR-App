using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using nsDB;
using UnityEngine;

public class StartPage : MonoBehaviour
{ 
    private SQLiteConnection _db = DB.database.getConn();
    
    private void Start()
    {
        string sr = PlayerPrefs.GetString("isfirst", "1");
        if (sr.Equals("1"))
        {
            DB.database.droptables();
            DB.database.createtables();
            PlayerPrefs.SetString("isfirst", "0");
        }
        try
        {
            SQLiteConnection db = DB.database.getConn();
        }
        catch (Exception e)
        {
            
        }

        int count = DB.database.selectCount("select count(*) from topics");
        if (count > 0) return;


        Topic newTopic0 = new () { top_name = "Subdiv" };
        Topic newTopic1 = new () { top_name = "Hard Surface оружие" };
        Topic newTopic2 = new () { top_name = "Bake" };
        Topic newTopic3 = new () { top_name = "Техника" };
        
        List<Topic> startTopics = new ()
        {
            newTopic0,
            newTopic1,
            newTopic2,
            newTopic3
        };

        foreach (Topic topic in startTopics)
        {
            _db.Insert(topic);
        }
        
        _db.Commit();
    }
}




