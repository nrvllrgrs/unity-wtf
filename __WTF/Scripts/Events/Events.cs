using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject>
{ }

[System.Serializable]
public class BoolEvent : UnityEvent<bool>
{ }

[System.Serializable]
public class Int32Event : UnityEvent<int>
{ }

[System.Serializable]
public class SingleEvent : UnityEvent<float>
{ }

[System.Serializable]
public class TransformEvent : UnityEvent<Transform>
{ }

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3>
{ }