using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWSIM
{
//create a script that move the car 
    [RequireComponent(typeof(NPCVehicle))]
    public class SimpleVehicleController : MonoBehaviour
    {
        [SerializeField] float duration;
        [SerializeField] float speed;
        [SerializeField] float yawSpeed;

        NPCVehicle npcVehicle;
        Vector3 startPosition;
        Quaternion startRotation;
        Vector3 currentPosition;
        Quaternion currentRotation;

        void Awake()
        {
            npcVehicle = GetComponent<NPCVehicle>();
            startPosition = transform.position;
            startRotation = transform.rotation;
            currentPosition = transform.position;
            currentRotation = transform.rotation;
        }

        void Start()
        {
            StartCoroutine(Loop());
        }

        IEnumerator Loop()
        {
            while (true)
            {
                yield return MoveForwardRoutine(duration, speed);
                yield return RotateRoutine(0.5f, 360f);
                yield return MoveForwardRoutine(duration, speed);
                yield return RotateRoutine(0.5f, 360f);
                var npcTransformPos = npcVehicle.transform.position;

                // reset
                npcVehicle.SetPosition(startPosition);
                npcVehicle.SetRotation(startRotation);
                currentPosition = startPosition;
                currentRotation = startRotation;
            }
        }

        IEnumerator MoveForwardRoutine(float duration, float speed)
        {
            var startTime = Time.fixedTime;
            while (Time.fixedTime - startTime < duration)
            {
                yield return new WaitForFixedUpdate();
                currentPosition += currentRotation * Vector3.forward * speed * Time.fixedDeltaTime;
                npcVehicle.SetRotation(currentRotation);
                npcVehicle.SetPosition(currentPosition);
            }
        }

        IEnumerator RotateRoutine(float duration, float yawSpeed)
        {
            var startTime = Time.fixedTime;
            while (Time.fixedTime - startTime < duration)
            {
                var euler = currentRotation.eulerAngles;
                currentRotation = Quaternion.Euler(euler.x, euler.y + yawSpeed * Time.fixedDeltaTime, euler.z);
                npcVehicle.SetRotation(currentRotation);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}