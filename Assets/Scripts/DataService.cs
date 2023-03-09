using SQLite4Unity3d;
using UnityEngine;
using nsDB;
using System;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{
    private SQLiteConnection _connection;

    static Type[] tables =
    {
        typeof(Category),
        typeof(FavoriteTopic),
        typeof(ForumMessage),
        typeof(ForumTopic),
        typeof(Model),
        typeof(Topic),
        typeof(User),
        typeof(UserRole),
        typeof(Homework),
        typeof(TopicView),
        typeof(Visit),
    };

    static string[] tablenames =
    {
        "categories",
        "favorite_topics",
        "forum_messages",
        "forum_topics",
        "models",
        "topics",
        "users",
        "user_roles",
        "homeworks",
        "topic_views",
        "visits",
    };


    public DataService(string DatabaseName)
    {
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb =
 Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb =
 Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);
    }

    public void CreateDB()
    {
        createtables();
    }

    public void droptables()
    {
        _connection.DropTable<Category>();
        _connection.DropTable<FavoriteTopic>();
        _connection.DropTable<ForumMessage>();
        _connection.DropTable<ForumTopic>();
        _connection.DropTable<Model>();
        _connection.DropTable<Topic>();
        _connection.DropTable<User>();
        _connection.DropTable<UserRole>();
        _connection.DropTable<Homework>();
        _connection.DropTable<TopicView>();
        _connection.DropTable<Visit>();
    }

    public void createtables()
    {
        _connection.CreateTable<Category>();
        _connection.CreateTable<FavoriteTopic>();
        _connection.CreateTable<ForumMessage>();
        _connection.CreateTable<ForumTopic>();
        _connection.CreateTable<Model>();
        _connection.CreateTable<Topic>();
        _connection.CreateTable<User>();
        _connection.CreateTable<UserRole>();
        _connection.CreateTable<Homework>();
        _connection.CreateTable<TopicView>();
        _connection.CreateTable<Visit>();

        _connection.CreateIndex("i_user_1", "users", new string[] { "usr_email" }, true);
    }

    public SQLiteConnection getConn()
    {
        return (_connection);
    }

    public void clearDB()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            _connection.Execute("delete from " + tablenames[i]);
        }

        _connection.Execute("vacuum;");
    }

    public int selectCount(string query)
    {
        int qq = _connection.ExecuteScalar<int>(query);
        return qq;
    }

    public int selectCount(string query, System.Object[] o)
    {
        int qq = _connection.ExecuteScalar<int>(query, o);
        return qq;
    }

    public string selectString(string query)
    {
        string qq = _connection.ExecuteScalar<string>(query);
        return qq;
    }

    public string reportCounts()
    {
        string rv = "";
        for (int i = 0; i < tablenames.Length; i++)
            rv += tablenames[i] + ": " + selectCount("select count(1) from " + tablenames[i]) +
                  System.Environment.NewLine;
        return rv;
    }

    public int recompMistakes()
    {
        string s = DateTime.Now.ToString("dd.MM.yy");
        // Сдвигаем ожидаемую дату прохождения если ранее не было пройдено
        _connection.Execute(
            " update mistakes set next_date = ? where status >=0 and (coalesce(next_date,'') = '' or date('20'||substr(next_date,7,2) ||'-'||substr(next_date,4,2) ||'-'||substr(next_date,4,2)) < date('now') )",
            new System.Object[] {s});
        // сбрасываем ранние статусы
        _connection.Execute(" update mistakes set status = 0 where status >=0 and last_date != ?",
            new System.Object[] {s});
        // Ставим статус к сегодняшнему прохождению если не проходили сегодня и если ожидаемая дата сегодня
        _connection.Execute(" update mistakes set status = 1 where status >=0 and next_date = ? and last_date != ? ",
            new System.Object[] {s, s});

        return selectCount("select count(1) from mistakes where next_date = ? and status = 1", new System.Object[] {s});
    }
}