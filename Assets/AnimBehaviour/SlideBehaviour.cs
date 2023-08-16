using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : StateMachineBehaviour {

    // original size and offset of player hitbox
    private Vector2 size;
    private Vector2 offset;

    // new size and offset of player hitbox
    private Vector2 slideSize = new Vector2(1.14f, 1.61f);
    private Vector2 slideOffset = new Vector2(-0.01f, -0.6f);

    // player boxcollider
    private BoxCollider2D boxCollider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Slide = true;

        // stores the orginal size of boxcollider
        if (boxCollider == null)
        {
            boxCollider = Player.Instance.GetComponent<BoxCollider2D>();
            size = boxCollider.size;
            offset = boxCollider.offset;
        }

        boxCollider.size = slideSize;
        boxCollider.offset = slideOffset;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Player.Instance.Slide = false;
        Debug.Log("slide");
        animator.ResetTrigger("slide");
        boxCollider.size = size;
        boxCollider.offset = offset;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
