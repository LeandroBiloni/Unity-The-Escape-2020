using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificBody : MonoBehaviour
{
    private ScientificBrain brain;
    Rigidbody rb;
    public float moveSpeed;
    public float rotationSpeed;
    public Animator anim;

    private float m_currentV = 0;
    private float m_currentH = 0;
    private float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;

    public Vector3 goToPos;
    public bool collideWithWP;
    public bool moveToAlarm;
    void Start()
    {
        brain = GetComponent<ScientificBrain>();
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToAlarm)
            MoveToAlarm(goToPos);
    }

    public void ManualMovement()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");


        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = new Vector3(1, 0, 0) * m_currentV + new Vector3(0, 0, -1) * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            //m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);
            //transform.rotation = Quaternion.LookRotation(m_currentDirection);

            //transform.position += m_currentDirection * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    public void WaypointMovement(Vector3 direction)
    {
		anim.SetBool("Dizzy", false);
		anim.SetFloat("VelZ", direction.magnitude);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Waypoint"))
        {            
            brain.SelectWaypoint();
        }*/

        if (other.gameObject.layer == LayerMask.NameToLayer("Alarm"))
		{
            moveToAlarm = false;
			anim.SetBool("MoveToAlarm", false);
            anim.SetBool("Dizzy", true);
		}
    }

    public void MoveToAlarm(Vector3 direction)
    {
		anim.SetBool("Dizzy", false);
		anim.SetBool("MoveToAlarm", true);
		transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
}
