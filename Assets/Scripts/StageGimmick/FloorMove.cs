using UnityEngine;

public class FloorMove : MonoBehaviour
{
    //基準値
    private Vector3 startPosition;
    //移動速度
    public float speed;
    private float moveSign = 1f;
    //移動範囲の限度
    public float range;
    //移動方向
    public Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
       if(direction != Vector3.zero)
        {
            direction.Normalize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        transform.Translate(direction * speed * moveSign *  Time.deltaTime);
        //スタート地点からの距離
        float currentDistance = Vector3.Distance(startPosition, transform.position);
        //距離がrangeを超えたら向きmoveSignを反転
        if(currentDistance >= range)
        {
            transform.position = startPosition + (direction * range * moveSign);
            moveSign = - moveSign;
        }
    }
    //プレイヤーが床と一緒に動く処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("床乗った");
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
