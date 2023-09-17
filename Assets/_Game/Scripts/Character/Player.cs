using System;
using CnControls;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Entity, IInteractWallObject
{
    [Title("Player Component")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Enemy target;
    [SerializeField] private PlayerCustomize playerCustomize;
    private Vector2 _input;

    private void Start()
    {
        OnEquipItem();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Ins.IsState(GameState.InGame)) return;
        switch (entityState)
        {
            case EntityState.Die:
                rb.velocity = Vector3.zero;
                OnDeSpawn();
                break;
            case EntityState.Alive:
            {
                _input = GetInputAxis();
                if (_input.sqrMagnitude < 0) return;
                if (CanMove(_input)) OnMove();
                else OnStop();
                break;
            }
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        gameObject.SetActive(true);
        skin.Tf.rotation = Quaternion.identity;
        CanAttack = true;
        // Test
        // Test Skill
        // SkillDic = new Dictionary<SkillType, Skill>();
        // MoreDamageSkill a = gameObject.AddComponent<MoreDamageSkill>();
        // a.SkillLevel = SkillLevel.One;
        // SkillDic.Add(SkillType.MoreDamage, a);
    }

    public void OnEquipItem()
    {
        OnEquipWeapon(GameData.Ins.PlayerData.WeaponType);
        playerCustomize.weapon.SetOwner(this);
        OnEquipHat(GameData.Ins.PlayerData.HatType);
        OnEquipPant(GameData.Ins.PlayerData.PantType);
    }

    public void OnFinishGame()
    {
        // LevelManager.Ins.OnWin();
    }

    private static Vector2 GetInputAxis()
    {
        return new Vector2(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"));
    }

    private static bool CanMove(Vector2 inputAxis)
    {
        return Mathf.Abs(inputAxis.y) > 0.01f || Mathf.Abs(inputAxis.x) > 0.01f;
    }

    private void OnMove()
    {
        CancelAttack();
        // Move
        Vector3 movement = new(_input.x, 0f, _input.y);
        if (movement != Vector3.zero)
        {
            Quaternion targetRos = Quaternion.LookRotation(movement);
            skin.Tf.rotation = Quaternion.Slerp(skin.Tf.rotation, targetRos, 10f * Time.deltaTime);
        }
        
        // Move by rigidbody velocity
        rb.velocity = movement * (entityData.moveSpeed * Time.deltaTime);
        
        // Tf.position += movement * (entityData.moveSpeed * Time.deltaTime);
        ChangeAnim(Constants.ANIM_RUN);
        SetCanAttack(true);
    }
    
    private void OnStop()
    {
        rb.velocity = Vector3.zero;
        target = LevelManager.Ins.GetNearestEnemy();
        if (target == null)
            ChangeAnim(Constants.ANIM_IDLE);
        else OnAttack();
    }

    private void OnAttack()
    {
        if (!CanAttack) return;
        CanAttack = false;
        IsCancelAttack = false;
        Utilities.LookTarget(skin.Tf, target.Tf);
        ChangeAnim(Constants.ANIM_ATTACK);
        Invoke(nameof(FireBullet), 0.4f);
    }

    protected override void DeSpawn()
    {
        LevelManager.Ins.OnPlayerDeath();
        gameObject.SetActive(false);
    }

    private void FireBullet()
    {
        if (Constants.ANIM_ATTACK != CurrentAnim) return;
        playerCustomize.weapon.OnFire(target, entityData.damage, entityData.bulletSpeed);
        ChangeAnim(Constants.ANIM_IDLE);
        CanAttack = true;
    }

    public void OnEquipWeapon(WeaponType weaponType)
    {
        if (playerCustomize.weapon != null) Destroy(playerCustomize.weapon.gameObject);
        playerCustomize.weapon = skin.OnEquipWeaponFromData(weaponType);
    }

    public void OnEquipHat(HatType hatType)
    {
        if (playerCustomize.hat != null) Destroy(playerCustomize.hat.gameObject);
        playerCustomize.hat = skin.OnEquipHatFromData(hatType);
    }

    public void OnEquipPant(PantType pantType)
    {
        // playerCustomize.pant = null;
        playerCustomize.pant = skin.OnEquipPantFromData(pantType);
    }

    // private void UpgradeSkill(Skill skill)
    // {
    //     if (!SkillDic.ContainsKey(skill.SkillType))
    //     {
    //         SkillDic.Add(skill.SkillType, skill);
    //         SkillDic[skill.SkillType].OnInit();
    //     }
    //     else
    //     {
    //         SkillDic[skill.SkillType].UpgradeSkill();
    //     }
    // }
    public void OnHitWall()
    { }
}

[Serializable]
public class PlayerCustomize
{
    public Weapon weapon;
    public Hat hat;
    public Material pant;
}