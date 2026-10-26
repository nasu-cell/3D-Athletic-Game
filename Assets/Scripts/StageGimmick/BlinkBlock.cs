using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

public class BlinkBlock : MonoBehaviour
{
    public bool exist;

    private MeshRenderer meshRenderer;
    private Collider blockCollider;
    private Material blockMaterial;
    private AudioSource audioSource;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioClip lowSound;
    [SerializeField] private AudioClip highSound;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        meshRenderer = GetComponent<MeshRenderer>();
        blockCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        blockMaterial = meshRenderer.material;


        Blink().Forget();
    }

    async UniTask Blink()
    {

        while (true)
        {

            await UniTask.WaitUntil(() => gameManager.isGamePlaying);


            // 3秒待つ
            for (int i = 0; i < 3; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));

                if (!gameManager.isGamePlaying)
                {
                    break;
                }

                audioSource.PlayOneShot(lowSound);
            }

            if (!gameManager.isGamePlaying)
                continue;

            exist = !exist;

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            if (!gameManager.isGamePlaying)
                continue;


            audioSource.PlayOneShot(highSound);

            Color color2 = blockMaterial.color;
            color2.a = exist ? 1.0f : 0.5f;

            blockCollider.enabled = exist;
            blockMaterial.color = color2;
        }
    }
}