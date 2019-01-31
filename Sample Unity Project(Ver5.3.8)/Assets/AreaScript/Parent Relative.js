

function OnControllerColliderHit(hit : ControllerColliderHit) {
	if (hit.gameObject.name == "MoveArea Cube") {
		// ベルトコンベヤーに乗ったら
		transform.parent = hit.gameObject.transform;
	} else {
		// ベルトコンベヤーじゃなかったら
		transform.parent = null;
	}
}