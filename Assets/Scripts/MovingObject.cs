using UnityEngine;
using System.Collections;

// Abstract class for moving objects. Currently only PlayerControl implements it, but it is useful for future 
// extensions like enemies or other moving objects.

public abstract class MovingObject : MonoBehaviour {
    
    public LayerMask wall;

    private float moveTime = 10.0f;

    private BoxCollider2D boxCollider;

    private Rigidbody2D rb2D;

    private bool canMove;
    
    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    // Move system, coroutines are used to ensure smooth movement in a grid-like system.
    protected void Move (int xDir, int yDir) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false; // To avoid bugs we disable and enable the boxcollider while casting
        bool hit = Physics.Linecast(start, end, wall);
        boxCollider.enabled = true;

        if (!hit && canMove) {
            StartCoroutine(SmoothMovement(end));
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end) {
        canMove = false;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, moveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        canMove = true;
    }
}