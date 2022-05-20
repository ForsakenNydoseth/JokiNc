using System;
using System.Threading;
using System.Threading.Tasks;
using JGeneral.IO.Threading;
using UnityEngine;

namespace JokiNc.Core.UnityCore
{
    [RequireComponent(typeof(Rigidbody))]
    public partial class ToolController : MonoBehaviour
    {
        public static ToolController Instance;
        public static GameObject GOInstance;
        public SyncThread<Action> ToolPositionUpdater;
        public ToolOptions Options;
        public WorkingObjectOptions WorkingObjOptions;
        public Rigidbody Body;
        public Vector3 CurrentDestination;
        public ToolOrientation Orientation;

        public virtual void Awake()
        {
            Instance = this;
            GOInstance ??= gameObject;
            Options = ToolOptions.Default;
            WorkingObjOptions = new WorkingObjectOptions();
            ToolPositionUpdater = new SyncThread<Action>();
            Body = GetComponent<Rigidbody>();
            Body.useGravity = false;
            Body.drag = 0;
            Body.angularDrag = 0;
            Body.maxAngularVelocity = Options.SpindleSpeed.ToRadiansPerSecond();
            Orientation = new ToolOrientation();
        }

        public virtual void LateUpdate()
        {
            if (Orientation.X)
            {
                if (transform.position.x >= CurrentDestination.x)
                {
                    PauseMovementOnX();
                    EnsureAtX();
                }
            }
            else
            {
                if (transform.position.x <= CurrentDestination.x)
                {
                    PauseMovementOnX();
                    EnsureAtX();
                }
            }
            
            if (Orientation.Y)
            {
                if (transform.position.y >= CurrentDestination.y)
                {
                    PauseMovementOnY();
                    EnsureAtY();
                }
            }
            else
            {
                if (transform.position.y <= CurrentDestination.y)
                {
                    PauseMovementOnY();
                    EnsureAtY();
                }
            }
            
            if (Orientation.Z)
            {
                if (transform.position.z >= CurrentDestination.z)
                {
                    PauseMovementOnZ();
                    EnsureAtZ();
                }
            }
            else
            {
                if (transform.position.z <= CurrentDestination.z)
                {
                    PauseMovementOnZ();
                    EnsureAtZ();
                }
            }
        }

        public virtual void SetSpindleSpeed(float rpm)
        {
            var radiansPerSecond = rpm.ToRadiansPerSecond();
            Body.maxAngularVelocity = radiansPerSecond;
            Body.angularVelocity = new Vector3(0, radiansPerSecond, 0);
            Options.SpindleSpeed = radiansPerSecond;
        }
        /// <summary>
        /// Sets the feed rate in mm/min.
        /// </summary>
        public void SetFeedRate(float feedRate)
        {
            Options.FeedRate = feedRate;
        }

        public virtual void AddRequiredVelocity(WorldAxis axis, bool positive = true)
        {
            var feedRate = Options.FeedRatePerSecond * (positive ? 1 : -1);
            Body.AddForce(axis switch
            {
                WorldAxis.X => new Vector3(feedRate, 0, 0),
                WorldAxis.Y => new Vector3(0, feedRate, 0),
                WorldAxis.Z => new Vector3(0, 0, feedRate),
                _           => Vector3.zero
            }, ForceMode.VelocityChange);
            if (positive)
            {
                Orientation.FaceForwards(axis);
            }
            else
            {
                Orientation.FaceBackwards(axis);
            }
            Debug.Log($"Added [{feedRate}] u/s force to the '{axis.ToString()}' axis!");
        }

        public virtual void Move(float distance, WorldAxis axis, CoordinateSystem coordinates)
        {
            var trans = WorkingObjOptions.TransOptions;
            //var destination = transform.position;
            switch (axis)
            {
                case WorldAxis.X:
                {
                    Orientation.X = distance > 0;
                    CurrentDestination.x += distance + trans.X;
                    break;
                }
                case WorldAxis.Y:
                {
                    Orientation.Y = distance > 0;
                    CurrentDestination.y += distance + trans.Y;
                    break;
                }
                case WorldAxis.Z:
                {
                    Orientation.Z = distance > 0;
                    CurrentDestination.z += distance + trans.Z;
                    break;
                }
            }

            switch (coordinates)
            {
                case CoordinateSystem.Absolute:
                {         
                    switch (axis)
                    {
                        case WorldAxis.X:
                        {
                            Orientation.X = distance > 0;
                            CurrentDestination.x = distance + trans.X;
                            break;
                        }
                        case WorldAxis.Y:
                        {
                            Orientation.Y = distance > 0;
                            CurrentDestination.y = distance + trans.Y;
                            break;
                        }
                        case WorldAxis.Z:
                        {
                            Orientation.Z = distance > 0;
                            CurrentDestination.z = distance + trans.Z;
                            break;
                        }
                    }
                    break;
                }
                case CoordinateSystem.Incremental:
                {
                    switch (axis)
                    {
                        case WorldAxis.X:
                        {
                            Orientation.X = distance > 0;
                            CurrentDestination.x += distance;
                            break;
                        }
                        case WorldAxis.Y:
                        {
                            Orientation.Y = distance > 0;
                            CurrentDestination.y += distance;
                            break;
                        }
                        case WorldAxis.Z:
                        {
                            Orientation.Z = distance > 0;
                            CurrentDestination.z += distance;
                            break;
                        }
                    }
                    break;
                }
            }
            
            AddRequiredVelocity(axis, axis switch{
                WorldAxis.X => Orientation.X,
                WorldAxis.Y => Orientation.Y,
                WorldAxis.Z => Orientation.Z,
                _           => throw new Exception("No axis specified at Move(float, WorldAxis, CoordinateSystem)!")
            });
        }

        public void PauseMovement()  
        {
            Body.velocity = Vector3.zero;
        }

        public void PauseMovementOnX()
        {
            var velocity = Body.velocity;
            Body.velocity = new Vector3(0, velocity.y, velocity.z);
        }
        
        public void PauseMovementOnY()
        {
            var velocity = Body.velocity;
            Body.velocity = new Vector3(velocity.x, 0, velocity.z);
        }
        
        public void PauseMovementOnZ()
        {
            var velocity = Body.velocity;
            Body.velocity = new Vector3(velocity.x, velocity.y, 0);
        }

        public void EnsureAtLocation()
        {
            transform.position = CurrentDestination;
        }

        public void EnsureAtX()
        {
            var pos = transform.position;
            transform.position = new Vector3(CurrentDestination.x, pos.y, pos.z);
        }

        public void EnsureAtY()
        {
            var pos = transform.position;
            transform.position = new Vector3(pos.x, CurrentDestination.y, pos.z);
        }
        
        public void EnsureAtZ()
        {
            var pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, CurrentDestination.z);
        }
    }
}