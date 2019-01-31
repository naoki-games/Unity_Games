
var moveobject : Transform;
var projectile1 : GameObject;
var projectile2 : GameObject;
var otherClip : AudioClip;
private var n : int;

function OnTriggerEnter (other : Collider) {
    Debug.Log("Enter");
    if (other.tag == "Player") {
     if( n==0 ){
      GetComponent.<AudioSource>().Play();
      }
     var clone2 : GameObject =
     Instantiate(projectile2, transform.position, transform.rotation);
     projectile2.transform.position.y -= 4;
     for(n=0;n<3;n++){
      moveobject.transform.rotation.x -= 0.003;
      moveobject.transform.position.y -= 0.06;
     }
     yield WaitForSeconds(1);
     var clone : GameObject = 
     Instantiate(projectile1, transform.position, transform.rotation) as GameObject;
     projectile1.transform.position.y -= 1;
     moveobject.gameObject.AddComponent.<Rigidbody>();
     moveobject.GetComponent.<BoxCollider>().enabled = false;
      scriptV = moveobject.GetComponent(Rigidbody);
       scriptV.mass = 6;
     yield WaitForSeconds(3);
     Destroy(moveobject.gameObject.gameObject);
	 yield WaitForSeconds(5);
     Destroy(clone);
     Destroy(clone2);
     Destroy(gameObject);
    }
}
