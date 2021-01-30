using UnityEditor;

public static class EditorHelpers
{
    [MenuItem("GGJ21/Compile and play #&p")]
    public static void CompileAndPlay()
    {
        AssetDatabase.Refresh();
        EditorApplication.isPlaying = true;
    }
}