using System;
using System.Collections.Generic;
using System.Linq;
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

    public void UpdateUser(string userName)
    {
        User user = GetUser(currentUserId);

        user.usr_name = userName;

        _db.Update(user);
        _db.Commit();
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

    public bool DeleteFavoriteTopic(int topicId)
    {
        try
        {
            List<FavoriteTopic> records =
                _db.Query<FavoriteTopic>("select * from favorite_topics where fato_usr_id = ?", Nucleus.currentUserId);

            _db.Delete(records[topicId]);
            _db.Commit();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            
            return false;
        }
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

    public bool AddVisit(DateTime entryTime, DateTime exitTime)
    {
        Visit newVisit = new()
        {
            vis_entry_date = entryTime.ToString(),
            vis_exit_date = exitTime.ToString(),
            vis_usr_id = currentUserId
        };

        _db.Insert(newVisit);
        _db.Commit();

        return true;
    }

    public void AddTopicView(int topicIndex)
    {
        List<TopicView> records = _db.Query<TopicView>("select * from topic_views where tovi_usr_id = ? and tovi_top_id = ?", currentUserId, topicIndex);

        if (records.Count == 0)
        {
            TopicView newTopicView = new()
            {
                tovi_top_id = topicIndex,
                tovi_usr_id = currentUserId,
                tovi_view_count = 0,
                tovi_video_view_time = 0,
            };

            newTopicView.tovi_view_count++;
            _db.Insert(newTopicView);
        }
        else
        {
            records[0].tovi_view_count++;
            _db.Update(records[0]);
        }
        
        _db.Commit();
    }

    public void AddTopicVideoViewTime(int topicIndex, int seconds)
    {
        List<TopicView> records = _db.Query<TopicView>("select * from topic_views where tovi_usr_id = ? and tovi_top_id = ?", currentUserId, topicIndex);
        
        records[0].tovi_video_view_time += seconds;
        _db.Update(records[0]);
        
        _db.Commit();
    }

    /// <summary>
    /// Получить время проведенное каждым пользователем в системе (1 критерий) 
    /// </summary>
    /// <returns></returns>
    public List<UserPlayTime> GetUsersPlayTime()
    {
        List<User> users = GetUsers();
        List<UserPlayTime> usersPlayTime = new();

        foreach (User user in users)
        {
            List<Visit> visitsRecords = _db.Query<Visit>("select * from visits where vis_usr_id = ?", user.usr_id);

            UserPlayTime userPlayTime = new ()
            {
                userID = user.usr_id
            };

            foreach (Visit visit in visitsRecords)
            {
                DateTime entryDate = DateTime.Parse(visit.vis_entry_date);
                DateTime exitTime = DateTime.Parse(visit.vis_exit_date);

                TimeSpan timeSpan = exitTime - entryDate;

                userPlayTime.minutesCount += (int)timeSpan.TotalMinutes;
            }

            usersPlayTime.Add(userPlayTime);
        }

        return usersPlayTime;
    }

    /// <summary>
    /// Получить среднее арифметическое количества просмотров тем для каждого пользователя (2 критерий)
    /// </summary>
    /// <returns></returns>
    public List<UserTopicViews> GetUsersTopicsAverageViews()
    {
        List<User> users = GetUsers();
        List<UserTopicViews> usersTopicViews = new();

        foreach (User user in users)
        {
            List<TopicView> topicViews = _db.Query<TopicView>("select * from topic_views where tovi_usr_id = ?", user.usr_id);

            if (topicViews.Count == 0) continue;

            UserTopicViews userTopicViews = new()
            {
                userID = user.usr_id,
                averageViews = 0
            };

            int averageViews = topicViews.Sum(topicView => topicView.tovi_view_count);
            averageViews /= topicViews.Count;

            userTopicViews.averageViews = averageViews;

            usersTopicViews.Add(userTopicViews);
        }

        return usersTopicViews;
    }

    /// <summary>
    /// Получить средние арифметические всех оценок домашних работ пользователей (3 критерий)
    /// </summary>
    /// <returns></returns>
    public List<UserHomework> GetHomeworksQuality()
    {
        List<User> users = GetUsers();
        List<UserHomework> usersHomework = new();

        foreach (User user in users)
        {
            List<Homework> homeworks = _db.Query<Homework>("select * from homework where howo_usr_id = ? and howo_mark IS NOT NULL", user.usr_id);

            if (homeworks.Count == 0) continue;

            UserHomework userHomework = new()
            {
                userID = user.usr_id,
            };

            float sum = homeworks.Sum(homework => homework.howo_mark);
            sum /= homeworks.Count;

            userHomework.averageMark = sum;

            usersHomework.Add(userHomework);
        }

        return usersHomework;
    }

    /// <summary>
    /// Получить время просмотра видеороликов всех пользователей (4 критерий)
    /// </summary>
    /// <returns></returns>
    public List<UserTopicViews> GetVideoViewTime()
    {
        List<User> users = GetUsers();
        List<UserTopicViews> usersTopicViews = new();

        foreach (User user in users)
        {
            List<TopicView> topicViews = _db.Query<TopicView>("select * from topic_views where tovi_usr_id = ?", user.usr_id);

            if (topicViews.Count == 0) continue;

            UserTopicViews userTopicViews = new()
            {
                userID = user.usr_id,
                averageViews = 0,
                videoViewTime = 0,
            };

            int viewTime = topicViews.Sum(topicView => topicView.tovi_video_view_time);
            userTopicViews.videoViewTime = viewTime;

            usersTopicViews.Add(userTopicViews);
        }

        return usersTopicViews;
    }

    /// <summary>
    /// Получить список домашних работ с проставленной оценкой (5 критерий)
    /// </summary>
    /// <returns></returns>
    public List<UserHomework> GetHomeworksCount()
    {
        List<User> users = GetUsers();
        List<UserHomework> usersHomework = new();

        foreach (User user in users)
        {
            List<Homework> homeworks = _db.Query<Homework>("select * from homework where howo_usr_id = ? and howo_mark IS NOT NULL", user.usr_id);
            
            UserHomework userHomework = new()
            {
                userID = user.usr_id,
                homeworksCount = homeworks.Count
            };
            
            usersHomework.Add(userHomework);
        }

        return usersHomework;
    }

    /// <summary>
    /// Получить список всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    public List<User> GetUsers()
    {
        List<User> records = _db.Query<User>("select * from users");

        return records;
    }

    public User GetUser(int userID)
    {
        List<User> records = _db.Query<User>("select * from users");

        return records.Single(e => e.usr_id == userID);
    }
    
    /// <summary>
    /// Проверить что сейчас авторизован какой-либо пользователь
    /// </summary>
    /// <returns>True если пользователь авторизован</returns>
    private bool CheckUser()
    {
        return currentUserId >= 0;
    }
}