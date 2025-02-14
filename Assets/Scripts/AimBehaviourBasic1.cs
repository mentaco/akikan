using UnityEngine;
using System.Collections;

public class AimBehaviourBasic1 : GenericBehaviour
{
    public string aimButton = "Aim", shoulderButton = "Aim Shoulder";  
    public Texture2D crosshair;                                      
    public float aimTurnSmoothing = 0.15f;                             
    public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f, 0f);      
    public Vector3 aimCamOffset   = new Vector3(0f, 0.4f, -0.7f);     

    private int aimBool;                                                
    private bool aim;                                                    
    
    public GameObject bulletPrefab;  // 弾のPrefab
    public Transform gunMuzzle;      // 銃口のTransform（Inspectorでセット）
    public float bulletSpeed = 20f;  // 弾の速度

    void Start()
    {
        aimBool = Animator.StringToHash("Aim");
    }

    void Update()
    {
        // AIMのON/OFF
        if (Input.GetAxisRaw(aimButton) != 0 && !aim)
        {
            StartCoroutine(ToggleAimOn());
        }
        else if (aim && Input.GetAxisRaw(aimButton) == 0)
        {
            StartCoroutine(ToggleAimOff());
        }

        canSprint = !aim;

        if (aim && Input.GetButtonDown(shoulderButton))
        {
            aimCamOffset.x *= -1;
            aimPivotOffset.x *= -1;
        }

        behaviourManager.GetAnim.SetBool(aimBool, aim);

        // 弾の発射
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }
    }

    private IEnumerator ToggleAimOn()
    {
        yield return new WaitForSeconds(0.05f);
        if (behaviourManager.GetTempLockStatus(this.behaviourCode) || behaviourManager.IsOverriding(this))
            yield break;

        aim = true;
        yield return new WaitForSeconds(0.1f);
        behaviourManager.GetAnim.SetFloat(speedFloat, 0);
        behaviourManager.OverrideWithBehaviour(this);
    }

    private IEnumerator ToggleAimOff()
    {
        aim = false;
        yield return new WaitForSeconds(0.3f);
        behaviourManager.GetCamScript.ResetTargetOffsets();
        behaviourManager.GetCamScript.ResetMaxVerticalAngle();
        yield return new WaitForSeconds(0.05f);
        behaviourManager.RevokeOverridingBehaviour(this);
    }

    public override void LocalFixedUpdate()
    {
        if (aim)
            behaviourManager.GetCamScript.SetTargetOffsets(aimPivotOffset, aimCamOffset);
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

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab が設定されていません！");
            return;
        }

        Vector3 spawnPos;
        Quaternion spawnRot;
        Vector3 shootDirection;

        // クロスヘアの方向を計算
        Vector3 targetDirection = GetCrosshairDirection();  // クロスヘアの方向を取得

        if (gunMuzzle != null)
        {
            spawnPos = gunMuzzle.position;
            spawnRot = gunMuzzle.rotation;
        }
        else
        {
            spawnPos = transform.position + transform.forward * 1.0f + Vector3.up * 1.2f;
            spawnRot = Quaternion.LookRotation(transform.forward);
        }

        // 発射方向をクロスヘアの方向に設定
        shootDirection = targetDirection.normalized;

        // レイキャストを使用して、物体に向かって発射する
        RaycastHit hit;
        if (Physics.Raycast(spawnPos, shootDirection, out hit, Mathf.Infinity))
        {
            // 物体がヒットした場合、その位置に弾を向ける
            shootDirection = (hit.point - spawnPos).normalized;
        }

        // 弾のインスタンスを生成
        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, spawnRot);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();

        if (bulletRigidbody == null)
        {
            Debug.LogError("BulletPrefab に Rigidbody がアタッチされていません！");
            return;
        }

        // 弾の速度を設定
        bulletRigidbody.velocity = shootDirection * bulletSpeed;

        Debug.Log($"Bullet instantiated at: {spawnPos}, velocity: {bulletRigidbody.velocity}, direction: {shootDirection}");

        // 弾のコリジョンを発射後に有効化
        StartCoroutine(EnableCollisionAfterDelay(bulletRigidbody, 0.1f));
    }

    // クロスヘアの方向を計算する関数（カメラの向きに基づいて）
    Vector3 GetCrosshairDirection()
    {
        // カメラの前方方向をターゲットにする（クロスヘアがカメラの視線の先に表示されると仮定）
        Vector3 crosshairDirection = Camera.main.transform.forward; // クロスヘアがカメラの視線の先に表示されると仮定
        return crosshairDirection;
    }

    // 弾のコリジョンを有効化するための遅延処理
    private IEnumerator EnableCollisionAfterDelay(Rigidbody bulletRb, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bulletRb != null)
        {
            bulletRb.detectCollisions = true;
        }
    }

    // クロスヘアを画面に描画
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
