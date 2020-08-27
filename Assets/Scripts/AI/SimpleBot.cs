using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleBot : MonoBehaviour
{

    [SerializeField]
    private Transform botTransform;

    [SerializeField]
    private HumanoidMotorObject humanoidMotorObject;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private bool repeatSimpleBotCommands;

    [SerializeField]
    private List<SimpleBotCommand> inspectorCommands;

    protected Queue<SimpleBotCommand> simpleBotCommands = new Queue<SimpleBotCommand>();

    protected SimpleBotCommand currentlyExecutingCommand;

    private void Start()
    {
        foreach(var command in inspectorCommands)
        {
            simpleBotCommands.Enqueue(command);
        }
    }

    private void Update()
    {
        ExecuteBotLogicFrame();
    }

    protected void ExecuteBotLogicFrame()
    {
        if(currentlyExecutingCommand == null)
        {
            if (simpleBotCommands.Count == 0)
            {
                humanoidMotorObject.OctagonalWalkUpdate(Vector2.up);
                return;
            }
            currentlyExecutingCommand = simpleBotCommands.Dequeue();
        }

        SimpleBotCommand com = currentlyExecutingCommand;

        switch (com.commandType)
        {
            case BotCommandType.WalkForwardRotate:
                Vector3 between = Vector3.ProjectOnPlane(com.target.transform.position - botTransform.position, transform.up);
                if (between.magnitude < com.toleranceVal)
                {
                    //done!
                    if (repeatSimpleBotCommands)
                    {
                        simpleBotCommands.Enqueue(com); //so that we repeat!
                    }
                    currentlyExecutingCommand = null;
                    return;
                }

                float signedAngleBetween = Vector3.SignedAngle(botTransform.forward, between, botTransform.up);

                if(Mathf.Abs(signedAngleBetween) <= com.specialArg1)
                {
                    humanoidMotorObject.OctagonalWalkUpdate(Vector2.up);
                }
                else
                {
                    humanoidMotorObject.OctagonalWalkUpdate(Vector2.zero);
                }

                Vector3 rotation = (botTransform.up * signedAngleBetween);

                botTransform.Rotate((rotation.magnitude > rotationSpeed)? rotation.normalized * rotationSpeed : rotation,Space.Self);


                break;
        }
    }

}


[Serializable]
public class SimpleBotCommand //TODO Do this in more expressive way. maybe inherit from a base command class?
{
    public BotCommandType commandType;
    public float specialArg1;
    public float toleranceVal;
    public GameObject target;
}

public enum BotCommandType { WalkForwardRotate}