using UnityEngine;
using UnityEngine.XR;

public class AbilityController : MonoBehaviour
{

    [SerializeField] private bool useArrow = false;

    [SerializeField] private KeyCode bottomKey = KeyCode.LeftAlt;
    [SerializeField] private KeyCode topKey = KeyCode.RightAlt;

    private Rigidbody2D rb;
    private AbilityAC ability;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ability = GetComponent<AbilityAC>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ability == null || rb == null)
        {
            return;
        }

        bool pressed = useArrow ? (Input.GetKey(topKey)) : (Input.GetKey(bottomKey));

        if (pressed)
        {
            ability.triggerAbility(rb);
        }
    }

    public void setUseArrows(bool value)
    {
        useArrow = value;
    }
}
