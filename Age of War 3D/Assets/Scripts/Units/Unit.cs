using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Unit : MonoBehaviour, IDamagable
{
    [SerializeField] HealthSlider unitHealth;
    [SerializeField] protected ColorSwitcher colorSwitcher;
    [SerializeField] protected List<UnitData> unitTiers;
    [SerializeField] protected GameObject dmgTakenText;

    [Header("Animations")]
    [SerializeField] protected Animator animator;
    [SerializeField] AnimationClip attackAnimationClip;
    [SerializeField] AnimationClip deathAnimationClip;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackClip;
    [SerializeField] List<AudioClip> deathClips;

    private bool _isAttacking;
    private bool _isDead;

    protected UnitData _unitData;
    protected float HEIGHT_OFFSET = 2.2f;

    protected Vector3 _destination;
    private int _nextCheckpointIndex;

    private bool _areSoundsEffectsOn;

    public Unit EnemyInRange { get; set; }
    public Unit UnitInMovementRange { get; set; }
    public Outpost OutpostInRange { get; set; }

    public UnityEvent<Unit> OnUnitDeath { get; set; }
    
    public int NumberOfUnitTiers => unitTiers.Count;
    public Line Line { get; private set; }
    public int UnitID { get; private set; }
    public FactionEnum Faction { get; protected set; }
    public Transform Transform { get; private set; }

    private void Awake()
    {
        _isAttacking = false;

        Transform = transform;
        OnUnitDeath = new UnityEvent<Unit>();

        _areSoundsEffectsOn = PlayerPrefs.GetInt("Sound Effects") == 1;
    }

    public UnitData GetUnitData(int currentTier)
    {
        return unitTiers[currentTier];
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    public int GetUnitReward()
    {
        return _unitData.Reward;
    }

    public void Initialize(int unitID, int unitTier, Line line, FactionEnum faction, Material factionMaterial)
    {
        UnitID = unitID;

        if (unitTier < 0 || unitTier > unitTiers.Count - 1)
        {
            _unitData = unitTiers[0];
        }
        else
        {
            _unitData = unitTiers[unitTier];
        }

        unitHealth.Initialize(_unitData.HitPoints, false);

        Line = line;
        Faction = faction;

        transform.position = Line.GetSpawnPointPosition(Faction);
        _nextCheckpointIndex = 0;
        _destination = Line.GetCheckpointPosition(_nextCheckpointIndex, Faction);
        transform.LookAt(_destination);

        if (colorSwitcher != null)
        {
            colorSwitcher.SwitchColors(factionMaterial.color);
        }
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (_isAttacking == false)
        {
            if (UnitInMovementRange == null)
            {
                MoveTowardsObjective();
                animator.SetFloat("Speed", 1);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }

            if (EnemyInRange != null || OutpostInRange != null)
            {
                StartCoroutine(AttackMode());
            }
        }
    }

    protected virtual void MoveTowardsObjective()
    {
        if (_destination == Vector3.zero)
            return;

        if (transform.position == _destination)
        {
            if (Line.HasNextCheckpoint(_nextCheckpointIndex, Faction) == false)
            {
                return;
            }

            _destination = Line.GetCheckpointPosition(_nextCheckpointIndex++, Faction);
            transform.LookAt(_destination);
        }

        transform.position = Vector3.MoveTowards(transform.position, _destination, _unitData.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator AttackMode()
    {
        _isAttacking = true;

        if (_isDead)
            yield break;

        animator.Play("Attack");

        while (true)
        {
            if (_isDead)
            {
                break;
            }

            //prioritize enemies before outpost
            if (EnemyInRange != null)
            {
                Attack(EnemyInRange);
            }
            else if (OutpostInRange != null)
            {
                Attack(OutpostInRange);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(attackAnimationClip.length);
        }

        if (_isDead)
            yield break;

        animator.Play("Walk");
        _isAttacking = false;
    }

    private void Attack(IDamagable damagable)
    {
        if (_areSoundsEffectsOn)
        {
            audioSource.clip = attackClip;
            audioSource.Play();
        }

        DealDamage(damagable);
    }

    protected virtual void DealDamage(IDamagable damagable)
    {
    }

    public virtual void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        unitHealth.SubtractHealth(damage);
        
        if (unitHealth.Health <= 0)
        {
            _isDead = true;

            EventManager.Instance.ExecuteEvent<IUnitDead>((x, y) => x.OnUnitDead(this));

            animator.Play(deathAnimationClip.name);

            if (_areSoundsEffectsOn)
            {
                audioSource.clip = deathClips[UnityEngine.Random.Range(0, deathClips.Count)];
                audioSource.Play();
            }

            //disable colliders
            var colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
                collider.enabled = false;

            Destroy(gameObject, deathAnimationClip.length + 1f);
        }
        var position = transform.position;
        position.Set(position.x, position.y + HEIGHT_OFFSET, position.z);
        var text_holder = Instantiate(dmgTakenText, position, Quaternion.identity);
        text_holder.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("- " + damage);
    }

    public float GetHealth()
    {
        return unitHealth.Health;
    }

    public float GetDamage()
    {
        return _unitData.Damage;
    }

    protected float getRandomizedDamage(float unitDamage)
    {
        float percentage = unitDamage * 0.1f;
        decimal acutal_dmg = (decimal)UnityEngine.Random.Range(unitDamage - percentage, unitDamage + percentage);
        float rounded = Convert.ToSingle(RoundDown(acutal_dmg, 1));
        return rounded;
    }

    private decimal RoundDown(decimal i, double decimalPlaces)
    {
        var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
        return Math.Floor(i * power) / power;
    }
}
