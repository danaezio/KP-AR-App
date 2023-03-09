using System.Collections.Generic;
using UnityEngine;

public class Recomendations : MonoBehaviour
{
    private void Start()
    {
        NormalizeCriteria();
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
        
        for (int i = 0; i < usersPlayTime.Count; i++)
        {
            //usersPlayTime[i] = usersPlayTime[i].minutesCount / usersPlayTime.Max();
        }
        /*for (int i = 0; i < usersTopicsAverageViews.Count; i++)
        {
            usersTopicsAverageViews[i] = usersTopicsAverageViews[i] / usersTopicsAverageViews.Max();
        }
        for (int i = 0; i < homeworksQuality.Count; i++)
        {
            homeworksQuality[i] = homeworksQuality[i] / homeworksQuality.Max();
        }
        for (int i = 0; i < videoViewTime.Count; i++)
        {
            videoViewTime[i] = videoViewTime[i] / videoViewTime.Max();
        }
        for (int i = 0; i < homeworksCount.Count; i++)
        {
            homeworksCount[i] = homeworksCount[i] / homeworksCount.Max();
        }*/
    }
}
