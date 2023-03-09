using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    public static ModelLoader instance;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    public void LoadModel()
    {
        
    }
}
