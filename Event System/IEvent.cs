public interface IEvent {

	int GetIdentity();
}

public abstract class Event<T> : IEvent {
	
	public static int Identity {
		get {
			return typeof(T).Name.GetHashCode();
		}
	}
	
	public int GetIdentity (){
		return Event<T>.Identity;
	}
}
