using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "Item";
    
    [Header("Hiệu ứng")]
    public GameObject collectEffect;
    public Vector3 effectOffset = Vector3.zero;
    public AudioClip collectSound;
    [Range(0f, 1f)]
    public float soundVolume = 1f;
    
    [Header("Thời gian chờ trước khi hiện popup")]
    public float popupDelay = 1.5f;
    
    private bool playerIsNear = false;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }
    
    void Collect()
    {
        // Tạo hiệu ứng tại vị trí tùy chỉnh
        if (collectEffect != null)
        {
            Vector3 effectPosition = transform.position + effectOffset;
            GameObject effect = Instantiate(collectEffect, effectPosition, Quaternion.identity);
            Destroy(effect, 2f);
        }
        
        // Phát âm thanh
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, soundVolume);
        }
        
        // Báo GameManager
        if (gameManager != null)
        {
            gameManager.CollectItem(itemName);
            gameManager.ShowPrompt(false);
        }
        
        // Ẩn hình ảnh đồ vật (nhưng giữ script chạy)
        HideVisuals();
        
        // Hiện popup sau delay (dùng Invoke thay vì Coroutine)
        Invoke("ShowPopupAndDestroy", popupDelay);
    }
    
    void HideVisuals()
    {
        // Ẩn tất cả Renderer (hình ảnh 3D)
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
        
        // Tắt Collider để không tương tác nữa
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
    }
    
    void ShowPopupAndDestroy()
    {
        // Hiện popup thông tin
        if (ItemInfoPopup.Instance != null)
        {
            ItemInfoPopup.Instance.ShowPopup();
        }
        
        // Xóa đồ vật
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            if (gameManager != null)
            {
                gameManager.ShowPrompt(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (gameManager != null)
            {
                gameManager.ShowPrompt(false);
            }
        }
    }
}