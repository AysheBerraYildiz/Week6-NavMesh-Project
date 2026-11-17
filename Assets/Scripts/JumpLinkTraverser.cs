
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class JumpLinkTraverser : MonoBehaviour
{
    public float duration = 0.6f;
    public float arcHeight = 2.5f;

    NavMeshAgent agent;

    bool jumping;
    float t;

    Vector3 a;
    Vector3 b;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Atlama hareketini kendimiz kontrol edeceðiz.
        agent.autoTraverseOffMeshLink = false;
    }

    void Update()
    {
        // NPC atlama baðlantýsýna ulaþtýysa atlayýþý baþlat.
        if (agent.isOnOffMeshLink && !jumping)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;

            a = transform.position;
            b = data.endPos + Vector3.up * agent.baseOffset;

            t = 0f;
            jumping = true;
        }

        if (!jumping) return;

        // Atlayýþ süresini ilerlet.
        t += Time.deltaTime / duration;

        float k = Mathf.Clamp01(t);

        // Parabol þeklinde atlayýþ.
        transform.position =
            Vector3.Lerp(a, b, k) +
            Vector3.up * (4f * arcHeight * k * (1f - k));

        // Varýþ noktasýna ulaþtýysa baðlantýyý tamamla.
        if (t >= 1f)
        {
            agent.CompleteOffMeshLink();
            jumping = false;
        }
    }
}
