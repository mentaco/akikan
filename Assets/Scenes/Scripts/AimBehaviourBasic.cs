using UnityEngine;
using System.Collections;

public class AimBehaviourBasic : GenericBehaviour
{
    public Texture2D crosshair;
    public float aimTurnSmoothing = 0.15f;
    public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f, 0f);
    public Vector3 aimCamOffset = new Vector3(0f, 0f, 0f); // ★ カメラ位置を通常時と同じにする

    private int aimBool;
    private bool aim = true;

    void Start()
    {
        aimBool = Animator.StringToHash("Aim");
        StartCoroutine(ToggleAimOn());
    }

    void Update()
    {
        canSprint = false;
        behaviourManager.GetAnim.SetBool(aimBool, true);
    }

    private IEnumerator ToggleAimOn()
    {
        yield return new WaitForSeconds(0.05f);

        if (!behaviourManager.GetTempLockStatus(this.behaviourCode) && !behaviourManager.IsOverriding(this))
        {
            int signal = 1;
            aimCamOffset.x = Mathf.Abs(aimCamOffset.x) * signal;
            aimPivotOffset.x = Mathf.Abs(aimPivotOffset.x) * signal;

            yield return new WaitForSeconds(0.1f);
            behaviourManager.GetAnim.SetFloat(speedFloat, behaviourManager.GetAnim.GetFloat(speedFloat)); // ★ 速度を保持
            behaviourManager.OverrideWithBehaviour(this);
        }
    }

    public override void LocalFixedUpdate()
    {
        // behaviourManager.GetCamScript.SetTargetOffsets(aimPivotOffset, aimCamOffset); // ★ コメントアウト
    }

    public override void LocalLateUpdate()
    {
        AimManagement();
    }

    void AimManagement()
    {
        Rotating();
    }

    void Rotating()
    {
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward = forward.normalized;

        Quaternion targetRotation = Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);
        float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * aimTurnSmoothing;

        behaviourManager.SetLastDirection(forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minSpeed * Time.deltaTime);
    }

    void OnGUI()
    {
        if (crosshair)
        {
            float mag = behaviourManager.GetCamScript.GetCurrentPivotMagnitude(aimPivotOffset);
            if (mag < 0.05f)
                GUI.DrawTexture(new Rect(Screen.width / 2.0f - (crosshair.width * 0.5f),
                                         Screen.height / 2.0f - (crosshair.height * 0.5f),
                                         crosshair.width, crosshair.height), crosshair);
        }
    }
}
