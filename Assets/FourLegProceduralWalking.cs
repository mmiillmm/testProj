using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class FourLegProceduralWalking : MonoBehaviour
{
    [SerializeField] private Transform[] legTargets; // IK célpontok a lábakhoz
    [SerializeField] private Transform body; // A test pozíciójának és forgásának követése
    [SerializeField] private float maxStepDistance = 1.5f; // Maximális lépés távolság
    [SerializeField] private float stepHeight = 0.2f; // Lépés magassága
    [SerializeField] private float stepSpeed = 5.0f; // Lépés sebessége
    [SerializeField] private float movementSpeed = 3.0f; // A test mozgási sebessége

    private Vector3[] initialLegOffsets;
    private Vector3[] currentLegTargets;
    private bool[] isLegMoving;

    private void Start()
    {
        // Lábak kezdeti eltolásának mentése a testhez képest
        initialLegOffsets = new Vector3[legTargets.Length];
        currentLegTargets = new Vector3[legTargets.Length];
        isLegMoving = new bool[legTargets.Length];

        for (int i = 0; i < legTargets.Length; i++)
        {
            initialLegOffsets[i] = body.InverseTransformPoint(legTargets[i].position);
            currentLegTargets[i] = legTargets[i].position;
            isLegMoving[i] = false;
        }
    }

    private void Update()
    {
        HandleBodyMovement();

        for (int i = 0; i < legTargets.Length; i++)
        {
            if (!isLegMoving[i] && ShouldMoveLeg(i))
            {
                StartCoroutine(MoveLeg(i));
            }
        }

        AdjustBodyPositionAndRotation();
    }

    private void HandleBodyMovement()
    {
        // Mozgás kezelése (WASD billentyűkkel vagy nyilakkal)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * movementSpeed * Time.deltaTime;

        // Mozgás alkalmazása a testre
        body.Translate(movement, Space.World);

        // Frissíti a lábak célpontjait a test mozgásával
        for (int i = 0; i < legTargets.Length; i++)
        {
            currentLegTargets[i] = body.TransformPoint(initialLegOffsets[i]);
        }
    }

    private bool ShouldMoveLeg(int legIndex)
    {
        // Ellenőrzi, hogy a láb célpontja távolabb van-e, mint a megengedett lépés távolság
        float distance = Vector3.Distance(currentLegTargets[legIndex], legTargets[legIndex].position);
        return distance > maxStepDistance && AreOppositeLegsGrounded(legIndex);
    }

    private bool AreOppositeLegsGrounded(int legIndex)
    {
        // Ellenőrzi, hogy az ellentétes lábak a földön vannak-e
        int oppositeLegIndex = (legIndex + 2) % legTargets.Length;
        return !isLegMoving[oppositeLegIndex];
    }

    private IEnumerator MoveLeg(int legIndex)
    {
        isLegMoving[legIndex] = true;
        Vector3 startPosition = legTargets[legIndex].position;
        Vector3 targetPosition = currentLegTargets[legIndex];

        float time = 0;
        while (time < 1.0f)
        {
            time += Time.deltaTime * stepSpeed;
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, time);
            currentPos.y += Mathf.Sin(time * Mathf.PI) * stepHeight; // Emelje meg a lábat sinus hullám alapján
            legTargets[legIndex].position = currentPos;
            yield return null;
        }

        legTargets[legIndex].position = targetPosition;
        isLegMoving[legIndex] = false;
    }

    private void AdjustBodyPositionAndRotation()
    {
        // Test pozíciójának beállítása az átlag láb pozíció alapján
        Vector3 averageLegPosition = Vector3.zero;
        foreach (var legTarget in legTargets)
        {
            averageLegPosition += legTarget.position;
        }
        averageLegPosition /= legTargets.Length;

        body.position = new Vector3(body.position.x, averageLegPosition.y + 0.5f, body.position.z); // Offset a test pozíciójához

        // Test forgásának beállítása a lábak közötti magasság különbség alapján
        float leftHeight = (legTargets[0].position.y + legTargets[2].position.y) / 2;
        float rightHeight = (legTargets[1].position.y + legTargets[3].position.y) / 2;
        float heightDifference = leftHeight - rightHeight;
        body.rotation = Quaternion.Euler(0, 0, heightDifference * 10); // Forgás beállítása
    }
}
