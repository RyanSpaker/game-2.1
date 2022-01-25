using UnityEngine;

public class Vault : MonoBehaviour
{
    [Tooltip("This is how fara away from an object the player can be to vault it")]
    public float maxVaultDistance = 5;
    [Tooltip("starting from the players feet, this is the percentage of its height that it can vault (50% would mean it can vault anything waist and below)")]
    public float vaultHeightPercentage = 0.5f;
    [Tooltip("The bottom collider in the set is below the players feet and allows you to vault something your already above, but may be falling down on. this variable determines the size of the bottom collider, being a percentage of the players height.")]
    public float bottomBufferPercentage = 0.1f;
    [Tooltip("this variable is the percentage of a single subdivison to add to the height gain for each vault just for safety.")]
    public float safetyBufferPercentage = 0.2f;
    [Tooltip("this is the number of vault hitboxes in the stack disregarding the bottom and top one.")]
    public float widthPercentage = 0.8f;
    public int subDivisions = 4;
    public LayerMask vaultLayers;
    public float playerHeight = 4f;
    public float modelHeight = 2f;
    private VaultColliderHandler[] colliders;
    private float colliderHeight;
    private float bottomHeight;
    private float gravity;
    private void Start()
    {
        colliders = new VaultColliderHandler[subDivisions + 2];
        for (int i = 0; i < subDivisions + 2; i++)
        {
            if (i == 0)
            {
                GameObject bottom = new GameObject("bottom", typeof(VaultColliderHandler), typeof(BoxCollider));
                bottom.transform.SetParent(transform);
                bottom.GetComponent<VaultColliderHandler>().layermask = vaultLayers;
                bottom.GetComponent<BoxCollider>().isTrigger = true;
                bottom.transform.localScale = new Vector3(widthPercentage, playerHeight * bottomBufferPercentage / modelHeight, maxVaultDistance);
                bottom.transform.localPosition = new Vector3(0, -0.5f * playerHeight / modelHeight - 0.5f * playerHeight * bottomBufferPercentage / modelHeight, 0.5f * maxVaultDistance);
                colliders[i] = bottom.GetComponent<VaultColliderHandler>();
            }
            else
            {
                GameObject temp = new GameObject("" + i, typeof(VaultColliderHandler), typeof(BoxCollider));
                if (i == subDivisions + 1) temp.name = "top";
                temp.transform.SetParent(transform);
                temp.GetComponent<VaultColliderHandler>().layermask = vaultLayers;
                temp.GetComponent<BoxCollider>().isTrigger = true;
                temp.transform.localScale = new Vector3(widthPercentage, playerHeight * vaultHeightPercentage / subDivisions / modelHeight, maxVaultDistance);
                temp.transform.localPosition = new Vector3(0, (-0.5f * playerHeight / modelHeight) + ((i-0.5f)*vaultHeightPercentage*playerHeight/subDivisions/modelHeight), 0.5f * maxVaultDistance);
                colliders[i] = temp.GetComponent<VaultColliderHandler>();
            }
        }

        colliderHeight = playerHeight * vaultHeightPercentage / subDivisions;
        bottomHeight = playerHeight * bottomBufferPercentage;
    }
    public void setGravity(float grav) 
    {
        gravity = grav;
    }
    public Vector3 vault(Vector3 vel, bool grounded, float jumpVel) 
    {
        if (!colliders[subDivisions + 1].Colliding()) 
        {
            int iMax = 1;
            for (int i = 1; i <= colliders.Length; i++) 
            {
                if (colliders[i-1].Colliding()) iMax = i;
            }
            float heightGain = (iMax - 1 + safetyBufferPercentage) * colliderHeight;
            if (heightGain <= 0) return new Vector3(vel.x, 0, vel.y);
            float newVel = ((-1f * gravity * Time.fixedDeltaTime) + Mathf.Sqrt(gravity * gravity * Time.fixedDeltaTime * Time.fixedDeltaTime - 8 * gravity * heightGain)) / 2f;
            if (grounded && newVel < jumpVel) newVel = jumpVel;
            return new Vector3(vel.x, newVel, vel.z);
        }
        return vel;
    }
    public bool canVault() 
    {
        if (colliders[subDivisions + 1].Colliding()) return false;
        for (int i = 0; i < colliders.Length; i++) 
        {
            if (colliders[i].Colliding()) return true;
        }
        return false;
    }
}
