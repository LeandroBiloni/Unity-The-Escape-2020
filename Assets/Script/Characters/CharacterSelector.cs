using UnityEngine;
public class CharacterSelector : MonoBehaviour
{
    private bool _controllingEnemy;

    private Boy _boy;

    private Girl _girl;

    private GameObject _selectedCharacter; 
    public delegate void CharacterChange();
    
    public event CharacterChange OnBoySelect;
    public event CharacterChange OnGirlSelect;

    public delegate void FocusChange(GameObject target);
    public event FocusChange OnFocusChange;
    // Start is called before the first frame update
    void Start()
    {
        _boy = FindObjectOfType<Boy>();
        _girl = FindObjectOfType<Girl>();
        
        _boy.Deselect();
        _girl.Select();
        _selectedCharacter = _girl.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_boy || !_girl)
        {
            _boy = FindObjectOfType<Boy>();
            _girl = FindObjectOfType<Girl>();
        
            _boy.Deselect();
            _girl.Select();
            _selectedCharacter = _girl.gameObject;
        }
        if (Input.GetKeyDown(KeyCode.C) && _controllingEnemy == false)
            ChangeCharacter();
    }

    /// <summary>
    /// Deselects the current character and selects the other.
    /// </summary>
    private void ChangeCharacter()
    {
        if (_boy.IsSelected())
        {
            _selectedCharacter = _girl.gameObject;
            OnGirlSelect?.Invoke();
            OnFocusChange?.Invoke(_selectedCharacter);
        }
        else if (_girl.IsSelected() && !_girl.IsControllingEnemy())
        {
            _selectedCharacter = _boy.gameObject;
            OnBoySelect?.Invoke();
            OnFocusChange?.Invoke(_selectedCharacter);
        }
    }

    public GameObject GetSelectedCharacter()
    {
        return _selectedCharacter;
    }
}
