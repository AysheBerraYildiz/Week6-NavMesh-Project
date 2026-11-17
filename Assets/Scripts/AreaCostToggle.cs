
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshAgent))]
public class AreaCostToggle : MonoBehaviour
{
    public NavMeshLink jumpLink;

    public string mudAreaName = "Camur";

    public float[] mudCosts = { 1f, 2f, 5f, 10f };

    NavMeshAgent agent;

    InputAction cycleMud;
    InputAction toggleLink;

    int mudArea;
    int costIndex = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        cycleMud = InputSystem.actions.FindAction("CycleMudCost");
        toggleLink = InputSystem.actions.FindAction("ToggleLink");

        mudArea = NavMesh.GetAreaFromName(mudAreaName);

        if (mudArea < 0)
        {
            Debug.LogError("Camur alaný bulunamadý! Navigation > Areas bölümünü kontrol et.");
        }

        if (cycleMud == null)
        {
            Debug.LogError("CycleMudCost eylemi bulunamadý!");
        }

        if (toggleLink == null)
        {
            Debug.LogError("ToggleLink eylemi bulunamadý!");
        }
    }

    void Start()
    {
        ApplyMudCost();
    }

    void Update()
    {
        // M tuþuyla çamur maliyetini deðiþtir.
        if (cycleMud != null && cycleMud.WasPressedThisFrame())
        {
            costIndex = (costIndex + 1) % mudCosts.Length;
            ApplyMudCost();
        }

        // J tuþuyla atlama baðlantýsýný aç veya kapat.
        if (toggleLink != null && toggleLink.WasPressedThisFrame())
        {
            if (jumpLink != null)
            {
                jumpLink.activated = !jumpLink.activated;
            }
        }
    }

    void ApplyMudCost()
    {
        if (mudArea >= 0)
        {
            agent.SetAreaCost(mudArea, mudCosts[costIndex]);
        }
    }

    void OnGUI()
    {
        string linkStatus = (jumpLink != null && jumpLink.activated)
            ? "Acik"
            : "Kapali";

        GUI.Label(
            new Rect(10, 60, 500, 25),
            "[M] Camur maliyeti: " + mudCosts[costIndex] +
            "   [J] Baglanti: " + linkStatus +
            "   [K] Kapi"
        );
    }
}
