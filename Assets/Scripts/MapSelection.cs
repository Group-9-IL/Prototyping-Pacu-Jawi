using UnityEngine;

public class MapSelectionLoopingController : MonoBehaviour
{
    public RectTransform contentPanel; // Panel yang akan digeser
    public float scrollAmount = 200f; // Jarak setiap kali scroll (lebar map)
    public int totalMaps; // Total jumlah map
    private Vector2 targetPosition;
    private int currentMapIndex = 0; // Index map yang sedang dipilih

    private void Start()
    {
        // Pastikan posisi awal disimpan
        targetPosition = contentPanel.anchoredPosition;
    }

    void Update()
    {
        // Deteksi input keyboard untuk panah kiri dan kanan
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScrollLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScrollRight();
        }

        // Lerping untuk efek smooth
        contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, targetPosition, Time.deltaTime * 10f);
    }

    public void ScrollLeft()
    {
        // Kurangi index map
        currentMapIndex--;

        // Jika di bawah index 0, pindah ke map terakhir (looping)
        if (currentMapIndex < 0)
        {
            currentMapIndex = totalMaps - 1;
        }

        // Hitung target posisi
        targetPosition = new Vector2(-currentMapIndex * scrollAmount, contentPanel.anchoredPosition.y);
    }

    public void ScrollRight()
    {
        // Tambah index map
        currentMapIndex++;

        // Jika melebihi index map terakhir, pindah ke map pertama (looping)
        if (currentMapIndex >= totalMaps)
        {
            currentMapIndex = 0;
        }

        // Hitung target posisi
        targetPosition = new Vector2(-currentMapIndex * scrollAmount, contentPanel.anchoredPosition.y);
    }
}
