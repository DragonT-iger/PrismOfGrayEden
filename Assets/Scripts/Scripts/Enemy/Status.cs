using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Status : MonoBehaviour
{
    [Header("ü��")]
    public int maxHP = 3;
    public int currentHP;

    [Header("�ǰ� ����Ʈ ����")]
    [Tooltip("���������� ������ �ð� (��)")]
    public float flashDuration = 0.1f;
    [SerializeField] private HpStarScript _starScript;
    [SerializeField] GameObject HitEffect;

    [SerializeField] float enemyClearRadius = 5f;

    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        // SpriteRenderer ������Ʈ �������� �� null �˻�
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"[{nameof(Status)}] SpriteRenderer ������Ʈ�� ã�� �� �����ϴ�.");
        }
        else
        {
            originalColor = sr.color;   // ���� �� ����
        }

        currentHP = maxHP;
    }

    /// <summary>
    /// ���ظ� �����ϴ�.
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (_starScript != null)
        {
            _starScript.SetHpStar(currentHP);
        }
        GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);

        // SpriteRenderer�� ������ �ǰ� ����Ʈ ����
        if (sr != null)
        {
            StartCoroutine(FlashDamageEffect());
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        // Ȥ�� �� null �˻�
        if (sr == null)
            yield break;

        // ���������� ����
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);

        // ���� ������ ����
        if (sr != null)
            sr.color = originalColor;
    }


    // üũ��
/*    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyClearRadius);
    }
*/


    // �״� ��� EnemyAi �ִ� �ֵ� ���� ������ �ҷ��ͼ� ���� Ÿ�� ����. 
    private void Die()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyClearRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyAI>(out var enemy))
            {
                enemy.NotifyTargetDied(this);
            }
        }

        if (tag == "Player")
        {
            PlayerMovement player = GetComponent<PlayerMovement>();
            SpriteDissolve sp = GetComponent<SpriteDissolve>();
            sp.StartDissolve();

            player.rb.bodyType = RigidbodyType2D.Static;
            player.rb.constraints = RigidbodyConstraints2D.FreezePosition;

            player.BlockInput();
        }
        
    }
}
