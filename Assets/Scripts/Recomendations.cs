using System.Collections.Generic;
using System.Linq;
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
        List<float> usersPlayTime = Nucleus.instance.GetUsersPlayTime();
        List<int> usersTopicsAverageViews = Nucleus.instance.GetUsersTopicsAverageViews();
        List<float> homeworksQuality = Nucleus.instance.GetHomeworksQuality();
        List<float> videoViewTime = Nucleus.instance.GetVideoViewTime();
        List<int> homeworksCount = Nucleus.instance.GetHomeworksCount();

        for (int i = 0; i < usersPlayTime.Count; i++)
        {
            usersPlayTime[i] = usersPlayTime[i] / usersPlayTime.Max();
        }
        for (int i = 0; i < usersTopicsAverageViews.Count; i++)
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
        }
    }
}
