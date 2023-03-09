using UnityEngine;
using UnityEngine.UI;

public class ForumTopicCreator : MonoBehaviour
{
    public static ForumTopicCreator instance;
    
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField categoryField;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    
    public void CreateTopic()
    {
        
    }
}
