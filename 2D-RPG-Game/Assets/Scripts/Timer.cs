using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public static float DashTimer;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DashTimer += Time.deltaTime;
    }
    public static void ResetDashTimer()
    {
        DashTimer = 0;
    }
    
}
