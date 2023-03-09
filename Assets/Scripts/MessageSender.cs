using UnityEngine;

public class MessageSender : MonoBehaviour
{
    public static MessageSender instance;
    
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    
    public void SendMessage(string subject, string message)
    {
        
    }
}
