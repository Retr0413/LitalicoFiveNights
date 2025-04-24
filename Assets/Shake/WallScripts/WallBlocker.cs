using UnityEngine;

public class WallBlocker : MonoBehaviour
{
    public bool Block = false;
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
            // ブロックする場合、上に移動
            transform.position = initialPosition + new Vector3(0, raiseHeight, 0);
        }
        else
        {
            // ブロックを解除する場合、元の位置に戻す
            transform.position = initialPosition;
        }
        Debug.Log($"[ToggleBlock] Block is now {Block}");
    }
}
