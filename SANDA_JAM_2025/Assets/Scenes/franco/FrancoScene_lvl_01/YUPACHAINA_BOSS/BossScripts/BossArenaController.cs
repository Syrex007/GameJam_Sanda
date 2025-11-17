using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BossArenaController : MonoBehaviour
{
    [Header("Referencia a la Arena")]
    public CombatArenaGenerator arenaGenerator;

    [Header("Movimiento de Salto / Reposicionamiento")]
    public float jumpHeight = 6f;
    public float jumpDuration = 1.2f;
    public float fallDuration = 0.8f;
    public float jumpOffsetY = 5f;

    [Header("Onda Expansiva")]
    public float maxWaveScale = 10f;
    public float waveExpandTime = 1.5f;
    public float currentWaveRadius = 0f;
    public float waveThickness = 0.5f;
    public LayerMask playerMask;
    public float waveDamage = 20f;
    private bool waveActive = false;

    [Header("State Machine")]
    public BossState currentState = BossState.Idle;
    public float stateDuration = 3f;
    private float stateTimer;

    private bool isJumping = false;

    private void Start()
    {
        if (arenaGenerator == null)
        {
            Debug.LogError("Falta asignar el CombatArenaGenerator en el BossArenaController.");
            enabled = false;
            return;
        }

        stateTimer = stateDuration;
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f && !isJumping)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    EndIdleState();
                    break;
                case BossState.Attack:
                    EndAttackState();
                    break;
            }
            stateTimer = stateDuration;
        }
    }

    private Vector3 GetRandomAnchor()
    {
        if (arenaGenerator == null || arenaGenerator.anchorPositions.Count == 0)
        {
            Debug.LogWarning("No hay anchors generados en la arena.");
            return transform.position;
        }

        return arenaGenerator.anchorPositions[Random.Range(0, arenaGenerator.anchorPositions.Count)];
    }

    private void JumpToRandomAnchor()
    {
        if (isJumping) return;
        isJumping = true;

        Vector3 startPos = transform.position;
        Vector3 targetAnchor = GetRandomAnchor();
        Vector3 tpPosition = new Vector3(targetAnchor.x, targetAnchor.y + jumpOffsetY, targetAnchor.z);

        Sequence seq = DOTween.Sequence();

        // Subir (simula salto hacia arriba)
        seq.Append(transform.DOMoveY(startPos.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad));

        // Teletransportarse arriba del anchor
        seq.AppendCallback(() =>
        {
            transform.position = tpPosition;
        });

        // Caer directamente sobre el anchor
        seq.Append(transform.DOMoveY(targetAnchor.y, fallDuration).SetEase(Ease.InQuad));

        seq.OnComplete(() =>
        {
            isJumping = false;
            TriggerWave();
            StartAttackState();
        });
    }

    private void TriggerWave()
    {
        if (waveActive) return;
        waveActive = true;
        currentWaveRadius = 0f;
        float damageActive = waveDamage;

        DOTween.To(() => currentWaveRadius, x => currentWaveRadius = x, maxWaveScale / 2f, waveExpandTime)
        .OnUpdate(() =>
        {
            float innerRadius = currentWaveRadius - waveThickness;
            float outerRadius = currentWaveRadius;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, outerRadius, playerMask);
            foreach (var h in hits)
            {
                float dist = Vector2.Distance(h.transform.position, transform.position);
                if (dist <= outerRadius && dist >= innerRadius)
                {
                    Debug.Log($"Jugador golpeado por el borde de la onda expansiva: {h.name}");
                }
            }
        })
        .OnComplete(() =>
        {
            currentWaveRadius = 0f;
            waveActive = false;
            damageActive = 0f;
        });
    }

    private void EndIdleState()
    {
        JumpToRandomAnchor();
        currentState = BossState.Attack;
    }

    private void EndAttackState()
    {
        JumpToRandomAnchor();
        currentState = BossState.Idle;
    }

    private void StartAttackState()
    {
        Debug.Log("Boss inicia su estado de ATAQUE tras aterrizar.");
    }

    public enum BossState
    {
        Idle,
        Attack
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentWaveRadius);
        }
    }
}
