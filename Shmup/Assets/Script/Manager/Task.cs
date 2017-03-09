using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task: MonoBehaviour {
    public enum TaskStatus{
        Detached,
        Pending,
        Working,
        Success,
        Fail,
        Aborted
    }
    void Start()
    {
        SetStatus(TaskStatus.Detached);
    }
    public TaskStatus Status{
        get; private set;
    }
    public bool ifDetached{get{return Status == TaskStatus.Detached;}}
    public bool ifPending{get{return Status == TaskStatus.Pending;}}
    public bool ifWorking{get{return Status == TaskStatus.Working;}}
    public bool ifSuccess{get{return Status == TaskStatus.Success;}}
    public bool ifFail{get{return Status == TaskStatus.Fail;}}
    public bool ifAborted{get{return Status == TaskStatus.Aborted;}}
    public bool ifFinished{get{return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted);}}
    public Task NextTask;

    internal void SetStatus(TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;

        switch (newStatus)
        {
            case TaskStatus.Working:
                // Initialize the task when the Task first starts
                // It's important to separate initialization from
                // the constructor, since tasks may not start
                // running until long after they've been constructed
                Init();
                break;

            // Success/Aborted/Failed are the completed states of a task.
            // Subclasses are notified when entering one of these states
            // and are given the opportunity to do any clean up
            case TaskStatus.Success:
                OnSuccess();
                CleanUp();
                break;

            case TaskStatus.Aborted:
                OnAbort();
                CleanUp();
                break;

            case TaskStatus.Fail:
                OnFail();
                CleanUp();
                break;

            // These are "internal" states that are mostly relevant for
            // the task manager
            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                Debug.Log("This Status is Null or not Available");
                break;
        }
    }

    protected virtual void Init()
    {
        Debug.Log("This is" + typeof(Task).ToString()+" Initialized");
    }
    protected virtual void OnSuccess()
    {}
    protected virtual void CleanUp()
    {}
    protected virtual void OnAbort()
    {}
    protected virtual void OnFail()
    {}
    internal virtual void TUpdate()
    {}
}
