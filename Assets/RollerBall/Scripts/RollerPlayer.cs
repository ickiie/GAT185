using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollerPlayer : MonoBehaviour, IDestructable
{
    [SerializeField] float maxForce = 5;
    [SerializeField] float friction = 10;
    [SerializeField] float jumpForce = 5;
    [SerializeField] ForceMode forceMode;
    [SerializeField] Transform viewTransform;
    [SerializeField] Rigidbody rigidbody;

    Rigidbody rb;
    Vector3 force = Vector3.zero;

    private bool hasJump = true;

    public float timer = 0.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {


        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        Quaternion viewSpace = Quaternion.AngleAxis(viewTransform.rotation.eulerAngles.y, Vector3.up);
        direction = viewSpace * direction;

        force = direction * maxForce;

        if (Input.GetButtonDown("Jump") && hasJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            timer = 0.5f;
            hasJump = false;
        }

        RollerGameManager.Instance.playerHealth = GetComponent<Health>().health;
    }

    private void FixedUpdate()
    {
        rb.AddForce(force, forceMode);
        
    }

    public void Destroyed()
    {
        RollerGameManager.Instance.OnPlayerDead();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "world")
        {
            hasJump = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "world")
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                hasJump = true;
            }
        }
    }
}
