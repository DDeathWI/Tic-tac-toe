using UnityEngine;
using UnityEngine.Networking;

public class Field : NetworkBehaviour
{
    // Field Index from 0 to 9
    // 0 1 2
    // 3 4 5
    // 6 7 8
    [SyncVar] public int Index;

    // player turn X or O
    [SyncVar (hook = "ChangeOwner")] public int Owner;

    // Keep Sprites X and O
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D spriteCollider;
    
    private void Awake()
    {
        spriteCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Player Click this Field
    /// </summary>
    /// <param name="playerIndex"></param>

    public void Click(int playerIndex)
    {
        Debug.LogError("Click" + playerIndex +" "+ Owner);

        Owner = playerIndex;

        // disable Collider when player Click this field isNotEmpty
        spriteCollider.enabled = false;

    }

    void ChangeOwner(int _newOwner)
    {
        // Change sprite to Owner
        spriteRenderer.sprite = sprites[_newOwner];
    }

}
