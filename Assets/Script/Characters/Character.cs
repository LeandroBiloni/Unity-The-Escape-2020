using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    protected float _speed;
    protected bool _canMove;
    
    protected bool _selected;

    protected Animator _animator;

    [SerializeField] protected GameObject _selectionIcon;

    [SerializeField] protected float _iconShowTime;
    
    protected FieldOfView _fieldOfView;

    protected AudioManager _audioManager;
    
    protected Rigidbody _rb;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _fieldOfView = GetComponent<FieldOfView>();

        var cinematic = FindObjectOfType<Cinematic>();
        cinematic.OnCinematicStart += FieldOfViewOff;
        cinematic.OnCinematicEnd += FieldOfViewOn;
        _animator = GetComponent<Animator>();
        _canMove = true;
        _speed = _moveSpeed;
        _audioManager = FindObjectOfType<AudioManager>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_selected) return;

        if (_canMove)
        {
            var x = Input.GetAxis("Vertical");
            var z = Input.GetAxis("Horizontal");

            if (x != 0 || z != 0)
            {
                Vector3 dir = Vector3.zero;
                dir = new Vector3(x, 0, -z);

                ManualMovement(dir);
            }
        }
    }

    /// <summary>
    /// Movement for selected characters.
    /// </summary>
    protected void ManualMovement(Vector3 dir)
    {
        if (_animator)
        {
            _animator.SetFloat("VelZ", dir.z);
            _animator.SetFloat("VelX", dir.x);  
        }
        
        //transform.LookAt(transform.position + dir);
        //transform.position += dir.normalized * (_moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position += dir * (Time.deltaTime * _speed);
        /*var tempVector = dir;
        tempVector = tempVector.normalized * _speed * Time.deltaTime;
        _rb.MovePosition(transform.position + tempVector);*/

        //TODO: Agregar sonido.
        //audMan.WalkingSound(dir);
        _audioManager.WalkingSound(dir);
    }

    public bool IsSelected()
    {
        return _selected;
    }

    /// <summary>
    /// Actions for the selected character.
    /// </summary>
    public virtual void Select()
    {
        _selected = true;
        StartCoroutine(ActivateSelectionIconWithTimer());
    }

    /// <summary>
    /// Actions for the deselected character.
    /// </summary>
    public virtual void Deselect()
    {
        _selected = false;
    }

    /// <summary>
    /// Activates the Selection Icon.
    /// </summary>
    protected IEnumerator ActivateSelectionIconWithTimer()
    {
        _selectionIcon.SetActive(true);

        yield return new WaitForSeconds(_iconShowTime);
        
        _selectionIcon.SetActive(false);
    }
    
    public virtual void Dead()
    {
        _canMove = false;

        StartCoroutine(LoseDelay());
    }

    IEnumerator LoseDelay()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<ScenesManager>().LoseScreen();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _speed = _moveSpeed/2;
        }
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _speed = _moveSpeed;
        }
    }

    public void FieldOfViewOn()
    {
        _fieldOfView.viewMeshFilter.gameObject.SetActive(true);
        _fieldOfView.enabled = true;
    }

    public void FieldOfViewOff()
    {
        _fieldOfView.viewMeshFilter.gameObject.SetActive(false);
        _fieldOfView.enabled = false;
    }

    public void StopMovement()
    {
        _canMove = false;
        _animator.SetFloat("VelZ", 0);
    }

    public void ResumeMovement()
    {
        _canMove = true;
    }

    public void CancelSelection()
    {
        _selected = false;
    }

    public void ActivateSelection()
    {
        _selected = true;
    }

    protected IEnumerator IconsOffOnStart(List<GameObject> objects)
    {
        yield return new WaitForEndOfFrame();

        foreach (var go in objects)
        {
            if (go) go.SetActive(false);
        }
    }
}
