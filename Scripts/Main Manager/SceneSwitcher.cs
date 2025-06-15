using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadUserScene()
    {
        SceneManager.LoadScene("User");  // Ensure "User" is the exact name of your User scene
    }
}

