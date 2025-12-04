using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform myObject;

    public void UpdateHandData(Vector3 screenPos, string gesture)
    {
        float x = (screenPos.x - 0.5f) * 15f;
        float y = (screenPos.y - 0.5f) * 10f;

        myObject.position = Vector3.Lerp(myObject.position, new Vector3(x, y, 0), Time.deltaTime * 15);

        Renderer rend = myObject.GetComponent<Renderer>();

        switch (gesture)
        {
            case "Fist":
                myObject.localScale = Vector3.one * 1.0f;
                break;

            case "Point":
                myObject.localScale = Vector3.one * 2.0f;
                break;

            case "Open":
                myObject.localScale = Vector3.one * 0.5f;
                break;

            default:
                rend.material.color = Color.white;
                break;
        }
    }
}