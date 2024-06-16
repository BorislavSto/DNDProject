using UnityEngine;

public class HighlightAndDelete : MonoBehaviour
{
    private Color originalColor;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    void OnMouseEnter()
    {
        // Change material color to indicate hover
        renderer.material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        // Revert to original color when mouse exits
        renderer.material.color = originalColor;
    }

    void OnMouseOver()
    {
        // Check for right mouse button click to delete
        if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button index
        {
            Destroy(gameObject);
        }
    }
}