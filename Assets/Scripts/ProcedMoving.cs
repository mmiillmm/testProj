using System.Collections;
using UnityEngine;

public class ProceduralWalking : MonoBehaviour
{
    public Transform[] legTargets; // A lábak célpontjai
    public Transform body; // A test
    public float legMoveThreshold = 1.0f; // Távolság a láb mozgatásához
    public LayerMask groundLayer; // A talaj réteg
    public float bodyHeightOffset = 1.0f; // A test magasságának kompenzálása
    public float movementSpeed = 5f; // A test mozgási sebessége
    public float stepHeight = 0.5f; // A lépés magassága
    public float stepDuration = 0.3f; // A lépés időtartama

    private Vector3[] defaultLegPositions;
    private bool[] isLegGrounded;

    void Start()
    {
        defaultLegPositions = new Vector3[legTargets.Length];
        isLegGrounded = new bool[legTargets.Length];

        // Alaphelyzetek mentése
        for (int i = 0; i < legTargets.Length; i++)
        {
            defaultLegPositions[i] = legTargets[i].position;
            isLegGrounded[i] = true;
        }
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 movement = input * movementSpeed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            MoveBody(movement);
            UpdateLegs();
        }

        UpdateBodyPosition();
    }

    void MoveBody(Vector3 movement)
    {
        // A test és a lábcélpontok mozgatása
        body.position += movement;

        for (int i = 0; i < legTargets.Length; i++)
        {
            legTargets[i].position += movement;
        }
    }

    void UpdateLegs()
    {
        for (int i = 0; i < legTargets.Length; i++)
        {
            int oppositeLegIndex = (i + 1) % 2; // Ellentétes láb

            // Csak akkor mozgassuk a lábat, ha az ellentétes láb a talajon van
            if (isLegGrounded[oppositeLegIndex])
            {
                Vector3 targetPoint = legTargets[i].position;
                RaycastHit hit;

                // Raycast lefelé a talaj kereséséhez
                if (Physics.Raycast(targetPoint + Vector3.up, Vector3.down, out hit, Mathf.Infinity, groundLayer))
                {
                    targetPoint.y = hit.point.y;
                }

                // Ha a távolság nagyobb, mint a küszöb, mozgassuk a lábat
                if (Vector3.Distance(legTargets[i].position, targetPoint) > legMoveThreshold)
                {
                    StartCoroutine(MoveLeg(i, targetPoint));
                }
            }
        }
    }

    IEnumerator MoveLeg(int legIndex, Vector3 targetPoint)
    {
        isLegGrounded[legIndex] = false;
        Vector3 startPosition = legTargets[legIndex].position;
        float elapsedTime = 0f;

        // Mozgás íve a lépés során
        Vector3 midPoint = (startPosition + targetPoint) / 2;
        midPoint.y += stepHeight;

        while (elapsedTime < stepDuration)
        {
            float t = elapsedTime / stepDuration;
            Vector3 currentPoint = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, targetPoint, t), t);
            legTargets[legIndex].position = currentPoint;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        legTargets[legIndex].position = targetPoint;
        isLegGrounded[legIndex] = true;
    }

    void UpdateBodyPosition()
    {
        // A test pozíciójának frissítése az átlagos lábhelyzet alapján
        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < legTargets.Length; i++)
        {
            averagePosition += legTargets[i].position;
        }
        averagePosition /= legTargets.Length;

        // A test magasságának kompenzálása, hogy a lábak megtartsák a testet
        body.position = new Vector3(averagePosition.x, averagePosition.y + bodyHeightOffset, averagePosition.z);

        // A test forgatása a bal és jobb láb közötti magasság különbség alapján
        float heightDifference = legTargets[0].position.y - legTargets[1].position.y;
        body.rotation = Quaternion.Euler(0, 0, -heightDifference * 10f);
    }
}
