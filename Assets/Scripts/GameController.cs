using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform myObject; // Dein Würfel

    // WICHTIG: Signatur geändert! Jetzt string statt bool.
    public void UpdateHandData(Vector3 screenPos, string gesture)
    {
        // 1. Position anwenden (angepasst an deinen Screen)
        float x = (screenPos.x - 0.5f) * 15f;
        float y = (screenPos.y - 0.5f) * 10f;

        myObject.position = Vector3.Lerp(myObject.position, new Vector3(x, y, 0), Time.deltaTime * 15);

        // 2. Gesten auswerten
        Renderer rend = myObject.GetComponent<Renderer>();

        switch (gesture)
        {
            case "Fist":
                // Faust -> Rot & Klein
                rend.material.color = Color.red;
                myObject.localScale = Vector3.one * 0.5f;
                Debug.Log("Geste: FAUST");
                break;

            case "Point":
                // Zeigen -> Gelb & Normal
                rend.material.color = Color.yellow;
                myObject.localScale = Vector3.one * 1.0f;
                Debug.Log("Geste: POINT");
                break;

            case "Open":
                // Offen -> Grün & Groß
                rend.material.color = Color.green;
                myObject.localScale = Vector3.one * 1.5f;
                Debug.Log("Geste: OPEN");
                break;

            default:
                // Unbekannt -> Weiß
                rend.material.color = Color.white;
                break;
        }
    }
}