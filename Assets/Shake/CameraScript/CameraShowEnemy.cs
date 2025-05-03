using UnityEngine;

public class CameraShowEnemy : MonoBehaviour
{
    public static CameraShowEnemy instance; // Singleton instance
    public bool isEnemyVisible = false; // Enemy visibility flag

    public float maxDistance = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                isEnemyVisible = true;
                Debug.Log("Enemy is visible");
            }
            else
            {
                isEnemyVisible = false;
            }
        }
        else
        {
            isEnemyVisible = false;
        }
    }

}
