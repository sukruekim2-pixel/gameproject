using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class gunraycastcontroller : MonoBehaviour
{
    [FormerlySerializedAs("range")]
    [SerializeField] private float maxShootDistance = 100f;

    [FormerlySerializedAs("impactForce")]
    [SerializeField] private float hitImpulseForce = 10f;

    [FormerlySerializedAs("shootPoint")]
    [SerializeField] private Transform muzzleTransform;

    [FormerlySerializedAs("hitLayers")]
    [SerializeField] private LayerMask shootableLayers = ~0;

    private void Awake()
    {
        if (muzzleTransform == null)
        {
            muzzleTransform = transform;
        }
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * maxShootDistance, Color.red, 0.5f);

        if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out RaycastHit raycastHit, maxShootDistance, shootableLayers))
        {
            // Fiziksel itme
            Rigidbody hitRigidbody = raycastHit.collider.attachedRigidbody;
            if (hitRigidbody != null)
            {
                hitRigidbody.AddForceAtPosition(muzzleTransform.forward * hitImpulseForce, raycastHit.point, ForceMode.Impulse);
            }

            // Hedefe vuruldu bilgisini gönder
            Target hitTarget = raycastHit.collider.GetComponentInParent<Target>();
            if (hitTarget != null)
            {
                hitTarget.TakeImpact();
            }
        }
    }
}