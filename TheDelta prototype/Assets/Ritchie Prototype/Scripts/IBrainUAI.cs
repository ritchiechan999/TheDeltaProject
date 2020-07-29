using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class IBrainUAI : MonoBehaviour
{
    public float DefaultReThinkTime = 1f;
    public ReThinkType DefaultReThinkType = ReThinkType.PerNTime;
    float _currentReThinkTime;
    ITask _currentTask;

    protected List<ITask> _tasks = new List<ITask>();
    public ReThinkType ReThinkFrequancy
    {
        get
        {
            if (_currentTask == null)
                return DefaultReThinkType;
            else
                return _currentTask.ReThinkType;
        }
    }

    public void RegisterTask(ITask task)
    {
        _tasks.Add(task);
    }

    public void UpdateBrain()
    {
        ReThinkType type = ReThinkFrequancy;
        if(type == ReThinkType.PerNTime)
        {
            _currentReThinkTime -= Time.deltaTime;
            if(_currentReThinkTime <= 0)
            {
                ReThink();
            }
        }
        else if(type == ReThinkType.PerUpdate)
        {
            ReThink();
        }

        _currentTask?.OnTaskUpdate();
    }
    public void TaskSucceeded()
    {
        _currentTask?.OnTaskDone();
        if (_currentTask.ReThinkType == ReThinkType.OnTaskDone)
            ReThink();
    }
    public void TaskFailed()
    {
        _currentTask?.OnTaskInterrupted();
        if (_currentTask.ReThinkType == ReThinkType.OnTaskDone)
            ReThink();
    }
    public void ReThink()
    {
        _currentReThinkTime = DefaultReThinkTime;

        foreach(ITask task in _tasks)
        {
            task.Analyze();
        }
        _tasks = _tasks.OrderByDescending((x) =>
        {
            return x.TotalWeight;
        }).ToList();

        ITask newtask = _tasks[0];
        if (_currentTask == newtask)
            return;

        _currentTask?.OnTaskExit();

        ITask previoustask = _currentTask;
        _currentTask = newtask;

        newtask.OnTaskEnter(previoustask);
    }
}

public enum ReThinkType
{
    Default,
    PerUpdate,
    PerNTime,
    OnTaskDone,
}

public abstract class ITask
{
    public float Weight { get; protected set; }
    public float Motivator { get; protected set; }
    public float TotalWeight { get{ return Weight * Motivator; } }
    protected IBrainUAI Machine;
    public ITask(IBrainUAI machine)
    {
        Machine = machine;
        Motivator = 1f;
    }
    public abstract ReThinkType ReThinkType { get; }
    public abstract string Name { get; }
    public abstract void Analyze();
    public virtual void OnTaskEnter(ITask previoustask) { }
    public virtual void OnTaskUpdate() { }
    public virtual void OnTaskExit() { }
    public virtual void OnTaskDone() { }
    public virtual void OnTaskInterrupted() { }
}
