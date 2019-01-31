using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Vector3 speed = new Vector3(0.05f, 0.05f);

    public int destroytime;
    private Vector3 Position;
    private float a;
	public ElementType elementType;

	public enum ElementType{
		normal,
		explosion
	}
    // Use this for initialization
    void Start()
    {
		if(elementType == ElementType.normal){
        	Destroy(this.gameObject, destroytime);
		}

        a = Random.Range(0, 90);
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {

            // 現在位置をPositionに代入

            Position = transform.position;

            // x += SPEED * cos(ラジアン)

            // y += SPEED * sin(ラジアン)

            // これで特定の方向へ向かって進んでいく。

            Position.x += speed.x * Mathf.Cos(a);

            Position.z += speed.z * Mathf.Sin(a);

            // 現在の位置に加算減算を行ったPositionを代入する

            transform.position = Position;
        }
    }
}