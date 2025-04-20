using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Boss : MonoBehaviour, IHit
{
    public int maxLife;
    public Slider hpBar;
    public Vector2 attackInterval;
    public GameObject bulletPrefab;
    public int attackCount;
    public float intervalAngle;
    public Transform attackTr;
    public static Boss instance;

    private int _nowLife;
    private Animator _animator;
    private float _nextAttackTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _nowLife = maxLife;
        hpBar.value = 1;
        _animator = GetComponent<Animator>();
        _nextAttackTime = Time.time + Random.Range(attackInterval.x, attackInterval.y);
    }

    private void Update()
    {
        if (_nowLife <= 0)
        {
            return;
        }
        
        if (Time.time > _nextAttackTime)
        {
            _animator.SetTrigger("Fire");
            _nextAttackTime = Time.time + Random.Range(attackInterval.x, attackInterval.y);
        }
    }

    public void OnAttack()
    {
        Vector3 baseDir = Player.instance.transform.position - attackTr.position;
        baseDir.Normalize();
        
        float startAngle = intervalAngle * (int)(attackCount / 2) * -1;
        for (int j = 0; j < attackCount; j++)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, attackTr.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.moveDir = Quaternion.Euler(0,0, startAngle + intervalAngle * j) * baseDir;
        }
    }

    public void Hit(int i, Vector2 dir)
    {
        if (_nowLife <= 0)
        {
            return;
        }
        
        _nowLife -= i;
        if (_nowLife <= 0)
        {
            _nowLife = 0;
            GameManager.instance.GameVictory();
        }

        hpBar.value = (float)_nowLife / maxLife;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_nowLife <= 0)
        {
            return;
        }
        if (col.transform.CompareTag("Player"))
        {
            
            IHit hit = col.gameObject.GetComponent<IHit>();
            if (hit != null)
            {
                hit.Hit(1, col.transform.position - transform.position);
            }
            
        }
    }
}
