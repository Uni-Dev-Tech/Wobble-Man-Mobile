using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float resX, resZ; // для хранения Input с клавиатуры
    [Header("Скорость движения ГГ")][SerializeField] private float speed;
    [Header("Время через которое появится игрок(секунды)")] [SerializeField] private float timeAppear;
    [SerializeField] private MeshRenderer body, head, cap_1, cap_2;
    private Rigidbody rb;
    [SerializeField] private ParticleSystem dust;
    private bool onGround;// ограничивает воспроизведение партиклов
    [HideInInspector] public bool catched; // блокирует управление после проигрыша
    private Animator animator;
    private Vector3 move;
    private void Start()
    {
        catched = false;
        onGround = false;
        StartCoroutine(PlayerAppear());
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        resX = InputTouchDetector.instance.resultY;
        resZ = InputTouchDetector.instance.resultX;
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
                move = new Vector3(-resX, 0, resZ);
        }
        if (Input.touchCount == 0)
            move = new Vector3(0, 0, 0);
        if (!catched)
        {
            transform.LookAt(transform.position + new Vector3(resZ, 0, resX)); // поворачиваем игрока в сторону движения
        }
    }
    void FixedUpdate()
    {
        if (!catched)
        {
            rb.AddForce(move * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// Активирует игрока через заданное время
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerAppear()
    {
        yield return new WaitForSeconds(timeAppear);
        body.enabled = true;
        head.enabled = true;
        cap_1.enabled = true;
        cap_2.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // единожды воспроизводит сист частиц
        if(!onGround)
            if(collision.gameObject.CompareTag("Floor"))
            {
                dust.Play();
                onGround = true;
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        //После прохождения игрока к финишу блокирует управление, передвигает игрока в центр
        //тригера, проигрывает анимацию и запускает панель выигрыша 
        if (other.CompareTag("Finish"))
        {
            catched = true;
            speed = 0;
            rb.useGravity = false;
            rb.isKinematic = true;
            transform.rotation = Quaternion.identity;
            do
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), 1f);
            } while (transform.position != new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
            animator.SetTrigger("EndLevel");
            StartCoroutine(UIManager.instance.SelectUI(UIManager.instance.UIPanels[1]));
        }
    }
}
