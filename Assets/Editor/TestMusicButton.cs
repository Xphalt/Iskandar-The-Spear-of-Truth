using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicManager))]
public class TestMusicButton : Editor
{
    MusicManager musicManager;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
        }

        if(GUILayout.Button("Fade Music"))
        {
            musicManager.FadeInMusic(musicManager.endingMusic);
        }
    }

    void DrawSettingsEditor(Object settings)
    {
        Editor editor = CreateEditor(settings);
        editor.OnInspectorGUI();
    }

    private void OnEnable()
    {
        musicManager = (MusicManager)target;
    }
}
