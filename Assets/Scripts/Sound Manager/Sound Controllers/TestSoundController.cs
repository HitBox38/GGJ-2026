using UnityEngine;
using UnityEngine.InputSystem;

public class TestSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip testSoundClip;
    
    private InputAction _interactAction;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    private void Update()
    {
        if (_interactAction.triggered)
        {
            _audioSource.PlayOneShot(testSoundClip);
        }
    }
}
