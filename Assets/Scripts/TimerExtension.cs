using UnityEngine;
using System.Collections;

// Timer extension collectible script.

public class TimerExtension : MonoBehaviour {

    public ParticleSystem partSystem;
    
    private void OnTriggerEnter2D(Collider2D other) {
        HUD.instance.timer += 1;
        Instantiate(partSystem, transform.position, Quaternion.identity);
        partSystem.Play();
        Destroy(gameObject);
        GameManager.instance.Extension();
    }
}