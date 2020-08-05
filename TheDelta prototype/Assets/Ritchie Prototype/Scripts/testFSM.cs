using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testFSM : IBrainFSM
{
    // Start is called before the first frame update
    void Start()
    {
        RegisterState(new FirstCombo(this));
        ChangeState(StateType.Combo1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBrain();
    }

    
}
public class FirstCombo : IState
{
    public FirstCombo(IBrainFSM brain) : base(brain)
    {

    }
    public override StateType StateType => StateType.Combo1;

    public override void OnStateEnter(object[] args)
    {
        
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
