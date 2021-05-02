using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AIUnitScanner;

public class AIDecisionCA
{
    // AI Decision Counter Attack DTO class
    public UnitType _unitType;
    public int _lineID;
    public bool _upgrade = false;
    public AIDecisionCA(UnitType type, int lineIndex, bool upgrade)
    {
        _unitType = type;
        _lineID = lineIndex;
        _upgrade = upgrade;
    }
}

public class UnitTransferObject
{
    public UnitType _unitType;
    public float _cost;
    public float _dmg;
    public float _health;

    public UnitTransferObject(UnitType type, float cost, float dmg, float health)
    {
        _unitType = type;
        _cost = cost;
        _dmg = dmg;
        _health = health;
    }
}



public class AIBrain : MonoBehaviour
{
    public enum PlayMode
    {
        ATTACK,
        DEFENSE,
        TRAIN,
        UPGRADE_OUTPOST,
        IDLE
    }

    [SerializeField] AIUnitScanner aiUnitScanner;
    [SerializeField] GoldController _playerGoldController;
    [SerializeField] AttackDecisionData attackDecisionData;
    [SerializeField] DefenseDecisonData defenseDecisonData;

    private List<Line> _lines = new List<Line>();
    private GoldController _goldController;
    private Outpost _outpost;
    private UnitSpawner _unitSpawner;
    private PlayMode _currentMode = PlayMode.ATTACK;
    private int _unitsTrained;
    private int _minersSpawned;
    private const float UNDECISIVE = 500;
    private const float DEF_OVERSPAWN = 0.2f;
    private const float ATCK_OVERSPAWN = 1.2f;
    private bool _firstRound = true;

    /* TODO LIST: 
     * - sometimes is better to not do anything
     */


