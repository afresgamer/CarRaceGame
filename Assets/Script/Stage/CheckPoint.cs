using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField, Header("チェックポイントの演出")] private ParticleSystem checkPointEffect;

    private Transform createEffectPoint;
    private GameObject effect;
    [HideInInspector] public BoxCollider boxCollider;
    void Start ()
    {
        createEffectPoint = transform.GetChild(0);
        boxCollider = GetComponent<BoxCollider>();
        effect = Instantiate(checkPointEffect.gameObject);
        effect.transform.SetParent(transform);
        effect.transform.position = createEffectPoint.position;
        effect.transform.rotation = createEffectPoint.rotation;
        effect.GetComponent<ParticleSystem>().Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConst.PLAYER_TAG))
        {
            if (!effect.GetComponent<ParticleSystem>().isPlaying) return;

            CarGameManager.Instance.CheckPointCnt++;
            effect.GetComponent<ParticleSystem>().Stop();
        }
        if (other.CompareTag(GameConst.CPUCAR_TAG))
        {
            var cpuCar = other.GetComponent<CpuCar>();
            if (cpuCar != null) 
            {
                cpuCar.CheckCnt++;
            }
        }
    }
}
