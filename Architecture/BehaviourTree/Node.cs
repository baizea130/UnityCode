using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviourTreeCore
{
    public interface Istrategy
    {
        Node.Status Process();
        void Reset();
    }
    public class Sequence : Node
    {
        public Sequence(string name) : base(name) { }
        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running: return Status.Running;
                    case Status.Failure: { Reset(); return Status.Failure; }
                    default: return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }
            Reset();
            return Status.Success;
        }

    }
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }
        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                currentChild++;
            }
            return Status.Success;
        }
    }
    public class Leaf : Node
    {
        readonly Istrategy strategy;
        public Leaf(string name, Istrategy strategy) : base(name)
        {
            this.strategy = strategy;
        }
        public override Status Process() => strategy.Process();
        public override void Reset() => strategy.Reset();
    }
    public class Node
    {

        public enum Status { Success, Failure, Running }
        public readonly string name;
        public readonly List<Node> children = new();
        protected int currentChild;
        public Node(string name)
        {
            this.name = name;
        }
        public void AddChild(Node child) => children.Add(child);
        public virtual Status Process() => children[currentChild].Process();
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }
}