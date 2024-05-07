using GXPEngine; // For GameObject

//track data about a collision of an object
public class CollisionInfo {
	public readonly Vec2 normal; //the collision normal
	public readonly GameObject other; //the other object that's collided with
	public readonly float timeOfImpact;

	public CollisionInfo(Vec2 pNormal, GameObject pOther, float pTimeOfImpact) {
		normal = pNormal;
		other = pOther;
		timeOfImpact = pTimeOfImpact;
	}
}
