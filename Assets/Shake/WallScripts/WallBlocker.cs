using UnityEngine;

public class WallBlocker : MonoBehaviour, IWallBlocker
{
    public bool Block { get; set; } = false;
    public float raiseHeight = 5f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void ToggleBlock()
    {
        Block = !Block;

        if (Block)
        {
            transform.position = initialPosition + new Vector3(0, raiseHeight, 0);
        }
        else
        {
            transform.position = initialPosition;
        }
        Debug.Log($"[ToggleBlock] Block is now {Block}");
    }
}
