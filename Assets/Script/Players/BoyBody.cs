using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBody : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed = 200.0f;
    public float x, y;
	public Transform powerPoint;
    private float m_currentV = 0;
    private float m_currentH = 0;
    private float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;
 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /*public void Movement(float x, float y)
    {
        transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        transform.Translate(0, 0, y * Time.deltaTime * moveSpeed);
    }*/
    public void Movement()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

		AudioManager audMan = FindObjectOfType<AudioManager>();
		

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = new Vector3(1, 0, 0) * m_currentV + new Vector3(0, 0, -1) * m_currentH;
		

		float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;
		audMan.WalkingSound(direction);

        if (direction != Vector3.zero)
        {
            //m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            //transform.rotation = Quaternion.LookRotation(m_currentDirection);
            //transform.position += m_currentDirection * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

    }
}
