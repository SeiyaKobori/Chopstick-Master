using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopsticks : MonoBehaviour
{

    [SerializeField]
    private Rigidbody right_chopstick = null;
    [SerializeField]
    private Rigidbody left_chopstick = null;

    private int right_fingerId = -1;
    private int left_fingerId = -1;

    private const float lift_down_height = 0.15f;
    private const float lift_up_height = 1f;

    public bool isLiftUp = false;
    public bool controlable = true;

    [SerializeField]
    private float move_chopstick_power = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateChopstickFinger();

        if (!controlable)
            return;

        UpdateChopstickPos();
        UpdateChopsticksHeight();
    }

    private void UpdateChopstickFinger()
    {
        if (Input.touchCount < 2)
        {
            right_fingerId = -1;
            left_fingerId = -1;
        }
        else
        {
            if (right_fingerId < 0) //設定されているIdが1以下=未設定の場合
            {
                var touches = Input.touches;
                right_fingerId = (touches[0].position.x > touches[1].position.x) ? 0 : 1;
                left_fingerId = right_fingerId == 0 ? 1 : 0;
            }
        }
    }

    private void UpdateChopstickPos()
    {
        if (Input.touchCount < 2)
            return;

        MoveChopstick(true);
        MoveChopstick(false);
    }

    private void MoveChopstick(bool isRightStick)
    {
        var pos = isRightStick ? right_chopstick.transform.position : left_chopstick.transform.position;
        var desPos = GetChopStickPos(isRightStick ? right_fingerId : left_fingerId);
        desPos.y = pos.y;
        var vec = (desPos - pos).normalized;

        if (isRightStick)
            right_chopstick.AddForce(vec * move_chopstick_power, ForceMode.Acceleration);
        else
            left_chopstick.AddForce(vec * move_chopstick_power, ForceMode.Acceleration);
    }

    private Vector3 GetChopStickPos(int fingerId)
    {
        Vector3 pos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(TouchUtility.GetTouchPosition(fingerId));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer(GameSystemManager.ChopstickMovableLayer)))
        {
            pos.x = hit.point.x;
            pos.z = hit.point.z;
        }

        return pos;
    }

    private void UpdateChopsticksHeight()
    {
        MoveChopstickHeight(true);
        MoveChopstickHeight(false);
    }

    private void MoveChopstickHeight(bool isRight)
    {
        var pos = isRight ? right_chopstick.transform.position : left_chopstick.transform.position;
        var liftPos = new Vector3(pos.x, (isLiftUp ? lift_up_height : lift_down_height), pos.z);
        var vec = (liftPos - pos).normalized;

        if (isRight)
            right_chopstick.AddForce(vec * move_chopstick_power);
        else
            left_chopstick.AddForce(vec * move_chopstick_power);
    }

    public void SetLift(bool isUp)
    {
        isLiftUp = isUp;
    }
}
