using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace BehaviourTreeCore
{
    public interface Istrategy
    {
        Node.Status Process();
        void Reset();
    }
    public class WeightedRandomSelector : Node
    {
        int chosenIndex = -1;
        List<int> weightList = new List<int>();
        public WeightedRandomSelector(string name) : base(name) { }
        public override Status Process()
        {
            if (children.Count == 0) return Status.Failure;
            if (chosenIndex == -1) chosenIndex = initWeight();
            if (chosenIndex < 0 || chosenIndex >= children.Count) { chosenIndex = -1; return Status.Failure; }

            var status = children[chosenIndex].Process();
            if (status == Status.Running) return Status.Running;

            chosenIndex = -1; // 本次决策完成，下一次重新抽取
            return status;
        }
        public int initWeight()
        {
            int current = 0;
            int target = 0;
            int total = 0;
            foreach (var item in weightList)
            {
                total += item;
            }
            target = UnityEngine.Random.Range(0, total);
            for (int i = 0; i < weightList.Count; i++)
            {
                current += weightList[i];
                if (current > target)
                {
                    return i;
                }
            }
            return -1;
        }
        public override void Reset()
        {
            currentChild = 0;
            chosenIndex = -1;
            base.Reset();
        }
        public override void AddChild(Node child, int weight)
        {
            children.Add(child);
            weightList.Add(Mathf.Max(1,weight));
        }
        public override void AddChild(Node child) => Debug.LogError($"Child [{child.name}] don't have match weight");
    }
    public class Selector : Node
    {
        public Selector(string name) : base(name) { }
        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Success: { Reset(); return Status.Success; }
                    default: return Status.Running;
                }
            }
            Reset();
            return Status.Failure;
        }
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
    public class Loop : Node
    {
        readonly int times;
        int currentTime = 0;
        public Loop(string name, int times = -1) : base(name)
        {
            this.times = times;
        }
        public override Status Process()
        {
            if (children.Count == 0) return Status.Failure;
            if (times > 0 && currentTime > times)
            {
                Reset();
                return Status.Success;
            }
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if (status == Status.Running) return Status.Running;
                if (status == Status.Failure) { Reset(); return Status.Failure; }
                // Success -> 继续下一个子节点
                currentChild++;
            }

            // 完成一轮
            currentTime++;

            if (times >= 0 && currentTime >= times)
            {
                Reset();
                return Status.Success;
            }

            // 重置子节点以开始下一轮
            for (int i = 0; i < children.Count; i++) children[i].Reset();
            currentChild = 0;
            return Status.Running;
        }
        public override void Reset()
        {
            currentTime = 0;
            currentChild = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
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
        public virtual void AddChild(Node child) => children.Add(child);
        public virtual void AddChild(Node child, int weight) { }
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