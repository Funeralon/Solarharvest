using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 2f;
    public float limiteX = 20f; // Point où le nuage disparait
    public float departX = -20f; // Point où il réapparaît

    void Update()
    {
        // Avance vers la droite
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Si on dépasse la limite à droite, on se téléporte à gauche (boucle)
        if (transform.position.x > limiteX)
        {
            transform.position = new Vector3(departX, transform.position.y, transform.position.z);
        }
    }
}