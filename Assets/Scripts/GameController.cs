using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform cubeToMove; // Ziehe hier deinen Würfel im Inspector rein!

    public void UpdateHandData(Vector3 screenPos, bool isFist)
    {
        // 1. DEBUGGEN: Wenn du das in der Konsole siehst, funktioniert die Verbindung!
        // Debug.Log($"Daten empfangen! Pos: {screenPos} | Faust: {isFist}");

        if (cubeToMove == null) return;

        // 2. Umrechnung: screenPos ist 0 bis 1.
        // Wir müssen das auf die Weltgröße strecken (z.B. x10)
        // Wir zentrieren es auch (minus 0.5)
        float x = (screenPos.x - 0.5f) * 15f;
        float y = (screenPos.y - 0.5f) * 10f;

        // 3. Position anwenden
        Vector3 zielPosition = new Vector3(x, y, 0);
        cubeToMove.position = Vector3.Lerp(cubeToMove.position, zielPosition, Time.deltaTime * 10);

        // 4. Farbe ändern bei Faust
        var renderer = cubeToMove.GetComponent<Renderer>();
        if (isFist)
        {
            renderer.material.color = Color.red; // Faust = Rot
            cubeToMove.localScale = Vector3.one * 0.5f; // Klein
        }
        else
        {
            renderer.material.color = Color.green; // Offen = Grün
            cubeToMove.localScale = Vector3.one * 1.5f; // Groß
        }
    }
}