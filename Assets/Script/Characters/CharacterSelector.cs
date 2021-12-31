using UnityEngine;
public class CharacterSelector : MonoBehaviour
{
    private bool _controllingEnemy;

    private Boy _boy;

    private Girl _girl;
    
    public delegate void CharacterChange();
    
    public event CharacterChange OnBoySelect;
    public event CharacterChange OnGirlSelect;
    public event CharacterChange OnCharacterChange;

    public delegate void FocusChange(GameObject target);
    public event FocusChange OnFocusChange;
    // Start is called before the first frame update
    void Start()
    {
        _boy = FindObjectOfType<Boy>();
        _girl = FindObjectOfType<Girl>();
        
        _boy.Deselect();
        _girl.Select();
        OnGirlSelect?.Invoke();
        OnFocusChange?.Invoke(_girl.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("if boy");
            OnGirlSelect?.Invoke();
            OnFocusChange?.Invoke(_girl.gameObject);
        }
        else if (_girl.IsSelected())
        {
            Debug.Log("if girl");
            OnBoySelect?.Invoke();
            OnFocusChange?.Invoke(_boy.gameObject);
        }
    }
}
