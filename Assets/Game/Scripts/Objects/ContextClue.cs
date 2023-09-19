using UnityEngine;

public class ContextClue : MonoBehaviour
{
    [SerializeField] private GameObject _context;

    public void ChangeContext()
    {
        _context.SetActive(!_context.activeSelf);
    }
}