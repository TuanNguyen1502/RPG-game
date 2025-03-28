using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    [SerializeField] private PlayerStats playerStats;


    protected override void Start()
    {
        base.Start();

        player= GetComponent<Player>();

    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostScoreAmount = PlayerManager.instance.score;
        PlayerManager.instance.score = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (isDead)
            return;

        if (_damage > GetMaxHealthValue() * .3f )
        {
            player.SetupKnockbackPower(new Vector2(10,6));
            player.fx.ScreenShake(player.fx.shakeHighDamage);


            int randomSound = Random.Range(34, 35);
            AudioManager.instance.PlaySFX(randomSound, null);
            
        }

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }


    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        int totalDamage = damage.GetValue();

        if (_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats); // remove if you don't want to apply magic hit on primary attack
    }
}
