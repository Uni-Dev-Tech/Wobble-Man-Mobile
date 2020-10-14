using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Длина выпускаемых лучей")] [SerializeField] private float maxDistance; 

    [Header("Точки передвиджения")] [SerializeField] private Transform[] points;
    [Header("Скорость передвижения")] [SerializeField] private float speed;
    [Header("True - движение циклично, False - движение последовательно")] [SerializeField] private bool cycle;

    private Animator animator;
    private Transform targetPoint; // хранит точку в которую передвигается объект
    private int currentPoint; // индекс массива точек
    private bool forward; // если движемся не циклично, определяет движимся мы вперед или назад
    void Start()
    {
        animator = GetComponent<Animator>();
        forward = true; // задаем движение вперед
        currentPoint = 0; // начинаем с движения в первую точку
        targetPoint = points[currentPoint]; // определяем цель движения 
        transform.LookAt(targetPoint, Vector3.up); // поворачиваем объект в сторону движения
        StartCoroutine(PlayerDetection(maxDistance)); // запускаем корутину "зоны видимости"
    }
    void Update()
    { 
        // по достижению точки 
        if(transform.position == targetPoint.position)
        {
            // переходим к следующей точке, при последовательном движении назад возвращаемся к предыдущей
            if (forward)
                currentPoint++;
            else
                currentPoint--;
            // если дошли до последней точки цикла то возвращаемся к первой
            if (currentPoint >= points.Length && cycle)
                currentPoint = 0;
            // если движение последовательно идем назад по массиву точек
            else if (currentPoint >= points.Length && !cycle)
            {
                forward = false;
                currentPoint = points.Length - 2;
            }
            // если движение последовательно идем снова вперед по массиву точек
            else if(currentPoint < 0)
            {
                forward = true;
                currentPoint = 1;
            }
            targetPoint = points[currentPoint]; // задаем новую точку для движения
            transform.LookAt(targetPoint, Vector3.up); // поворачиваем объект сторону точки(движения)
        }
        //двигаемся в сторону точки
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
    }
    /// <summary>
    /// Создает зону видимости ГГ через выпускание лучей
    /// </summary>
    /// <param name="maxDistance">Длина зоны видимости</param>
    /// <returns></returns>
    IEnumerator PlayerDetection(float maxDistance)
    {
        // запуск цикла параллельного к Update
        do
        {
            yield return null;
            Ray[] ray = new Ray[7]; // массив лучей который образует зону видимости противника
            ray[0] = new Ray(transform.position, transform.forward); // центральная линия
            ray[1] = new Ray(transform.position, (transform.forward - transform.right)); // боковая линия (слева, если смотреть от лица противника)
            ray[2] = new Ray(transform.position, (transform.forward + transform.right)); // боковая линия (справа, если смотреть от лица противника)
            ray[3] = new Ray(transform.position, (transform.forward - transform.right + transform.right / 3f)); // первая линия от боковой(слева, если смотреть от лица противника)
            ray[5] = new Ray(transform.position, (transform.forward - transform.right + (transform.right / 3f) * 2f)); // первая линия от центральной(слева, если смотреть от лица противника)
            ray[4] = new Ray(transform.position, (transform.forward + transform.right - transform.right / 3f)); // первая линия от боковой(справа, если смотреть от лица противника)
            ray[6] = new Ray(transform.position, (transform.forward + transform.right - (transform.right / 3f) * 2f)); // первая линия от центральной(справа, если смотреть от лица противника)
            RaycastHit hit;
            //цикл проверки каждого луча на столкновение с объектами
            for (int i = 0; i < ray.Length; i++)
            {
                // длина каждого луча
                switch(i)
                {
                    case 0:
                        maxDistance = 2.85f;
                        break;
                    case 1:
                        maxDistance = 2.97f;
                        break;
                    case 3:
                        maxDistance = 2.886f;
                        break;
                    case 5:
                        maxDistance = 2.85f;
                        break;
                }
                // проверка на попадание луча в ГГ
                if (Physics.Raycast((ray[i]), out hit, maxDistance))
                {
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        animator.SetTrigger("Detect");
                        PlayerController player = hit.transform.gameObject.GetComponent<PlayerController>();
                        player.catched = true;
                        speed = 0;
                        StartCoroutine(UIManager.instance.SelectUI(UIManager.instance.UIPanels[0]));
                    }
                }
            }
            maxDistance = 3f;
        } while (true);
    }
}
