using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class OnSecondEventWithFloat : UnityEvent<float> { }

[Serializable] public class OnFirstEventWithFloat : UnityEvent<float> { }

[Serializable] public class OnCharacterAccelerate : UnityEvent<float> { }

[Serializable] public class OnTriggerEnter : UnityEvent<Collider> { }

[Serializable] public class OnTriggerExit : UnityEvent<Collider> { }

[Serializable] public class OnCollisionEnter : UnityEvent<Collision> { }

[Serializable] public class OnCollisionExit : UnityEvent<Collision> { }
