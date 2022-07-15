using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private Rigidbody2D rb; 
    public float runSpeed = 20f; 
    public float jumpSpeed = 20f; 

    private Vector2 velocity = Vector2.zero;

    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayerMask; 
    private float groundRadius = 0.1f;  
    private bool isGrounded = false; 

    void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        var move = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;

        // Flip. Get the transform modulus and make it negative if the pressed key was left
        var newx = Mathf.Abs(this.transform.localScale.x) * Mathf.Sign(move); 
        transform.localScale = new Vector3(newx, transform.localScale.y, transform.localScale.z);

        var jump = Input.GetAxisRaw("Vertical") * jumpSpeed * Time.deltaTime;
        jump = jump > 0 && isGrounded ? jump : 0; 

		Vector2 targetVelocity = new Vector2(move * 10f, rb.velocity.y + jump * 10f);
		rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }

    private void FixedUpdate()
	{
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundRadius, groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
			if (colliders[i].gameObject != gameObject) 
                isGrounded = true;
	}

}
