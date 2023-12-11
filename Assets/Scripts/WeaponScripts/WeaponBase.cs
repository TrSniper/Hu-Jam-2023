using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Assign")]
    public int damage;
    public float cooldownTime;
    public bool canAutoFire;

    [Header("Info - No Touch")] public bool isLaser;

    protected Transform outTransform;
    protected PlayerAimManager pam;
    protected RaycastHit hit;
    private Camera mainCamera;

    protected virtual void OnAwake()
    {
        outTransform = transform.GetChild(0);
        pam = GameObject.Find("Player").GetComponent<PlayerAimManager>();
        mainCamera = Camera.main;
    }

    protected virtual void OnUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
        if (Physics.Raycast(ray, out hit)) outTransform.LookAt(hit.point);
        else outTransform.LookAt(GetMiddleOfTheScreen(pam.attackRange));
    }
    public virtual void Attack() {}

    protected Vector3 GetMiddleOfTheScreen(float zValue)
    {
        Vector3 viewportMiddle = new Vector3(0.5f, 0.5f, zValue);
        return Camera.main.ViewportToWorldPoint(viewportMiddle);
    }
}
