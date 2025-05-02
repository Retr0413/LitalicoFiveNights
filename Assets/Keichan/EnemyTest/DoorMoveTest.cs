using UnityEngine;

public class DoorMoveTest : MonoBehaviour, IDoor
{
    private bool _lock = false;

    public bool Lock
    {
        get => _lock;
        set => _lock = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Lock = true; // Toggle the lock state
            Debug.Log("Door lock state: " + Lock);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Lock = false; // Toggle the lock state
            Debug.Log("Door lock state: " + Lock);
        }
    }
}
