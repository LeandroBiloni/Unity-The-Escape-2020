using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private EnemyBrain brain;
    Rigidbody rb;
    public float moveSpeed;
    public float runSpeed;
    public float rotationSpeed;
	public Animator anim;

    private float m_currentV = 0;
    private float m_currentH = 0;
    private float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;

    public bool moveToPosition;
    public Vector3 goToPos;
    public bool collideWithWP;

	// Start is called before the first frame update
	void Start()
	{
		brain = GetComponent<EnemyBrain>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPosition)
            MoveToPosition(goToPos);
    }

    public void ManualMovement()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
		anim.SetFloat("VelZ", v);
		anim.SetFloat("VelX", h);

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
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
		anim.SetFloat("VelZ", direction.magnitude);
	}

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Waypoint") && collideWithWP == true)
        {
            brain.SelectWaypoint();
        }
    }*/

    public void FollowMovement(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * Time.deltaTime * runSpeed;
    }

    public void MoveToPosition(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * Time.deltaTime * moveSpeed;
    }
}
