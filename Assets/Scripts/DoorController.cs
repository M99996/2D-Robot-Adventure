using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class DoorController : MonoBehaviour
{
    public Animator animator;
    public KeycardController keycard;
    public GameOver gameOverPanel;
    public bool isFinal = false;
    public string nextSceneName = "FinalScene";

    private bool opened = false;

    private void Reset(){ GetComponent<Collider2D>().isTrigger = true; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;
        if (!other.CompareTag("Player")) return;
        if (keycard == null)
        {
            Debug.LogWarning("Coudln't find Keycard reference.");
            return;
        }

        if (keycard.IsCollected)
        {
            opened = true;
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("Door locked. Need keycard.");
        }
    }

    public void onDoorOpened()
    {
        if (!isFinal) {
            if (nextSceneName == "") return;
            
            SceneManager.LoadScene(nextSceneName);
            return;
        }
        
        if (gameOverPanel != null) { gameOverPanel.ShowGameOver(); }
        else { Debug.LogWarning("[GameOver] panel not bound,"); }
    }
}
