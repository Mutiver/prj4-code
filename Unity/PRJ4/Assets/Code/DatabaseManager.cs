using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private string userID;
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    private List<string> highscoreList = new List<string>();
    public TMP_InputField Email;
    public TMP_InputField Password;
    public TMP_Text Username;
    public TMP_Text UserLoggedIn; 
    public int highScore { get; set; }
    private bool isLoggedIn = false;
    public GameObject MainMenu;
    public GameObject LoggedInScreen;
    public GameObject NotLoggedInScreen;
    
    public static DatabaseManager Instance { get; private set; }

    public DependencyStatus status;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        //dbReference = FirebaseDatabase
        // .GetInstance(
        //  "https://console.firebase.google.com/project/prj4-57b30/database/prj4-57b30-default-rtdb/data/~2F")
        // .RootReference;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            status = task.Result;

            if (status == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("could not resolve all firebase dependencies: " + status);
            }

        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        //AuthStateChanged(this, null);
    }


    public void CreateUserWithAuth()
    {
        var tmpEmail = Email.text;
        var tmpPassword = Password.text;

        if (auth == null)
        {
            Debug.LogError("auth is null");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(tmpEmail, tmpPassword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void CheckLoggedIn()
    {
        MainMenu.SetActive(false);

        if (isLoggedIn == true)
        {
            LoggedInScreen.SetActive(true);
        }
        else
        {
            NotLoggedInScreen.SetActive(true);
        }

    }

    public void Login()
    {
        var tmpEmail = Email.text;
        var tmpPassword = Password.text;

        auth.SignInWithEmailAndPasswordAsync(tmpEmail, tmpPassword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            isLoggedIn = true;
        });
    }

    public void LogOut()
    {

        auth.SignOut();
        isLoggedIn = false;
        Username.text = "Guest";
        UserLoggedIn.text = "Guest";
        LoggedInScreen.SetActive(false);
        MainMenu.SetActive(true);

    }

    public void SetHighscore()
    {
        string userEmail = auth.CurrentUser.Email;

        // Function to remove the second half of the email after the '@' sign
        string EncodeEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            if (atIndex >= 0)
            {
                email = email.Substring(0, atIndex);
            }
            return email;
        }

        string encodedEmail = EncodeEmail(userEmail);

        DatabaseReference highscoreRef = dbReference.Child("highscores").Child(encodedEmail);
        highscoreRef.Child("score").SetValueAsync(highScore).ContinueWith(task =>
        {
            //error handle
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("failed to save highscore: " + task.Exception.Message);
            }
            else
            {
                Debug.Log("highscore saved successfully");
            }
        });
    }


    public void GetHighscore(Action<string> callback)
    {
        Debug.Log("gethighscore called");

        DatabaseReference highScoreRef = dbReference.Child("highscores");

        highScoreRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error getting data");
                return;
            }
            StringBuilder highScoreText = new StringBuilder();
            // Get the data snapshot
            DataSnapshot snapshot = task.Result;

            // Loop through the snapshot and add each item to a list
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                // Get the score for the current user
                long score = (long)childSnapshot.Child("score").Value;

                // Get the encoded email (with the part after the '@' removed)
                string encodedEmail = childSnapshot.Key;

                // Format the data and append it to the highScoreText StringBuilder

                string data = string.Format("{0, -12} {1, 6}<br>", encodedEmail, score);
                highScoreText.Append(data);
                Debug.Log($"{data} in databaseManager");
                Debug.Log($"Email: {encodedEmail}");

            }
            // Call the callback function with the list of data
            callback(highScoreText.ToString());
        });
    }

    void Update()
    {
        if (isLoggedIn == true)
        {
            Username.text = Email.text;
            UserLoggedIn.text = Username.text;
        }
    }
}
