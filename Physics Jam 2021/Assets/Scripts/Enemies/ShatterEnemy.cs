using System.Collections;
using UnityEngine;

public class ShatterEnemy : Enemy
{
    [SerializeField] ShatterProjectile ProjectilePrefab;

    public override EnemyType EnemyType => EnemyType.Shatter;

    protected override bool AffectedByGravitySwitch => false;

    protected bool IsExploding { get; set; } = false;

    private bool _isMoving = true;

    private void Update()
    {
        if (IsGrounded && rb.gravityScale == transform.localScale.y && !IsExploding)
        {
            if (_isMoving)
            {
                SpriteRenderer.sprite = NormalImage;
                MoveController.Move(transform, Direction, 2);
            }
            else
            {
                SpriteRenderer.sprite = WalkImage;
            }
        }
        else
        {
            SpriteRenderer.sprite = FrownImage;
        }
    }

    protected override void Metronome_OnTick()
    {
        if (gameObject.activeSelf)
            StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _isMoving = true;

        yield return new WaitForSeconds(0.2f);

        _isMoving = false;
    }

    protected override void SetAsInUseInternal()
    {
        base.SetAsInUseInternal();

        _isMoving = false;
    }

    protected override void OnTouchedGround()
    {
        IsExploding = true;

        Explode();
    }

    protected void Explode()
    {
        if (IsDead)
            return;

        var projectilePrefab = Instantiate(ProjectilePrefab, transform.parent);
        projectilePrefab.transform.position = transform.position;
        projectilePrefab.Direction = 1;

        projectilePrefab = Instantiate(ProjectilePrefab, transform.parent);
        projectilePrefab.transform.position = transform.position;
        projectilePrefab.Direction = -1;

        Kill();
    }


}