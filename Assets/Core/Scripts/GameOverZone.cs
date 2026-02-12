using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways] // gør at den røde boks vises i editoren
public class GameOverZone2D : MonoBehaviour
{
    [Header("Scene to load when triggered")]
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    [Header("Gizmo settings (Scene View only)")]
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f); // halvgennemsigtig rød

    // Dette bliver kaldt i 2D-fysik når noget rører en Trigger Collider 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💀 Game Over triggered by 2D collider 💀");
            SceneManager.LoadSceneAsync(gameOverSceneName, LoadSceneMode.Additive);
        }
    }

    // Tegner en gennemsigtig rød kasse i Scene View, så du visuelt kan se zonen
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        var box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            // BoxCollider2D har offset og size i 2D-verdenen (XY-plan)
            Vector2 worldPos = (Vector2)transform.position + box.offset;
            Vector3 size3 = new Vector3(box.size.x, box.size.y, 0.1f);

            // Fyld
            Gizmos.DrawCube(worldPos, size3);

            // Kant
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, size3);
        }
    }
}
