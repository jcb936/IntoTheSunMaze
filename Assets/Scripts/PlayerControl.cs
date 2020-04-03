using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation of MovingObject that manages player control.

public class PlayerControl : MovingObject {
    private Animator animator;

    private AudioSource audioSource;

    private void Awake() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Moves the player and set animations.

    void Update() {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0) {
            Move(horizontal, vertical);
            if (horizontal > 0)
                animator.SetBool("isGoingRight", true);
            else if (horizontal < 0)
                animator.SetBool("isGoingRight", false);
        }
    }

    // This function is used to determine whether the player instance
    // is visible by the main camera, in order to resize the camera
    // horizontally if needed. (More info in GameManager)

    public bool CheckIfVisible() {
        return GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main);
    }

    // Play clip when touch a time extension.

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<TimerExtension>() != null)
            audioSource.Play();
    }
}
