using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour changer de scène
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI highScoreText;
    public GameObject panelOptions;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonBouton;

    void Start()
    {
        // Au démarrage, on va chercher le meilleur score
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Meilleur Score : " + bestScore;

        // On s'assure que le panel options est fermé
        if (panelOptions != null) panelOptions.SetActive(false);
    }

    public void PlayGame()
    {
        JouerSon();
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        JouerSon();
        Debug.Log("Quitter le jeu !"); 
        Application.Quit();
    }

    public void OuvrirOptions()
    {
        JouerSon();
        panelOptions.SetActive(true); // Affiche les options
    }

    public void FermerOptions()
    {
        JouerSon();
        panelOptions.SetActive(false); // Cache les options
    }

    void JouerSon()
    {
        if (audioSource && sonBouton)
        {
            audioSource.PlayOneShot(sonBouton);
        }
    }
}