using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    private Animator anim;

    public float moveSpeed = 3;

    public bool moving;
    private int direct = 0;
    private Vector3 start;
    private Vector3 target;
    private float moveTime;

    public static int idel = Animator.StringToHash("Base Layer.idel");
    public static int front = Animator.StringToHash("Base Layer.front");
    public static int back = Animator.StringToHash("Base Layer.back");
    public static int left = Animator.StringToHash("Base Layer.left");
    public static int right = Animator.StringToHash("Base Layer.right");
    public static int attack = Animator.StringToHash("Base Layer.attack");

    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
        target = this.transform.localPosition;
        anim.Play(idel);
        //Debug.Log(idel+"||"+front+"||"+back+"||"+left+"||"+right);
    }
	

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moving = true;
            direct = 0;
            Action(ActionType.eAT_back);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moving = true;
            direct = 1;
            Action(ActionType.eAT_front);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moving = true;
            direct = 2;
            Action(ActionType.eAT_left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moving = true;
            direct = 3;
            Action(ActionType.eAT_right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action(ActionType.eAT_Hit);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)
            || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow)
            || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moving = false;
            Action(ActionType.eAT_None);
        }
        Move(direct);
        //Moving();
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
            //Vector3 ver = Input.mousePosition;
            //Debug.Log(ver);
            //MoveTo(ver, 1);
            //if(ver.x>target.x)
            //{
            //    Action(ActionType.eAT_right);
            //}
            //else if(ver.x<target.x)
            //{
            //    Action(ActionType.eAT_left);
            //}
            //else if(ver.y>target.y)
            //{
            //    Action(ActionType.eAT_back);
            //}
            //else if(ver.y<target.y)
            //{
            //    Action(ActionType.eAT_front);
            //}
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, 1000))
            //{
            //    MoveTo(hit.point, 0);
            //}
        }
    }

    public void Move(int dir)
    {
        if (!moving) return;
        switch(dir)
        {
            case 0:
                target.y+= moveSpeed;
                break;
            case 1:
                target.y-= moveSpeed;
                break;
            case 2:
                target.x-= moveSpeed;
                break;
            case 3:
                target.x+= moveSpeed;
                break;
            default:
                break;
        }
        this.transform.localPosition = target;
    }

    public void Moving()
    {
        if (moving)
        {
            anim.SetFloat("speed", 1.0f);
            moveTime += Time.deltaTime * (1 / moveSpeed);
            moveTime = Mathf.Min(moveTime, 1.0f);
            transform.localPosition = Vector3.Lerp(start, target, moveTime);
            if (moveTime == 1.0f)
            {
                moving = false;
            }
        }
        else
            anim.SetFloat("speed", 0.0f);
    }

    public void MoveTo(Vector3 tartetPos, float ms)
    {
        start = transform.localPosition;
        target = tartetPos;
        moveSpeed = ms;
        moving = true;
        moveTime = 0.0f;
    }

    public enum ActionType
    {
        eAT_None,
        eAT_front,
        eAT_back,
        eAT_left,
        eAT_right,
        eAT_Hit
    }

    public void Action(ActionType at)
    {
        switch (at)
        {
            case ActionType.eAT_None:
                anim.Play(idel);
                break;
            case ActionType.eAT_front:
                anim.Play(front);
                break;
            case ActionType.eAT_back:
                anim.Play(back);
                break;
            case ActionType.eAT_left:
                anim.Play(left);
                break;
            case ActionType.eAT_right:
                anim.Play(right);
                break;
            case ActionType.eAT_Hit:
                anim.Play(attack);
                break;
            default:
                break;
        }
    }
}
