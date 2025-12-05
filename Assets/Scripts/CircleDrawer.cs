using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Anzahl der Punkte. 50 ist meist rund genug.")]
    public int segments = 50;

    [Tooltip("Der Radius des Kreises")]
    public float radius = 2.0f;

    [Tooltip("Dicke der Linie")]
    public float lineWidth = 0.1f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        // WICHTIG: useWorldSpace = false sorgt dafür, dass der Kreis
        // sich mit dem Objekt mitbewegt, ohne dass wir neu rechnen müssen.
        line.useWorldSpace = false;
        line.loop = true; // Verbindet den letzten Punkt mit dem ersten

        DrawCircle();
    }

    // OnValidate sorgt dafür, dass sich der Kreis im Editor sofort aktualisiert,
    // wenn du am Slider ziehst (ohne Play zu drücken).
    void OnValidate()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        DrawCircle();
    }

    // Diese Methode kannst du auch von außen aufrufen, wenn sich der Radius ändert
    public void DrawCircle()
    {
        if (line == null) return;

        // Liniendicke setzen
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        // Anzahl der Punkte im LineRenderer setzen
        line.positionCount = segments;

        float angle = 0f;
        float angleStep = (2f * Mathf.PI) / segments;

        for (int i = 0; i < segments; i++)
        {
            // Mathematik: Wir berechnen x und z auf einer flachen Ebene
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            // Wir setzen y auf 0, damit der Kreis flach um den Pivot liegt
            line.SetPosition(i, new Vector3(x, 0f, z));

            angle += angleStep;
        }
    }
}
