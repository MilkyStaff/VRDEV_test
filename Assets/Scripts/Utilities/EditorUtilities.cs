using DG.Tweening.Plugins.Core.PathCore;
using System.IO;
using UnityEditor;

public static class SceneEditorTool
{
    [MenuItem("PreviewData/ClearPreviewCashe")]
    private static void ClearPreviewCashe()
    {
        var files = Directory.GetFiles(ResourseHelper.TempPreviewPath);
        foreach (var file in files)
            File.Delete(file);
    }

}
