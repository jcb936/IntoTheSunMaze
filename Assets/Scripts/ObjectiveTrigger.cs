using UnityEngine;

// When player touches the objective, it tiggers to loading of the next level.
public class ObjectiveTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        GameManager.instance.NextLevel();
        Destroy(this);
    }
}