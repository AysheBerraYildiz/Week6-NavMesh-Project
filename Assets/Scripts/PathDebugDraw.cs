
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshAgent))]
public class PathDebugDraw : MonoBehaviour
{
    public NavMeshLink jumpLink;
    public float sampleStep = 0.25f;

    NavMeshAgent agent;

    float length;
    float cost;
    int cornerCount;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.pathPending || !agent.hasPath)
            return;

        Vector3[] corners = agent.path.corners;

        cornerCount = corners.Length;
        length = 0f;
        cost = 0f;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            float segmentLength =
                Vector3.Distance(corners[i], corners[i + 1]);

            length += segmentLength;

            cost += SegmentCost(
                corners[i],
                corners[i + 1],
                segmentLength
            );
        }
    }

    float SegmentCost(Vector3 a, Vector3 b, float segmentLength)
    {
        // Bu parça atlama baðlantýsý mý?
        if (IsLink(a, b))
        {
            return segmentLength * agent.GetAreaCost(jumpLink.area);
        }

        // Normal yol parçalarýný küçük adýmlarla ölç.
        int steps = Mathf.Max(
            1,
            Mathf.CeilToInt(segmentLength / sampleStep)
        );

        float stepLength = segmentLength / steps;
        float total = 0f;

        for (int s = 0; s < steps; s++)
        {
            Vector3 point = Vector3.Lerp(
                a,
                b,
                (s + 0.5f) / steps
            );

            int area = 0;

            if (NavMesh.SamplePosition(
                point,
                out NavMeshHit hit,
                0.5f,
                NavMesh.AllAreas))
            {
                area = AreaIndex(hit.mask);
            }

            total += stepLength * agent.GetAreaCost(area);
        }

        return total;
    }

    bool IsLink(Vector3 a, Vector3 b)
    {
        if (jumpLink == null || !jumpLink.activated)
            return false;

        Vector3 start =
            jumpLink.transform.TransformPoint(jumpLink.startPoint);

        Vector3 end =
            jumpLink.transform.TransformPoint(jumpLink.endPoint);

        return
            (Near(a, start) && Near(b, end)) ||
            (Near(a, end) && Near(b, start));
    }

    static bool Near(Vector3 p, Vector3 q)
    {
        p.y = 0f;
        q.y = 0f;

        return Vector3.Distance(p, q) < 0.3f;
    }

    static int AreaIndex(int mask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((mask & (1 << i)) != 0)
                return i;
        }

        return 0;
    }

    void OnGUI()
    {
        GUI.Label(
            new Rect(10, 10, 500, 25),
            $"Kose sayisi: {cornerCount}   Yol uzunlugu: {length:F2} m"
        );

        GUI.Label(
            new Rect(10, 30, 500, 25),
            $"Toplam yol maliyeti: {cost:F2}"
        );

        GUI.Label(
            new Rect(10, 50, 500, 25),
            $"Yol durumu: {agent.pathStatus}"
        );
    }

    void OnDrawGizmos()
    {
        if (agent == null || !agent.hasPath)
            return;

        Gizmos.color = Color.yellow;

        foreach (Vector3 point in agent.path.corners)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }
}
