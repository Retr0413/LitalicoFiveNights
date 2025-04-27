using UnityEngine;

public interface IWallBlocker
{
    bool Block { get; set; }
    void ToggleBlock();
}
