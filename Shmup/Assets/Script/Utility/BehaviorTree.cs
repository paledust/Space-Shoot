using System;


namespace BehaviorTree {
	public abstract class Node<T>
	{
		public abstract bool Update(T Context);
	}
	public class Tree<T> : Node<T>
	{
		private readonly Node<T> _root;
		public Tree(Node<T> root)
		{
			_root = root;
		}
		public override bool Update(T Context)
		{
			return _root.Update(Context);
		}
	}
	public class Do<T> : Node<T>
	{
		public delegate bool NodeAction(T Context);
		private readonly NodeAction _action;
		public Do(NodeAction action)
		{
			_action = action;
		}
		public override bool Update(T Context)
		{
			return _action(Context);
		}
	}
	public class Condition<T>: Node<T>
	{
		private readonly Predicate<T> _condition;
		public Condition(Predicate<T> condition)
		{
			_condition = condition;
		}
		public override bool Update(T Context)
		{
			return _condition(Context);
		}
	}
	public abstract class BranchNode<T> : Node<T>
	{
		protected Node<T>[] Children {get; private set;}
		protected BranchNode(params Node<T>[] children)
		{
			Children = children;
		}
	}
	public class Selector<T> : BranchNode<T>
	{
		public Selector(params Node<T>[] Children): base(Children){}

		public override bool Update(T Context)
		{
			foreach(Node<T> child in Children)
			{
				if(child.Update(Context)) return true;
			}
			return false;
		}
	}
	public class Sequence<T>: BranchNode<T>
	{
		public Sequence(params Node<T>[] Children): base(Children){}

		public override bool Update(T Context)
		{
			foreach(Node<T> child in Children)
			{
				if(!child.Update(Context)) return false;
			}
			return true;
		}
	}
	public abstract class Decorator<T>: Node<T>
	{
		protected Node<T> Child{get; private set;}

		protected Decorator(Node<T> child)
		{
			Child = child;
		}
	}
	public class Not<T>: Decorator<T>
	{
		public Not(Node<T> child): base(child){}
		public override bool Update(T Context)
		{
			return !Child.Update(Context);
		}
	}
}
