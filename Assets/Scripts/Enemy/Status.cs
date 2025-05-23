using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Status : MonoBehaviour
{
    [Header("체력")]
    public int maxHP = 3;
    public int currentHP;

    [Header("피격 이펙트 설정")]
    [Tooltip("빨간색으로 유지될 시간 (초)")]
    public float flashDuration = 0.1f;
    [SerializeField] private HpStarScript _starScript;
    [SerializeField] GameObject HitEffect;

    [SerializeField] float enemyClearRadius = 5f;

    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        // SpriteRenderer 컴포넌트 가져오기 및 null 검사
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"[{nameof(Status)}] SpriteRenderer 컴포넌트를 찾을 수 없습니다.");
        }
        else
        {
            originalColor = sr.color;   // 원래 색 저장
        }

        currentHP = maxHP;
    }

    /// <summary>
    /// 피해를 입힙니다.
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (_starScript != null)
        {
            _starScript.SetHpStar(currentHP);
        }
        GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);

        // SpriteRenderer가 없으면 피격 이펙트 생략
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
        // 혹시 모를 null 검사
        if (sr == null)
            yield break;

        // 빨간색으로 변경
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);

        // 원래 색으로 복원
        if (sr != null)
            sr.color = originalColor;
    }


    // 체크용
/*    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyClearRadius);
    }
*/


    // 죽는 경우 EnemyAi 있는 애들 범위 내에서 불러와서 전부 타겟 멈춤. 
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
