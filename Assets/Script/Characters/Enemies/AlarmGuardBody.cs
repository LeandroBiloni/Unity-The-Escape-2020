using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmGuardBody : MonoBehaviour
{
    private AlarmGuardBrain brain;
    Rigidbody rb;
    public float moveSpeed;
    public float rotationSpeed;

    private float m_currentV = 0;
    private float m_currentH = 0;
    private float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponent<AlarmGuardBrain>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);


            transform.position += m_currentDirection * moveSpeed * Time.deltaTime;
        }
    }

    public void FollowMovement(Vector3 direction)
    {
		transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction.normalized * Time.deltaTime * moveSpeed;
    }

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
		brain.anim.SetBool("Chasing", true);
		transform.position += direction.normalized * Time.deltaTime * moveSpeed;
    }
}
