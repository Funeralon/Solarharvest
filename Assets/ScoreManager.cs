using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Permet d'appeler ce script depuis n'importe où
    public TextMeshProUGUI textScore;    // La case pour glisser ton texte

    private int score = 0;

    void Awake()
    {
        instance = this; 
    }

    public void AjouterPoint()
    {
        score++; // +1 point
        textScore.text = "Légumes : " + score; // Met à jour l'écran
    }
    public void CheckHighScore()
    {
        // On récupère l'ancien record (0 par défaut)
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

        // Si le score actuel est plus grand que le record
        if (score > currentHighScore)
        {
            // On sauvegarde le nouveau record
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }
}

