using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MinionUAI : IBrainUAI
{

    public float adjustHpPos;
    public Slider showHealth;

    public float MinionDamage;
    public bool isSummon;
    
    public float IdleTimer;
    public Transform Targets;
    
   
    public string PlayerName = "player";
    public string EnemyName = "Enemy";
    public string Name;

    public bool isDetect;
    public int Layer => 1 << LayerMask.NameToLayer(Name);

    //health
    public float Mhealth = 20;
    public float Health => Mhealth;

    public int DetectionRadius;
    public Transform TargetMinion;
    public NavMeshAgent Agent;
    

    public float AttackingRangeRadius;
    public int RandomPosRadius;
    public Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        Agent = this.GetComponent<NavMeshAgent>();
        RegisterTask(new IdleTask(this));
        RegisterTask(new GotoTask(this));
        RegisterTask(new DetectMinion(this));
        RegisterTask(new Attacking(this));
        IdleTimer = 3f;

        showHealth.maxValue = Mhealth;
    }


    void Update()
    {
        showHealth.transform.position = new Vector3(this.transform.position.x, adjustHpPos, this.transform.position.z);
        showHealth.value = Mhealth;

        UpdateBrain();

    }
    private void OnGUI()
    {
        int currenty = 0;
        foreach (ITask t in _tasks)
        {
            string weight = t.Name + ":" + t.TotalWeight.ToString();
            GUI.Box(new Rect(0, currenty, 500, 100), weight);
            currenty += 100;
        }
    }
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, DetectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, RandomPosRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, AttackingRangeRadius);
    }

    public void DoDamage(float damage)
    {
        Mhealth -= damage;
        Debug.Log(Mhealth);
    }

  
}


public abstract class IBaseTask : ITask
{
    public MinionUAI Brain;
    public IBaseTask(IBrainUAI machine) : base(machine)
    {
        Brain = (MinionUAI)machine;
    }
}
public class IdleTask : IBaseTask
{
    public IdleTask(IBrainUAI machine) : base(machine)
    {

    }
    public override ReThinkType ReThinkType => ReThinkType.PerNTime;

    public override string Name => "Idle";

    public override void Analyze()
    {
        Debug.Log("idle");
        if(Brain.IdleTimer <= 0)
            Weight = 0;
        else
            Weight = 15f;
    }

    public override void OnTaskEnter(ITask previoustask)
    {
        Brain.anim.SetFloat("speed", 0);
    }
    public override void OnTaskUpdate()
    {
        Brain.Agent.SetDestination(Brain.transform.position);
        Brain.IdleTimer -= Time.deltaTime;
        
      
    }
}

public class GotoTask : IBaseTask
{
    public GotoTask(IBrainUAI machine) : base(machine)
    {

    }
    public override ReThinkType ReThinkType => ReThinkType.PerNTime;

    public override string Name => "Goto";

    public override void Analyze()
    {
        Debug.Log("Goto");

        Vector3 targetpos = Brain.RandomNavmeshLocation(Brain.RandomPosRadius);
        targetpos.y = Brain.transform.position.y;
        if (Vector3.Distance(Brain.transform.position, targetpos) < 1f)
        {
            Debug.Log("weight1");
            
            Weight = 0;

            
        }
        else
            Weight = 10;

    }

    public override void OnTaskEnter(ITask previoustask)
    {
        
       

    }
    public override void OnTaskUpdate()
    {
        

    }
    public override void OnTaskExit()
    {
        //Brain.anim.SetFloat("speed", 0);
    }
    public override void OnTaskDone()
    {
       //Brain.IdleTimer = 1f;//do not hardcode in actual projects, only for learning purpose
    }
    public override void OnTaskInterrupted()
    {
        
    }
}
public class DetectMinion : IBaseTask
{
    public DetectMinion(IBrainUAI machine) : base(machine)
    {

    }
    public override ReThinkType ReThinkType => ReThinkType.PerNTime;

    public override string Name => "DetectMinion";

    public override void Analyze()
    {
        

    }
    public override void OnTaskUpdate()
    {
        
    }
    public override void OnTaskEnter(ITask previoustask)
    {
        
    }
    public override void OnTaskExit()
    {
      
    }
}
public class Attacking : IBaseTask
{
    public Attacking(IBrainUAI machine) : base(machine)
    {

    }
    public override ReThinkType ReThinkType => ReThinkType.PerNTime;

    public override string Name => "Attacking";

    public override void Analyze()
    {
        //Collider[] cols = Physics.OverlapSphere(Brain.transform.position, Brain.AttackingRangeRadius, Brain.Layer);

        //if (cols.Length != 0)
        //{



        //    for (int i = 0; i < cols.Length; i++)
        //    {
        //        if (cols[i] == Brain.gameObject)


        //            continue;

        //        Brain.TargetMinion = cols[i].transform;
        //        Weight = 40f;

        //    }
        //}
        //else
        //{
        //    Weight = 0f;
        //}

    }
    public override void OnTaskEnter(ITask previoustask)
    {
        
    }
    public override void OnTaskUpdate()
    {
        
    }
    public override void OnTaskExit()
    {

    }
}
