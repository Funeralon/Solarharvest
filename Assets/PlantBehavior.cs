using UnityEngine;
using System.Collections;

public class PlantBehavior : MonoBehaviour
{
    [Header("---- STATISTIQUES ----")]
    public int pointsDonnes = 1;
    public float dureeCroissance = 3f;
    public float dureeAvantPourrissement = 10f;

    [Header("---- RÉGLAGES ARROSAGE ----")]
    public float vitesseArrosage = 5f;

    [Header("---- VISUELS ----")]
    public GameObject visuelGraine;
    public GameObject visuelLegume;
    public GameObject explosionParticles;

    [HideInInspector] public bool estMur = false;
    private float tempsRestant; // Compteur interne

    void Start()
    {
        if (visuelGraine != null) visuelGraine.SetActive(true);
        if (visuelLegume != null) visuelLegume.SetActive(false);

        // On initialise le compteur
        tempsRestant = dureeCroissance;

        StartCoroutine(CycleDeVie());
    }

    IEnumerator CycleDeVie()
    {
        // PHASE 1 : Croissance Dynamique
        // Tant qu'il reste du temps, on attend
        while (tempsRestant > 0)
        {
            // On retire le temps écoulé normalement (1 seconde par seconde)
            tempsRestant -= Time.deltaTime;

            // On attend la prochaine image avant de recommencer la boucle
            yield return null;
        }

        // Le temps est écoulé -> Transformation !
        DevenirMur();

        // PHASE 2 : Attente avant pourrissement
        yield return new WaitForSeconds(dureeAvantPourrissement);

        // PHASE 3 : Trop tard !
        Exploser();
    }

    void DevenirMur()
    {
        estMur = true;
        if (visuelGraine != null) visuelGraine.SetActive(false);
        if (visuelLegume != null) visuelLegume.SetActive(true);
    }

    // Cette fonction est appelée 60 fois par seconde quand tu arroses
    public void Arroser()
    {
        if (!estMur)
        {
            // On retire du temps supplémentaire !
            // Exemple : Si vitesseArrosage = 5, on retire 5x le temps normal
            tempsRestant -= Time.deltaTime * vitesseArrosage;
        }
    }

    public void Recolter()
    {
        if (estMur)
        {
            if (GameManager.instance != null) GameManager.instance.AjouterPoints(pointsDonnes);
            Destroy(gameObject);
        }
    }

    void Exploser()
    {
        if (explosionParticles != null) Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}