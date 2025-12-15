using UnityEngine;

public class MenuCameraMove : MonoBehaviour
{
    public float vitesse = 2f;
    public float amplitude = 1f; // Distance du mouvement

    private Vector3 positionDepart;

    void Start()
    {
        positionDepart = transform.position;
    }

    void Update()
    {
        // Fait osciller la caméra doucement de gauche à droite
        // Mathf.Sin crée une vague régulière
        float nouveauX = positionDepart.x + Mathf.Sin(Time.time * (vitesse / 10)) * amplitude;

        transform.position = new Vector3(nouveauX, transform.position.y, transform.position.z);
    }
}