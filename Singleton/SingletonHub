using Godot;
using Godot.Collections;

public static class SingletonHub{

    public static Dictionary<string, GodotObject> singletonFinder = new Dictionary<string, GodotObject>();

    private static Dictionary<string, Tuple<string, string>> scenePaths = new Dictionary<string, Tuple<string, string>>()
    {
        { "ExampleController", Tuple.Create("res://Scenes/example.tscn", "/path/") },
    };

    public static T AddSingleton<T>(T node) where T : Node
    {
        if (singletonFinder.ContainsKey(typeof(T).Name))
        {
            node.GetParent().AddChild((T)singletonFinder[typeof(T).Name]);
            node.Free();
            return (T)singletonFinder[typeof(T).Name];
        }

        singletonFinder.Add(typeof(T).Name, node);

        return node;
    }

    public static void RemoveSingleton<T>()
    {
        if (singletonFinder.ContainsKey(typeof(T).Name))
        {
            singletonFinder.Remove(typeof(T).Name);
        }
    }

    public static T GetSingleton<T>() where T : Node
    {
        if (singletonFinder.ContainsKey(typeof(T).Name))
        {
            // Freed objects are not null, for some reason, so an object will be found but type should be different
            if(singletonFinder[typeof(T).Name].GetType() == typeof(GodotObject))
                singletonFinder.Remove(typeof(T).Name);
            else
                return (T)singletonFinder[typeof(T).Name];
        }

        if (scenePaths.ContainsKey(typeof(T).Name))
        {
            if (ResourceLoader.Exists(scenePaths[typeof(T).Name].Item1))
            {
                Node scene = ResourceLoader.Load<PackedScene>(scenePaths[typeof(T).Name].Item1).Instantiate();
                (Engine.GetMainLoop() as SceneTree).Root.AddChild(scene);
                if (string.IsNullOrEmpty(scenePaths[typeof(T).Name].Item2))
                {
                    return (T)scene;
                }
                T singletonNode = scene.GetNode<T>(scenePaths[typeof(T).Name].Item2);
                return singletonNode;
            }
            else
            {
                GD.PrintErr("Couldn't find Scene for " + typeof(T).Name + " at path " + scenePaths[typeof(T).Name] + "!");
            }
        }
        T node = (T)(new Node());

        singletonFinder.Add(typeof(T).Name, node);

        return node;
    }
}
