using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Actors;
using Game.GlobalVariables;
using Pathfinding;
using TriflesGames.ManagerFramework;
using UnityEngine;

public class PatientMovementControllerActor : Actor<PatientManager>
{
    private PatientStatusActor psa;
    
    public AIDestinationSetter setter;
    public AIPath path;

    public bool outLine;
    public Animator anim;
    
    protected override void MB_Start()
    {
        psa = GetComponent<PatientStatusActor>();
        outLine = false;
        StartCoroutine(SetTarget());
    }

    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            PatientManager.Instance.Subscribe(CustomManagerEvents.SetNewPositions,Set);
        }
        else
        {
            PatientManager.Instance.Unsubscribe(CustomManagerEvents.SetNewPositions,Set);
        }
    }

    protected override void MB_Update()
    {
        if (psa.patientStatus != PatientStatusActor.PatientStatus.PatientInRoom)
        {
            if (path.remainingDistance > 0.5f)
            {
                anim.SetBool("Idle", false);
            }
            else
            {
                anim.SetBool("Idle", true);
            }
        }
    }

    private void Set(object[] arguments)
    {
        StartCoroutine(SetTarget());
    }

    IEnumerator SetTarget()
    {
        yield return new WaitForSeconds(.5f);
        
        if (!outLine)
        {
            if (Manager.psaList.Count == 1)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
            }
            
            if (Manager.psaList.Count == 2)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
            }
            
            if (Manager.psaList.Count == 3)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
            }
            
            if (Manager.psaList.Count == 4)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
            }
            
            if (Manager.psaList.Count == 5)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
            }
            if (Manager.psaList.Count == 6)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
                Manager.psaList[5].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sixP;
            }
            
            if (Manager.psaList.Count == 7)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
                Manager.psaList[5].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sixP;
                Manager.psaList[6].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sevenP;
            }
            
            if (Manager.psaList.Count == 8)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
                Manager.psaList[5].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sixP;
                Manager.psaList[6].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sevenP;
                Manager.psaList[7].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.eightP;
            }
            
            if (Manager.psaList.Count == 9)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
                Manager.psaList[5].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sixP;
                Manager.psaList[6].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sevenP;
                Manager.psaList[7].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.eightP;
                Manager.psaList[8].GetComponent<PatientMovementControllerActor>().setter.target = 
                    Manager.nineP;
            }
            
            if (Manager.psaList.Count == 10)
            {
                Manager.psaList[0].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.firstPoint;
                Manager.psaList[1].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.secondP;
                Manager.psaList[2].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.thirdP;
                Manager.psaList[3].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fourthP;
                Manager.psaList[4].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.fifthP;
                Manager.psaList[5].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sixP;
                Manager.psaList[6].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.sevenP;
                Manager.psaList[7].GetComponent<PatientMovementControllerActor>().setter.target =
                    Manager.eightP;
                Manager.psaList[8].GetComponent<PatientMovementControllerActor>().setter.target = 
                    Manager.nineP;
                Manager.psaList[9].GetComponent<PatientMovementControllerActor>().setter.target = 
                    Manager.tenP;
            }
        }
    }
}
