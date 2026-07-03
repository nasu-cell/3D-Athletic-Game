using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float speed;
    public float maxAngle;
    public float timeOffset;

    // 💡 起動時の「親オブジェクト自身の傾き」をしっかり記憶する
    private Quaternion baseRotation;

    void Start()
    {
        // エディタ上で親オブジェクトをどの方向（X軸やY軸）に回転させて配置したかを記憶
        baseRotation = transform.localRotation;
    }

    void Update()
    {
        // 常に一定のリズムで -maxAngle 〜 +maxAngle の間を往復する角度を計算
        float angle = Mathf.Sin((Time.time + timeOffset) * speed) * maxAngle;
        
        // 💡 最初にエディタ上で配置した向き（baseRotation）に対して、Z軸の揺れを掛け合わせる
        // これにより、親をどの方向（Rotation）に回して配置しても、その向きを基準に正しく振り子運動します！
        transform.localRotation = baseRotation * Quaternion.Euler(0, 0, angle);
    }
}