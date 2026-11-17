
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(LineRenderer))]
public class NpcNavChase : MonoBehaviour
{
    public Transform target;
    public float repathInterval = 0.25f;

    NavMeshAgent agent;
    LineRenderer line;
    float timer;

    NavMeshPathStatus lastStatus = NavMeshPathStatus.PathComplete;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Hedefe giden yolu belirli aralýklarla güncelle.
        timer -= Time.deltaTime;

        if (timer <= 0f && !agent.isOnOffMeshLink)
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
            }

            timer = repathInterval;
        }

        // Yolun durumu deðiþtiðinde Console'a yaz.
        if (!agent.pathPending && agent.pathStatus != lastStatus)
        {
            lastStatus = agent.pathStatus;
            Debug.Log("Yol durumu: " + lastStatus);
        }

        // Bulunan yolu çizgiyle göster.
        Vector3[] corners = agent.path.corners;

        line.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
        {
            line.SetPosition(i, corners[i] + Vector3.up * 0.2f);
        }
    }

    void OnDisable()
    {
        if (line != null)
        {
            line.positionCount = 0;
        }
    }
}

