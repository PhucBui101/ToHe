using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Level", menuName = "Puzzle Game/Level Data")]
public class PuzzleLevelData : ScriptableObject
{
    [Header("=== LEVEL INFO ===")]
    public string levelName = "Level 1";
    
    [Header("=== PUZZLE IMAGE ===")]
    [Tooltip("Hình ảnh chính của puzzle (sẽ được chia thành các phần)")]
    public Sprite puzzleImage;
    
    [Header("=== DESCRIPTION ===")]
    [TextArea(3, 5)]
    public string description = "Mô tả về hình ảnh này...";
    
    [Header("=== INITIAL ROTATIONS ===")]
    [Tooltip("Góc xoay ban đầu cho 9 ô (0, 90, 180, hoặc 270). Thêm phần tử nếu cần nhiều hơn.")]
    public int[] initialRotations = new int[9] { 90, 180, 270, 0, 90, 180, 270, 0, 90 };
}