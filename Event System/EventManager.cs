using System.Collections.Generic;
using Godot;

public class EventManager {

	private static Dictionary<int, List<EventDelegate>> eventListenerMap = new Dictionary<int, List<EventDelegate>>();
	private static List<Tuple<float, IEvent>> eventList = new List<Tuple<float, IEvent>>();

	public delegate void EventDelegate(IEvent EventObject);
	
	public static void ClearListenerAndEvents() {
		eventListenerMap = new Dictionary<int, List<EventDelegate>>();
		eventList = new List<Tuple<float, IEvent>>();
	}

	public static void AddEventListener(EventDelegate Function, int EventType) {
		List<EventDelegate> list;
		if (!eventListenerMap.ContainsKey(EventType))	{
			list = new List<EventDelegate>();
			eventListenerMap.Add(EventType, list);
		}
		else {
			list = eventListenerMap[EventType];
		}
		
		if(list.Contains(Function)) {
			GD.PrintErr("Attempt to double-register delegate: " + Function.ToString());
			return;
		}
		
		list.Add(Function);
	}
	
	public static void RemoveEventListener(EventDelegate Function, int EventType) {
		if(eventListenerMap.ContainsKey(EventType)) {
			eventListenerMap[EventType].Remove(Function);
		}
	}
	
	public static void TriggerEvent(IEvent EventObject, float delayTime = 0){
		if(delayTime == 0) {
			try {
				foreach(EventDelegate delegateFunction in eventListenerMap[EventObject.GetIdentity()]) {
					delegateFunction(EventObject);
				}
			} catch (System.Exception e) {
                GD.PrintErr("Failed to trigger " + EventObject.ToString() + "\n" + e.StackTrace);
			}
		} else eventList.Add(new Tuple<float, IEvent>(delayTime, EventObject));
	}
	
	public static void Update(float delaTime) {
		if(eventList.Count == 0) return;
		List<Tuple<float, IEvent>> toRemove = new List<Tuple<float, IEvent>>();
		foreach(Tuple<float, IEvent> pair in eventList) {
			if(pair.Item1 > 0) {
				pair.Item1 -= delaTime;
			}
			else {
				toRemove.Add(pair);
				try {
					foreach(EventDelegate delegateFunction in eventListenerMap[pair.Item2.GetIdentity()]) {
						delegateFunction(pair.Item2);
					}
				} catch (System.Exception e) {
                    GD.PrintErr("Failed to  trigger event " + pair.Item2.ToString() + "\n" + e.StackTrace);
				}
			}
		}
		foreach(Tuple<float, IEvent> pair in toRemove) {
			eventList.Remove(pair);
		}
	}
}
