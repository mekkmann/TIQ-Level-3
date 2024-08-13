using System;
using TMPro;
using UnityEngine;

namespace MonsterQuest
{
    public class Console : MonoBehaviour
    {
        private static Console _instance;
        public bool verboseOutput;
        private int _indentationLevel;

        private TextMeshProUGUI _textMeshProUGui;

        public static bool verbose => _instance.verboseOutput;

        private void Awake()
        {
            _textMeshProUGui = GetComponent<TextMeshProUGUI>();
            _instance = this;
        }

        public static void Clear()
        {
            _instance.ClearInternal();
        }

        public static void Write(string text)
        {
            _instance.WriteInternal(text);
        }

        public static void WriteLine(string text)
        {
            _instance.WriteLineInternal(text);
        }

        public static void Indent()
        {
            _instance.IndentInternal();
        }

        public static void Outdent()
        {
            _instance.OutdentInternal();
        }

        private void ClearInternal()
        {
            _textMeshProUGui.text = "";
        }

        private void WriteInternal(string text)
        {
            // Add indentation to text.
            if (_indentationLevel > 0)
            {
                string indent = GetIndentPadding();
                text = text.Replace("\n", $"\n{indent}");
            }

            _textMeshProUGui.text += text;
        }

        private void WriteLineInternal(string text)
        {
            WriteInternal($"{text}\n");
        }

        private void IndentInternal()
        {
            ChangeIndentation(1);
        }

        private void OutdentInternal()
        {
            ChangeIndentation(-1);
        }

        private void ChangeIndentation(int change)
        {
            string oldIndent = GetIndentPadding();

            _indentationLevel = Math.Max(0, _indentationLevel + change);

            string newIndent = GetIndentPadding();

            // Remove indentation if we're on a new line.
            if (_textMeshProUGui.text.EndsWith($"\n{oldIndent}", StringComparison.Ordinal))
            {
                _textMeshProUGui.text = _textMeshProUGui.text[..^oldIndent.Length] + newIndent;
            }
            else if (_textMeshProUGui.text == oldIndent)
            {
                _textMeshProUGui.text = newIndent;
            }
        }

        private string GetIndentPadding()
        {
            return new string(' ', _indentationLevel * 4);
        }
    }
}
