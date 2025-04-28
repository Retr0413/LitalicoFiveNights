using UnityEngine;

public class DoorMove : MonoBehaviour, IDoor
{
    private bool _lock = false;

    public bool Lock
    {
        get => _lock;
        set => _lock = value;
    }
}
