using StarterAssets;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int pickupRange = 2;

    [Header("Stamina Stats")]
    [SerializeField] float stamina = 100f;
    [SerializeField] float staminaDeduct = 2f;
    [SerializeField] float staminaReturn = 3f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float minStamina = 0f;
    [SerializeField] float returnDelay = 1f;
    [SerializeField] float returnTimer = 0f;

    [Header("Collectible Amount")]
    public int cogwheelCount = 0;
    StarterAssetsInputs starterAssetsInputs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        starterAssetsInputs = GetComponentInChildren<StarterAssetsInputs>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PickupRay();
        Stamina();
    }

    private void PickupRay()
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupRange, Color.red);
        bool pickupRayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange, ~0, QueryTriggerInteraction.Collide);

        if (pickupRayHit && hit.collider.CompareTag("Pickup") && Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider.name.Contains("Cogwheel"))
            {
                cogwheelCount++;
                Debug.Log(cogwheelCount);
                Destroy(hit.collider.gameObject);
            }
        }
    }
    void Stamina()
    {
        bool isMoving = starterAssetsInputs.move.magnitude > 0f; //Checks if the player is moving
        // Deduct stamina when sprinting
        if (starterAssetsInputs.sprint && isMoving)
        {
            stamina -= staminaDeduct * Time.deltaTime;
            stamina = Mathf.Max(stamina, minStamina);
            Debug.Log(stamina);
            returnTimer = 0; // Reset the return timer when sprinting
        }
        else
        {
            returnTimer += Time.deltaTime; //return timer when not sprinting
            if (returnTimer >= returnDelay)
            {
                // Return stamina only after delay
                stamina += staminaReturn * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
                Debug.Log(stamina);
            }
        }
        
    }
}