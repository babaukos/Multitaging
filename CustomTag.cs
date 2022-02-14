using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Multitaging
{
    public class CustomTag : MonoBehaviour 
    {
        [SerializeField] private float _offset = 3f;
        public float offset
        {
            get
            {
                return _offset;
            }
        }
        public List<string> tags = new List<string>();
        private static List<CustomTag> taggers = null;
        public HashSet<string> TagsSet;
        
        private void Awake()
        {
            if (taggers == null) 
            {
                taggers = new List<CustomTag>();
            }
            
            taggers.Add(this);
            TagsSet = new HashSet<string>(tags);
        }

#region Public Methods
        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }  
        public IEnumerable<string> GetTags()
        {
            return tags;
        }    
        public void Rename(int index, string tagName)
        {
            tags[index] = tagName;
        } 
        public string GetAtIndex(int index)
        {
            return tags[index];
        }
        public int Count
        {
            get { return tags.Count; }
        }
#endregion

#region Public Static Methods
        public static IEnumerable<GameObject> FindGameObjectsWithTag(string tag) 
        {
            if (taggers == null || tag == null) return null;
        
            return taggers.Where(x => x.TagsSet.Contains(tag)).Select(x => x.gameObject);
        }
        public static IEnumerable<GameObject> FindGameObjectsWithTags(IEnumerable<string> tags, bool loose = false) 
        {
            if (taggers == null || tags == null) return null;
            
            return taggers
                .Where(x => loose ? x.TagsSet.Intersect(tags).Any() : x.TagsSet.Intersect(tags).Count() == tags.Count())
                .Select(x => x.gameObject);
        }
        // public static GameObject FindGameObjectWithTags(IEnumerable<string> tags, bool loose = false) 
        // {
        //     if (taggers == null || tags == null) return null;
            
        //     return taggers 
        //         .Where(x => loose ? x.TagsSet.Intersect(tags).Any() : x.TagsSet.Intersect(tags).Count() == tags.Count())
        //         .Select(x => x.gameObject);
        // }
#endregion
    }
}