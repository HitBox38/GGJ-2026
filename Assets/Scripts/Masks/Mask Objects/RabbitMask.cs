using UnityEngine;

public class RabbitMask : MaskObject
{
    public Sprite rabbitMaskSprite;
    public float speedBoostAmount = 2f;
    public float jumpBoostAmount = 2f;

    private Sprite _originalSprite;

    public class RabbitAgility : MaskEffect
    {
        public float speedBoost;
        public float jumpBoost;
        private PlayerMovement _playerController;

        public void Initialize(PlayerMovement playerController, float speed, float jump)
        {
            // add a speed boost to the player
            _playerController = playerController;
            speedBoost = speed;
            jumpBoost = jump;

            if (_playerController != null)
            {
                // keep the sign of the speed modifier (direction)
                _playerController.SetSpeedModifier(Mathf.Sign(_playerController.GetSpeedModifier()) * speedBoost);
                _playerController.SetJumpModifier(jumpBoost);
            }
        }

        private void OnDestroy()
        {
            // remove the speed boost from the player
            if (_playerController != null)
            {
                _playerController.SetSpeedModifier((/* Mathf.Sign(_playerController.GetSpeedModifier()) * */
                                                     _playerController.GetSpeedModifier()) / 
                                                     speedBoost);
                _playerController.SetJumpModifier(1f);
            }
        }
    }

    public class PoisonVulnerability : MaskEffect
    {
        // increase poison duration on the player
        DizzyingCauldronHazard cauldron;
        private float originalEffectDelayExit;
        private float increasedDelayFactor = 1.5f;

        private void Start()
        {
            cauldron = FindObjectOfType<DizzyingCauldronHazard>();
            // increase poison effect duration on exit only!
            if (cauldron != null)
            {
                originalEffectDelayExit = cauldron.GetEffectDelayExit();
                cauldron.SetEffectDelayExit(originalEffectDelayExit + increasedDelayFactor);
            }
        }

        private void OnDestroy()
        {
            // restore original poison effect duration
            if (cauldron != null)
            {
                cauldron.SetEffectDelayExit(originalEffectDelayExit);
            }
        }
    }

    public override void Initialize(GameObject player)
    {
        base.Initialize(player);
        // change player sprite to rabbit mask sprite
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null && rabbitMaskSprite != null)
        {
            _originalSprite = playerSpriteRenderer.sprite;
            playerSpriteRenderer.sprite = rabbitMaskSprite;
        }
    }

    public override void ApplyEffects()
    {
        // add the rabbit agility buff component
        PlayerMovement playerController = player.GetComponent<PlayerMovement>();
        RabbitAgility agilityComp = AddBuff<RabbitAgility>();
        agilityComp.Initialize(playerController, speedBoostAmount, jumpBoostAmount);
        // add the poison vulnerability debuff component
        AddDebuff<PoisonVulnerability>();
    }

    protected override void OnDestroy()
    {
        // restore original sprite
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null && _originalSprite != null)
        {
            playerSpriteRenderer.sprite = _originalSprite;
        }
        base.OnDestroy();
    }
}
