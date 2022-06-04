using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    public static PlayerRecoil playerRecoil;

    Vector3 currentRotation;
    Vector3 targetRotation;

    [SerializeField] private Vector3 recoil;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public bool recoilEnabled;


    private void Start()
    {
        playerRecoil = this;
    }


    void Update()
    {
        targetRotation = Vector3.MoveTowards(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);

        if (!recoilEnabled)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z);
        }
    }


    public void RecoilFire()
    {
        targetRotation += new Vector3(recoil.x, RNG.RangePosNeg(recoil.y), RNG.RangePosNeg(recoil.z));
    }
}
