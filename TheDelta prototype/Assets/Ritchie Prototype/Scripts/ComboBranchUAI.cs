using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ComboBranchUAI : IBrainUAI
{

    public Animator anim;
    public tdBasicAttack BasicAttack;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        BasicAttack = this.GetComponent<tdBasicAttack>();
        RegisterTask(new IdleTask(this));
        RegisterTask(new GotoTask(this));
        RegisterTask(new DetectMinion(this));
        RegisterTask(new Attacking(this));
        
    }


    void Update()
    {
        
        UpdateBrain();

        if(BasicAttack.ComboNumber == 1)
        {
            Debug.Log("first combo attakc");
        }
        else if(BasicAttack.ComboNumber == 2)
        {
            Debug.Log("second combo attakc");
        }
        else if (BasicAttack.ComboNumber == 3)
        {
            Debug.Log("third combo attack");
        }
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
  
    }

   

  
}


public abstract class IBaseTask : ITask
{
    public ComboBranchUAI Brain;
    public IBaseTask(IBrainUAI machine) : base(machine)
    {
        Brain = (ComboBranchUAI)machine;
    }
}
public class IdleTask : IBaseTask
{
    public IdleTask(IBrainUAI machine) : base(machine)
    {

    }
    public override ReThinkType ReThinkType => ReThinkType.PerUpdate;

    public override string Name => "Idle";

    public override void Analyze()
    {
        //Brain.BasicAttack.ComboNumber = 1;

        //Weight = 30;
    }

    public override void OnTaskEnter(ITask previoustask)
    {
      //UI selection pop out from here maybe?
    }
    public override void OnTaskUpdate()
    {
        Debug.Log("first combo attakc");
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
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
        //Brain.BasicAttack.ComboNumber = 2;

        //Weight = 20;

    }

    public override void OnTaskEnter(ITask previoustask)
    {
        
       

    }
    public override void OnTaskUpdate()
    {
        Debug.Log("second combo attakc");

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
        //Brain.BasicAttack.ComboNumber = 3;

        //Weight = 10;

    }
    public override void OnTaskUpdate()
    {
        Debug.Log("third combo attakc");
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
