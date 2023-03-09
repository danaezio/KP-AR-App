using System;
using UnityEngine;

public class TopicViewTimeCounter : MonoBehaviour
{
    [SerializeField] private int topicIndex;
    
    private float _time;

    private void OnDisable()
    {
        _time = 0;
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    public void SaveViewTime()
    {
        Nucleus.instance.AddTopicVideoViewTime(topicIndex, Mathf.FloorToInt(_time));
    }
}
