using System.Collections.Generic;
using nsDB;
using UnityEngine;

public class ForumLoader : MonoBehaviour
{
    public static ForumLoader instance;

    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform messageLayout;
    
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    
    /// <summary>
    /// Загрузить сообщения темы форума
    /// </summary>
    /// <param name="topicId"></param>
    public void LoadMessages(int topicId)
    {
        /*List<ForumMessage> forumMessages = Nucleus.instance.GetForumMessages(topicId);

        foreach (ForumMessage message in forumMessages)
        {
            Message newMessage = Instantiate(messagePrefab).GetComponent<Message>();

            newMessage.subject.SetText(message.fome_subject);
            newMessage.text.SetText(message.fome_message);

            string userPhotoPath = Nucleus.instance.GetUserPhotoPath();
            Sprite userPhoto = Resources.Load<Sprite>(userPhotoPath);
            newMessage.photo.sprite = userPhoto;
        }*/
    }

    public void LoadTopics(int categoryId)
    {
        
    }

    public void LoadCategories()
    {
        
    }
}
