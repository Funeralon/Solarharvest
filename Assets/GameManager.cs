using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("---- UI JEU ----")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("---- MENUS ----")]
    public GameObject pauseMenuPanel;
    public GameObject ecranFinPanel;
    public TextMeshProUGUI scoreFinalText;

    [Header("---- AUDIO ----")]
    public AudioSource musiqueDeFond; 
    public AudioClip musiqueFin;      

    [Header("---- PARAMÈTRES ----")]
    public float tempsDeJeu = 300f;

    [HideInInspector] public int scoreActuel = 0;
    private bool jeuEstFini = false;
    private bool estEnPause = false;
    private PlayerControls controls;

    void Awake()
    {
        instance = this;
        controls = new PlayerControls();
        controls.Player.Pause.performed += ctx => TogglePause();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Start()
    {
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (ecranFinPanel) ecranFinPanel.SetActive(false);

        Time.timeScale = 1;
        UpdateUI();
    }

    void Update()
    {
        if (jeuEstFini) return;

        if (tempsDeJeu > 0)
        {
            tempsDeJeu -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            FinDuJeu();
        }
    }

    public void TogglePause()
    {
        if (jeuEstFini) return;

        estEnPause = !estEnPause;

        if (estEnPause)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            // Optionnel : Baisser le volume de la musique pendant la pause
            if (musiqueDeFond) musiqueDeFond.volume = 0.2f;
        }
        else
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1;
            // Remettre le volume normal
            if (musiqueDeFond) musiqueDeFond.volume = 0.5f;
        }
    }

    public void Reprendre() => TogglePause();

    public void RelancerPartie()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetourMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitterJeu() => Application.Quit();

    public void AjouterPoints(int points)
    {
        scoreActuel += points;
        UpdateUI();
    }

    void FinDuJeu()
    {
        jeuEstFini = true;
        tempsDeJeu = 0;

        // Sauvegarde Score
        int oldHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (scoreActuel > oldHighScore)
        {
            PlayerPrefs.SetInt("HighScore", scoreActuel);
            PlayerPrefs.Save();
        }

        // Affichage UI
        ecranFinPanel.SetActive(true);
        scoreFinalText.text = "Temps écoulé !\nScore Final : " + scoreActuel;

        // --- CHANGEMENT DE MUSIQUE  ---
        if (musiqueDeFond != null && musiqueFin != null)
        {
            musiqueDeFond.Stop();            
            musiqueDeFond.clip = musiqueFin; 
            musiqueDeFond.volume = 0.5f;     
            musiqueDeFond.loop = false;      
            musiqueDeFond.Play();            
        }

        Time.timeScale = 0;
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score : " + scoreActuel;
    }

    void UpdateTimerUI()
    {
        float minutes = Mathf.FloorToInt(tempsDeJeu / 60);
        float secondes = Mathf.FloorToInt(tempsDeJeu % 60);
        if (timerText) timerText.text = string.Format("{0:00}:{1:00}", minutes, secondes);
    }
}