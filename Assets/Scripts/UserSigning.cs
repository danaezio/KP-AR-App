using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserSigning : MonoBehaviour
{
    public static UnityAction onSigning;
    
    [Header("Registration")]
    [SerializeField] private TMP_InputField registrationNameField;
    [SerializeField] private TMP_InputField registrationPasswordField;
    [SerializeField] private TMP_InputField registrationEmailField;

    [Header("Authorization")]
    [SerializeField] private TMP_InputField authorizeEmailField;
    [SerializeField] private TMP_InputField authorizePasswordField;

    [Header("UI")] 
    [SerializeField] private GameObject entryMenu;
    [SerializeField] private GameObject registrationMenu;
    [SerializeField] private GameObject authorizationMenu;
    [SerializeField] private GameObject cabinetMenu;
    [SerializeField] private TMP_InputField userNameTitle;
    [SerializeField] private TextMeshProUGUI userPhotoTitle;

    [Header("Animators")]
    [SerializeField] private Animator authorizeErrorAnim;
    [SerializeField] private Animator registerErrorAnim;
    
    private Nucleus _nucleus;
    
    private void Start()
    {
        _nucleus = Nucleus.instance;
        
        CheckUserLogin();
    }

    public void AuthorizeUser()
    {
        if (_nucleus.AuthorizeUser(authorizeEmailField.text, authorizePasswordField.text))
        {
            authorizationMenu.SetActive(false);
            
            SignIn();
        }
        else
        {
            Debug.LogError("Authorization Error!");
            authorizeErrorAnim.SetTrigger("Error");
        }
    }

    public void RegisterUser()
    {
        if (_nucleus.CreateUser(registrationNameField.text, registrationPasswordField.text, registrationEmailField.text))
        {
            registrationMenu.SetActive(false);
            
            SignIn();
        }
        else
        {
            Debug.LogError("Registration Error!");
            registerErrorAnim.SetTrigger("Error");
        }
    }

    public void SignOut()
    {
        TimeCounter.instance.SaveTime();
        Nucleus.currentUserId = -1;

        EnableEntry();
    }

    private void SignIn()
    {
        authorizeEmailField.text = string.Empty;
        authorizePasswordField.text = string.Empty;
        
        registrationNameField.text = string.Empty;
        registrationEmailField.text = string.Empty;
        registrationPasswordField.text = string.Empty;

        string userName = _nucleus.GetUserName();
        
        userNameTitle.text = userName;
        if(userPhotoTitle != null) userPhotoTitle.SetText(userName[0].ToString());

        EnableCabinet();
        onSigning?.Invoke();

        TimeCounter.instance.SaveEntry();
    }
    
    private void EnableCabinet()
    {
        entryMenu.SetActive(false);
        
        cabinetMenu.SetActive(true);
    }

    private void EnableEntry()
    {
        cabinetMenu.SetActive(false);

        entryMenu.SetActive(true);
    }

    private void CheckUserLogin()
    {
        if (Nucleus.currentUserId == -1) return;
        
        SignIn();
    }
}