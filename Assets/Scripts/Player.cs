using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    public Animator ani;
    public GameObject log;
    public Transform leftHand;
    public Transform rightHand;
    public PlayableDirector director;
    private int speedUpId = Animator.StringToHash("isSpeedUp");
    private int rotateSpeedId = Animator.StringToHash("RotateSpeed");
    private int verticalSpeedId = Animator.StringToHash("VerticalSpeed");
    private int valutId = Animator.StringToHash("IsValut");
    private int colliderId = Animator.StringToHash("Collider");
    private int sliderId = Animator.StringToHash("IsSlide");
    private int holdLogId = Animator.StringToHash("IsHoldLog");
    private Vector3 matchTarget = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    } 

    // Update is called once per frame
    void Update()
    {
        //ani.SetFloat("Speed", Input.GetAxis("Vertical"));
        //ani.SetBool(speedUpId, Input.GetKeyDown(KeyCode.LeftShift));
        ani.SetFloat(rotateSpeedId, Input.GetAxis("Horizontal") * 126);
        ani.SetFloat(verticalSpeedId, Mathf.Abs(Input.GetAxis("Vertical") *4.1f));

        isValut();

        isSlide();



    }

    void isValut()
    {

        RaycastHit hit;
        bool isCollider = Physics.Raycast(transform.position + (transform.up * 0.3f), transform.forward, out hit, 4.0f);
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree") && isCollider)
        {
            if (hit.distance > 3.0f && hit.collider.tag == "Obstacle")
            {
                Vector3 point = hit.point;
                point.y = hit.collider.transform.position.y + hit.collider.bounds.size.y + 0.07f;
                matchTarget = point;
                ani.SetBool(valutId, true);
                //Debug.Log(matchTarget);

            }
        }
        else
        {
            ani.SetBool(valutId, false);
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {

            transform.GetComponent<CharacterController>().enabled = ani.GetFloat(colliderId) < 0.5;

            ani.MatchTarget(matchTarget, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0), 0.32f, 0.4f);
        }
    }

    void isSlide()
    {

        RaycastHit hit;
        bool isCollider = Physics.Raycast(transform.position + (Vector3.up * 1.2f), transform.forward, out hit, 5.0f);
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree") && isCollider)
        {
            if (hit.distance > 4.0f && hit.collider.tag == "Obstacle")
            {
                Vector3 point = hit.point;
                point.y = 0;
                point = point + transform.forward * 2;
                matchTarget = point;
                ani.SetBool(sliderId, true);
                Debug.Log(matchTarget);

            }
        }
        else
        {
            ani.SetBool(sliderId, false);
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            Debug.Log("Slide");
            transform.GetComponent<CharacterController>().enabled = ani.GetFloat(colliderId) < 0.5;

            ani.MatchTarget(matchTarget, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(1,0,1), 0), 0.17f, 0.6f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Log")
        {
            //ani.SetLayerWeight(1, 1);
            log.SetActive(true);
            ani.SetBool(holdLogId, true);
            Debug.Log("enter");
            Destroy(other.gameObject);
        }
        else if (other.tag == "TimeLine")
        {
            director.Play();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex == 1)
        {
            print("IK");
            int weight = ani.GetBool(holdLogId) ? 1 : 0;
            ani.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            ani.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);

            ani.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            ani.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);

            ani.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
            ani.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
        }
    }
}
