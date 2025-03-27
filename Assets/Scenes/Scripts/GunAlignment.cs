using UnityEngine;

public class GunAlignment : MonoBehaviour
{
    public Transform cameraTransform;   // TPSカメラのTransform
    public Transform shoulderBone;      // 肩のボーン（左肩か右肩）
    public float distanceFromShoulder = 0.5f;  // 肩から銃までの距離

    void Update()
    {
        if (shoulderBone == null || cameraTransform == null) return;

        // カメラの前方向を基準に銃を回転
        Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.forward);

        // 銃の位置を肩の位置から一定の距離で維持
        Vector3 gunPosition = shoulderBone.position + cameraTransform.forward * distanceFromShoulder;

        // 銃の位置を更新
        transform.position = gunPosition;

        // 銃をカメラの向きに合わせ、さらに90度左に回転
        transform.rotation = targetRotation * Quaternion.Euler(0, -90, 0);
    }
}
