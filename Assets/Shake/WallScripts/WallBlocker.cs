using UnityEngine;

public class WallBlocker : MonoBehaviour, IWallBlocker
{
    public bool Block { get; set; } = false;

    private Vector3 initialPosition;

    private void Start()
    {
        gameObject.SetActive(false);
        initialPosition = transform.position;
    }

    public void ToggleBlock()
    {
        Block = !Block;

        if (Block)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        Debug.Log($"[ToggleBlock] Block is now {Block}");
    }
}
