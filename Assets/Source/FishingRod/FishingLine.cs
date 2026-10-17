using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform startPoint;   // кончик удочки
    [SerializeField] private Transform endPoint;     // поплавок / крючок

    [Header("Sag")]
    [Tooltip("Максимальное провисание в метрах")]
    [SerializeField] private float maxSag = 0.5f;
    [Tooltip("Дистанция, на которой провисание достигает максимума")]
    [SerializeField] private float sagDistance = 10f;
    [Tooltip("Скорость реакции на изменение натяжения")]
    [SerializeField] private float sagSmooth = 8f;

    [Header("Resolution")]
    [SerializeField] private int segments = 20;

    [Header("Wobble")]
    [Tooltip("Амплитуда колебаний лески")]
    [SerializeField] private float wobbleAmplitude = 0.02f;
    [SerializeField] private float wobbleFrequency = 3f;

    private LineRenderer _line;
    private float _currentSag;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = segments + 1;
        _line.useWorldSpace = true;
    }

    private void LateUpdate()
    {
        if (startPoint == null || endPoint == null) return;

        Vector3 a = startPoint.position;
        Vector3 b = endPoint.position;
        
        float distance = Vector3.Distance(a, b);
        float t = Mathf.Clamp01(distance / sagDistance);
        float targetSag = maxSag * t;

        _currentSag = Mathf.Lerp(_currentSag, targetSag, sagSmooth * Time.deltaTime);

        DrawLine(a, b);
    }

    private void DrawLine(Vector3 a, Vector3 b)
    {
        Vector3 sagDir = Vector3.down;

        for (int i = 0; i <= segments; i++)
        {
            float u = i / (float)segments;
            
            Vector3 point = Vector3.Lerp(a, b, u);
            
            float sagCurve = 4f * u * (1f - u);
            point += sagDir * _currentSag * sagCurve;

           
            float wobble = Mathf.Sin(Time.time * wobbleFrequency + u * Mathf.PI * 4f)
                         * wobbleAmplitude * sagCurve;
            point += Vector3.right * wobble;

            _line.SetPosition(i, point);
        }
    }
    
    public void Yank(float force)
    {
        _currentSag = Mathf.Max(0f, _currentSag - force);
    }
}