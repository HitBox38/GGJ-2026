using UnityEngine;
using System.Collections;

public class GasMask : MaskObject
{
    public Sprite gasMaskSprite;
    public float oxygenDuration = 10f; // duration in seconds the gas mask provides oxygen
    private SpriteRenderer _playerSpriteRenderer;
    private Sprite _originalSprite;

    public class ConfusionImmune : MaskEffect
    {
        private Collider2D _playerCollider;
        private Collider2D _cauldronCollider;
        // This class can be expanded to include properties or methods
        // that define the behavior of confusion immunity.
        public void Initialize(Collider2D playerCollider)
        {
            // exclude the player layer from the cauldron's confusion effect
            DizzyingCauldronHazard cauldron = FindObjectOfType<DizzyingCauldronHazard>();
            if (cauldron != null)
            {
                _cauldronCollider = cauldron.GetComponent<Collider2D>();
            }
            if (playerCollider != null && _cauldronCollider != null)
            {
                _playerCollider = playerCollider;
                Physics2D.IgnoreCollision(playerCollider, _cauldronCollider, true);
            }
        }

        private void OnDestroy()
        {
            // re-enable collision when the effect is destroyed
            if (_playerCollider != null && _cauldronCollider != null)
            {
                Physics2D.IgnoreCollision(_playerCollider, _cauldronCollider, false);
            }
        }
    }

    public class ShortBurst : MaskEffect
    {
        private GasMask _maskInstance;
        private float _duration;
        private GameObject _player;

        public void Initialize(GasMask maskInstance, float duration, GameObject player)
        {
            _maskInstance = maskInstance;
            _duration = duration;
            _player = player;
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            print("Removing gas mask after oxygen duration ended.");
            // start the oxygen duration countdown
            yield return new WaitForSeconds(_duration);
            

            // destroy the mask after duration ends
            if (_maskInstance != null)
            {
                // get the mask handler (safer to find it in scene if it might not be a child)
                MaskHandler maskHandler = FindObjectOfType<MaskHandler>();
                if (maskHandler != null)
                {
                    maskHandler.RemoveMask();
                }
            }
        }   
    }
    public override void Initialize(GameObject player)
    {
        base.Initialize(player);
        // change player sprite to gas mask sprite
        _playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (_playerSpriteRenderer != null && gasMaskSprite != null)
        {
            _originalSprite = _playerSpriteRenderer.sprite;
            _playerSpriteRenderer.sprite = gasMaskSprite;
        }
    }

    public override void ApplyEffects()
    {
        // add the confusion immunity buff component
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        ConfusionImmune immunityComp = AddBuff<ConfusionImmune>();
        immunityComp.Initialize(playerCollider);
        // add the short burst debuff component
        ShortBurst burstComp = AddDebuff<ShortBurst>();
        burstComp.Initialize(this, oxygenDuration, player);
    }

    protected override void OnDestroy()
    {
        // restore original sprite
        if (_playerSpriteRenderer != null && _originalSprite != null)
        {
            _playerSpriteRenderer.sprite = _originalSprite;
        }
        base.OnDestroy();
    }
}
