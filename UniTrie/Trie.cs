using System;
using System.Text;
using System.Collections.Generic;

namespace UniTrie {

    public class Trie {

        const char MARKER_WORD = (char)3;
        const char MARKER_NEXT = (char)11;

        private TrieNode _root;
        
        public Trie(string[] words = null) {
            _root = new TrieNode();
            if(words != null) { 
                for(int i = 0; i < words.Length; i++) {
                    Insert(words[i]);
                }
            }
        }

        /**
         * Insert a word into the trie
         * Note: this does not check if the given word already exists.
         * If there is potential that you are inserting a duplicate word,
         * you should first call Contains() to determing if you should insert
         */
        public void Insert(string word) {
            if(String.IsNullOrEmpty(word)) { return; }
            var curr = _root;
            for(int i = 0; i < word.Length; i++) {
                curr = curr.Set(word[i]);
                curr.PrefixCount++;
            }
            curr.IsWord = true;
        }

        /**
         * Delete a word from the trie
         * Note: this does not check if the word exists, and thus
         * will affect the prefix counts even if no word exists in the trie
         * If this is a concern you should first call Contains() before calling
         * this method
         */
        public void Delete(string word) {
            if(String.IsNullOrEmpty(word)) { return; }
            var curr = _root;
            for(int i = 0; i < word.Length; i++) {
                curr = curr.Get(word[i]);
                if(curr == null) { return; }
                curr.PrefixCount--;
            }
            curr.IsWord = false;
        }

        /**
         * Return the node of the tail of the given prefix
         */
        public TrieNode GetPrefixNode(string prefix) {
            if(String.IsNullOrEmpty(prefix)) { return null; }
            var curr = _root;
            for(int i = 0; i < prefix.Length && curr != null; i++) {
                curr = curr.Get(prefix[i]);
            }
            return curr;
        }

        /**
         * Determines if the given prefix is in this trie
         */
        public bool ContainsPrefix(string prefix) {
            return GetPrefixNode(prefix) != null;
        }

        /**
         * Determines if the given word is in this trie
         */
        public bool Contains(string word) {
            var node = GetPrefixNode(word);
            return node != null && node.IsWord;
        }

        /**
         * Serializes this trie to a string
         */
        public string Serialize() {
            StringBuilder result = new StringBuilder("");
            Serialize(_root, result);
            return result.ToString();
        }
        
        private void Serialize(TrieNode root, StringBuilder result) {
            foreach(KeyValuePair<char, TrieNode> child in root.Letters) {
                result.Append(child.Key);
                if(child.Value.IsWord) { result.Append(MARKER_WORD); }
                Serialize(child.Value, result);
                result.Append(MARKER_NEXT);
            }
        }

        /**
         * Deserialize a serialized string into a Trie instance
         */
        public static Trie Deserialize(string serialized) {
            var trie = new Trie();
            var stack = new Stack<TrieNode>();
            stack.Push(trie._root);

            for(int i = 0; i < serialized.Length; i++) { 
                var c = serialized[i];
                switch(c) {
                    case MARKER_NEXT: 
                        stack.Pop();
                        break;
                    case MARKER_WORD:
                        stack.Peek().IsWord = true;
                        break;
                    default:
                        stack.Push(stack.Peek().Set(c));
                        break;
                }
            }

            return trie;
        }

    }

}
