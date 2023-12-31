using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Entity : GameUnit
{
    [Title("Base Component")] [SerializeField]
    protected Skin skin;

    [SerializeField] protected Animator anim;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] protected Collider entityCollider;


    [Title("Entity Data")] [SerializeField]
    protected EntityPrimitiveData entityPrimitiveData;


    [SerializeField] protected EntityData entityData;

    public bool notBeDetected;
    protected bool CanAttack;
    protected string CurrentAnim = " ";
    protected bool IsCancelAttack;
    public Dictionary<SkillType, Skill> SkillDic { get; protected set; }

    private void Start()
    {
        GameManager.Ins.RegisterListenerEvent(EventID.Pause, OnPause);
        GameManager.Ins.RegisterListenerEvent(EventID.UnPause, OnUnPause);
    }

    protected virtual void OnPause()
    {
        anim.speed = 0;
    }

    protected virtual void OnUnPause()
    {
        anim.speed = 1f;
    }

    public bool IsDie()
    {
        return entityData.health <= 0;
    }

    private void GetPrimitiveData()
    {
        entityData.health = entityPrimitiveData.health;
        entityData.damage = entityPrimitiveData.damage;
        entityData.bulletSpeed = entityPrimitiveData.bulletSpeed;
        entityData.attackSpeed = entityPrimitiveData.attackSpeed;
        entityData.moveSpeed = entityPrimitiveData.moveSpeed;
        entityData.deSpawnTime = entityPrimitiveData.deSpawnTime;
    }

    public virtual void OnInit()
    {
        GetPrimitiveData();
        healthBar.OnInit();
        skin.OnInit();
        entityCollider.enabled = true;
    }

    public virtual void OnHit(int damageHit)
    {
        // MORE: Add effect when hit
        if (IsDie()) return;
        entityData.health -= damageHit;
        healthBar.OnChangeHealthBar(damageHit);

        if (IsDie()) OnDie();
    }

    protected virtual void OnDie()
    {
        ChangeAnim(Constants.ANIM_DIE);
        entityCollider.enabled = false;
        DOVirtual.DelayedCall(entityData.deSpawnTime, DeSpawn);
    }

    protected void OnDeSpawn()
    {
        if (entityData.deSpawnTime > 0)
        {
            entityData.deSpawnTime -= Time.deltaTime;
            return;
        }

        DeSpawn();
    }

    protected abstract void DeSpawn();

    protected void ChangeAnim(string animName)
    {
        if (CurrentAnim == animName) return;
        anim.ResetTrigger(CurrentAnim);
        CurrentAnim = animName;
        anim.SetTrigger(CurrentAnim);
    }

    protected void ChangeAnimWithOutCheckCurrent(string animName)
    {
        anim.ResetTrigger(CurrentAnim);
        CurrentAnim = animName;
        anim.SetTrigger(CurrentAnim);
    }

    public int GetHealth()
    {
        return entityData.health;
    }

    public int GetMaxHealth()
    {
        return entityPrimitiveData.health;
    }

    public Vector3 GetSkinPosition()
    {
        return skin.Tf.position;
    }

    public void SetHealth(int health)
    {
        entityData.health = health;
        healthBar.OnChangeHealthBar();
    }

    [Serializable]
    protected class SpawnBulletPointAndDirection
    {
        public Transform spawnPoint;
        public Transform direction;
    }

    [Serializable]
    protected class EntityData
    {
        public int health;
        public int damage;
        public float bulletSpeed;
        public float attackSpeed;
        public float moveSpeed;
        public float deSpawnTime;
    }
}
