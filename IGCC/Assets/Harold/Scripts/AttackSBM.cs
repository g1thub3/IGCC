using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AttackSBM : StateMachineBehaviour
{
    [SerializeField]
    private float _attackNormalizedTime;

    bool _hasTriggered = false;


    //bool _onReadyForNextStep = false;

    MeleeAttackController _meleeAttackController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hasTriggered = false;
        //_onReadyForNextStep = false;
        _meleeAttackController = animator.GetComponent<MeleeAttackController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(_rangedAttackController.attac);

        if (stateInfo.normalizedTime >= _attackNormalizedTime && !_hasTriggered)
        {
            _meleeAttackController.attackHit();
            _hasTriggered = true;
            //Debug.Log("Exevute");
        }

        //if (stateInfo.normalizedTime >= 0.7f)
        //{
        //    _meleeAttackController.onAttackReadyForNextStepContinuous();
        //}

        if (stateInfo.normalizedTime >= 1f)
        {
            _meleeAttackController.finishAttack();
            //_onReadyForNextStep = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
