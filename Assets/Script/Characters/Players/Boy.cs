using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : Character
{
    [Header("Keys")]
    [SerializeField] private KeyCode _pickDropObjectKey;
    [SerializeField] private KeyCode _throwObjectKey;
    [SerializeField] private KeyCode _nextTargetKey;
    [SerializeField] private KeyCode _previousTargetKey;
    [SerializeField] private KeyCode _knockBackKey;

    [Header("Ability")]
    [SerializeField] private Transform _powerPoint;
    [SerializeField] private float _telekinesisCooldown;
    [SerializeField] private float _attractionForce;
    [SerializeField] private float _knockBackRadius;
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _knockBackStunDuration;

    [Header("Sounds")]
    [SerializeField] private AudioClip _throwBoxSfx;
    [SerializeField] private AudioClip _pullBoxSfx;

    private bool _objectsInFov;
    private Transform _currentObject;
    private int _targetObjectIndex = 0;
    private MovableObjects _previousObject;
    private bool _holdingObject;
    private bool _canUsePower;
    private bool _pulling;

    protected override void Start()
    {
        base.Start();
        _holdingObject = false;
        _canUsePower = true;
        _pulling = false;
        _objectsInFov = false;
        var selector = FindObjectOfType<CharacterSelector>();

        selector.OnBoySelect += Select;
        selector.OnGirlSelect += Deselect;
    }

    protected override void Update()
    {
        if (!_selected) return;
        base.Update();

        CheckObjectInFOV();
        
        if (Input.GetKeyDown(_pickDropObjectKey))
        {
            if (_objectsInFov && !_holdingObject && _canUsePower)
            {
                StartCoroutine(Telekinesis());
            }
            
            else if (_holdingObject)
            {
                DropObject();
            }
        }
        
        if (Input.GetKeyDown(_throwObjectKey) && _holdingObject)
        {
            ThrowObject();
        }
        
        if (_objectsInFov && !_holdingObject)
        {
            if (Input.GetKeyDown(_nextTargetKey))
                NextTarget();
            
            if (Input.GetKeyDown(_previousTargetKey))
                PreviousTarget();
        }

        if (Input.GetKeyDown(_knockBackKey) && !_holdingObject && _canUsePower)
        {
            KnockBack();
        }

    }
    
    public override void Select()
    {
        base.Select();
    }
    
    public override void Deselect()
    {
        base.Deselect();
        if (_animator)
        {
            _animator.SetFloat("VelZ", 0);
            _animator.SetFloat("VelX", 0);  
        }
        
    }
    /// <summary>
    /// Activates Telekinesis ability.
    /// </summary>
    IEnumerator Telekinesis()
    {
        _pulling = true;
        //TODO: Agregar el sonido
        //manager.audioManager.PlaySFX(pullBoxSound);
        _audioManager.PlaySFX(_pullBoxSfx);
        _animator.SetTrigger("poder");
        
        _canUsePower = false;
        _canMove = false;
        
        
        _currentObject = _fieldOfView.visibleTargets[_targetObjectIndex].transform;
        var currentObjectTransform = _currentObject.transform;
        
        //Will execute while the object travels to _powerPoint location.
        while (_pulling)
        {
            Vector3 direction = (_powerPoint.position - currentObjectTransform.position);
            _animator.SetFloat("VelZ", 0);
            _animator.SetFloat("VelX", 0);

            if (Vector3.Distance(_currentObject.transform.position, _powerPoint.position) >= 0.1f)
            {
                currentObjectTransform.position = currentObjectTransform.position  + direction * (_attractionForce * Time.deltaTime);
				
                if (Vector3.Distance(currentObjectTransform.position, _powerPoint.position) <= 0.5)
                {
                    _holdingObject = true;
                    _pulling = false;
                    var currObj = _currentObject.GetComponent<MovableObjects>();
                    currObj.Deselected();
                    currObj.Interaction(true);
                    currentObjectTransform.parent = _powerPoint;
                    var objRb = _currentObject.GetComponent<Rigidbody>();
                    objRb.constraints = RigidbodyConstraints.FreezePositionY;
                    _canMove = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Throws the picked object.
    /// </summary>
    private void ThrowObject()
    {
        //TODO: Agregar el sonido
        //manager.audioManager.PlaySFX(throwBoxSound, .5f);
        _audioManager.PlaySFX(_throwBoxSfx, .5f);
        _animator.SetTrigger("poder");
		
        _holdingObject = false;
        
        var obj = _currentObject.GetComponent<MovableObjects>();
        obj.Interaction(false);
        
        _currentObject.transform.parent = null;

        var objRb = _currentObject.GetComponent<Rigidbody>();
        objRb.constraints = RigidbodyConstraints.None;
        objRb.AddForce(_powerPoint.forward * 10f, ForceMode.VelocityChange);  //Lanza el objeto hacia donde esta mirando

        StartCoroutine(TelekinesisCooldown(false));
    }

    /// <summary>
    /// Drops the picked object.
    /// </summary>
    private void DropObject()
    {
        var obj = _currentObject.GetComponent<MovableObjects>();
        obj.Interaction(false);
        
        _currentObject.transform.parent = null;
        var objRb = _currentObject.GetComponent<Rigidbody>();
        objRb.constraints = RigidbodyConstraints.None;
        _holdingObject = false;
        
        StartCoroutine(TelekinesisCooldown(false));
    }

    IEnumerator TelekinesisCooldown(bool maxCooldown)
    {
        if(!maxCooldown)
            yield return new WaitForSeconds(_telekinesisCooldown);
        else
            yield return new WaitForSeconds(_telekinesisCooldown * 4);

        _canUsePower = true;
    }

    /// <summary>
    /// Knocks back every nearby enemy and stuns them for a second
    /// </summary>
    private void KnockBack()
    {
        //FALTA AGREGAR EN EL HUD EL COOLDOWN DEL USO DE PODER 
        //Agregar sonido, animación y efecto(?

        _canUsePower = false;
        _canMove = false;

        var enemyTargets = Physics.OverlapSphere(transform.position, _knockBackRadius);
        Debug.Log("List Size: " + enemyTargets.Length);
        foreach (var item in enemyTargets)
        {
            var currentEnemy = item.GetComponent<BaseEnemy>();
            if (!currentEnemy) continue;
            Debug.Log(currentEnemy.name + "Knock Back");
            currentEnemy.GetKnockedBack((currentEnemy.transform.position - transform.position) * _knockBackForce, _knockBackStunDuration);
        }
        StartCoroutine(TelekinesisCooldown(true));
        _canMove = true;
    }

    void CheckObjectInFOV()
    {
        if (_fieldOfView.visibleTargets.Count == 0) //Checkea si hay objetos a la vista
        {
            _objectsInFov = false;
            _targetObjectIndex = 0;
            ObjectInPlayerFOV(_objectsInFov, null);
        }
        else
        {
            _objectsInFov = true;
            if (_fieldOfView.visibleTargets.Count == 1)
                _targetObjectIndex = 0;

            if (_fieldOfView.visibleTargets.Count > _targetObjectIndex)
            {
                var targetObject = _fieldOfView.visibleTargets[_targetObjectIndex].GetComponent<MovableObjects>();
                ObjectInPlayerFOV(_objectsInFov, targetObject);
            }
        }
    }
    
    void ObjectInPlayerFOV(bool watching, MovableObjects selectedObject)
    {

    	if (watching)
    	{
    		if (_previousObject == null) //Esto solamente se va a hacer cuando la lista de objetos del fov esta vacia
    		{
    			//Guardo una copia del objeto para tener acceso cuando selecciono uno nuevo
                _previousObject = selectedObject;
                selectedObject.Selected();
            }

            if (_previousObject != selectedObject && !_holdingObject)
            {
	            _previousObject.Deselected();
	            
	            _previousObject = selectedObject;
                
                selectedObject.Selected();
            }
        }

        //Si tengo un objeto guardado y no estoy viendo a ningun otro, lo restablece
    	if (_previousObject != null && watching == false)
    	{
            _previousObject.Deselected();
            _previousObject = null;
        }
    }
    
    /// <summary>
    /// Selects the next visible object to interact.
    /// </summary>
    void NextTarget()
    {
       _targetObjectIndex--;
        if (_targetObjectIndex < 0)
        {
            _targetObjectIndex = _fieldOfView.visibleTargets.Count-1;
        }
        
        ObjectInPlayerFOV(_objectsInFov, _fieldOfView.visibleTargets[_targetObjectIndex].GetComponent<MovableObjects>());
    }
    
    /// <summary>
    /// Selects the previous visible object to interact.
    /// </summary>
    void PreviousTarget()
    {
        _targetObjectIndex++;
        if (_targetObjectIndex == _fieldOfView.visibleTargets.Count)
        {
            _targetObjectIndex = 0;
        }
        
        ObjectInPlayerFOV(_objectsInFov, _fieldOfView.visibleTargets[_targetObjectIndex].GetComponent<MovableObjects>());
    }

    public bool IsHoldingObject()
    {
        return _holdingObject;
    }
    public override void Dead()
    {
        base.Dead();
        _canUsePower = false;
    }
}
