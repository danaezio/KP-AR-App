using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using nsDB;
using TMPro;
using UnityEngine.Events;

public class Recomendations : MonoBehaviour
{
    public static UnityAction<IEnumerable<AdditiveCriterion>> onGetAdditiveCriteria;

    [SerializeField] private List<RecommendationField> recommendationFields;
    [SerializeField] private TextMeshProUGUI recommendationText;

    private void Start()
    {
        UserSigning.onSigning += NormalizeCriteria;

        recommendationFields = recommendationFields.OrderBy(field => field.threshold).ToList();
    }

    /// <summary>
    /// Нормализовать критерии
    /// </summary>
    public void NormalizeCriteria()
    {
        // 1 Критерий
        List<UserPlayTime> usersPlayTime = Nucleus.instance.GetUsersPlayTime();
        // 2 Критерий
        List<UserTopicViews> usersTopicsAverageViews = Nucleus.instance.GetUsersTopicsAverageViews();
        // 3 Критерий
        List<UserHomework> usersHomeworks = Nucleus.instance.GetHomeworksQuality();
        // 4 Критерий
        List<UserTopicViews> usersVideoViewTime = Nucleus.instance.GetVideoViewTime();
        // 5 Критерий
        List<UserHomework> usersWithMarkedHomework = Nucleus.instance.GetHomeworksCount();

        foreach (UserPlayTime userPlayTime in usersPlayTime)
        {
            Debug.Log($"userID: {userPlayTime.userID}; PlayTimeMinutes: {userPlayTime.minutesCount}");
        }
        foreach (UserTopicViews userTopicViews in usersTopicsAverageViews)
        {
            Debug.Log($"userID: {userTopicViews.userID}; averageTopicViews: {userTopicViews.averageViews}");
        }
        foreach (UserHomework userHomework in usersHomeworks)
        {
            Debug.Log($"userID: {userHomework.userID}; averageMark: {userHomework.averageMark}");
        }
        foreach (UserTopicViews userVideoViewTime in usersVideoViewTime)
        {
            Debug.Log($"userID: {userVideoViewTime.userID}; topicVideoViewTime: {userVideoViewTime.videoViewTime}");
        }
        foreach (UserHomework userHomework in usersWithMarkedHomework)
        {
            Debug.Log($"userID: {userHomework.userID}; homeworksCount: {userHomework.homeworksCount}");
        }

        float maxCriteria = usersPlayTime.Max(max => max.minutesCount);
        
        foreach (UserPlayTime t in usersPlayTime)
        {
            t.minutesCount /= maxCriteria;
            t.minutesCount *= 0.5f;
        }
        
        maxCriteria = usersTopicsAverageViews.Max(max => max.averageViews);

        foreach (UserTopicViews user in usersTopicsAverageViews)
        {
            user.averageViews /= maxCriteria;
            user.averageViews *= 0.25f;
        }
        
        maxCriteria = usersHomeworks.Max(max => max.averageMark);

        foreach (UserHomework user in usersHomeworks)
        {
            user.averageMark /= maxCriteria;
        }
        
        maxCriteria = usersVideoViewTime.Max(max => max.videoViewTime);

        foreach (UserTopicViews user in usersVideoViewTime)
        {
            user.videoViewTime /= maxCriteria;
        }
        
        maxCriteria = usersWithMarkedHomework.Max(max => max.homeworksCount);

        foreach (UserHomework user in usersWithMarkedHomework)
        {
            user.homeworksCount /= maxCriteria;
            user.homeworksCount *= 0.5f;
        }

        List<User> users = Nucleus.instance.GetUsers();
        List<AdditiveCriterion> additiveCriteria = new List<AdditiveCriterion>();

        for (int i = 0; i < users.Count; i++)
        {
            AdditiveCriterion additiveCriterion = new AdditiveCriterion();
            
            try
            {
                additiveCriterion.userId = users[i].usr_id;

                additiveCriterion.additiveCriteria =
                    usersWithMarkedHomework.Single(e => e.userID == users[i].usr_id).homeworksCount +
                    usersVideoViewTime.Single(e => e.userID == users[i].usr_id).videoViewTime +
                    usersHomeworks.Single(e => e.userID == users[i].usr_id).averageMark +
                    usersTopicsAverageViews.Single(e => e.userID == users[i].usr_id).averageViews +
                    usersPlayTime.Single(e => e.userID == users[i].usr_id).minutesCount;
            }
            catch (Exception e)
            {
                continue;
            }
            finally
            {
                additiveCriteria.Add(additiveCriterion);
            }
        }

        float sum = additiveCriteria.Sum(e => e.additiveCriteria);

        for (int index = 0; index < additiveCriteria.Count; index++)
        {
            AdditiveCriterion item = additiveCriteria[index];
            item.additiveCriteria /= sum;
        }

        onGetAdditiveCriteria?.Invoke(additiveCriteria);
        if (Nucleus.currentUserId > 0 && Nucleus.currentUserId < additiveCriteria.Count)
        {
            SetRecommendation(additiveCriteria[Nucleus.currentUserId].additiveCriteria);   
        }
    }

    private void SetRecommendation(float additiveCriteria)
    {
        for (int i = recommendationFields.Count - 1; i >= 0; i--)
        {
            if (recommendationFields[i].threshold <= additiveCriteria)
            {
                recommendationText.text = recommendationFields[i].recommendationText;
            }
        }
    }

    [System.Serializable]
    struct RecommendationField
    {
        public string recommendationText;
        public float threshold;
    }
}
