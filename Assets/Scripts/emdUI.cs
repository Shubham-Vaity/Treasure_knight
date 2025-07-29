using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class emdUI : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public float typeSpeed = 0.05f;
    public string fullText;


    private void Start()
    {
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
        SceneManager.LoadSceneAsync(0);
    }

}
