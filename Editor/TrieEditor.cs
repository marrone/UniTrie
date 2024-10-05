using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace UniTrie {

    public class TrieEditor : EditorWindow {

        private TextAsset selectedSourceAsset;
        private int wordCount;

         [MenuItem("Window/TrieEditor")]
         public static void ShowEditor() {
             EditorWindow wnd = GetWindow<TrieEditor>();
             wnd.titleContent = new GUIContent("Trie Editor");
             // Limit size of the window.
             wnd.minSize = new Vector2(450, 200);
             wnd.maxSize = new Vector2(1920, 720);
         }

        private void OnGUI() {
            EditorGUILayout.HelpBox("Select the text file containing your source words. The file should have one word per line", MessageType.Info);
            selectedSourceAsset = EditorGUILayout.ObjectField("Source Words", selectedSourceAsset, typeof(TextAsset), false) as TextAsset;
            GUI.enabled = selectedSourceAsset != null;
            if(GUILayout.Button("Generate Trie")) {
                GenerateTrie();
            }
            GUI.enabled = true;

            if(wordCount > 0) {
                EditorGUILayout.HelpBox($"Generated trie with {wordCount} words!", MessageType.Info);
            }
        }

        private void GenerateTrie() { 
            //var path = AssetDatabase.GetAssetPath(selectedDestAsset);
            string path = EditorUtility.SaveFilePanelInProject(
                    "Save Serialized Trie", 
                    selectedSourceAsset.name + "-trie", 
                    "txt",
                    "Please enter a file name to save the serialized trie asset to");
            if(path.Length == 0) { return; }

            string sourceText = selectedSourceAsset.text;
            var trie = new Trie();
            System.Text.StringBuilder currWord = new System.Text.StringBuilder("", 15);
            wordCount = 0;
            foreach(char c in sourceText) {
                if(c == '\n' && currWord.Length > 0) {
                    trie.Insert(currWord.ToString());
                    currWord.Remove(0, currWord.Length);
                    wordCount++;
                }
                else {
                    currWord.Append(c);
                }
            }
            if(currWord.Length > 0) {
                trie.Insert(currWord.ToString());
                wordCount++;
            }

            File.WriteAllText(path, trie.Serialize());
            AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
            AssetDatabase.ForceReserializeAssets(new string[] { path });
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
            EditorGUIUtility.PingObject(Selection.activeObject);
            Debug.Log($"done generating trie with {wordCount} words");
        }

    }

}
