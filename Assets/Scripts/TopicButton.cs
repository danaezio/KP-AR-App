using TMPro;
using UnityEngine;

public class TopicButton : MonoBehaviour
{
    public int topicIndex;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void LoadTopic()
    {
        TopicLoader.instance.LoadTopic(topicIndex);
    }

    public void AddToFavorite(int topicId)
    {
        Nucleus.instance.AddFavoriteTopic(topicIndex);
    }

    public void AddTopicView()
    {
        Nucleus.instance.AddTopicView(topicIndex);
    }

    public void DeleteFromFavorite()
    {
        Nucleus.instance.DeleteFavoriteTopic(topicIndex);
        Destroy(gameObject);
    }

    public void SetText(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
}