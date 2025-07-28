using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUi : MonoBehaviour
{
   


    public void playGame()
    {
        SceneManager.LoadSceneAsync(0);
    }




    public void QuitGame()
    {
        Application.Quit();
    } 
}
