using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IHit
{
    public float moveSpeed;
    public Transform footTr;
    public float checkR;
    public LayerMask groundLayer;
    public float jumpForce;
    public float hitForce;
    public RectTransform heartParentTr;
    public GameObject heartPrefab;
    public int MaxLife;

    [HideInInspector] 
    public bool canAttack = true;
    [HideInInspector]
    public float minX;
    [HideInInspector]
    public float maxX;
    [HideInInspector] 
    public bool LimitRightX = false;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private bool _isGround;
    private int _nowLife;
    private List<Image> _heartLis;
    private bool _isLife = true;
    public static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        _heartLis = new List<Image>();
        SetHeart(MaxLife);
    }

    public void Update()
    {
        _isGround = Physics2D.OverlapCircle(footTr.position, checkR, groundLayer);
        
        if (!_isLife)
        {
            return;
        }
        
        PlayerMove();
        PlayerJump();
        PlayerAttack();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetHeart(1);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetHeart(-1);
        }
    }

    private void LateUpdate()
    {
        if (transform.position.x < minX)
        {
            var position = transform.position;
            position = new Vector3(minX, position.y, 0);
            transform.position = position;
        }

        if (LimitRightX)
        {
            if (transform.position.x > maxX)
            {
                var position = transform.position;
                position = new Vector3(maxX, position.y, 0);
                transform.position = position;
            }
        }

        if (transform.position.y < -20)
        {
            Vector3 pos = transform.position;
            pos.y = 5;
            transform.position = pos;
            _rigidbody2D.velocity = Vector2.zero;
            SetHeart(-1);
        }
    }

    private void PlayerMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }else if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        _animator.SetBool("Move", Mathf.Abs(horizontal) > 0);
        
        if (canAttack)
        {
            // _rigidbody2D.velocity = new Vector2(horizontal * moveSpeed, _rigidbody2D.velocity.y);
            transform.position += horizontal * moveSpeed * Time.deltaTime * Vector3.right;
        }
        
    }

    private void PlayerJump()
    {
        _animator.SetFloat("Accelerated", _rigidbody2D.velocity.y);

        if (_isGround && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            _rigidbody2D.velocity = Vector2.up * jumpForce + new Vector2(_rigidbody2D.velocity.x, 0);
        }
    }

    private void PlayerAttack()
    {
        if (canAttack && Input.GetKeyDown(KeyCode.Mouse0)||Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            _animator.SetTrigger("Attack");
        }
    }

    public void SetHeart(int count)
    {
        if (!_isLife)
        {
            return;
        }
        
        _nowLife += count;
        if (_nowLife <= 0)
        {
            _nowLife = 0;
            _isLife = false;
            _animator.SetTrigger("Die");
            StartCoroutine(GameOver());
        }
        MaxLife = _nowLife > MaxLife ? _nowLife : MaxLife;
        for (int i = _heartLis.Count; i < MaxLife; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartParentTr);
            _heartLis.Add(heartObj.GetComponent<Image>());
        }

        for (int i = 0; i < MaxLife; i++)
        {
            _heartLis[i].color = i < _nowLife ? Color.white : Color.black;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.GameOver();
    }

    public void Hit(int i, Vector2 dir)
    {
        if (!_isLife)
        {
            return;
        }
        
        SetHeart(i * -1);
        _animator.SetTrigger("Hit");
        if (dir.sqrMagnitude > 0)
        {
            dir.Normalize();
        }
        _rigidbody2D.AddForce( dir * hitForce);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(footTr.position, checkR);
    }
}
