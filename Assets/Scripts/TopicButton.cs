using TMPro;
using UnityEngine;

public class TopicButton : MonoBehaviour
{
    public int topicIndex;
    public TextMeshProUGUI title;

    public void LoadTopic()
    {
        TopicLoader.instance.LoadTopic(topicIndex);
    }

    public void AddToFavorite(int topicId)
    {
        Nucleus.instance.AddFavoriteTopic(topicIndex);
    }
}