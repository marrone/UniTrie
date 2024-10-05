using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;
using UnityEditor;

namespace UniTrie { 

    public class TrieTests {

        [Test]
        public void ConstructorTest() {
            var trie = new Trie();
            Assert.False(trie.Contains("foo"));
            trie = new Trie(new string[] { "foo", "bar", "apple" });
            Assert.True(trie.Contains("foo"));
            Assert.True(trie.Contains("bar"));
            Assert.True(trie.Contains("apple"));
            Assert.False(trie.Contains("pear"));
        }

        [Test]
        public void InsertDeleteTest() {
            var trie = new Trie();
            trie.Insert("foo");
            Assert.True(trie.Contains("foo"));
            trie.Delete("foo");
            Assert.False(trie.Contains("foo"));
        }

        [Test]
        public void ContainsTest() {
            var trie = new Trie();
            Assert.False(trie.Contains("foo"));
            trie.Insert("foo");
            Assert.True(trie.Contains("foo"));
            Assert.False(trie.Contains("fo"));
            Assert.False(trie.Contains("food"));
            trie.Insert("foobar");
            Assert.True(trie.Contains("foo"));
            Assert.True(trie.Contains("foobar"));
        }

        [Test]
        public void ContainsPrefixTest() {
            var trie = new Trie();
            Assert.False(trie.Contains("foo"));
            trie.Insert("foo");
            Assert.True(trie.ContainsPrefix("foo"));
            Assert.True(trie.ContainsPrefix("fo"));
            Assert.False(trie.ContainsPrefix("food"));
            trie.Insert("foobar");
            Assert.True(trie.ContainsPrefix("foo"));
            Assert.True(trie.ContainsPrefix("foobar"));
            Assert.True(trie.ContainsPrefix("foob"));
        }

        [Test]
        public void PrefixCountTest() {
            var trie = new Trie();
            var node = trie.GetPrefixNode("foo");
            Assert.Null(node);
            trie.Insert("foo");

            node = trie.GetPrefixNode("f");
            Assert.NotNull(node);
            Assert.That(node.PrefixCount, Is.EqualTo(1));
            node = trie.GetPrefixNode("fo");
            Assert.That(node.PrefixCount, Is.EqualTo(1));
            node = trie.GetPrefixNode("foo");
            Assert.That(node.PrefixCount, Is.EqualTo(1));

            trie.Insert("food");
            trie.Insert("foobar");
            node = trie.GetPrefixNode("foo");
            Assert.That(node.PrefixCount, Is.EqualTo(3));
            node = trie.GetPrefixNode("food");
            Assert.That(node.PrefixCount, Is.EqualTo(1));
            node = trie.GetPrefixNode("foob");
            Assert.That(node.PrefixCount, Is.EqualTo(1));

            trie.Delete("foobar");
            node = trie.GetPrefixNode("foo");
            Assert.That(node.PrefixCount, Is.EqualTo(2));
        }

        [Test]
        public void CaseInsensitivityTest() {
            var trie = new Trie();
            trie.Insert("foo");
            var variants = new string[] { "foo", "Foo", "FOO" };
            foreach(string word in variants) {
                Assert.True(trie.Contains(word));
            }
            Assert.False(trie.Contains("bar"));
        }

        [Test]
        public void AllocationTest() {
            var trie = new Trie();
            trie.Insert("foo");
            var words = new string[] { "foo", "Foo", "FOO", "bar" };
            Assert.That(() => {
                for(int i = 0; i < words.Length; i++) {
                    trie.Contains(words[i]);
                }
            }, Is.Not.AllocatingGCMemory());
        }

        [Test]
        public void SerializeDeserializeTest() {
            var sampleWords = new string[] { "foo", "bar", "alpha", "bravo", "charlie" };
            var trie = new Trie(sampleWords);
            string serialized = trie.Serialize();
            var trie2 = Trie.Deserialize(serialized);
            foreach(string word in sampleWords) { 
                Assert.True(trie2.Contains(word));
            }
            Assert.False(trie2.Contains("nonword"));
        }

        [Test]
        public void DictTest() {
            var textAsset = AssetDatabase.LoadAssetAtPath("Assets/UniTrie/Dicts/en_us-trie.txt", typeof(TextAsset)) as TextAsset;
            var trie = Trie.Deserialize(textAsset.text);
            Assert.True(trie.Contains("apple"));
            Assert.True(trie.Contains("zoological"));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*
        [UnityTest]
        public IEnumerator TrieTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        */
    }

}
