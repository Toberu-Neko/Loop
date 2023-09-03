using TMPro;
using UnityEngine;

public class DebugEntityStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    private Canvas canvas;
    private Core core;

    private Movement movement;
    private Stats stats;
    private Combat combat;

    private Camera cam;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        core = GetComponentInParent<Core>();
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
        combat = core.GetCoreComponent<Combat>();

    }
    void Start()
    {
    }

    

    private void Update()
    {
        if(transform.rotation != cam.transform.rotation)
        {
            transform.rotation = cam.transform.rotation;
        }
    }

    private void OnEnable()
    {
        stats.Health.OnValueChanged += UpdateText;
        stats.Stamina.OnValueChanged += UpdateText;


        UpdateText();

        cam = Camera.main;
        canvas.worldCamera = cam;
    }

    private void OnDisable()
    {
        stats.Health.OnValueChanged -= UpdateText;
        stats.Stamina.OnValueChanged -= UpdateText;
    }

    void UpdateText()
    {
        hpText.text = "HP: " + stats.Health.CurrentValue.ToString() +
            "\nST: " + stats.Stamina.CurrentValue.ToString();
    }
}
