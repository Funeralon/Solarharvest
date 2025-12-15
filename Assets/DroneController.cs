using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("---- PARAMÈTRES DE VOL ----")]
    public float speed = 10f;
    public float flightHeight = 2f;
    public float hoverForce = 5f;

    [Header("---- JARDINAGE AVANCÉ (Tableaux) ----")]
    // On remplace la variable unique par une liste (Array)
    public GameObject[] grainesPrefabs;
    // Le score qu'il faut avoir pour utiliser la graine correspondante
    public int[] scoresPourDebloquer;

    [Header("---- OUTILS ----")]
    public ParticleSystem eauParticules;

    [Header("---- AUDIO ----")]
    public AudioClip sonPlantation;
    public AudioClip sonRecolte;
    public AudioClip sonErreur; // Petit son si on n'a pas le niveau (optionnel)
    private AudioSource audioSource;

    // Variables internes
    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isWatering = false;
    private int indexGraineActuelle = 0; // 0 = Graine de base

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // --- INPUTS ---

        // Mouvement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Planter
        controls.Player.Plant.performed += ctx => PlantSeed();

        // Arroser
        controls.Player.Water.performed += ctx => StartWatering();
        controls.Player.Water.canceled += ctx => StopWatering();

        // CHANGER DE GRAINE (NOUVEAU)
        // On appuie sur TAB pour passer à la suivante
        controls.Player.SwitchSeed.performed += ctx => ChangerGraine();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        // Rotation visuelle
        if (moveInput != Vector2.zero)
        {
            Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);
        }

        // Arrosage continu
        if (isWatering)
        {
            DetecterPlantesSousLeDrone();
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyHover();
    }

    // --- LOGIQUE PHYSIQUE ---

    void ApplyMovement()
    {
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * speed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void ApplyHover()
    {
        float heightError = flightHeight - transform.position.y;
        rb.AddForce(Vector3.up * heightError * hoverForce, ForceMode.Acceleration);
    }

    // --- NOUVELLE LOGIQUE DE CHOIX DE GRAINE ---

    void ChangerGraine()
    {
        // On calcule quel serait le prochain index
        int prochainIndex = indexGraineActuelle + 1;

        // Si on dépasse la fin de la liste, on revient à 0
        if (prochainIndex >= grainesPrefabs.Length)
        {
            prochainIndex = 0;
        }

        // VÉRIFICATION DU SCORE
        // On vérifie si le joueur a assez de points
        if (GameManager.instance.scoreActuel >= scoresPourDebloquer[prochainIndex])
        {
            // C'est bon, on change !
            indexGraineActuelle = prochainIndex;
            Debug.Log("Graine équipée : " + grainesPrefabs[indexGraineActuelle].name);
        }
        else
        {
            // Pas assez de points ! On revient à la graine de base (0) ou on reste sur l'actuelle
            Debug.Log("Niveau insuffisant ! Score requis : " + scoresPourDebloquer[prochainIndex]);

            // Si on essayait de passer à une graine bloquée, on revient à la graine 0 par sécurité
            indexGraineActuelle = 0;

            if (audioSource && sonErreur) audioSource.PlayOneShot(sonErreur);
        }
    }

    // --- ACTIONS JARDINAGE ---

    void PlantSeed()
    {
        // Sécurité : Vérifier si la liste n'est pas vide
        if (grainesPrefabs.Length == 0) return;

        Vector3 solPosition = new Vector3(transform.position.x, 0.15f, transform.position.z);

        // ON UTILISE L'INDEX ACTUEL POUR CHOISIR LE PRÉFAB
        Instantiate(grainesPrefabs[indexGraineActuelle], solPosition, Quaternion.identity);

        if (audioSource && sonPlantation) audioSource.PlayOneShot(sonPlantation);
    }

    void StartWatering()
    {
        isWatering = true;
        if (eauParticules != null) eauParticules.Play();
    }

    void StopWatering()
    {
        isWatering = false;
        if (eauParticules != null) eauParticules.Stop();
    }

    void DetecterPlantesSousLeDrone()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            PlantBehavior plante = hit.collider.GetComponent<PlantBehavior>();
            if (plante != null) plante.Arroser();
        }
    }

    // --- RÉCOLTE ---

    private void OnTriggerEnter(Collider other)
    {
        PlantBehavior plante = other.GetComponent<PlantBehavior>();

        if (plante != null)
        {
            plante.Recolter();
            // Petit fix : On joue le son seulement si la plante était mûre
            if (audioSource && sonRecolte && plante.estMur)
                audioSource.PlayOneShot(sonRecolte);
        }
    }
}