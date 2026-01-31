using UnityEngine;

public class WitchMask : MaskObject
{
    [SerializeField] private float debuffSpeedModifier = 0.5f;
    
    public float DebuffSpeedModifier => debuffSpeedModifier;
    
    public class WitchMaskBuff : MaskEffect
    {
        public bool IsMoving { get; private set; }
        
        private Rigidbody2D _rb2d;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            IsMoving = _rb2d.linearVelocity.sqrMagnitude > 0.01f;
        }
    }
    
    // public class WitchMaskDebuff : MaskEffect {}
    
    public override void ApplyEffects()
    {
        // add the goose friend buff component
        AddBuff<WitchMaskBuff>();
        /*AddDebuff<WitchMaskDebuff>();*/
    }
}