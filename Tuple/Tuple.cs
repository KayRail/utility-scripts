using Godot;
using System;

[Serializable]
public partial class Tuple<T,U> : GodotObject
{
	public T Item1 { get;  set; }
	public U Item2 { get;  set; }
	
	public Tuple() { }
	public Tuple(T item1, U item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}

public static class Tuple
{
	public static Tuple<T, U> Create<T, U>(T item1, U item2)
	{
		return new Tuple<T, U>(item1, item2);
	}
}
