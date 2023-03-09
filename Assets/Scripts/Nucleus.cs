using System;
using System.Collections.Generic;
using nsDB;
using SQLite4Unity3d;
using UnityEngine;

public class Nucleus : MonoBehaviour
{
    public static Nucleus instance;

    public static int currentUserId = -1;

    private SQLiteConnection _db = DB.database.getConn();

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="userPassword">Пароль пользователя</param>
    /// <param name="userEmail">E-mail пользователя</param>
    public bool CreateUser(string userName, string userPassword, string userEmail)
    {
        try
        {
            DateTime date = DateTime.Now;

            User newUser = new User();
            newUser.usr_name = userName;
            newUser.usr_password = userPassword;
            newUser.usr_email = userEmail;
            newUser.usr_register_date = date.ToShortDateString();
            newUser.usr_photo = userName[0].ToString();

            _db.Insert(newUser);

            currentUserId = newUser.usr_id;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="userEmail">E-Mail пользователя</param>
    /// <param name="userPassword">Пароль пользователя</param>
    /// <returns>ID пользователя, при успешной авторизации</returns>
    public bool AuthorizeUser(string userEmail, string userPassword)
    {
        List<User> records = _db.Query<User>("select * from users where usr_email = ? and usr_password = ?",
            new object[] { userEmail, userPassword });

        if (records.Count == 0) return false;

        currentUserId = records[0].usr_id;
        return true;
    }

    /// <summary>
    /// Получить избранные темы пользователя
    /// </summary>
    /// <returns>Список из избранных тем пользователя</returns>
    public List<Topic> GetFavoriteTopics()
    {
        List<Topic> records = _db.Query<Topic>("select * from topics where top_id in (" +
                                               "select fato_top_id from favorite_topics where fato_usr_id = ? )"
            , new object[] { currentUserId });

        return records;
    }

    /// <summary>
    /// Добавить избранную тему к пользователю
    /// </summary>
    /// <param name="topicId">ID избранной темы</param>
    public bool AddFavoriteTopic(int topicId)
    {
        FavoriteTopic newFavoriteTopic = new();
        newFavoriteTopic.fato_top_id = topicId;
        newFavoriteTopic.fato_usr_id = currentUserId;

        _db.Insert(newFavoriteTopic);
        _db.Commit();

        return true;
    }

    /// <summary>
    /// Получить имя пользователя
    /// </summary>
    /// <returns>Имя пользователя</returns>
    public string GetUserName()
    {
        List<User> records = _db.Query<User>("select * from users where usr_id = ?", new object[] { currentUserId });

        return records[0].usr_name;
    }

    public List<float> GetUsersPlayTime()
    {
        return new List<float>();
    }

    public List<int> GetUsersTopicsAverageViews()
    {
        return new List<int>();
    }

    public List<float> GetHomeworksQuality()
    {
        return new List<float>();
    }

    public List<float> GetVideoViewTime()
    {
        return new List<float>();  
    }

    public List<int> GetHomeworksCount()
    {
        return new List<int>();
    }

    private bool CheckUser()
    {
        return currentUserId >= 0;
    }
}