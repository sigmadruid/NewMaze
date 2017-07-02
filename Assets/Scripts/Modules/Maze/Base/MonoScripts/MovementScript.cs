﻿using UnityEngine;

using System;

using Pathfinding;

namespace Base
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(FunnelModifier))]
    public class MovementScript : MonoBehaviour
    {
        public Action CallbackMoveStart;
        public Action CallbackMoveTurn;
        public Action CallbackMoveEnd;

        public bool IsControllable = true;

        public bool LerpRotation;

        public float Gravity = 1f;

        public float AngularSpeed = 5;

        public float StopDistance = 0.1f;

        public bool EnableLog = false;

        private float speed;

        private Path path;
        private int nextPathIndex;
        private Vector3 nextPosition;
        private Vector3 currentDirection;
        private Vector3 desiredDirection;

        private Vector3 moveDirection;
        private Vector3 rollDirection;

        private Seeker seeker;
        private CharacterController controller;

        void Awake() 
        {
            seeker = GetComponent<Seeker>();
            controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            currentDirection = transform.forward;
        }

        void Update()
        {
            if(!IsControllable)
                return;
            
            if(path != null)
            {
                if (nextPathIndex < path.vectorPath.Count) 
                {
                    nextPosition = path.vectorPath[nextPathIndex];
                    desiredDirection = (nextPosition - transform.position).normalized;
                    if(LerpRotation)
                    {
                        currentDirection = transform.forward;
                        currentDirection = Vector3.Lerp(currentDirection, desiredDirection, Time.deltaTime * AngularSpeed);
                        currentDirection.Normalize();
                    }
                    else
                    {
                        currentDirection = desiredDirection;
                    }
                    LookAt(currentDirection);

                    controller.Move(desiredDirection * speed * Time.deltaTime);
                    
                    if (MathUtils.XZDistance(transform.position, nextPosition) < StopDistance)
                    {
                        nextPathIndex++;
                        if (nextPathIndex == path.vectorPath.Count)
                        {
                            transform.position = path.vectorPath[path.vectorPath.Count - 1];
                            IsMoving = false;
                            if (CallbackMoveEnd != null) CallbackMoveEnd();
                            path = null;
                        }
                        else
                        {
                            if (CallbackMoveTurn != null) CallbackMoveTurn();
                        }
                    }
                }
            }
            else if (controller.enabled)
            {
                if(rollDirection != Vector3.zero)
                {
                    controller.Move((rollDirection * speed + Vector3.down * Gravity) * Time.deltaTime);
                }
                else if(moveDirection != Vector3.zero)
                {
                    controller.Move((moveDirection * speed + Vector3.down * Gravity) * Time.deltaTime);
                }
            }
        }

        public bool IsMoving { get; private set; }
        public bool IsRolling { get; private set; }

        public Vector3 Destination { get; private set; }

        public Vector3 Direction { get { return currentDirection; } }

        public void SetDestination(Vector3 destPosition, float speed)
        {
            if (destPosition == Vector3.zero)
            {
                Destination = Vector3.zero;
                path = null;
                IsMoving = false;
                if (CallbackMoveEnd != null) CallbackMoveEnd();
            }
            else if (destPosition != Destination)
            {
                Destination = destPosition;
                this.speed = speed;
                IsMoving = true;
                seeker.StartPath(transform.position, destPosition, OnPathComplete);
            }
        }

        public void SetMove(Vector3 direction, float speed)
        {
            if(Destination == Vector3.zero)
            {
                this.speed = speed;
                moveDirection = direction;
                IsMoving = direction != Vector3.zero;
                if(IsMoving)
                {
                    LookAt(direction);
                }
            }
        }
        public void SetRoll(Vector3 direction, float speed)
        {
            if(Destination == Vector3.zero)
            {
                this.speed = speed;
                rollDirection = direction;
                IsRolling = direction != Vector3.zero;
            }
        }

        public void LookAt(Vector3 direction)
        {
            Vector3 lookDirection = MathUtils.XZDirection(direction);
            transform.localRotation = Quaternion.LookRotation(lookDirection);
        }

        private void OnPathComplete(Path p)
        {
            if(!p.error && p.vectorPath.Count > 1)
            {
                path = p;
                nextPathIndex = 1;
                transform.position = path.vectorPath[0];
                if (CallbackMoveStart != null) CallbackMoveStart();
            }
        }

        private void Log(string log)
        {
            if (EnableLog) Debug.LogError(log);
        }
    }
}

