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
        line.useWorldSpace = false;
        line.loop = true;
        DrawCircle();
    }

    void OnValidate()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        DrawCircle();
    }

    public void DrawCircle()
    {
        if (line == null) return;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.positionCount = segments;

        float angle = 0f;
        float angleStep = (2f * Mathf.PI) / segments;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            line.SetPosition(i, new Vector3(x, 0f, z));

            angle += angleStep;
        }
    }
}
