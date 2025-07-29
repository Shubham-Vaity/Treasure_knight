using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainUi : MonoBehaviour
{


    public GameObject mainUi;
    public GameObject CutUI;

    public TextMeshProUGUI textComponent;
    public float typeSpeed = 0.05f;
    public string fullText;

    public void PlayButton()
    {
        mainUi.SetActive(false);
        CutUI.SetActive(true);
        StartCoroutine(TypeText());
    }


    IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        StartCoroutine(playGame());
    }

    


     IEnumerator playGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(1);
    }




    public void QuitGame()
    {
        Application.Quit();
    } 
}
