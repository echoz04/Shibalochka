using UnityEngine;
using Unity.Cinemachine;

public class OrbitalRadiusController : MonoBehaviour
{
    [Header("Radius Settings")]
    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private float radiusSpeed = 5f;   
    [SerializeField] private float smoothSpeed = 10f;  

    private CinemachineOrbitalFollow orbitalFollow;
    private float targetRadius;

    void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        if (orbitalFollow == null)
        {
            Debug.LogError("Не найден компонент CinemachineOrbitalFollow на этом объекте!");
        }
        else
        {
            targetRadius = orbitalFollow.Radius;
        }
    }

    void Update()
    {
        if (orbitalFollow == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            targetRadius -= scroll * radiusSpeed;
            targetRadius = Mathf.Clamp(targetRadius, minRadius, maxRadius);
        }

        orbitalFollow.Radius = Mathf.Lerp(
            orbitalFollow.Radius,
            targetRadius,
            Time.deltaTime * smoothSpeed
        );
    }
}
