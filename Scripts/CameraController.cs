using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private bool gyroEnabled = true;
    private const float lowPassFilterFactor = 0.2f;

    private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
    private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
    private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
    private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);

    private Quaternion cameraBase = Quaternion.identity;
    private Quaternion calibration = Quaternion.identity;
    private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
    private Quaternion baseOrientationRotationFix = Quaternion.identity;

    private Quaternion referanceRotation = Quaternion.identity;

    protected void Start() {
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true  
        Input.gyro.enabled = true;
        //获取设备重力加速度向量  
        Vector3 deviceGravity = Input.gyro.gravity;
        //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）  
        Vector3 rotationVelocity = Input.gyro.rotationRate;
        //获取更加精确的旋转  
        Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次  
        Input.gyro.updateInterval = 0.1f;
        //获取移除重力加速度后设备的加速度  
        Vector3 acceleration = Input.gyro.userAcceleration;

        ResetBaseOrientation();
        UpdateCalibration(true);
        UpdateCameraBaseRotation(true);
        RecalculateReferenceRotation();
    }

    protected void Update() {

        if (!gyroEnabled)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
    }

    private void UpdateCalibration(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = (Input.gyro.attitude) * (-Vector3.forward);
            fw.z = 0;
            if (fw == Vector3.zero)
            {
                calibration = Quaternion.identity;
            }
            else
            {
                calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
            }
        }
        else
        {
            calibration = Input.gyro.attitude;
        }
    }

    private void UpdateCameraBaseRotation(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = transform.forward;
            fw.y = 0;
            if (fw == Vector3.zero)
            {
                cameraBase = Quaternion.identity;
            }
            else
            {
                cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
            }
        }
        else
        {
            cameraBase = transform.rotation;
        }
    }

    private static Quaternion ConvertRotation(Quaternion q) {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    private Quaternion GetRotFix() {
        return Quaternion.identity;
    }

    private void ResetBaseOrientation() {
        baseOrientationRotationFix = GetRotFix();
        baseOrientation = baseOrientationRotationFix * baseIdentity;
    }

    private void RecalculateReferenceRotation() {
        referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
    }
}