    public void Initialize(List<Line> lines, GoldController goldController, Outpost outpost)
    {
        _lines = lines;
        aiUnitScanner.Initialize(lines);
        _goldController = goldController;
        _outpost = outpost;
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    public List<AIDecisionCA> getNextDecisions()
    {
        var maxRatio = new Tuple<int, float>(1, 2);
        var minRatio = new Tuple<int, float>(3, 1);
        if (_firstRound || aiUnitScanner.LineUnitHolders.Count == 0)
        {
            _currentMode = PlayMode.ATTACK;
        } 
        else
        {
            // <line index, line ratio>
            List<Tuple<int, float>> lineHpDmgRatios = new List<Tuple<int, float>>();
            for (int i = 0; i < aiUnitScanner.LineUnitHolders.Count; i++)
            {
                var line = aiUnitScanner.LineUnitHolders[i];
                lineHpDmgRatios.Add(new Tuple<int,float> (i , GetAIEnemyHealthRatio(line) * GetAIEnemyDamageRatio(line)));
            }
            lineHpDmgRatios = lineHpDmgRatios.OrderBy(i => i.Item2).ToList();

             maxRatio = lineHpDmgRatios[lineHpDmgRatios.Count - 1];
             minRatio = lineHpDmgRatios[0];

            var attackViability = GetAttackViability(maxRatio.Item2);
            var defenseViability = Math.Abs(GetDefenseViability(minRatio.Item2));

            Debug.Log("Attack viability:" + attackViability);
            Debug.Log("Defense viability:" + defenseViability);

            if (attackViability > defenseViability)
            {
                _currentMode = PlayMode.ATTACK;
            }
            else
            {
                _currentMode = PlayMode.DEFENSE;
            }

            if (Math.Abs((attackViability - defenseViability)) <  UNDECISIVE)
            {
                if (_minersSpawned == 0)
                {
                    _currentMode = PlayMode.UPGRADE_OUTPOST;
                }
                else
                {
                    if (UnityEngine.Random.value >= 0.5)
                    {
                        _currentMode = PlayMode.UPGRADE_OUTPOST;
                    }
                    else
                    {
                        _currentMode = PlayMode.TRAIN;
                    }
                }
            }

        }

        List<AIDecisionCA> decisions = new List<AIDecisionCA>();
        switch (_currentMode)
        {
            case PlayMode.ATTACK:
                Debug.Log("ATTACK mode");
                decisions = PrepareToAttack(maxRatio.Item1);
                break;
            case PlayMode.DEFENSE:
                Debug.Log("DEFENSE mode");
                Debug.Log("***************/n Defensing line " + minRatio.Item1 + "/n ratio is: " + minRatio.Item2 + "/n*************");
                Debug.Log("***************/n Max ratio is on line " + maxRatio.Item1 + "/n ratio is: " + maxRatio.Item2 + "/n*************");
                decisions = PrepareToDefense(minRatio.Item1);
                break;
            case PlayMode.UPGRADE_OUTPOST:
                Debug.Log("Upgrading outpost mode");
                decisions = PrepareToUpgrade();
                break;
            case PlayMode.TRAIN:
                Debug.Log("TRAIN mode");
                decisions = PrepareToTrain();
                break;
        }
        return decisions;
    }

    private List<AIDecisionCA> PrepareToTrain()
    {
        var prefabs = _unitSpawner.GetUnitPrefabs();
        List<AIDecisionCA> decisions = new List<AIDecisionCA>();
        int rndTier = UnityEngine.Random.Range(0, 5); // inclusive with int

        var trainCost = prefabs[rndTier].GetUnitData(
            _unitSpawner.GetCurrentUnitTiers()[rndTier]).TrainCost;
        if (_goldController.GetBalance() > trainCost)
        {
            decisions.Add(new AIDecisionCA((UnitType)rndTier, 0, true));
        }
        return decisions;
    }

    private List<AIDecisionCA> PrepareToUpgrade()
    {
        List<AIDecisionCA> decisions = new List<AIDecisionCA>();
        if (_goldController.GetBalance() > _outpost.GetUpgradePrice())
        {
            decisions.Add(new AIDecisionCA(UnitType.MINER, 0, true));
            _minersSpawned++;
        }
        return decisions;
    }

    private List<AIDecisionCA> PrepareToAttack(int lineIndex)
    {
        var balance = _goldController.GetBalance();
        var units = GetAvailableUnits(balance);
        List<AIDecisionCA> decisions = new List<AIDecisionCA>();
        if (units.Count == 0)
        {
            Debug.Log("No attack units available");
        }

        List<UnitTransferObject> spawnedUnits = new List<UnitTransferObject>();
        var baseRatio = GetNewLineRatio(lineIndex, spawnedUnits);
        var ratio = 1000f;
        while (units.Count > 0 && (ratio/baseRatio) < ATCK_OVERSPAWN || _firstRound)
        {
            AIDecisionCA decision = new AIDecisionCA(units[0]._unitType, lineIndex, false);
            spawnedUnits.Add(units[0]);
            decisions.Add(decision);
            balance -= (int)units[0]._cost;
            ratio = GetNewLineRatio(lineIndex, spawnedUnits);
            units = GetAvailableUnits(balance);
            _firstRound = false;
        }
        return decisions;
    }

    private List<AIDecisionCA> PrepareToDefense(int lineIndex)
    {
        var balance = _goldController.GetBalance();
        var units = GetAvailableUnits(balance);
        List<AIDecisionCA> decisions = new List<AIDecisionCA>();
        if (units.Count == 0)
        {
            Debug.Log("No defense units available");
        }

        //int maxSpawn = 2;
        List<UnitTransferObject> spawnedUnits = new List<UnitTransferObject>();
        var baseRatio = GetNewLineRatio(lineIndex, spawnedUnits);
        var ratio = 1000f;
        while (units.Count > 0 && (ratio - baseRatio) > DEF_OVERSPAWN)
        {
            AIDecisionCA decision = new AIDecisionCA(units[0]._unitType, lineIndex, false);
            spawnedUnits.Add(units[0]);
            decisions.Add(decision);
            balance -= (int)units[0]._cost;
            ratio = GetNewLineRatio(lineIndex, spawnedUnits);
            units = GetAvailableUnits(balance);
        }
        return decisions;
    }

    private float GetNewLineRatio(int lineIndex, List<UnitTransferObject> newSpawnedUnits)
    {
        var health = GetAIEnemyHealthRatio(aiUnitScanner.LineUnitHolders[lineIndex], newSpawnedUnits);
        var dmg = GetAIEnemyDamageRatio(aiUnitScanner.LineUnitHolders[lineIndex], newSpawnedUnits);
        return health * dmg;
    }

    private List<UnitTransferObject> GetAvailableUnits(float balance)
    {
        var list = new List<UnitTransferObject>();
        var prefabs = _unitSpawner.GetUnitPrefabs();
        for (int i = 0; i < prefabs.Count ; i++)
        {
            var cost = prefabs[i].GetUnitData(_unitSpawner.GetCurrentUnitTiers()[i]).TrainCost;
            var type = prefabs[i].GetUnitData(_unitSpawner.GetCurrentUnitTiers()[i]).Type;
            var dmg = prefabs[i].GetUnitData(_unitSpawner.GetCurrentUnitTiers()[i]).Damage;
            var health = prefabs[i].GetUnitData(_unitSpawner.GetCurrentUnitTiers()[i]).HitPoints;
            if (cost <= balance && type != UnitType.MINER)
            {
                list.Add(new UnitTransferObject(type, cost, dmg, health));
            }
        }
        return list.OrderBy(a => UnityEngine.Random.Range(0, 100)).ToList(); // TODO is it shuffled?
    }

    private float GetAIEnemyHealthRatio(LineUnitHolder line)
    {
        float healthAISum = 0f;
        float healthEnemySum = 0f;

        foreach (var enemyUnit in line.enemyUnits)
        {
            healthEnemySum += enemyUnit.GetHealth();
        }

        foreach (var myUnit in line.myUnits)
        {
            healthAISum += myUnit.GetHealth();
        }

        if (healthEnemySum == 0 && healthAISum == 0)
        {
            // noone on the line
            return 0;
        }

        if (healthEnemySum != 0 && healthAISum == 0)
        {
            // only enemy on the line
            return -1;
        }

        if (healthEnemySum == 0)
        {
            // only AI on the line
            healthEnemySum = 1;
        }

        return healthAISum / healthEnemySum;
    }

    private float GetAIEnemyDamageRatio(LineUnitHolder line)
    {
        float dmgAISum = 0f;
        float dmgEnemySum = 0f;

        foreach (var enemyUnit in line.enemyUnits)
        {
            dmgEnemySum += enemyUnit.GetHealth();
        }

        foreach (var myUnit in line.myUnits)
        {
            dmgAISum += myUnit.GetHealth();
        }

        if (dmgEnemySum == 0 && dmgAISum == 0)
        {
            // noone on the line
            return 1;
        }

        if (dmgEnemySum != 0 && dmgAISum == 0)
        {
            // only enemy on the line
            return 1;
        }

        if (dmgEnemySum == 0)
        {
            // only AI on the line
            dmgEnemySum = 1;
        }

        return dmgAISum / dmgEnemySum;
    }

    private float GetAIEnemyHealthRatio(LineUnitHolder line, List<UnitTransferObject> newUnits)
    {
        float healthAISum = 0f;
        float healthEnemySum = 0f;

        foreach (var enemyUnit in line.enemyUnits)
        {
            healthEnemySum += enemyUnit.GetHealth();
        }

        foreach (var myUnit in line.myUnits)
        {
            healthAISum += myUnit.GetHealth();
        }

        foreach (var newUnit in newUnits)
        {
            healthAISum += newUnit._health;
        }

        if (healthEnemySum == 0 && healthAISum == 0)
        {
            // noone on the line
            return 0;
        }

        if (healthEnemySum != 0 && healthAISum == 0)
        {
            // only enemy on the line
            return -1;
        }

        if (healthEnemySum == 0)
        {
            // only AI on the line
            healthEnemySum = 1;
        }

        return healthAISum / healthEnemySum;
    }

    private float GetAIEnemyDamageRatio(LineUnitHolder line, List<UnitTransferObject> newUnits)
    {
        float dmgAISum = 0f;
        float dmgEnemySum = 0f;

        foreach (var enemyUnit in line.enemyUnits)
        {
            dmgEnemySum += enemyUnit.GetHealth();
        }

        foreach (var myUnit in line.myUnits)
        {
            dmgAISum += myUnit.GetHealth();
        }

        foreach (var newUnit in newUnits)
        {
            dmgAISum += newUnit._dmg;
        }

        if (dmgEnemySum == 0 && dmgAISum == 0)
        {
            // noone on the line
            return 1;
        }

        if (dmgEnemySum != 0 && dmgAISum == 0)
        {
            // only enemy on the line
            return 1;
        }

        if (dmgEnemySum == 0)
        {
            // only AI on the line
            dmgEnemySum = 1;
        }

        return dmgAISum / dmgEnemySum;
    }
    private float GetAttackViability(float maxLineRatio)
    {
        return (100/_playerGoldController.GetBalance()) * attackDecisionData.PlayerLowCashFactor
            + _unitsTrained * attackDecisionData.UnitsTrainedFactor
            + _minersSpawned * attackDecisionData.MinerSpawnedFactor
            + maxLineRatio * attackDecisionData.Health_dmgRatioFactor
            + _goldController.GetBalance() * attackDecisionData.AICashFactor;
    }

    private float GetDefenseViability(float minLineRatio)
    {
        int attackFactor = 0;
        if (_outpost.UnderAttack)
        {
            attackFactor = 1;
        }

        if (minLineRatio < 0)
        {
            minLineRatio = Math.Abs(minLineRatio) * 1000;
        }

        return 100/_outpost.GetHealth() * defenseDecisonData.AiBaseHealthFactor
            + minLineRatio * defenseDecisonData.Health_dmgRatioFactor
            + attackFactor * defenseDecisonData.CurrentAttackFactor;
    }
}