using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;

    public Tilemap walkableTilemap;
    public Tilemap brokenFloorTilemap;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 input;
    private bool isDashing;

    private int playerLayer;
    private int brokenFloorLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerLayer = LayerMask.NameToLayer("Player");
        brokenFloorLayer = LayerMask.NameToLayer("BrokenFloor");
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        if (input.x < 0) spriteRenderer.flipX = false;
        else if (input.x > 0) spriteRenderer.flipX = true;

        if (!isDashing && input != Vector2.zero && Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(Dash(input));
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Vector2 targetPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        if (IsWalkable(targetPos))
            rb.MovePosition(targetPos);
    }

    bool IsWalkable(Vector2 worldPos)
    {
        Vector3Int cellPos = walkableTilemap.WorldToCell(worldPos);
        return walkableTilemap.HasTile(cellPos);
    }

    IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        float elapsed = 0f;
        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + direction * dashDistance;

        Physics2D.IgnoreLayerCollision(playerLayer, brokenFloorLayer, true);

        while (elapsed < dashDuration)
        {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, elapsed / dashDuration);
            rb.MovePosition(newPos);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(endPos);
        Physics2D.IgnoreLayerCollision(playerLayer, brokenFloorLayer, false);
        isDashing = false;
    }
}
