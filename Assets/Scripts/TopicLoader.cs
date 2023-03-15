using System.Collections.Generic;
using nsDB;
using TMPro;
using UnityEngine;

public class TopicLoader : MonoBehaviour
{
    public static TopicLoader instance;

    [SerializeField] private GameObject topicButtonPrefab;
    [SerializeField] private TopicUI topicUI;

    [SerializeField] private Transform topicsLayout;
    [SerializeField] private Transform favoriteTopicsLayout;

    private List<Topic> _topics;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    /// <summary>
    /// Загрузить все обучающие темы
    /// </summary>
    public void LoadTopics()
    {
        /*_topics = Nucleus.instance.GetTopics();

        for (int i = 0; i < _topics.Count; i++)
        {
            GameObject newTopicButton = Instantiate(topicButtonPrefab);

            newTopicButton.GetComponentInChildren<TextMeshProUGUI>().SetText(_topics[i].top_name);
            newTopicButton.GetComponent<TopicButton>().topicIndex = i;
        }*/
    }
    
    /// <summary>
    /// Загрузить все избранные темы
    /// </summary>
    public void LoadFavoriteTopics()
    {
        _topics = Nucleus.instance.GetFavoriteTopics();

        for (int i = 0; i < _topics.Count; i++)
        {
            GameObject newTopicButton = Instantiate(Resources.Load<GameObject>("Topic Button"), favoriteTopicsLayout);

            TopicButton topicButton = newTopicButton.GetComponent<TopicButton>();
            
            topicButton.topicIndex = i;

            switch (_topics[i].top_name)
            {
                case "UV-Развертка":
                    topicButton.SetText(_topics[i].top_name,
                        "Создавайте качественную развертку - для качественных текстур!");
                    break;
                case "Hard Surface":
                    topicButton.SetText(_topics[i].top_name,
                        "Твердотельное моделлирование - создание различных объектов с ярко выраженными углами.");
                    break;
                case "Substance Painter":
                    topicButton.SetText(_topics[i].top_name,
                        "Научитесь текстурировать свои модели по всем канонам геймдева!");
                    break;
                case "Soft Surface":
                    topicButton.SetText(_topics[i].top_name,
                        "Мягкотельное моделлирование - создание объектов с арганичной и плавной формой.");
                    break;
                default:
                    topicButton.SetText(_topics[i].top_name, string.Empty);
                    break;
            }
        }
    }

    /// <summary>
    /// Загрузить экран обучающей темы
    /// </summary>
    /// <param name="topicId"></param>
    public void LoadTopic(int topicId)
    {
        topicUI.title.SetText(_topics[topicId].top_name);
        topicUI.description.SetText(_topics[topicId].top_description);

        topicUI.videoUrl = _topics[topicId].top_video_url;
        topicUI.additionalInfoLink = _topics[topicId].top_additional_information_link;
    }

    /// <summary>
    /// Очистить UI избранных тем
    /// </summary>
    public void ClearFavoriteTopics()
    {
        for (int i = 0; i < favoriteTopicsLayout.childCount; i++)
        {
            Destroy(favoriteTopicsLayout.GetChild(i).gameObject);
        }
    }
}